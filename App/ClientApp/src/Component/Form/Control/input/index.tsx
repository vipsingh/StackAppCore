import React, { SyntheticEvent } from "react";
import PropTypes from "prop-types";
import moment from "moment";
import { Input, InputNumber, Checkbox, DatePicker } from 'antd';

export { SelectBox } from "./SelectBox";
export { EntityPicker } from "./EntityPicker";

export class TextBox extends React.Component<WidgetInfoProps> {

    handleOnChange = (event: any) => {
        const textValue = event.target.value;
        const {onChange} = this.props;

        if (typeof onChange === "function") {            
            onChange(textValue);            
        }
    }

    render() {
        const {Value, IsReadOnly} = this.props;
        return(<Input 
            value={Value}
            disabled={IsReadOnly}
            onChange={this.handleOnChange}
        />);
    }
}

export class TextArea extends TextBox {

    render() {
        const {Value, IsReadOnly} = this.props;
        return(<Input.TextArea 
            value={Value}
            disabled={IsReadOnly}
            onChange={this.handleOnChange}
        />);
    }
}

export class NumberBox extends TextBox {

    handleOnChange = (val: any) => {
        const {onChange} = this.props;

        if (typeof onChange === "function") {            
            onChange(val);            
        }
    }

    render() {
        const {Value, IsReadOnly} = this.props;
        return(<InputNumber  
            value={Value}
            disabled={IsReadOnly}
            onChange={this.handleOnChange}
        />);
    }
}

export class DecimalBox extends TextBox {

    handleOnChange = (val: any) => {
        const {onChange} = this.props;

        if (typeof onChange === "function") {            
            onChange(val);            
        }
    }

    render() {
        const {Value, IsReadOnly} = this.props;
        return(<InputNumber  
            value={Value}
            disabled={IsReadOnly}
            onChange={this.handleOnChange}
            formatter={value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
            parser={value => value ? value.replace(/\$\s?|(,*)/g, ''): ""}
            style={{ width: "100%" }}
        />);
    }
}

export class CheckBox extends React.Component<WidgetInfoProps> {

    handleOnChange = (event: any) => {
        const v = event.target.checked;
        const {onChange} = this.props;

        if (typeof onChange === "function") {            
            onChange(v);
        }
    }

    render() {
        const {Value, IsReadOnly, IsViewMode} = this.props;

        let isDisabled = IsReadOnly;
        if (IsViewMode)
            isDisabled =true;

        return(<Checkbox
            checked={!!Value}
            disabled={isDisabled}
            onChange={this.handleOnChange}
          >
          </Checkbox>);
    }
}

export class DateBox extends React.Component<WidgetInfoProps> {
    handleOnChange = (dateObject: any) => {        
        const {onChange} = this.props;

        if (typeof onChange === "function") {            
            onChange(dateObject.format());            
        }
    }

    render() {
        const {Value, IsReadOnly} = this.props;
        let dateVal = undefined;
        if (Value) {
            dateVal = moment(Value);
        }
        
        return(<DatePicker
            value={dateVal}
            disabled={IsReadOnly}
            onChange={this.handleOnChange}
            allowClear={true}
            format={_AppSetting.DateFormat}
            style={{ width: "100%" }}
          />);
    }
}