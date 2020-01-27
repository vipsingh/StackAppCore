using System;
using StackErp.Core.Layout;
using StackErp.Model.Layout;

namespace StackErp.ViewModel.ViewContext
{
    public class LayoutContext
    {
        public FormContext FormContext {get;}
        public int LayoutId {get;}
        public TView View {protected set; get;}
        public LayoutContext(FormContext formContext)
        {
            this.FormContext = formContext;
            this.LayoutId = formContext.RequestQuery.LayoutId;
        }

        public virtual TView Build()
        {
            var service = new EntityLayoutService(FormContext.Context, FormContext.Entity.EntityId);
            return View = service.PrepareView(this.LayoutId);
        }
    }
}
