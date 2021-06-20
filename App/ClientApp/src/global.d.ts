/// <reference types="lodash" />

declare var _App: { 
    Request: { getData: Function }, 
    Notify: any,
    FormatString: (str: string, data: any) => string
};
declare var _AppSetting: {
    UserId: number,
    RoleId: number,
    Language: string,
    DateFormat: string,
    BaseUrl: string,
    ApiUrl: string,
    AssetUrl: string,
    AppRoot: string,
    BaseCurrency: number,
    BaseCurrencySymbol: string,
    DecimalPlaces: number
};
declare var _PAGE_DATA_: any;
declare var _Debug: { error: Function, log: Function };
declare var __L: (key: string, def?: string) => string
declare var _isEntityStudioApp: bool;

interface IDictionary<T> {
    [index: string]: T;
}

interface IPageInfo {
    Widgets: IDictionary<WidgetInfoProps>,
    Actions?: IDictionary<any>,
    EntityInfo: ObjectModelInfo,
    ErrorMessage?: string,
    Layout?: any,
    PostUrl: string|undefined,
    FormRules?: any,
    Errors?: IDictionary<{ IsValid: boolean, Message?: string }>,
    Dependencies: IDictionary<Array<any>>
    getField: (widgetId: string) => WidgetInfoProps
}

interface WidgetInfo {
    WidgetId: string,
    WidgetType: number,
    Value?: any,
    Caption?: string,
    CaptionPosition?: number,
    IsRequired?: boolean,
    IsReadOnly?: boolean,
    Options?: Array<any>,
    IsViewMode?: boolean,
    DataActionLink?: any,
    IsHidden?: boolean,
    RuleToFire?: Array<any>,
    Properties?: IDictionary<any>,
    Attributes?: IDictionary<any>,
    Dependency?: {Parents?: Array<any>, Children?: Array<{WidgetId: string, Clear: boolean}>},
    Validation?: IDictionary<any>,
    AdditionalValue?: any, 
    FormatedValue?: any,
    FormatInfo?: any,
    IsMultiSelect?: boolean,
    DisplayType?: string,
    IsInLayout?: boolean,
    DecimalPlaces?: number,
    Features?: IDictionary<any>
}

interface WidgetInfoProps extends WidgetInfo{
    IsDirty?: boolean
    api: FormApi,
    onChange: Function,
    onBlur?: Function,
    ValidationResult?: any,
    children?: any,
    HasError?: boolean,
    Invisible?: boolean,
    VisibleOptions?: Array<number>,
    onFocus?: React.FocusEventHandler<HTMLElement>
}

interface IFieldData {
    Value?: any,
    IsDirty?: boolean,
    HasError?: boolean,
    Invisible?: boolean,
    IsReadOnly?: boolean,
    VisibleOptions?: Array<number>
}

// interface IFormData extends IDictionary<IFieldData> {
//     _UniqueId: string,
//     _EntityInfo: ObjectModelInfo
// }

interface FormApi {
    getEntitySchema: () => IPageInfo,
    setValue: Function,
    updateField: (widgetId: string, data: any, afterUpdate?: Function) => any,
    getField: (widgetId: string) => WidgetInfo,
    getErrorResult: Function,
    validateField: Function,
    prepareFieldRequest: Function,
    setWidgetTempdata: Function,
    getWidgetTempdata: Function
}

interface ObjectModelInfo extends IDictionary<any> {
    ObjectId: number
}

interface ListingProps extends WidgetInfoProps {
    ListData?: any,
    api?: FormApi,
    SelectionConfig?: any,
    FilterBox?: any,
    IsLocalStore?: boolean,
    formatCell?: Function,
    renderAction?: Function
}

interface ActionInfo {
    ActionId: string,
    Title: string,
    Url?: string,
    ActionType?: number,
    DisplayType?: number,
    Icon?: string,
    ExecutionType?: number,
    onClick?: Function,
    LinkTarget?: string,
    Attributes?: IDictionary<any>
}

interface RequestResultInfo {
    Status: number,
    Message: string,
    RedirectUrl: string
}