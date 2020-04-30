import update from "immutability-helper";
import FilterCriteria from "./FilterCriteria";

function execute(rule: any, field: string, model: IDictionary<IFieldData>, formApi: FormApi): IDictionary<IFieldData> {
  const fieldsToHide = rule.Fields;

  let isCriteriaMetch = true;
  if (rule.Criteria) {
    isCriteriaMetch = FilterCriteria.execute(rule.Criteria, model);
  }

  let isHidden = false;
  if (isCriteriaMetch) {
    isHidden = true;
  }
  fieldsToHide.forEach((fId: string) => {    
    let fHide = formApi.getField(fId);
    let fTempdata = formApi.getWidgetTempdata(fId);
    let fdata = model[fId];
    if (fHide.IsHidden) return;

    if (!isHidden && fdata.Invisible && fTempdata.hiddenBy !== field) {
    } else {       
      model = update(model, {[fId]: { $merge: {Invisible: isHidden}}});      
    }

    formApi.setWidgetTempdata(fId, { hiddenBy: field });
  });

  return model;
}

export default {
  execute,
};
