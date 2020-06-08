import _ from "lodash";

export function getFormDataToSubmit(entitySchema: IPageInfo, model: IDictionary<IFieldData>,  onlyChanged = true) {
    const { EntityInfo } = entitySchema;
    const submitModel: any = {EntityInfo};
    const f: any = {};
    _.forIn(model, (val, key) => {
        if (!onlyChanged || val.IsDirty) {
            if (key.startsWith("_")) return;
            
            const fieldInfo = entitySchema.Widgets[key];
            let value = val.Value;
            if (value && value._model) {
                value = getFormDataToSubmit(value._schema, value._model, onlyChanged);
            } else if (_.isArray(value)) {
                value = _.map(val.Value, v => {
                    if (v && v._model) {
                        return getFormDataToSubmit(v._schema, v._model, onlyChanged);
                    } else if (fieldInfo.WidgetType === 99) {
                        var listField = fieldInfo as any;
                        var localOnlyC = false;
                        if (v._EntityInfo && v._EntityInfo.ObjectId > 0) {
                            localOnlyC = true;
                        }
                        const changedModel = getFormDataToSubmit(listField.FormPage, v, localOnlyC);
                        changedModel.EntityInfo = v._EntityInfo.Value;

                        return changedModel;
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
        req.DependencyContext = { Dependency };

        if (Dependency.Parents) {
            const refData: any = { };
            _.each(Dependency.Parents, (p) => {
                const f = model[p.WidgetId];
                const wInfo = entitySchema.getField(p.WidgetId);
                if (f) {
                    refData[p.WidgetId] = { WidgetId: wInfo.WidgetId, WidgetType: wInfo.WidgetType, Value: f.Value };
                }                
            });
            
            req.DependencyContext.RefData = refData;
        }
    }

    //req.Parameters = {};

    return req;
}

export function prepareWidgetRequest(widget: WidgetInfoProps) {
    const { WidgetId, Properties, WidgetType, Caption, Value } = widget;

    const req: any = {
        WidgetId: WidgetId,
        Properties,
        Value,
        WidgetType,
        Caption      
    };

    return req;
}