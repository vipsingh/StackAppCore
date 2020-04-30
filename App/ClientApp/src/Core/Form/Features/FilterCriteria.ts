import { criteriaEvaluator } from "../Utils/EvalCriteria";
import _ from "lodash";

function execute(criteria: Array<any>, model: IDictionary<IFieldData>) {
    if (!criteria) {
        return false;
    }

    const result: Array<boolean> = [];
    criteria.forEach(c => {
        if (_.isArray(c)) {
            c = { FieldName: c[0], Op: c[1], Value: c.length > 2 ? c[2]: null };
        }
        const f = model[c.FieldName];

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