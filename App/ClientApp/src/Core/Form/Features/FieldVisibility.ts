import _ from "lodash";
import FilterCriteria from "./FilterCriteria";

function execute(rule: any, field: string, model: any) {
    const fieldsToHide = rule.Fields;

    let isCriteriaMetch = true;
    if (rule.Criteria) {
        isCriteriaMetch = FilterCriteria.execute(rule.Criteria, model);
    }

    if(isCriteriaMetch) {
        fieldsToHide.forEach((fId: string) => {
            let fHide = model.getField(fId);
            Object.assign(fHide, { IsHidden: true });
            Object.assign(model, {Fields: _.assign({}, model.Fields, {[fId]: fHide}) });
        });
    }

    return model;
}

export default {
    execute
};