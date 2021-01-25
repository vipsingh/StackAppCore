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
using Newtonsoft.Json.Linq;
using StackErp.Core.Entity;
using StackErp.Core.Form;

namespace StackErp.UI.View.Desginer
{
    public class LayoutDesignerBuilder
    {
        public static DesignerViewPage BuildPage(StackAppContext appContext, RequestQueryString requestQuery) {
            var layoutModel = EntityMetaData.Get(EntityCode.EntityLayout).GetSingle(appContext, requestQuery.ItemId);
            var entity = EntityMetaData.Get(requestQuery.RelatedEntityId);

            var view = TView.ParseFromJSON(layoutModel.GetValue("layoutjson", ""));
            if (view == null) {
                view = entity.GetDefaultLayoutView(0);
            }

            var pallet = BuildPallet(appContext, entity);
            
            var page = new DesignerViewPage(pallet, view);
            page.LayoutFields = view.Fields;
            page.Actions = new InvariantDictionary<Model.Form.ActionInfo>();
            
            var saveAction = new ActionInfo("Studio/SaveDesigner", requestQuery, "BTN_SAVE"){ 
                Title = "Save",
                ActionType = ActionType.Save,
                ExecutionType = ActionExecutionType.Custom
            };

            page.Actions.Add("BTN_SAVE", saveAction);

            return page;
        }

        #region Pallet
        private static DesignerPallet BuildPallet(StackAppContext appContext, IDBEntity entity)
        {
            var pallet = new DesignerPallet();

            pallet.Fields = PrepareFieldList(entity);
            pallet.Buttons = PrepareButtonNLinks(appContext, entity);

            return pallet;
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
        
        private static List<DynamicObj> PrepareButtonNLinks(StackAppContext appContext, IDBEntity entity)
        {
            var actions = EntityActionService.GetActions(appContext, entity.EntityId, EntityLayoutType.View);
            
            var coll = new List<DynamicObj>();
            foreach(var f in actions) 
            {
                var d = new DynamicObj();
                d.Add("Id", f.Id);
                d.Add("Text", f.Text);
                coll.Add(d);
            }

            return coll;
        }
        #endregion
        public static ActionResponse SaveLayout(StackAppContext appContext, RequestQueryString requestQuery, JObject data) {
            var view = data["Layout"].ToObject<TView>();

            if (view != null) {
                var tFields = data["Fields"].ToObject<List<TField>>();
                view.Fields = tFields;

                view.ClearBlankRows();

                var entity = EntityMetaData.GetAs<EntityLayoutEntity>(EntityCode.EntityLayout);
                var sts = entity.SaveLayoutData(appContext, requestQuery, view);

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
        public DesignerPallet Pallet {get;}
        public TView LayoutInfo {get;}
        public List<TField> LayoutFields {set; get;}

        public DesignerViewPage(DesignerPallet pallet, TView layoutInfo): base() {
            Pallet = pallet;
            PageType = AppPageType.Designer;
            LayoutInfo = layoutInfo;
        }
    }

    public class DesignerPallet
    {
        public List<DynamicObj> Fields {get;set;}
        public List<DynamicObj> Buttons {get;set;}
    }
}