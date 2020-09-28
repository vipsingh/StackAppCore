using System;
using StackErp.ViewModel.Model;
using StackErp.UI.View.PageGenerator;
using StackErp.ViewModel.ViewContext;
using StackErp.Model;
using StackErp.Core;
using System.Collections.Generic;
using StackErp.Core.Layout;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.Model.Layout;

namespace StackErp.UI.View.Desginer
{
    public class LayoutDesignerBuilder
    {
        public static DesignerViewPage BuildPage(StackAppContext appContext, RequestQueryString requestQuery) {
            var layoutModel = EntityMetaData.Get(EntityCode.EntityLayout).GetSingle(requestQuery.ItemId);
            var entity = EntityMetaData.Get(requestQuery.RelatedEntityId);

            var fields = PrepareFieldList(entity);
            
            var page = new DesignerViewPage(fields);
            page.Actions = new InvariantDictionary<Model.Form.ActionInfo>();
            
            var saveAction = new ActionInfo("Studio/SaveDesigner", requestQuery, "BTN_SAVE"){ 
                Title = "Save",
                ActionType = ActionType.Save,
                ExecutionType = ActionExecutionType.Custom
            };

            page.Actions.Add("BTN_SAVE", saveAction);

            return page;
        }

        private static List<DynamicObj> PrepareFieldList(IDBEntity entity) {
            
            var fields = entity.GetLayoutFields(EntityLayoutType.None);

            var coll = new List<DynamicObj>();
            foreach(var f in fields) {
                var d = new DynamicObj();
                d.Add("Name", f.Name);
                d.Add("Text", f.Text);
                d.Add("Type", f.Type);
                d.Add("WidgetType", f.ControlType);
                d.Add("Isrequired", f.IsRequired);
                d.Add("ShouldFullRow", EntityLayoutService.IsWidgetOnFullRow(f.ControlType));
                coll.Add(d);
            }

            return coll;
        }

        public static ActionResponse SaveLayout(StackAppContext appContext, RequestQueryString requestQuery, TView view) {
            if (view != null) {
                var entity = EntityMetaData.Get(EntityCode.EntityLayout);
                var layoutModel = entity.GetSingle(requestQuery.ItemId);

                var json = Model.Utils.JSONUtil.SerializeObject(view);
                layoutModel.SetValue("layoutjson", json);

                var sts = entity.Save(appContext, layoutModel);
                if (sts == AnyStatus.Success) {
                    var res = new SubmitActionResponse(null) {
                        Status = SubmitStatus.Success,
                        Message = "Layout saved successfully"
                    };

                    return res;
                } else {
                    return new ErrorResponse(null) { Message = sts.Message };
                }
            }
            return new ErrorResponse(null) { Message = "Invalid parameters." };
        }
    }

    public class DesignerViewPage: ViewPage {
        public List<DynamicObj> Fields {get;}
        public DesignerViewPage(List<DynamicObj> fields): base() {
            Fields = fields;
            PageType = AppPageType.Designer;
        }
    }
}