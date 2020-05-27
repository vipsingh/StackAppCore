
export class CustomRequestInfo {
    EntityInfo: ObjectModelInfo
    WidgetId: string
    Value!: any
    WidgetType!: number
    DependencyContext: any
    Parameters!: IDictionary<object>
    Properties!: IDictionary<object>

    constructor(widgetId: string, entityInfo: ObjectModelInfo) {
        this.WidgetId = widgetId;
        this.EntityInfo = entityInfo;
    }

    add(key: string, data: any) {
        this.Parameters[key] = data;
    }
}