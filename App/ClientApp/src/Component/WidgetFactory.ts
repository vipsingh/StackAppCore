import React from "react";
import { registerComponent, registerBasicComponent } from "../Core/ComponentFactory";
import { getAsyncComponent } from "./Helper/asyncImport";

import { FormFieldComponent } from "./Form/FormField";

import * as CTRL from "./Form/Control/input";
import * as HField from "./Form/Control/HiddenField";
import LabelField from "./Form/Control/LabelField";
import ListView from "./ListView";
import { FormControlType } from "../Constant";
import FilterBox from "./Form/Control/FilterBox";
import ListForm from "./Form/Control/ListForm";
import HtmlTextBox from "./Form/Control/ContentEditor/HtmlTextBox";

import XmlEditor from "./Form/Control/ContentEditor/XmlEditor";
//const XmlEditor = getAsyncComponent(() => import("./Form/Control/ContentEditor/XmlEditor"));
import JsonEditor from "./Form/Control/ContentEditor/JsonEditor";
import StackScriptEditor from "./Form/Control/ContentEditor/StackScriptEditor";


const HiddenField = HField.default;
const BlockElement = (props: any) => { return React.createElement("div", props) }

export function RegisterAllComponents() {
    registerBasicComponent(BlockElement, LabelField, HiddenField, FormFieldComponent);

    registerComponent(FormControlType.TextBox, CTRL.TextBox);
    registerComponent(FormControlType.DatePicker, CTRL.DateBox);
    registerComponent(FormControlType.DecimalBox, CTRL.DecimalBox);
    registerComponent(FormControlType.CheckBox, CTRL.CheckBox, CTRL.CheckBox);
    registerComponent(FormControlType.NumberBox, CTRL.NumberBox);
    registerComponent(FormControlType.Dropdown, CTRL.SelectBox);
    registerComponent(FormControlType.EntityPicker, CTRL.EntityPicker);
    registerComponent(FormControlType.LongText, CTRL.TextArea);
    registerComponent(FormControlType.Image, CTRL.ImageField);
    registerComponent(FormControlType.HtmlText, HtmlTextBox);
    registerComponent(FormControlType.XmlEditor, XmlEditor);
    registerComponent(FormControlType.JsonEditor, JsonEditor);
    registerComponent(FormControlType.StackScriptEditor, StackScriptEditor);

    registerComponent(99, ListForm);
    registerComponent(100, ListView);
    registerComponent(101, FilterBox);
}