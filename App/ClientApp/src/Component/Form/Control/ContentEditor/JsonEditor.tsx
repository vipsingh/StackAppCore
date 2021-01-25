import React from "react";
import CodeMirror from "../../../Widget/CodeMirror";

export default class JsonEditor extends React.Component<WidgetInfoProps> {
    render() {
        const { Value, onChange } = this.props;

        return (<CodeMirror mode="javascript" code={Value} onChange={onChange} />);
    }
}