import React from "react";
import CodeMirror  from "react-codemirror";
import "codemirror/lib/codemirror.css";
import "codemirror/mode/xml/xml";
import "codemirror/mode/javascript/javascript";

export default class CodeMirrorBox extends React.Component<{mode: string, code: string, onChange: Function}> {

    updateCode = (newCode: string) => {
		this.props.onChange(newCode);
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