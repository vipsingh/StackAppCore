using System;
using StackErp.Core;
using StackErp.Model;
using StackErp.ViewModel;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;
using StackErp.Model.Entity;

namespace StackErp.UI.View.Studio
{
    public class StudioPageGenerator
    {
        public ViewPage CreateEntityPage()
        {
            return null;
        }

        public ViewPage CreateStudio()
        {
            return null;
        }
    }

    public class StudioPage : CustomPage
    {
        private StackAppContext _appContext;
        public StudioPage(StackAppContext appContext): base() 
        {
            _appContext = appContext;
        }

        public override ViewPage GetPage(RequestQueryString reqQuery) 
        {            
            int entityItemId = reqQuery.EntityId.Code;
            View.PageType = AppPageType.AppStudio;            
            View.CurrentQuery = reqQuery.ToQueryString();
           
            var formContext = new DetailFormContext(_appContext, EntityCode.EntityMaster, new RequestQueryString(){ EntityId = EntityCode.EntityMaster, ItemId = entityItemId });
            formContext.Build();            

            AddHeaderActions(reqQuery, entityItemId);

            AddFieldListField(formContext);
            AddLayoutListField(formContext);
            AddListListField(formContext);
            AddEntityActionsListField(formContext);

            return this.View;
        }

        private void AddFieldListField(DetailFormContext formContext) {
            var widgetContext = WidgetContext.BuildContext(formContext, "FieldList"); 
            widgetContext.WidgetType = FormControlType.EntityListView;

            var widget = new EntityListWidget(widgetContext, "fields");
            widget.OnCompile();
            widget.SetValue(null);

            AddFieldsInRow(new ViewModel.FormWidget.IWidget[]{ widget });
        }

        private void AddLayoutListField(DetailFormContext formContext) {
            var widgetContext = WidgetContext.BuildContext(formContext, "LayoutList"); 
            widgetContext.WidgetType = FormControlType.EntityListView;

            var widget = new EntityListWidget(widgetContext, "layouts");
            widget.OnCompile();
            widget.SetValue(null);

            AddFieldsInRow(new ViewModel.FormWidget.IWidget[]{ widget });
        }

        private void AddListListField(DetailFormContext formContext) {
            var widgetContext = WidgetContext.BuildContext(formContext, "EntityListList"); 
            widgetContext.WidgetType = FormControlType.EntityListView;

            var widget = new EntityListWidget(widgetContext, "entitylists");
            widget.OnCompile();
            widget.SetValue(null);

            AddFieldsInRow(new ViewModel.FormWidget.IWidget[]{ widget });
        }

        private void AddEntityActionsListField(DetailFormContext formContext) {
            var widgetContext = WidgetContext.BuildContext(formContext, "EntityActionList"); 
            widgetContext.WidgetType = FormControlType.EntityListView;

            var widget = new EntityListWidget(widgetContext, "entityactions");
            widget.OnCompile();
            widget.SetValue(null);

            AddFieldsInRow(new ViewModel.FormWidget.IWidget[]{ widget });
        }

        private void AddHeaderActions(RequestQueryString reqQuery, int entityItemId)
        {
            View.Actions = new InvariantDictionary<Model.Form.ActionInfo>();
            var qs = new RequestQueryString() { EntityId = EntityCode.EntitySchema, RelatedEntityId = EntityCode.EntityMaster, RelationField="entityid", RelatedObjectId = entityItemId};
            View.Actions.Add("BTN_NEW", new Model.Form.ActionInfo(AppLinkProvider.NEW_ENTITY_URL, qs){ Title = "New Field", LinkTarget="POPUP" });

            var qs1 = new RequestQueryString() { EntityId = EntityCode.EntityAction, RelatedEntityId = EntityCode.EntityMaster, RelationField="entityid", RelatedObjectId = entityItemId };
            View.Actions.Add("BTN_NEW_ACTION", new Model.Form.ActionInfo(AppLinkProvider.NEW_ENTITY_URL, qs1){ Title = "New Action", LinkTarget="POPUP" });

            var refEntity = EntityMetaData.Get(entityItemId);
            var layoutEntity = EntityMetaData.GetAs<Core.Entity.EntityLayoutEntity>(EntityCode.EntityLayout);
            var fExp = new Model.Entity.FilterExpression(EntityCode.EntityLayout);
            fExp.Add(new Model.Entity.FilterExpField("itemtype", FilterOperationType.Equal, refEntity.DefaultItemTypeId));
            fExp.Add(new Model.Entity.FilterExpField("entityid", FilterOperationType.Equal, entityItemId));
            fExp.Add(new Model.Entity.FilterExpField("viewtype", FilterOperationType.Equal, 0));
            var lids = layoutEntity.ReadIds(_appContext, fExp);

            if(lids.Count > 0)
            {
                var qs2 = new RequestQueryString() { EntityId = EntityCode.EntityLayout, ItemId = lids[0] };
                View.Actions.Add("BTN_VIEW_Layout", new Model.Form.ActionInfo(AppLinkProvider.DETAIL_ENTITY_URL, qs2){ Title = "View Layout", LinkTarget="POPUP" });
            }
            else {
                var qs2 = new RequestQueryString() { EntityId = EntityCode.EntityLayout, RelatedEntityId = EntityCode.EntityMaster, RelationField="entityid", RelatedObjectId = entityItemId };
                View.Actions.Add("BTN_NEW_Layout", new Model.Form.ActionInfo(AppLinkProvider.NEW_ENTITY_URL, qs2){ Title = "New Layout", LinkTarget="POPUP" });
            }

        }
    }
}
