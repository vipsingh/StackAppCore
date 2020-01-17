using System;

namespace StackErp.Model.Entity
{
    public class BaseField
    {
        public IDBEntity Entity  { set; get; }
        public FieldType Type { set; get; }
        public BaseTypeCode BaseType { set; get; }
        public string Name { set; get; }
        public string DBName { set; get; }
        public string Text { set; get; }
        public object DefaultValue { set; get; }
        public bool IsCustomField { set; get; }
        public short ViewId { set; get; }
        public bool IsObjectListType { set; get; }
        public bool IsReadOnly { set; get; }
        public bool IsRequired { set; get; }
        public bool IsUnique { set; get; }
        public IFieldValidation Validations { set; get; }
        public object Computed { set; get; }
        public bool IsDbStore { set; get; }
        public bool Copy { set; get; }
        public EntityCode RefObject { set; get; }
        public string Domain { set; get; }
        public DynamicObj Properties { set; get; }
        public string TableName { set; get; }
        
        public FormControlType ControlType { set; get; }
        private string _viewName;
        public string ViewName { set { _viewName = value; } get { return String.IsNullOrEmpty(_viewName)? this.Name: _viewName; } }
        public ControlDefinition ControlInfo { set; get; }

        public string LinkFieldName { set; get; }
        //Related: {LinkFieldName: string, Field: string}        
        //OnChangeInfo: { DependUpon: string[] }

        public short DecimalPlace {set;get;}
        public BaseField() {
            Properties = new DynamicObj();
        }
        private bool _isInit = false;
        public void Init()
        {
            if(_isInit) return;
            OnInit();
            _isInit  = true;
        }
        public virtual void OnInit()
        {
            
        }
        public virtual string ResolveDBName()
        {            
            return this.Name;
        }

        public void AddProperty(string key, object value)
        {
            Properties.Add(key, value);
        }

        public virtual object ResolveSetValue(object val, out bool isValid)
        {
            isValid = true;

            return val;
        }
        
    }
    
    public class ControlDefinition 
    {
        public object Visibility {set;get;}
        public object DataLinking {set;get;}
        public DataMap DataMapping {set;get;}
        public object DataSource {set;get;}        

    }

    public class DataMap {
        public object Text {set;get;}
        public object Value {set;get;}
        public object Code {set;get;}
    }
}