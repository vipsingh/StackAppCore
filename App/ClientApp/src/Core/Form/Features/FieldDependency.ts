import update from "immutability-helper";

function execute(field: string, model: IDictionary<IFieldData>, formApi: FormApi): IDictionary<IFieldData> {
    const widget = formApi.getField(field);
    if (!widget.Dependency) return model;

    const { Dependency: { Children } } = widget;
    const fieldDataUpdateColl: Array<string> = [];
    if (Children) {
        Children.forEach(function (childField) {
            model = processDeps(childField, widget, model, formApi, fieldDataUpdateColl);
        });
    }

    fieldDataUpdateColl.forEach(f => {
        formApi.setWidgetTempdata(f, { isReloadRequired: true });
    });
    
    return model;
}

function processDeps(childField: { WidgetId: string, Clear: boolean }, parentWidget: WidgetInfo, model: IDictionary<IFieldData>, formApi: FormApi, fieldDataUpdateColl: Array<string>): IDictionary<IFieldData> {
    const child = formApi.getField(childField.WidgetId);
    if (!child) {
        _Debug.error(`FieldDependency: Child Widget ${childField.WidgetId} not found.`);
        return model;
    }

    let val = model[child.WidgetId].Value;
    if (childField.Clear)
        val = "";

    model = update(model, {[child.WidgetId]: {Value: { $set: val }, IsDirty: { $set: true }}});    

    if (!child.DataActionLink) {
        _Debug.error(`FieldDependency: Data action for Widget ${childField.WidgetId} not found.`);
        return model;
    }

    //formApi.prepareFieldRequest(child.WidgetId, )
    // process data for fieldtype Dropdown, ObjectList
    fieldDataUpdateColl.push(child.WidgetId);

    return model;
}

export default {
    execute
}