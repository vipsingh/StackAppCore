using System;

namespace StackErp.Model
{
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
    ObjectNumber = 11,
    BigInt = 12,
    LongText = 13,
    Image = 14,
    Email = 15,
    Url = 16,
    Html = 17,
    Xml = 18,
    KeyPair = 19,
    ObjectList = 20,
    Computed = 21,
    Password = 22,
    File = 23,
    Json = 24,
    ObjectKey = 25
}

public enum BaseTypeCode {
    None = -1,
        AnsiString = 0,
        Binary = 1,
        Byte = 2,
        Boolean = 3,
        Currency = 4,
        Date = 5,
        DateTime = 6,
        Decimal = 7,
        Double = 8,
        Guid = 9,
        Int16 = 10,
        Int32 = 11,
        Int64 = 12,
        Object = 13,
        SByte = 14,
        Single = 15,
        String = 16,
        Time = 17,
        UInt16 = 18,
        UInt32 = 19,
        UInt64 = 20,
        VarNumeric = 21,
        AnsiStringFixedLength = 22,
        StringFixedLength = 23,
        Xml = 25,
        DateTime2 = 26,
        DateTimeOffset = 27,
    Entity = 50
}
    
    public enum EntityRelationType 
    {
        LINK = 1,
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

}