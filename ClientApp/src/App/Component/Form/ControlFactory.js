import * as CTRL from "./Control/input";
import LabelField from "./Control/LabelField";


export function getControlComponent(controlType, isViewMode) {
    let editComponent = null;
    let viewComponent = LabelField;
    switch(controlType) {
        case 1:
            editComponent = CTRL.TextBox;
            break;
        case 2:
            editComponent = CTRL.DateBox;
            break;
        case 3:
            editComponent = CTRL.DecimalBox;
            break;
        case 4:
            editComponent = viewComponent = CTRL.CheckBox;
            break;
        case 5:
            editComponent = CTRL.NumberBox;
            break;
        case 6:
            editComponent = CTRL.SelectBox;
            break;
        case 7:
            editComponent = CTRL.EntityPicker;
            break;
        case 8:
            editComponent = CTRL.TextArea;
            break;
    }
    if(isViewMode) {
        return viewComponent;
    }
    return editComponent;
}

export function getBasicValidation(controlInfo) {

}