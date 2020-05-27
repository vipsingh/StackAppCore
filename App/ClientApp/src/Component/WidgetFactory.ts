import * as CTRL from "./Form/Control/input";
import LabelField from "./Form/Control/LabelField";
import ListView from "./ListView";
import { Dictionary } from "lodash";
import { FormControlType } from "../Constant";
import FilterBox from "./Form/Control/FilterBox";


export function getComponent(controlType: number, isViewMode?: boolean) {
    let editComponent = null;
    let viewComponent: any = LabelField;
    switch(controlType) {
        case FormControlType.TextBox:
            editComponent = CTRL.TextBox;
            break;
        case FormControlType.DatePicker:
            editComponent = CTRL.DateBox;
            break;
        case FormControlType.DecimalBox:
            editComponent = CTRL.DecimalBox;
            break;
        case FormControlType.CheckBox:
            editComponent = viewComponent = CTRL.CheckBox;
            break;
        case FormControlType.NumberBox:
            editComponent = CTRL.NumberBox;
            break;
        case FormControlType.Dropdown:
            editComponent = CTRL.SelectBox;
            break;
        case FormControlType.EntityPicker:
            editComponent = CTRL.EntityPicker;
            break;
        case FormControlType.LongText:
            editComponent = CTRL.TextArea;
            break;
        case FormControlType.Label:
            editComponent = LabelField;
            break;
        case FormControlType.Image:
            editComponent = viewComponent =  CTRL.ImageField;
            break;
        default:
            const w = widgets[controlType];
            if (w) {
                editComponent = w.edit;
                viewComponent = w.view;
            }
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
        view: viewComponent? viewComponent : editComponent
    };
}

registerWidget(100, ListView);
registerWidget(101, FilterBox);
