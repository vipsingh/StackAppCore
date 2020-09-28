import React from "react";
import { Input } from 'antd';


// export const TextBox: React.FC<WidgetInfoProps> =  ({ Value, IsReadOnly, onChange, onBlur }) => {    

//     const handleOnChange = (event: any) => {
//         const textValue = event.target.value;        

//         if (typeof onChange === "function") {            
//             onChange(textValue);            
//         }
//     }

//     const handleOnBlur = (e: any) => {        
//         if (onBlur && typeof onBlur === "function") {            
//             onBlur();            
//         }
//     }
    
//     return(<Input 
//             value={Value}
//             disabled={IsReadOnly}
//             onChange={handleOnChange}
//             onBlur={handleOnBlur}
//     />);    
// }

export class TextBox extends React.Component<WidgetInfoProps> {

    handleOnChange = (event: any) => {
        const textValue = event.target.value;
        const {onChange} = this.props;

        if (typeof onChange === "function") {            
            onChange(textValue);            
        }
    }

    handleOnBlur = (e: any) => {      
        const {onBlur} = this.props;  
        if (onBlur && typeof onBlur === "function") {            
            onBlur();            
        }
    }

    render() {
        const {Value, IsReadOnly} = this.props;
        return(<Input 
            value={Value}
            disabled={IsReadOnly}
            onChange={this.handleOnChange}
            onBlur={this.handleOnBlur}
        />);
    }
}

export class TextArea extends TextBox {

    render() {
        const {Value, IsReadOnly} = this.props;

        return(<Input.TextArea 
            value={Value}
            disabled={IsReadOnly}
            autoSize={{ minRows: 3, maxRows: 6 }}
            onChange={this.handleOnChange}
            onBlur={this.handleOnBlur}
        />);
    }
}