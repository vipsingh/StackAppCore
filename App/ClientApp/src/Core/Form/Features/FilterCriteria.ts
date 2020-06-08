import { criteriaEvaluator } from "../Utils/EvalCriteria";
import _ from "lodash";

function execute(criteria: any, model: IDictionary<IFieldData>) {
    if (!criteria) {
        return false;
    }

    const result: Array<boolean> = [];
    const groupType = Object.keys(criteria)[0]; //$and || $or

    criteria[groupType].forEach((ct: any) => {        
        const fieldName = Object.keys(ct)[0];
        const vals = ct[fieldName];

        const c = { FieldName: fieldName, Op: vals[0], Value: vals.length > 1 ? vals[1]: null }

        const f = model[fieldName];

        let r = false;
        if (f) {
            let val = f.Value;
            if (f.Value && typeof f.Value === "object") {
                val = val.Value;
            }

            if (typeof val === "string" && val.indexOf("@") === 0) {

            }

            r = criteriaEvaluator.execute(c.Op, val, c.Value);
        }

        result.push(r);
    });

    return result.indexOf(false) === -1;
}

export default {
    execute
};