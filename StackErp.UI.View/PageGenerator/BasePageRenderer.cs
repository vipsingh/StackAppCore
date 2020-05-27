using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Core.Form;
using StackErp.Core.Layout;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.Model.Layout;
using StackErp.ViewModel;
using StackErp.ViewModel.FormWidget;
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
            FieldCompiler = new LayoutFieldCompiler(cntxt);     
        }

        public void Generate(LayoutContext layoutContext)
        {
            this.LayoutContext = layoutContext;
            View = new ViewPage(this.FormContext);

            this.Compile(layoutContext);
            this.OnCompileComplete(layoutContext);

            this.OnRenderComplete();
        }

        protected virtual void Compile(LayoutContext layoutContext)
        {            
            this.BuildLayoutRules(layoutContext.View);
            this.CompileWidgets(layoutContext.View);
            this.CompileActions(layoutContext.View); 
        }

        protected virtual void CompileWidgets(TView view)
        {
            foreach(var field in view.GetAllFields())
            {
                CompileWidget(field);   
            }
        }

        private void CompileWidget(TField field)
        {
            if (!this.FormContext.Widgets.ContainsKey(field.FieldId))
            {
                BaseField fieldSchema;
                fieldSchema = this.FormContext.GetField(field.FieldId);

                if(fieldSchema != null)
                {
                    this.FieldCompiler.Compile(fieldSchema, field);
                }
            }
        }

        protected virtual void BuilDependency()
        {
            var controls = this.FormContext.Widgets;
            if (controls != null)
            {
                foreach (var control in controls)
                {
                    var fieldSchema = this.FormContext.GetField(control.Value.WidgetId);
                    if (fieldSchema != null)
                    {
                        ViewModel.Services.FilterDependencyBuilder.Build(this.FormContext, fieldSchema, ((BaseWidget)control.Value));
                    }
                }
            }
        }

        protected virtual void BuildLayoutRules(TView view)
        {
            View.FormRules = ViewModel.Services.FormRuleBuilder.BuildRules(FormContext, view);
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
            var pageTitle = new DynamicObj();
            var entity = this.FormContext.Entity;
            if (entity != null) {
                pageTitle.Add(ViewConstant.PageTitle, entity.Text);
            }

            View.PageTitle = pageTitle;
        }    

        protected virtual void OnCompileComplete(LayoutContext layoutContext)
        {
            foreach(var field in FormContext.MissingFields)
            {
                CompileWidget(new TField(){ FieldId = field }); 
            }

            foreach(var formrule in View.FormRules)
            {
                var cFields = formrule.Criteria.GetCriteriaFields();
                foreach(var f in cFields)
                {
                    var w = this.FormContext.GetWidget(f);
                    if(w!= null)
                        ((BaseWidget)w).SetRule(formrule.Id);
                }
            }

            //Add Form parameters. 

            FieldOnCompileComplete();
        }

        protected void FieldOnCompileComplete()
        {
            var controls = this.FormContext.Widgets;
            if (controls != null)
            {
                foreach (var control in controls)
                {                    
                    control.Value.OnCompileComplete(FormContext);
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
