import React from "react";
import {UnControlled as CodeMirror}  from "react-codemirror2";
import "codemirror/lib/codemirror.css";
import "codemirror/mode/xml/xml";
import "codemirror/mode/javascript/javascript";

export default class CodeMirrorBox extends React.Component<{mode: string, code: string, onChange: Function}> {

    updateCode = (editor: any, data: any, value: string) => {
		this.props.onChange(value);
    }
    
    render() {
        const { mode, code } = this.props;
        const options = {
            lineNumbers: true,
            mode
		};        

        return <CodeMirror value={code} onChange={this.updateCode} options={options} />;
    }
}