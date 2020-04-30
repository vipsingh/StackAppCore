import React from "react";
import moment from "moment";
import { DatePicker } from 'antd';

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