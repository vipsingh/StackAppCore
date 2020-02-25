import * as CTRL from "./Form/Control/input";
import LabelField from "./Form/Control/LabelField";
import ListView from "./ListView";
import { Dictionary } from "lodash";


export function getComponent(controlType: number, isViewMode: boolean) {
    let editComponent = null;
    let viewComponent: any = LabelField;
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
        default:
            const w = widgets[controlType];
            editComponent = w.edit;
            viewComponent = w.view;
            break;
    }
    if(isViewMode) {
        return viewComponent;
    }
    return editComponent;
}

export function getBasicValidation(controlInfo: any) {

}


const widgets: Dictionary<{edit: any, view: any}> = {};

export function registerWidget(type: number, editComponent: any, viewComponent: any = null, config: any = null) {
    widgets[type] = {
        edit: editComponent,
        view: viewComponent
    };
}

registerWidget(100, ListView);
