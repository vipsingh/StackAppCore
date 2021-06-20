const _components: IDictionary<{edit: any, view: any, config: any}> = {};
let _BlockElement: any, _LabelComponent: any, _HiddenField: any, _FormFieldItem: any;

export function registerComponent(type: number, editComponent: any, viewComponent: any = null, config: any = null) {
    _components[type] = {
        edit: editComponent,
        view: viewComponent? viewComponent : editComponent,
        config
    };
}

export function registerComponentByCallback(callback: Function) {
    //callback.call(null, registerComponent)
}

export function getComponent(controlType: number, isViewMode?: boolean) {
    let editComponent = null;
    let viewComponent: any = _LabelComponent;

    const w = _components[controlType];
    if (w) {
        editComponent = w.edit;
        viewComponent = w.view;
    }

    if(isViewMode) {
        return viewComponent;
    }
    return editComponent;
}

export function getComponentConfig(controlType: number) {
    const w = _components[controlType];

    if (w) return w.config;

    return null;
}

export function registerBasicComponent(BlockElement: any, LabelComponent: any, HiddenField: any, FormFieldItem: any) {
    _LabelComponent = LabelComponent;
    _BlockElement = BlockElement;
    _HiddenField = HiddenField;
    _FormFieldItem = FormFieldItem;
}

export function HiddenField() {
    return _HiddenField;
};
export function BlockElement() {
    return _BlockElement;
};
export function FormFieldItem() {
    return _FormFieldItem;
};