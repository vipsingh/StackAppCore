import _ from "lodash";

export function getFormDataToSubmit(entitySchema: IPageInfo, model: IDictionary<IFieldData>,  onlyChanged = true) {
    const { EntityInfo } = entitySchema;
    const submitModel: any = {EntityInfo};
    const f: any = {};
    _.forIn(model, (val, key) => {
        if (!onlyChanged || val.IsDirty) {
            if (key === "_UniqueId") return;
            
            const fieldInfo = entitySchema.getField(key);
            let value = val.Value;
            if (value && value._model) {
                value = getFormDataToSubmit(value._schema, value._model, false);
            } else if (_.isArray(value)) {
                value = _.map(val.Value, v => {
                    if (v && v._model) {
                        return getFormDataToSubmit(v._schema, v._model, false);
                    } else {
                        return v;
                    }
                });
            }
            f[key] = {
                Properties: fieldInfo.Properties,
                WidgetId: fieldInfo.WidgetId,
                Value: value,
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