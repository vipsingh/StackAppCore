using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.FormWidget;

namespace StackErp.ViewModel.ViewContext
{
    public class FormContext
    {
        public StackAppContext Context { get; }
        public IDBEntity Entity { protected set; get;}
        public EntityLayoutType EntityLayoutType { private set; get;}
        public int ObjectId { private set; get;}
        public RequestQueryString RequestQuery { private set; get;}
        private DynamicObj _parms;
        public DynamicObj Parameters { get => _parms; }
        private DynamicObj _props;
        public DynamicObj Properties { get => _props; }

        public EntityModelInfo EntityModelInfo { protected set; get;}
        private Dictionary<string, BaseWidget> _controls;
        public Dictionary<string, BaseWidget> Controls { get => _controls;}
        public bool IsViewMode { private set; get;}
        public PageActions Actions { get;}
        public UIFormModel SubmitModel {protected set; get;}

        public virtual bool IsNew
        {
            get => false;            
        }

    //     PageHeader: any
    // PageFooter: any
    //  JSIncludes: Array<string>
    // CSSIncludes: Array<string>

    // FormRules: Array<{Type: string, Index: number, Criteria: FilterCriteria, Fields: string[]}>
    // FieldDependency: Array<{Type: string, Field: string, Parent: string[]}>
    // _ruleIndex: number

        protected EntityCode _entity;
        public FormContext(StackAppContext context, EntityCode entity, RequestQueryString requestQuery)
        {
            Context = context;
            RequestQuery = requestQuery;
            _entity = entity;
            _controls = new Dictionary<string, BaseWidget>();
            _parms = new DynamicObj();
            _props = new DynamicObj();
            Actions = new PageActions();

            ObjectId = RequestQuery.ItemId;            
        }

        public virtual void Build(UIFormModel model = null)
        {
            this.SubmitModel = model;
            this.AddEntityContext();
        }

        public void AddControl(BaseWidget control) 
        {
            this.Controls.Add(control.ControlId.ToUpper(), control);
        }

        public virtual void AddEntityContext() {
            var entityCntxt = new EntityModelInfo(this.ObjectId, this._entity);     
            this.EntityModelInfo = entityCntxt;
            if (this.SubmitModel != null)
            {
                this.EntityModelInfo.Parameters = this.SubmitModel.EntityInfo.Parameters;
            }
        }

        public void AddProperty(string key, object value)
        {
            this._props.Add(key, value, true);
        }
        public T GetProperty<T>(string key, T def)
        {
            return this._props.Get(key, def);
        }
        public void AddParameter(string key, object value)
        {
            this._parms.Add(key, value, true);
        }
        public BaseField GetField(string fieldViewName)
        {
            return this.Entity.GetFieldSchemaByViewName(fieldViewName);
        }

    //     addRules(ruleType: string, criteria: FilterCriteria, fields: string[]): number {
    //     if (!this.FormRules) this.FormRules = [];
        
    //     this.FormRules.push({Type: ruleType, Index: this._ruleIndex, Criteria: criteria, Fields: fields});
    //     this._ruleIndex++;
    //     return this._ruleIndex - 1;
    // }

    // setFieldDependency(field: string, depandUpon: string[], type: string = "") {
    //     if (!this.FieldDependency) this.FieldDependency = [];

    //     this.FieldDependency.push({Type: type, Field: field, Parent: depandUpon});

    //     depandUpon.forEach(f => {
    //         if (this.RefFields.indexOf(f) < 0) {
    //             this.RefFields.push(f);
    //         }
    //     });
    // }
    }
}
