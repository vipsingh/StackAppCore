using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.FormWidget;
using StackErp.Model.Layout;
using StackErp.ViewModel.ValueProvider;

namespace StackErp.ViewModel.ViewContext
{
    public class FormContext
    {
        public StackAppContext Context { get; }
        public IDBEntity Entity { protected set; get;}
        public EntityLayoutType EntityLayoutType { protected set; get;}
        public int ObjectId { private set; get;}
        public RequestQueryString RequestQuery { private set; get;}
        public LayoutContext LayoutContext { set; get;}
        private DynamicObj _parms;
        public DynamicObj Parameters { get => _parms; }

        public ObjectModelInfo EntityModelInfo { protected set; get;}
        private InvariantDictionary<IWidget> _controls;
        public InvariantDictionary<IWidget> Widgets { get => _controls;}
        public bool IsViewMode { protected set; get;}
        public PageActions Actions { get;}
        public UIFormModel SubmitModel {protected set; get;}
        public EntityModelBase EntityModel {protected set; get;}
        public CustomRequestInfo Customrequest {protected set; get;}

        public virtual bool IsNew
        {
            get => false;            
        }

    //     PageHeader: any
    // PageFooter: any
    //  JSIncludes: Array<string>
    // CSSIncludes: Array<string>

       public List<FormRule> FormRules {set;get;}
       public List<string> MissingFields {set;get;}
    // FieldDependency: Array<{Type: string, Field: string, Parent: string[]}>
    // _ruleIndex: number

        protected EntityCode _entity;
        public FormContext(StackAppContext context, EntityCode entity, RequestQueryString requestQuery)
        {
            Context = context;
            RequestQuery = requestQuery;
            _entity = entity;
            _controls = new InvariantDictionary<IWidget>();
            _parms = new DynamicObj();
            Actions = new PageActions();
            MissingFields = new List<string>();

            ObjectId = RequestQuery.ItemId;            
        }

        public virtual void Build(UIFormModel model = null)
        {
            this.SubmitModel = model;
            this.PrepareEntityContext();
        }

        public void AddControl(IWidget control) 
        {
            this.Widgets.Add(control.WidgetId, control);
        }

        public IWidget GetWidget(string widgetId) 
        {
            if (this.Widgets.ContainsKey(widgetId))
                return this.Widgets[widgetId];
            
            return null;
        }

        protected virtual void PrepareEntityContext() {
            var entityCntxt = new ObjectModelInfo(this.ObjectId, this._entity);     
            this.EntityModelInfo = entityCntxt;
            // if (this.SubmitModel != null)
            // {
            //     this.EntityModelInfo.Parameters = this.SubmitModel.EntityInfo.Parameters;
            // }
        }

        public void AddEntityModelInfo(string key, object value)
        {
            if (this.EntityModelInfo != null) {
                this.EntityModelInfo.Add(key, value);
            }            
        }
        // public T GetEntityModelInfo<T>(string key, T def)
        // {
        //     return this._props.Get(key, def);
        // }
        public void AddParameter(string key, object value)
        {
            this._parms.Add(key, value, true);
        }
        public T GetParameter<T>(string key, T value)
        {
            return this._parms.Get(key, value);
        }
        public BaseField GetField(string fieldName)
        {
            return this.Entity.GetFieldSchema(fieldName);
        }

        public void AddMissingField(string field)
        {
            if (!this.MissingFields.Contains(field))
                this.MissingFields.Add(field);
        }

        public TView GetLayoutView()
        {
            return this.LayoutContext.View;
        }
            
    }
}
