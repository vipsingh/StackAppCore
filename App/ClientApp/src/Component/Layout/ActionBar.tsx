import React from "react";
import _ from "lodash";
import { Space } from "antd";
import ActionButton from "../ActionLink";

export default class ActionBar extends React.Component<{
    commands?: IDictionary<any>,
    onCommandClick: Function
}> {

    onCommandClick = (cmd: any) => {
        if (this.props.onCommandClick) {
            this.props.onCommandClick(cmd);
        }
    }

    render() {
        const { commands } = this.props;

        return (
            <div className="object-action-btn">
                <Space>
                    {
                        _.map(commands, (c) => {
                            return (<ActionButton key={c.ActionId} {...c} DisplayType={2} onClick={this.onCommandClick} />);
                        })
                    }
                </Space>
            </div>
        );
    }
}