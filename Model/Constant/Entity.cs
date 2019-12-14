using System;

namespace Model
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
    None = 0,
    Boolean = 1,
    Binary = 2,
    DateTime = 3,    
    DateTimeOffset = 4,
    Decimal = 5,
    String = 6,
    Double = 7,
    Guid = 8,
    Int16 = 9,
    Int32 = 10,
    Int64 = 11,
    Date = 12,
    Table = 13
}

}