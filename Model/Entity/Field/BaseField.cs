using System;
using System.Collections.Generic;
using StackErp.Model.Form;

namespace StackErp.Model.Entity
{
    public abstract class BaseField
    {
        public IDBEntity Entity  { set; get; }
        public FieldType Type { set; get; }
        public TypeCode BaseType { set; get; }
        public int FieldId { set; get; }
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
        public bool IsComputed { set; get; }
        public EvalExpression ComputeExpression { set; get; }
        public bool IsDbStore { set; get; }
        public bool Copy { set; get; }
        public EntityCode RefObject { set; get; }
        public DynamicObj Properties { set; get; }
        public string TableName { set; get; }
        public bool IsArrayData { protected set; get; }
        public int ViewOrder { set; get; }
        
        public FormControlType ControlType { set; get; }
        private string _viewName;
        public string ViewName { set { _viewName = value; } get { return String.IsNullOrEmpty(_viewName)? this.Name: _viewName; } }
        public ControlDefinition ControlInfo { set; get; }

        public string LinkFieldName { set; get; }
        public FieldPathExpression Related { set; get; }
        //OnChangeInfo: { DependUpon: string[] }

        public short DecimalPlace {set;get;}
        public bool AllowZero { get; set; }

        public FormatInfo FormatInfo { get; set; }

        public DynamicObj _TempSchemaData { get; set; }

        public BaseField() {
            Properties = new DynamicObj();
            ControlInfo = new ControlDefinition();
            FormatInfo = new FormatInfo();
        }
        private bool _isInit = false;
        public void Init()
        {
            if(_isInit) return;
            this.Text = String.IsNullOrEmpty(this.Text)? this.Name: this.Text;
            FormatInfo.FieldBaseType = this.BaseType;
            OnInit();
            _isInit  = true;
            _TempSchemaData = null;
        }
        public virtual void OnInit()
        {
            ControlInfo.FieldAttribute = new FieldAttribute(){ValueField = this.Name};
            ControlInfo.WidgetType = ControlType;
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

        public virtual object ResolveDbValue(DbObject db)
        {
            var v = db.Get(this.DBName, String.Empty);

            return DataHelper.GetDataValue(v, this.BaseType);
        }
        
    }
    
    public class ControlDefinition 
    {
        public object Visibility {set;get;}
        //public object DataLinking {set;get;}
        public SourceDataMap DataMapping {set;get;}
        public FieldDataSource DataSource {set;get;}        

        public FieldAttribute FieldAttribute {set;get;}
        public int CollectionId  {set;get;}
        public bool IsMultiSelect  {set;get;}

        public FormControlType WidgetType {set;get;}
    }

    public class SourceDataMap {
        public SourceFieldMap Text {set;get;}
        public SourceFieldMap Value {set;get;}
        public SourceFieldMap Code {set;get;}
        public List<SourceFieldMap> Others {set;get;}
    }
    public class SourceFieldMap
    {
        public string Source {set;get;}
        public string Field {set;get;}
    }

    public class FieldAttribute
    {
        public string ValueField {set;get;}
        public string TextField {set;get;}
        public string CodeField {set;get;}
        public object DefaultValue {set;get;}
        public EntityCode RefEntity {set;get;}
    }

    public class FieldDataSource
    {
        public int SourceId {set;get;} 
        public DataSourceType Type {set;get;}
        public EntityCode Entity {set;get;}
        // public List<string> Fields {set;get;}
        // public string IdField {set;get;}
        // public string SortOnField {set;get;}
        public string FunctionName {set;get;}
        public List<EvalParam> ParamMappings {set;get;}
        public FilterExpression Domain {set;get;}
    }

    public class DummyField: BaseField
    {
        public DummyField(string name, FieldType type, TypeCode baseType)
        {
            this.Name = DBName = name;
            this.Type = type;
            this.BaseType = baseType;
        }
    }

}