import React from "react";
const widgets = {};

export function registerWidget(type, editComponent, viewComponent, config) {
    widgets[type] = {
        edit: editComponent,
        view: viewComponent
    };
}

export function getWidgetComponent(type, isViewMode) {
    const w = widgets[type];
    if (w) {
        return w.edit;
    }
    return null;
}

import EntityList from "./List/EntityList";

registerWidget(100, EntityList);
