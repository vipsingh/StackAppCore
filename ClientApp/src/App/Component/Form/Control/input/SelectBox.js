import React from "react";
import PropTypes from "prop-types";
import { Select } from 'antd';

const Option = Select.Option;

export class SelectBox extends React.Component {
    static propTypes = {
        onChange: PropTypes.func,
        Value: PropTypes.any,
        ControlId: PropTypes.string,
        Disabled: PropTypes.bool,
        ListValues: PropTypes.array
    }

    handleOnChange = (value) => {
        const { onChange, ListValues } = this.props;        

        if (typeof onChange === "function") {            
            const v = _.find(ListValues, l => { return l.Value.toString() === value.toString(); });
            onChange(v);
        }
    }

    renderOptions() {
        
    }

    render() {
        const {Value, Disabled, ControlId, ListValues} = this.props;
        
        const v = typeof Value === "object" ? Value.Value : Value;
        return(<Select 
            value={v}
            disabled={Disabled}
            onChange={this.handleOnChange}
            style={{ width: "100%" }}
        >
        {ListValues.map(op => <Option key={op.Value}>{op.Text}</Option>)}
        </Select>);
    }
}