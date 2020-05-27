import React from "react";
import { Checkbox } from 'antd';

export { SelectBox } from "./SelectBox";
export { EntityPicker } from "./EntityPicker";
export { TextBox, TextArea } from "./TextBox";
export { DecimalBox, NumberBox } from "./NumberBox";
export { DateBox } from "./DateBox";
export { ImageField } from "./ImageField";

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