using System;

namespace Model
{
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

    ListView = 100
}

public enum EntityLayoutType {
    None = 0,
    Edit = 1,
    View = 2,
    QuickView = 3,
    EmailView = 4
}

public enum LayoutViewType {
    None = 0,
    Simple = 1
}

public enum LayoutRenderMode {
    None = 0,
    Tabs = 1,
    Sections = 2,
    Wizards = 3
}
}