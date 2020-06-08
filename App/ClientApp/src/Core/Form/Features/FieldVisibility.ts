import update from "immutability-helper";
import FilterCriteria from "./FilterCriteria";

function execute(
  toField: WidgetInfo,
  field: string,
  model: IDictionary<IFieldData>,
  formApi: FormApi
): IDictionary<IFieldData> {
  //const fieldsToHide = toField.WidgetId;
  const feature = toField.Features ? toField.Features["Invisible"] : {};

  let isCriteriaMetch = true;
  if (feature.Criteria) {
    isCriteriaMetch = FilterCriteria.execute(feature.Criteria, model);
  }

  let isHidden = false;
  if (isCriteriaMetch) {
    isHidden = true;
  }

  let fTempdata = formApi.getWidgetTempdata(toField.WidgetId);
  let fdata = model[toField.WidgetId];
  if (toField.IsHidden) return model;

  if (!isHidden && fdata.Invisible && fTempdata.hiddenBy !== field) {
  } else {
    model = update(model, {
      [toField.WidgetId]: { $merge: { Invisible: isHidden } },
    });
  }

  formApi.setWidgetTempdata(toField.WidgetId, { hiddenBy: field });

  return model;
}

export default {
  execute,
};
