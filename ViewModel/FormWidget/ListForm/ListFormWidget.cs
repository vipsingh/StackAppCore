using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackErp.Model;
using StackErp.Model.Utils;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class ListFormWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.ListForm; }
        public ListFormDisplayMode ListDisplayMode {set;get;} 
        public ViewPage FormPage  {set;get;} 
        [JsonIgnore]
        public string RelatedField  {set;get;} 
        public ListFormWidget(WidgetContext cntxt): base(cntxt)
        {
            ListDisplayMode = ListFormDisplayMode.Form;
        }

        public override void OnCompile()
        {
            this.CaptionPosition = 1;            
        }

        protected override bool OnSetData(object value)
        {
            var entityModel = this.Context.FormContext.EntityModelInfo;
            DataLink = new StackErp.Model.Form.ActionInfo("widget/GetListFormData", new RequestQueryString() { EntityId = entityModel.EntityId, ItemId = entityModel.ObjectId, FieldName = this.RelatedField });

            return base.OnSetData(value);
        }

        protected override bool OnFormatSetData(object value)
        {
            var entityModel = this.Context.FormContext.EntityModelInfo;
            DataLink = new StackErp.Model.Form.ActionInfo("widget/GetListFormData", new RequestQueryString() { EntityId = entityModel.EntityId, ItemId = entityModel.ObjectId, FieldName = this.RelatedField });

            return base.OnFormatSetData(value);
        }

        public override object GetValue()
        {
            List<EntityModelBase> list = new List<EntityModelBase>();
            if(PostValue is JArray)
            {
                foreach(JObject obj in (JArray)PostValue) 
                {
                    var uiModel = JSONUtil.DeserializeObject<UIFormModel>(obj.ToString());
                    if (uiModel.Widgets.Count == 0) continue;
                    
                    var qs = new RequestQueryString();
                    qs.EntityId = uiModel.EntityInfo.EntityId; 
                    qs.ItemId = uiModel.EntityInfo.ObjectId;

                    var context = new EditFormContext(this.Context.FormContext.Context, uiModel.EntityInfo.EntityId, qs);
                    context.Build(uiModel);

                    var generator = new EntityModelGenerator(context);
                    generator.Generate(uiModel);

                    if(generator.Status == AnyStatus.Success)
                    {
                        var recordModel = generator.RecordModel;
                        list.Add(recordModel);
                    }
                }
                           
            }

            return list;
        }
    }

    public enum ListFormDisplayMode
    {
        Grid = 1,
        Card = 2,
        Form = 3,
    }
}

