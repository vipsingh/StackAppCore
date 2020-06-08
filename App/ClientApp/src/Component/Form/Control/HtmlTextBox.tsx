import React from "react";
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';


export default class HtmlTextBox extends React.Component<WidgetInfoProps> {

    handleOnChange = (content:string) => {
        const {onChange} = this.props;

        if (typeof onChange === "function") {            
            onChange(content);            
        }
    }

    handleOnBlur = () => {
        const {onBlur} = this.props;

        if (onBlur && typeof onBlur === "function") {            
            onBlur();            
        }
    }

    renderView() {
        const {Value} = this.props;

        return (<div dangerouslySetInnerHTML={{__html: Value}}></div>);
    }

    render() {
        const {Value, IsViewMode, IsReadOnly} = this.props;
        
        if (IsViewMode) return this.renderView();

        return(<ReactQuill 
            theme="snow" 
            value={Value} 
            onChange={this.handleOnChange}/>);
    }
}
