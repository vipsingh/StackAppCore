using System;

namespace Model
{
public enum OperationType {
    None = 0,
    Add = 1,
    Update = 2,
    Delete = 3,
    Save = 4,
    View = 5,    
    Execute = 6,
    Import = 7,
    Print = 8,
    Merge = 9,
    Cancel = 10
}

public enum SubmitStatus {
    Success = 0,
    ConfirmationRequired = 1,
    Fail = 2,
    AccessDenied = 3,
    ConfirmAndRedirect = 4,
    ConfirmWithForm = 5,
    ParentReload = 6
}
}