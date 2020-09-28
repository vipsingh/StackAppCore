import React from "react";
import CodeMirror from "../../../Widget/CodeMirror";

export default class XmlEditor extends React.Component<WidgetInfoProps> {
    render() {
        const { Value, onChange } = this.props;

        return (<CodeMirror mode="xml" code={Value} onChange={onChange} />);
    }
}