import update from "immutability-helper";
import FilterCriteria from "./FilterCriteria";
import _ from "lodash";

function execute(rule: any, field: string, model: IDictionary<IFieldData>, formApi: FormApi): IDictionary<IFieldData> {  
  let isCriteriaMetch = true;
  if (rule.Criteria) {
    isCriteriaMetch = FilterCriteria.execute(rule.Criteria, model);
  }
  
  let fdata = model[rule.Field];
  const { VisibleOptions } = fdata;

  let visOptions: Array<number> = [];
  if (isCriteriaMetch) {
    visOptions = _.union(visOptions, VisibleOptions, rule.Options)
  } else {
    visOptions = _.filter(VisibleOptions, x => rule.Options.indexOf(x) < 0);
  }

  model = update(model, {[rule.Field]: { $merge: {VisibleOptions: visOptions}}});      

  return model;
}

export default {
  execute,
};
