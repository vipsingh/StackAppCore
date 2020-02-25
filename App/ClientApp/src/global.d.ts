/// <reference types="lodash" />

declare var _App: { Request: any, Notify: any };
declare var _AppSetting: any;
declare var _PAGE_DATA_: any;
declare var _Debug: any;

interface WidgetInfo {
    WidgetId: string,
    WidgetType: number,
    Value: any,
    Caption: string,
    IsRequired: bool,
    Disabled: boolean,
    ListValues: Array<any>,
    IsViewMode: boolean,
    DataActionLink: any,
    IsHidden: boolean,
    RuleToFire: Array<any>,
    Properties: Dictionary<any>,
    Parents: any,
    Validation: Dictionary<any>
}

interface WidgetInfoProps extends WidgetInfo{
    IsDirty: boolean
    api: any,
    onChange: Function,
    ValidationResult: any,
    children: any
}

interface ObjectModelInfo extends Dictionary {
    ObjectId: number
}

interface ListingProps extends WidgetInfoProps {
    listingSchema: any,
    ListData: any
}

interface ActionInfo {
    ActionId: string,
    Title: string,
    Url?: string,
    ActionType: number,
    DisplayType?: number,
    Icon?: string,
    ExecutionType?: number,
    onClick?: Function
}