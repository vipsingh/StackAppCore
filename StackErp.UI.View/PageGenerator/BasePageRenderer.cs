using System;
using StackErp.Core.Layout;
using StackErp.Model.Layout;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.PageGenerator
{    
    public class BasePageRenderer
    {
        protected FormContext FormContext {private set; get;}
        protected LayoutFieldCompiler FieldCompiler {set; get;}
        public BasePageRenderer(FormContext cntxt)
        {
            FormContext = cntxt;
        }

        public void Generate(LayoutContext layoutContext)
        {
            this.Compile(layoutContext);

            this.OnRenderComplete();
        }

        protected virtual void Compile(LayoutContext layoutContext)
        {
            this.CompileWidgets(layoutContext.View);            
        }

        protected virtual void CompileWidgets(TView view)
        {
            foreach(var field in view.GetAllFields())
            {
                var fieldSchema = this.FormContext.GetField(field.FieldId);
                if(fieldSchema != null)
                {
                    this.FieldCompiler.Compile(fieldSchema, field);
                }                
            }
        }

        protected virtual void CompileActions()
        {

        }

        protected virtual void OnRenderComplete()
        {
            this.CompileActions();
        }        

        public virtual ViewPage GetViewPage()
        {
            return null;
        }
    }
}
