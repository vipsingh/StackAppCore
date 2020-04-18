import _, { Dictionary } from "lodash";
import { ViewPageInfo } from "../../../../Component/Form/Form";

function validateForm(model: ViewPageInfo): PromiseLike<Dictionary<{IsValid: boolean, Message: string}>> {
    return new Promise((resolve, reject) => {
        const { Widgets } = model;
        const cErrors: Dictionary<{IsValid: boolean, Message: string}> = {};
        _.forIn(Widgets, (v, k) => {
            const vRes = validateField(v);
            //this.updateField(v.WidgetId, { HasError: !vRes.IsValid });

            if (vRes.IsValid) {
                delete cErrors[v.WidgetId];
            } else {
                cErrors[v.WidgetId] = vRes;
            }
        });
        
        resolve(cErrors);        
        
    });
}

function validateField(controlInfo: WidgetInfoProps) {
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
        if (r) return r;
    }

    return {
        IsValid: true
    };
}

const validator: Dictionary<Function> = {
    REQUIRED: (widgetInfo: WidgetInfoProps, validationInfo: any) => {
        const value = widgetInfo.Value;
        if (!value) {
            return {
                IsValid: false,
                Message: validationInfo.Msg
            };
        }
    }
};

export default { validateForm, validateField };