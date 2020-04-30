import React from "react";
import { Form } from "antd";
import cs from "classnames";

const FormItem = Form.Item;

export default class FormField extends React.Component<WidgetInfoProps> {    

    shouldComponentUpdate(nextProps: any) {
      if (this.props.WidgetType === 100) {
        return true;
      }
      
      return nextProps.Value !== this.props.Value || nextProps.HasError !== this.props.HasError || nextProps.Invisible !== this.props.Invisible || nextProps.VisibleOptions !== this.props.VisibleOptions;
    }

    render() {        
        const { WidgetId, Caption, HasError, api, Invisible, IsHidden } = this.props;
        
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
          validResult = api.getErrorResult(WidgetId);
        }
        if (IsHidden || Invisible) {
          return <div>{this.props.children}</div>;
        }

        return (
            <FormItem
            {...formItemLayout}            
            label={(IsHidden || Invisible) ? "" : Caption}        
            >
            <div className={cs("form-field-control", {"ant-form-item-has-error": HasError})}>
              {this.props.children}
              { !HasError || <div className="ant-form-item-explain"><div>{validResult.Message}</div></div>}
            </div>
        </FormItem>
        );
    }    
}