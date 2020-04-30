import _ from "lodash";

export function getFormDataToSubmit(entitySchema: IPageInfo, model: IDictionary<IFieldData>,  onlyChanged = true) {
    const { EntityInfo } = entitySchema;
    const submitModel: any = {EntityInfo};
    const f: any = {};
    _.forIn(model, (val, key) => {
        if (!onlyChanged || val.IsDirty) {
            const fieldInfo = entitySchema.getField(key);
            f[key] = {
                Properties: fieldInfo.Properties,
                WidgetId: fieldInfo.WidgetId,
                Value: val.Value,
                WidgetType: fieldInfo.WidgetType
            };
        }
    });

    submitModel.Widgets = f;

    return submitModel;
}

export function prepareFieldRequest(entitySchema: IPageInfo, model: IDictionary<IFieldData>, widgetId: string) {    
    const { Properties, Dependency, WidgetType, Caption } = entitySchema.getField(widgetId);
    const Value = model[widgetId];

    const req: any = {
        WidgetId: widgetId,
        Properties,
        Value,
        WidgetType,
        Caption,
        EntityInfo: model.EntityInfo,
    };

    if (Dependency) {
        req.DependencyInfo = { Dependency };

        if (Dependency.Parents) {
            const refData: any = { };
            _.each(Dependency.Parents, (p) => {
                const f = model[p.WidgetId];
                if (f) {
                    refData[p.WidgetId] = f.Value;
                }                
            });
            
            req.DependencyInfo.RefData = refData;
        }
    }

    return req;
}