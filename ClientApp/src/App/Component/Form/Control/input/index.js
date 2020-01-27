import React from "react";
import PropTypes from "prop-types";
import moment from "moment";
import { Input, InputNumber, Checkbox, DatePicker } from 'antd';

export { SelectBox } from "./SelectBox";
export { EntityPicker } from "./EntityPicker";

export class TextBox extends React.Component {
    static propTypes = {
        onChange: PropTypes.func,
        Value: PropTypes.any,
        ControlId: PropTypes.string,
        Disabled: PropTypes.bool,
        ListValues: PropTypes.array
    }

    handleOnChange = (event) => {
        const textValue = event.target.value;
        const {onChange} = this.props;
        const {Value, ControlId} = this.props;

        if (typeof onChange === "function") {            
            onChange(textValue);            
        }
    }

    render() {
        const {Value, Disabled, ControlId} = this.props;
        return(<Input 
            value={Value}
            disabled={Disabled}
            onChange={this.handleOnChange}
        />);
    }
}

export class TextArea extends TextBox {

    render() {
        const {Value, Disabled, ControlId} = this.props;
        return(<Input.TextArea 
            value={Value}
            disabled={Disabled}
            onChange={this.handleOnChange}
        />);
    }
}

export class NumberBox extends TextBox {

    handleOnChange = (val) => {
        const {onChange} = this.props;
        const {Value, ControlId} = this.props;

        if (typeof onChange === "function") {            
            onChange(val);            
        }
    }

    render() {
        const {Value, Disabled, ControlId} = this.props;
        return(<InputNumber  
            value={Value}
            disabled={Disabled}
            onChange={this.handleOnChange}
        />);
    }
}

export class DecimalBox extends TextBox {

    handleOnChange = (val) => {
        const {onChange} = this.props;
        const {Value, ControlId} = this.props;

        if (typeof onChange === "function") {            
            onChange(val);            
        }
    }

    render() {
        const {Value, Disabled, ControlId} = this.props;
        return(<InputNumber  
            value={Value}
            disabled={Disabled}
            onChange={this.handleOnChange}
            formatter={value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
            parser={value => value.replace(/\$\s?|(,*)/g, '')}
        />);
    }
}

export class CheckBox extends TextBox {

    render() {
        const {Value, Disabled, ControlId, IsViewMode} = this.props;

        let isDisabled = Disabled;
        if (IsViewMode)
            isDisabled =true;

        return(<Checkbox
            checked={!!value}
            disabled={isDisabled}
            onChange={this.handleOnChange}
          >
          </Checkbox>);
    }
}

export class DateBox extends React.Component {
    static propTypes = {
        onChange: PropTypes.func,
        Value: PropTypes.any,
        ControlId: PropTypes.string,
        Disabled: PropTypes.bool        
    }

    handleOnChange = (dateObject) => {        
        const {onChange} = this.props;
        const {Value, ControlId} = this.props;

        if (typeof onChange === "function") {            
            onChange(dateObject.format());            
        }
    }

    render() {
        const {Value, Disabled, ControlId} = this.props;
        let dateVal = moment();
        if (Value) {
            dateVal = moment(Value);
        }
        
        return(<DatePicker
            value={dateVal}
            disabled={Disabled}
            onChange={this.handleOnChange}
            allowClear={true}
            format={_AppSetting.DateFormat}
            style={{ width: "100%" }}
          />);
    }
}