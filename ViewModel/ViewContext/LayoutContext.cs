using System;
using System.Collections.Generic;
using StackErp.Core.Layout;
using StackErp.Model;
using StackErp.Model.Layout;

namespace StackErp.ViewModel.ViewContext
{
    public class LayoutContext
    {
        public StackAppContext Context {get;}
        public EntityCode EntityId {get;}
        public int ItemTypeId {get;}
        public TView View {protected set; get;}
        public LayoutContext(StackAppContext context, int itemTypeId, EntityCode entityId)
        {
            this.Context = context;
            this.ItemTypeId = itemTypeId;
            EntityId= entityId;
        }

        public virtual TView Build()
        {
            var service = new EntityLayoutService(Context, EntityId);
            return View = service.PrepareView(this.ItemTypeId, EntityLayoutType.None);
        }
    }

    public class DeskPageLayoutContext: LayoutContext
    {
        public DeskPageLayoutContext(StackAppContext context, int itemTypeId, EntityCode entityId)
            :base(context, itemTypeId, entityId)
        {
        }
        public override TView Build()
        {
            var view = new TView();
            var rows = new List<TRow>();

            var f = new TField(){
                FieldId = "entitylist_" + this.EntityId.Code,
                Widget = FormControlType.EntityListView,
                FullRow = true
            };
            rows.Add(new TRow(){ Fields= new List<TField>() { f } });

            view.Pages.Add(new TPage() { Groups = new List<TGroup>() { new TGroup() { Rows = rows } } });
            view.Header = PrepareHeader();

            return View = view;
        }

        private TPage PrepareHeader()
        {
            var header = new TPage();
            var trow = new TRow();
            
            header.Groups.Add(new TGroup() { Rows = new List<TRow>() {  } });
            
            return header;
        }
    }
}
