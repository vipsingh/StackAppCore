import update from "immutability-helper";
import FilterCriteria from "./FilterCriteria";

function execute(
  toField: WidgetInfo,
  field: string,
  model: IDictionary<IFieldData>,
  formApi: FormApi
): IDictionary<IFieldData> {
  //const fieldsToHide = toField.WidgetId;
  const feature = toField.Features ? toField.Features["ReadOnly"] : {};

  let isCriteriaMetch = true;
  if (feature.Criteria) {
    isCriteriaMetch = FilterCriteria.execute(feature.Criteria, model);
  }

  let isReadonly = false;
  if (isCriteriaMetch) {
    isReadonly = true;
  }

  let fTempdata = formApi.getWidgetTempdata(toField.WidgetId);
  let fdata = model[toField.WidgetId];
  if (toField.IsReadOnly) return model;

  if (!isReadonly && fdata.IsReadOnly && fTempdata.readonlyBy !== field) {
  } else {
    model = update(model, {
      [toField.WidgetId]: { $merge: { IsReadOnly: isReadonly } },
    });
  }

  formApi.setWidgetTempdata(toField.WidgetId, { readonlyBy: field });

  return model;
}

export default {
  execute,
};
