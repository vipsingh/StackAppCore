using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.ViewModel;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.PageAction
{
    public class EntityPageAction
    {
        private StackAppContext _AppContext;
        public EntityPageAction(StackAppContext appContext)
        {
            _AppContext = appContext;
        }

        public SubmitActionResponse GetSaveAction(RequestQueryString requestQuery, UIFormModel model)
        {
            var context = new EditFormContext(_AppContext, requestQuery.EntityId, requestQuery);
            context.Build(model);

            var generator = new EntityModelGenerator(context);
            generator.Generate(model);

            var response = new SubmitActionResponse(null);
            response.Status = SubmitStatus.Fail;

            if(generator.Status == AnyStatus.Success)
            {
                var recordModel = generator.RecordModel;
                var saveStatus = context.Entity.Save(recordModel);
                if (saveStatus == AnyStatus.Success)
                {
                    response.Status = SubmitStatus.Success;
                    response.RedirectUrl = AppLinkProvider.GetDetailPageLink(recordModel.EntityId, recordModel.ID).Url;
                    return response;
                }
                else if(saveStatus == AnyStatus.InvalidData)
                {
                    PrepareErrorModel(recordModel, ref model);
                    response.Model = model;                    
                    return response;
                }
                response.Message = saveStatus.Message;
                return response;
            }
            response.Message = generator.Status.Message;

            return response;
        }

        private void PrepareErrorModel(EntityModelBase recordModel, ref UIFormModel model)
        {
            var fields = new InvariantDictionary<UIFormField>();
            foreach(var fData in recordModel.GetInvalidFields())
            {
                fields.Add(fData.Field.ViewName.ToUpper(), new UIFormField(){ WidgetId = fData.Field.ViewName, ErrorMessage = fData.ErrorMessage });
            }

            model.Widgets = fields;
        }
    }
}
