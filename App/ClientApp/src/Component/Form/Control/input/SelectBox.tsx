import React from "react";
import { Select } from 'antd';
import _ from "lodash";

const Option = Select.Option;

export class SelectBox extends React.Component<WidgetInfoProps> {
 
    handleOnChange = (value: any) => {
        const { onChange, Options, IsMultiSelect } = this.props;        

        if (typeof onChange === "function") {     
            if (IsMultiSelect) {
                const v = _.filter(Options, l => { return value.indexOf(l.Value) >= 0; });
                onChange(v);
            } else {    
                const v = _.find(Options, l => { return l.Value.toString() === value.toString(); });
                onChange(v);
            }
        }
    }

    renderOptions() {
        
    }

    getOptions() {
        const {Options, VisibleOptions} = this.props;
        if (VisibleOptions && VisibleOptions.length === 0) return Options;

        return VisibleOptions ? _.filter(Options, x => VisibleOptions.indexOf(x.Value) >= 0) : Options;
    }

    render() {
        const {Value, IsReadOnly, IsMultiSelect, onFocus} = this.props;
        const opts = this.getOptions();
        let v: any;
        if (IsMultiSelect) {
            if (Value && _.isArray(Value)) {
                v = _.map(Value, x => typeof x === "object" ? x.Value : x);
            } else
                v = [];
        } else {
           v = Value && typeof Value === "object" ? Value.Value : Value;
        }
        
        return(<Select 
            defaultValue={v}
            disabled={IsReadOnly}
            onChange={this.handleOnChange}
            style={{ width: "100%" }}
            value={v}
            mode={ IsMultiSelect?  "multiple" : undefined }
            onFocus={onFocus}
        >
            {!opts || opts.map(op => <Option key={op.Value} value={op.Value}>{op.Text}</Option>)}
        </Select>);
    }
}