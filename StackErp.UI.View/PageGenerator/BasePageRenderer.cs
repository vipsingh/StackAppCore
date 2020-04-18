using System;
using StackErp.Core.Form;
using StackErp.Core.Layout;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.Model.Layout;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.PageGenerator
{    
    public class BasePageRenderer
    {
        protected LayoutContext LayoutContext {private set; get;}
        protected FormContext FormContext {private set; get;}
        protected LayoutFieldCompiler FieldCompiler {set; get;}
        public ViewPage View {private set; get;}
        public BasePageRenderer(FormContext cntxt)
        {
            FormContext = cntxt;            
        }

        public void Generate(LayoutContext layoutContext)
        {
            this.LayoutContext = layoutContext;
            View = new ViewPage(this.FormContext);
            this.Compile(layoutContext);

            this.OnRenderComplete();
        }

        protected virtual void Compile(LayoutContext layoutContext)
        {            
            this.BuildFormRules(layoutContext.View);
            this.CompileWidgets(layoutContext.View);
            this.CompileActions(layoutContext.View);

            this.OnCompileComplete();                        
        }

        protected virtual void CompileWidgets(TView view)
        {
            foreach(var field in view.GetAllFields())
            {
                CompileWidget(field);   
            }

            foreach(var field in FormContext.MissingFields)
            {
                CompileWidget(new TField(){ FieldName = field }); 
            }
        }

        private void CompileWidget(TField field)
        {
            BaseField fieldSchema;
            if(!String.IsNullOrEmpty(field.FieldName))
                fieldSchema = this.FormContext.Entity.GetFieldSchema(field.FieldName);
            else
                fieldSchema = this.FormContext.GetField(field.FieldId);

            if(fieldSchema != null)
            {
                this.FieldCompiler.Compile(fieldSchema, field);
            }
        }

        protected virtual void BuildFormRules(TView view)
        {
            View.FormRules = ViewModel.Helper.FormRuleBuilder.BuildRules(FormContext, view);
        }

        protected virtual void CompileActions(TView view)
        {
            if(view.Commands != null) 
            {
                foreach(var command in view.Commands)
                {
                    var c = EntityActionService.GetViewAction(this.FormContext.Context, this.FormContext.Entity.EntityId, FormContext.EntityLayoutType, command.Id);
                    if(c!= null)
                    {
                        this.FormContext.Actions.Add(PageActionCreator.BuildActionFromDefinition(c, this.FormContext));
                    }
                }
            }
        }

        protected virtual void OnRenderComplete()
        {
            
        }    

        protected virtual void OnCompileComplete()
        {
            foreach(var formrule in View.FormRules)
            {
                var cFields = formrule.Criteria.GetCriteriaFields();
                foreach(var f in cFields)
                {
                    var w = this.FormContext.GetWidget(f);
                    if(w!= null)
                        w.SetRule(formrule.Id);
                }
            }
        }      

        public virtual ViewPage GetViewPage()
        {
            View.EntityInfo = FormContext.EntityModelInfo;
            View.Actions = FormContext.Actions.ActionButtons;
            View.Layout = FormContext.GetLayoutView();
            return View;
        }
    }
}
