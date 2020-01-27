import React from "react";
import PropTypes from "prop-types"; 
import { Typography } from 'antd';

const { Text } = Typography;

export default class LabelField extends React.Component {
    static propTypes = {
        Value: PropTypes.any
    }

    constructor(props) {
        super(props);
    }

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