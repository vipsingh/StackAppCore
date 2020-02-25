import _, { Dictionary } from "lodash";

function validateField(controlInfo: WidgetInfoProps, value: object) {    
    if (controlInfo.IsRequired) {
        return validator.required(controlInfo, value);
    }

    const Validation = controlInfo.Validation;
    _.each(Object.keys(Validation), (k) => {
        if (validator[k]) {
            validator[k](controlInfo, Validation[k]);
        }
    });
}

const validator: Dictionary<Function> = {
    required: (widgetInfo: WidgetInfoProps, validationInfo: any) => {
        if (!widgetInfo.Value) {
            return {
                IsValid: false,
                Message: "Field is required"
            };
        }
    }
};

export default { validateField };