import React from "react";
import _ from "lodash";
import ActionButton from "../ActionLink";

export default class ActionBar extends React.Component<{
    commands: Array<any>,
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
            <div className="object-action-btn react-form-btns">
                <div className="object-action-inner">
                    {
                        _.map(commands, (c) => {
                            return (<ActionButton {...c} onClick={this.onCommandClick} />);
                        })
                    }
                </div>
            </div>
        );
    }
}