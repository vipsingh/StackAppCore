import React from "react";
import _ from "lodash";
import { Row, Col, Tabs } from "antd";
import { UIField } from "./UIView";

export const LayoutView: React.FC<any> = ({ layout, getControl }) => {
    
    return getBlock(layout, getControl);
}

function getBlock(node: any, getControl: Function): any {
    if (_HtmlComponents[node.tagName]) {
        return React.createElement(_HtmlComponents[node.tagName].component, { className: node.class, style: node.style, ...node.attrs }, node.text ? node.text: getBlocks(node.components, getControl));
    } else if (node.tagName === "widget") {
        return React.createElement(UIField, { getControl, FieldId: node.id, ...node.attrs }, null);
    } else if (isHtmlElement(node.tagName)) {
        return React.createElement(node.tagName, { className: node.class, style: node.style, ...node.attrs }, node.text ? node.text: getBlocks(node.components, getControl));
    }
}

function getBlocks(nodes: Array<any> | undefined, getControl: Function): any {
    if (nodes) {
        return _.map(nodes, (n) => {
            return getBlock(n, getControl);
        });
    }

    return null;
}

function isHtmlElement(tagName: string): boolean {
    const tags = ["div", "header", "br", "table", "th", "tr", "td", "label", "span", "h1", "h2", "h3", "h4", "h5", "i", "b", "a", "img", "form", "input", "select", "ul", "li"]

    return tags.indexOf(tagName.toLowerCase()) >= 0;
}

const _HtmlComponents: IDictionary<{component: any}> = {};

_HtmlComponents["row"] = { component: Row };
_HtmlComponents["col"] = { component: Col };
_HtmlComponents["tabs"] = { component: Tabs };
_HtmlComponents["tabpane"] = { component: Tabs.TabPane };
//_HtmlComponents["card"] = { component: Col };
//_HtmlComponents["list"] = { component: Col };