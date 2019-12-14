using System;

namespace Model.Entity
{
    public class BaseField
    {
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
        public object Validations { set; get; }
        public object Computed { set; get; }
        public bool IsDbStore { set; get; }
        public bool Copy { set; get; }
        public string RefObject { set; get; }
        public string Domain { set; get; }
        //Related: {LinkFieldName: string, Field: string}
        //UIParams: any //ControlType, Model, DataSource    
        //OnChangeInfo: { DependUpon: string[] }

public short DecimalPlace {set;get;}
        public string ResolveDBName()
        {
            return this.Name;
        }

    }
}