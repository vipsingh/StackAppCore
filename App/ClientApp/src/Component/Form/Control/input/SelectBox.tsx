import React from "react";
import { Select } from 'antd';
import _ from "lodash";

const Option = Select.Option;

export class SelectBox extends React.Component<WidgetInfoProps> {
 
    handleOnChange = (value: any) => {
        const { onChange, Options } = this.props;        

        if (typeof onChange === "function") {            
            const v = _.find(Options, l => { return l.Value.toString() === value.toString(); });
            onChange(v);
        }
    }

    renderOptions() {
        
    }

    render() {
        const {Value, IsReadOnly, Options} = this.props;
        
        const v = Value && typeof Value === "object" ? Value.Value : Value;
        
        return(<Select 
            defaultValue={v}
            disabled={IsReadOnly}
            onChange={this.handleOnChange}
            style={{ width: "100%" }}
        >
            {Options.map(op => <Option key={op.Value} value={op.Value}>{op.Text}</Option>)}
        </Select>);
    }
}