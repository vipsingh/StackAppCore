import _ from "lodash";
import { rangeAdaptor } from "./Range";

function validateForm(entityInfo: IPageInfo, dataModel: IDictionary<IFieldData>): PromiseLike<IDictionary<{IsValid: boolean, Message: string}>> {
    return new Promise((resolve, reject) => {
        //const { Widgets } = model;
        const cErrors: IDictionary<{IsValid: boolean, Message: string}> = {};
        _.forIn(dataModel, (v, k) => {
            const field = entityInfo.getField(k);
            let vRes = validateField({...field, ...v}, entityInfo, dataModel);
            if (!vRes) {
                vRes = { IsValid: true };
            }
            //this.updateField(v.WidgetId, { HasError: !vRes.IsValid });

            if (vRes.IsValid) {
                delete cErrors[field.WidgetId];
            } else {
                cErrors[field.WidgetId] = vRes;
            }
        });
        
        resolve(cErrors);        
        
    });
}

function validateField(controlInfo: WidgetInfoProps, entityInfo?: IPageInfo, dataModel?: IDictionary<IFieldData>) {
    if (controlInfo.IsRequired) {
        let v = validator["REQUIRED"](controlInfo);

        return v || { IsValid: true };
    }

    const Validation = controlInfo.Validation;
    if (Validation) {
        let r: any = null;
        _.forIn(Validation, (v, k) => {
            if (validator[k] && !r) {
                const vRes = validator[k](controlInfo, v);
                if (vRes && !vRes.IsValid) {
                    r = vRes; 
                }
            }
        });
        if (r) {
            if (!r.Message)
                r.Message = "Invalid field value!";
            return r;
        }
    }

    return {
        IsValid: true
    };
}

const validator: IDictionary<Function> = {
    REQUIRED: (widgetInfo: WidgetInfoProps, validationInfo: any = {}) => {
        const value: any = widgetInfo.Value;
        if (!value || (_.isArray(value) && value.length === 0) || (_.has(value, "Value") && !value.Value)) {
            let msg = validationInfo.Msg || "${caption} is required.";
            return {
                IsValid: false,
                Message: _App.FormatString(msg, { caption: widgetInfo.Caption })
            };
        }
    },
    RANGE: rangeAdaptor
};

export default { validateForm, validateField };