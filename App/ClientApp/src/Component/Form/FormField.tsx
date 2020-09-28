import React from "react";
import { Form } from "antd";
import cs from "classnames";
import { FormLabelAlign } from "antd/lib/form/interface";

const FormItem = Form.Item;

export default class FormField extends React.Component<{widgetInfo: WidgetInfoProps, dataModel: any, api?: FormApi, otherProps?: any}> {    

    shouldComponentUpdate(nextProps: any) {
      if (this.props.widgetInfo.WidgetType === 100) {
        return true;
      }
      
      return nextProps.widgetInfo !== this.props.widgetInfo || nextProps.dataModel !== this.props.dataModel;
    }

    render() {        
        const { WidgetId, Caption, IsHidden, CaptionPosition } = this.props.widgetInfo;
        const { HasError, Invisible } = this.props.dataModel;
        
        const formItemLayout = {
            labelCol: {
              xs: { span: 24 },
              sm: { span: 8 },
            },
            wrapperCol: {
              xs: { span: 24 },
              sm: { span: 16 },
            }
          };

        if(!Caption || CaptionPosition === 1) {
          formItemLayout.wrapperCol.sm.span = 24;
        }

        let labelAlign: FormLabelAlign = "right";
        if (CaptionPosition === 1) {
          labelAlign = "left";
        }

        let validResult: any;
        if (this.props.api && HasError) {
          validResult = this.props.api.getErrorResult(WidgetId);
        }
        if (IsHidden || Invisible) {
          return <div>{this.props.children}</div>;
        }

        return (
            <FormItem
            {...formItemLayout}  
            labelAlign={labelAlign}          
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