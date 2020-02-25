import { criteriaEvaluator } from "../Utils/EvalCriteria";

function execute(criteria: Array<any>, model: any) {
    if (!criteria) {
        return false;
    }

    const result: Array<boolean> = [];
    criteria.forEach(c => {
        const f = model.getField(c.FieldName);

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