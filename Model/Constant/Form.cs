using System;

namespace StackErp.Model
{

public enum AppPageType
{
    None = 0,
    Edit = 1,
    Detail = 2,
    Desk = 3,
    Report = 4,
    Dashboard = 5,
    Error = 10,
    AppStudio = 7,
    Designer = 8
}
public enum FormControlType {
    None = 0,
    TextBox = 1,
    DatePicker = 2,
    DecimalBox = 3,
    CheckBox = 4,
    NumberBox = 5,
    Dropdown = 6,
    EntityPicker = 7,
    LongText = 8,
    DateTimePicker = 9,
    Label = 10,
    Image = 11,
    File =12,
    HtmlText=13,
    Email=14,  
    Phone=15,
    Url=16, 
    Password=17, 
    FormattedText = 21,
    Avatar = 22,
    XmlEditor = 23,
    StackScriptEditor = 24,
    JsonEditor = 25,
    ListForm = 99,
    EntityListView = 100,
    EntityFilter=101
}

public enum EntityLayoutType {
    None = 0,
    Edit = 1,
    View = 2,
    QuickView = 3,
    EmailView = 4,
    Print = 5
}

public enum ReportLayoutType
{
    Grid = 0,
    Card = 1,
    Calendar = 2,
    Kanban = 3,
    Graph = 4
}

// public enum LayoutViewType {
//     None = 0,
//     Simple = 1
// }

public enum LayoutRenderMode {
    None = 0,
    Tabs = 1,
    Sections = 2,
    Wizards = 3
}

public enum CalendarMode
    {
        DayMode,
        WeekMode,
        MonthMode
    }

    public enum ValidationAt
    {
        None = 0,
        Client = 1,
        Server = 2,
        Both = 3 
    }

    public enum DateFormateType
    {
        None = -1,
        Default = 0,
        DobFormat = 1,
        FinnacialFormat = 2,
        MMYYYY = 3,
        YYYYMM = 4,
        YYYY = 5,
        DDMMYYYY = 6,
        MMDDYYYY = 7,
        YYYYMMDD = 8,
        FY = 9,
        Quarter = 10
    }

public enum ActionType {
    None = 0,
    New = 1,
    Edit = 2,
    View = 3,
    Print = 4,
    Delete = 5,
    Clone = 6,
    Cancel = 9,
    Save = 10,
    SaveClose = 11,
    SaveContinue = 12,
    Close = 13,
    Import = 14,
    Export= 15,
    Update = 16,
    Page=17,

    Report = 20,
    Url = 21,
    Client = 22,
    Function = 23,
    Script=25,

    Custom = 50
}

public enum ActionButtonPosition {
    None=0,
    Left=1,
    Right=2,
    Center=3,
    Bottom=4,

    Hide=10
}

public enum ActionDisplayType {
    None=0,
    Link=1,
    Button=2
}

public enum ActionExecutionType {
    None=0,
    Submit=1,
    Close=2,
    Function =3,
    Redirect=4,
    Client =5,
    Custom=6
}
public enum EvalSourceType {
    ModelField = 1,
    Constant = 2,
    RequestQuery = 3,
    User = 4,
    Env = 5,
    Parameters = 6
}
}