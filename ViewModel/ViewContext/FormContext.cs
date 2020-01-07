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
        public IDBEntity Entity { get;}
        public EntityLayoutType EntityLayoutType { private set; get;}
        public int ObjectId { private set; get;}
        public RequestQueryString RequestQuery { private set; get;}
        private DynamicObj _parms;
        public DynamicObj Parameters { get => _parms; }

        public EntityModelInfo EntityModelInfo { private set; get;}
        private Dictionary<string, BaseWidget> _controls;
        public Dictionary<string, BaseWidget> Controls { get => _controls;}
        public bool IsViewMode { private set; get;}

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

        private string _entityName;
        public FormContext(StackAppContext context, string entity, RequestQueryString requestQuery)
        {
            Context = context;
            RequestQuery = requestQuery;
            _entityName = entity;
            _controls = new Dictionary<string, BaseWidget>();
            _parms = new DynamicObj();

            ObjectId = RequestQuery.ItemId;

            this.Entity = (IDBEntity)Core.EntityMetaData.Get(_entityName);
        }

        public virtual void Build()
        {
            this.AddEntityContext();
        }

        public void AddControl(BaseWidget control) 
        {
            this.Controls.Add(control.Name, control);
        }

        public virtual void AddEntityContext() {
            var entityCntxt = new EntityModelInfo(this.ObjectId, this._entityName);         
            this.EntityModelInfo = entityCntxt;
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
