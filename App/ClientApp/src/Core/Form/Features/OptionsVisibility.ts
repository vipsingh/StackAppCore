import update from "immutability-helper";
import FilterCriteria from "./FilterCriteria";
import _ from "lodash";

function execute(toField: WidgetInfo, field: string, model: IDictionary<IFieldData>, formApi: FormApi): IDictionary<IFieldData> {  
  const feature = toField.Features ? toField.Features["Options"] : {};

  let isCriteriaMetch = true;
  if (feature.Criteria) {
    isCriteriaMetch = FilterCriteria.execute(feature.Criteria, model);
  }
  
  let fdata = model[toField.WidgetId];
  const { VisibleOptions } = fdata;

  let visOptions: Array<number> = [];
  if (isCriteriaMetch) {
    visOptions = _.union(visOptions, VisibleOptions, feature.Options)
  } else {
    visOptions = _.filter(VisibleOptions, x => feature.Options.indexOf(x) < 0);
  }

  let fieldVal = fdata.Value && _.isObjectLike(fdata.Value) ? fdata.Value.Value : fdata.Value;
  if (visOptions && visOptions.indexOf(fieldVal) < 0)
    fieldVal = null;

  model = update(model, {[toField.WidgetId]: { $merge: {VisibleOptions: visOptions}}});
  if (!fieldVal) {
    model = update(model, {[toField.WidgetId]: { $merge: {Value: null}}});
  }

  return model;
}

export default {
  execute,
};
