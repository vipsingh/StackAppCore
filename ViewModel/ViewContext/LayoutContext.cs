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
        public int LayoutId {get;}
        public TView View {protected set; get;}
        public LayoutContext(StackAppContext context, RequestQueryString query)
        {
            this.Context = context;
            this.LayoutId = query.LayoutId;
            EntityId= query.EntityId;
        }

        public virtual TView Build()
        {
            var service = new EntityLayoutService(Context, EntityId);
            return View = service.PrepareView(this.LayoutId);
        }
    }

    public class DeskPageLayoutContext: LayoutContext
    {
        public DeskPageLayoutContext(StackAppContext context, RequestQueryString query)
            :base(context, query)
        {
        }
        public override TView Build()
        {
            var view = new TView();
            var rows = new List<TRow>();

            var f = new TField(){
                FieldId = "entitylist_" + this.EntityId.Code,
                Widget = FormControlType.EntityListView
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
