using System;
using StackErp.Model;
using StackErp.ViewModel;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

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
            View.PageType = AppPageType.AppStudio;            
            View.CurrentQuery = reqQuery.ToQueryString();

            View.Actions = new InvariantDictionary<Model.Form.ActionInfo>();
            var qs = new RequestQueryString() { EntityId = EntityCode.EntitySchema, RelatedEntityId = EntityCode.EntityMaster, RelationField="layouts" };
            View.Actions.Add("BTN_NEW", new Model.Form.ActionInfo(AppLinkProvider.NEW_ENTITY_URL, qs){ Title = "New Field", LinkTarget="POPUP" });

            var qs1 = new RequestQueryString() { EntityId = EntityCode.EntityAction, RelatedEntityId = EntityCode.EntityMaster, RelationField="entityactions" };
            View.Actions.Add("BTN_NEW_ACTION", new Model.Form.ActionInfo(AppLinkProvider.NEW_ENTITY_URL, qs1){ Title = "New Action", LinkTarget="POPUP" });

            var formContext = new DetailFormContext(_appContext, EntityCode.EntityMaster, new RequestQueryString(){ EntityId = EntityCode.EntityMaster, ItemId = reqQuery.EntityId.Code });
            formContext.Build();

            AddLayoutListField(formContext);
            AddListListField(formContext);
            AddEntityActionsListField(formContext);

            return this.View;
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
    }
}
