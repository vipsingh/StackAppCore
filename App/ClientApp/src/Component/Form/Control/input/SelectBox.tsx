import React from "react";
import { Select } from 'antd';
import _ from "lodash";

const Option = Select.Option;

export class SelectBox extends React.Component<{
    onChange: Function,
    Value: any,
    WidgetId: string,
    Disabled: boolean,
    ListValues: Array<any>,
    IsViewMode: boolean
}> {
 
    handleOnChange = (value: any) => {
        const { onChange, ListValues } = this.props;        

        if (typeof onChange === "function") {            
            const v = _.find(ListValues, l => { return l.Value.toString() === value.toString(); });
            onChange(v);
        }
    }

    renderOptions() {
        
    }

    render() {
        const {Value, Disabled, ListValues} = this.props;
        
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