import React from "react";
import { Typography } from 'antd';

const { Text } = Typography;

export default class LabelField extends React.Component<WidgetInfoProps> {

    getFormattedValue() {
        const { Value } = this.props;
        if (!Value) return "";

        if(typeof Value === "object") {
            if (Value.Text){
                return Value.Text;
            }
        }

        return Value.toString();
    }

    render() {
        const v = this.getFormattedValue();

        return (<Text>{v}</Text>);
    }
}