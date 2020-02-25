import React from "react";
import { Form } from "antd";

const FormItem = Form.Item;

export default class FormField extends React.Component<WidgetInfoProps> {    

    shouldComponentUpdate(nextProps: any) {
      if (this.props.WidgetType === 100) {
        return true;
      }
      
      return nextProps.Value !== this.props.Value || nextProps.ValidationResult !== this.props.ValidationResult;
    }

    render() {        
        const label = this.props.Caption;
        
        const formItemLayout = {
            labelCol: {
              xs: { span: 24 },
              sm: { span: 8 },
            },
            wrapperCol: {
              xs: { span: 24 },
              sm: { span: 16 },
            },
          };

        if(!label) {
          formItemLayout.wrapperCol.sm.span = 24;
          }  

        return (
            <FormItem
            {...formItemLayout}
            label={label}
            >
            {this.props.children}
        </FormItem>
        );
    }    
}