import React from "react";
import PropTypes from "prop-types";
import { Form } from "antd";

const FormItem = Form.Item;

export default class FieldWrapper extends React.Component {
    static propTypes = {
        Value: PropTypes.any,
        ValidationResult: PropTypes.object,
        IsViewMode: PropTypes.bool,
        Label: PropTypes.string,
        children: PropTypes.any
    }

    shouldComponentUpdate(nextProps) {
        return nextProps.Value !== this.props.Value || nextProps.ValidationResult != this.props.ValidationResult;
    }

    render() {
        const {IsViewMode} = this.props;
        const label = this.props.Label;
        
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