using System;
using StackErp.Model;
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

                    return response;
                }
                response.Message = saveStatus.Message;
                return response;
            }
            response.Message = generator.Status.Message;

            return response;
        }
    }
}
