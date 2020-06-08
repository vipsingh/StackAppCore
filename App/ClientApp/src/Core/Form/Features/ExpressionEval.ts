import update from "immutability-helper";

function execute(toField: WidgetInfo, model: IDictionary<IFieldData>, formApi: FormApi): IDictionary<IFieldData> {    
    const feature = toField.Features ? toField.Features["Eval"] : {};
    const { Expression: Exp, Depends: ExpFields } = feature;

    try {
        const arrVar: any[] = [];
        ExpFields.forEach((ef: string) => {
            const v = model[ef].Value;
            let d = typeof v === "object" ? v.Value : v;
            if (typeof d === "string") {
                d = `'${d}'`;
            }

            d = `var ${ef} = ${d};`
            arrVar.push(d);
        });

        if (Exp.match(/function|document|<script/g)) {
            throw new Error("invalid expression");
        }

        const str = `function(){ ${arrVar.join(';')} return ${Exp} }`;
        const res = Function(`return (${str})()`)();   
        
        model = update(model, {[toField.WidgetId]: { Value: { $set: res}, IsDirty: { $set: true}}});              
    } catch(ex) {
        _Debug.error(`ExpressionEval: error in evaluating expression on ${toField.WidgetId}. ${ex}`);
    }
    return model;

}

export default {
    execute
}