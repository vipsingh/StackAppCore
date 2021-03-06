using System;

namespace StackErp.Model
{

    public enum EntityType
    {
        CoreEntity=1,
        ChildEntity=2,
        MetadataEntity=3,
        Transiant=4
    }
public enum FieldType {
    None = 0,
    Text = 1,
    Integer = 2,
    Decimal = 3,
    DateTime = 4,
    Date = 5,
    MonataryAmount = 6,
    Bool = 7,
    Time = 8,
    Select = 9,
    ObjectLink = 10,
    MultiObjectLink = 11,
    BigInt = 12,
    LongText = 13,
    Image = 14,
    Email = 15,
    Url = 16,
    Html = 17,
    Xml = 18,
    KeyPair = 19,
    OneToMany = 20,
    OneToOne = 21,
    Password = 23,    
    Json = 24,
    ObjectKey = 25,
    FilterField = 26,
    File = 27,
    Phone=28,
    ObjectNumber=29,
    MultiSelect = 30,
    StackScript = 31
}
    
    public enum EntityRelationType 
    { 
        ManyToOne = 1,    
        OneToMany = 2,
        OneToOne = 3,
        ManyToMany=4
    }
public enum EntityModelHooksType {
    AfterModelPrepareToSave,
    BeforeValidate,
    AfterValidate,
    BeforeSave,
    AfterSave,
    AfterModelCreate
}

public enum ListingHookType {
    OnDefinition,
    BeforeDataExec,
    AfterDataExec,
    OnRowPrepare
}

    public enum AppStageCode
    {
        New = 0,
        Open = 1,
        Approve = 2,
        Reject = 9,
        Close = 10,
    }

        public enum ReportFormat
    {
        None,
        CSV,
        Excel,
        Xml,
        PDF,
        Html,
        ExcelStatic
    }
public enum QueryType
    {
        None = -1,
        View = 0,
        List = 1,
        Report = 2,
        Search = 3
    }

   public enum DataSourceType
    {
        None = 0,
        Entity = 1,
        StackScript = 2,
        Function = 3,
        Service = 4,
        Table = 5, // table should be defined in t_customtables
        Enum=6
        
    }

    public enum EntityDeletePolicyType
    {
        None = 0,
        AllowDelete = 1,
        
        OnlyUnlink = 2
    }
}