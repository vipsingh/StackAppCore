using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.Model.Utils;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class EntityFilterWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.EntityFilter; }
        public ObjectPickerWidget SourcePicker { private set; get; }
        public ActionInfo FilterFormLink { protected set; get; }
        public EntityCode FilterEntityId {set;get;}
        public EntityFilterWidget(WidgetContext cntxt): base(cntxt)
        {
        }

        public override void OnCompile()
        {
            base.OnCompile();

            var qs = new RequestQueryString();
            qs.EntityId = FilterEntityId;
            FilterFormLink = new ActionInfo("/Filter/GetFilterFieldForms", qs, "FILTERFORM");
            DataActionLink = new ActionInfo("/Filter/GetFilterFieldData", qs, "FILTERDATA");
            CreateSourcePicker();
        }

        private void CreateSourcePicker() 
        {
            SourcePicker = new ObjectPickerWidget(this.Context);
            
            var sourceParam = new DynamicObj();
            sourceParam.Add("EntityId", this.FilterEntityId);
            SourcePicker.AddProperty("PickerSource", "SYSTEM_2");
            SourcePicker.AddProperty("PickerSourceParams", sourceParam);
            
            SourcePicker.OnCompile();

            //  var q = new RequestQueryString();
            // q.WidgetId = this.WidgetId;
            // q.EntityId = this.Context.FormContext.RequestQuery.EntityId;
            
            // var link = new ActionInfo("Filter/GetFilterPickerData", q);
            // link.ActionType = ActionType.Custom;
            // link.ExecutionType = ActionExecutionType.Custom;
            
        }
        

        // protected override bool OnSetData(object value)
        // {
        //     //this.Value = 
        //     return true;
        // }

        public override object GetValue()
        {
            if(PostValue is JArray)
            {
                var qs = new RequestQueryString();
                qs.EntityId = EntityCode.User; 
                var formContext = new EditFormContext(this.Context.FormContext.Context, FilterEntityId, qs);
                var valueProvider = new ValueProvider.FormValueProvider(formContext);

                var filterExp = new FilterExpression(qs.EntityId);

                foreach(JObject obj in (JArray)PostValue) 
                {
                    BuildFilter(obj, valueProvider, formContext.Entity, ref filterExp);
                }                

                return filterExp.ToJSONFormat();
            }

            return null;
        }

        private void BuildFilter(JObject obj, ValueProvider.FormValueProvider valueProvider, IDBEntity entity, ref FilterExpression filterExp)
        {
            var uiModel = JSONUtil.DeserializeObject<UIFormModel>(obj.ToString());
            var fieldName = uiModel.EntityInfo.Get("FieldName", "");
                    
            var f = uiModel.Widgets["Field"];
            f.WidgetId = fieldName;
            object val;
            if (valueProvider.ResolveFieldValue(f, out val))
            {
                var fieldInfo = entity.GetFieldSchema(fieldName);
                bool isValid;
                val = fieldInfo.ResolveSetValue(val, out isValid);

                var op = JSONUtil.DeserializeObject<SelectOption>(uiModel.Widgets["OP"].Value.ToString());
                
                var expVal = val.ToString();
                if (val is DateTime) expVal = ((DateTime)val).ToString("d");
                var ecp = new FilterExpField(fieldName, (FilterOperationType)op.Value, expVal);
                //handle range type
                filterExp.Add(ecp);
            }
        }
    }
}

