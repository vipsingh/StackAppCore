using System;

namespace StackErp.Model
{
    public enum AccessType
    {
        Full = 0,
        Create = 2,
        Update = 3,
        Read = 4,
        Delete = 5
    }
    public enum OperationType
    {
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

    public enum SubmitStatus
    {
        Success = 0,
        ConfirmationRequired = 1,
        Fail = 2,
        AccessDenied = 3,
        ConfirmAndRedirect = 4,
        ConfirmWithForm = 5,
        ParentReload = 6
    }

    public enum AppValueType
    {
        None = 0,
        EntityValue = 1,
        EntityField = 2,
        ReservedField = 3,
        Parameter = 4,
        RequestQuery = 5
    }

    public enum FirstDayOfWeek
    {
        Sunday = 0,
        Monday = 1,
        TuesDay = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

        public enum UserLoginStatus
    {
        NotInitialized = -1,
        LoggedIn = 0,
        NotLoggedIn = 1,
        WrongPassword = 2,
        InvalidLoginName = 3,
        UserDoesNotExists = 4,
        AccountSuspended = 5,
        InternalError = 6,
        WrongOldPassword = 7,
        ChangePasswordSuccessfull = 8,
        ChangePasswordFailed = 9,
        InvalidOTP = 10
    }

public enum AlertType
    {
        Information = 0,
        NewTask = 1,
        Reminder = 2,
        SystemAlert = 3,
        Warning = 4,
        UIAlert = 5        
    }
 public enum AlertCategory
    {
        Actionable = 1,
        Informational = 2
    }
        public enum AlertPriority
    {
        Low = 1,
        Normal = 2,
        High = 3,
        VeryHigh = 4

    }

        public enum TimeFormat
    {
        Default = 0,
        In12Hrs = 1,
        In24Hrs = 2
    }

    public enum ApplicationType
    {
        Web = 0,
        Mobile = 1,
        Window = 2
    }

    public enum NotificationMode
    {
        Invalid = -1,
        Popup = 0,
        Email = 1,
        Sms = 2
    }
}