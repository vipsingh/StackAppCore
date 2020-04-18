import React from "react";
import { Form } from "antd";
import cs from "classnames";

const FormItem = Form.Item;

export default class FormField extends React.Component<WidgetInfoProps> {    

    shouldComponentUpdate(nextProps: any) {
      if (this.props.WidgetType === 100) {
        return true;
      }
      
      return nextProps.Value !== this.props.Value || nextProps.HasError !== this.props.HasError;
    }

    render() {        
        const { WidgetId, Caption, HasError, api } = this.props;
        
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

        if(!Caption) {
          formItemLayout.wrapperCol.sm.span = 24;
        }

        let validResult: any;
        if (HasError) {
          validResult = api.getErrorResult(WidgetId)
        }

        return (
            <FormItem
            {...formItemLayout}            
            label={Caption}        
            >
            <div className={cs("form-field-control", {"ant-form-item-has-error": HasError})}>
              {this.props.children}
              { !HasError || <div className="ant-form-item-explain"><div>{validResult.Message}</div></div>}
            </div>
        </FormItem>
        );
    }    
}