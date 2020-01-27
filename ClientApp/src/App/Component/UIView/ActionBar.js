import React from "react";
import PropTypes from "prop-types";
import { Row, Col, Button } from 'antd';
import _ from "lodash";
import { Link} from "react-router-dom";

export default class ActionBar extends React.Component {
    static propTypes= {
        commands: PropTypes.array,
        onCommandClick: PropTypes.func
    }

    onCommandClick(cmd) {
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
                        return this.renderCommand(c);
                    })
                }
            </div>
        </div>
        );
    }

    renderCommand(cmd) {
        return (<Button 
            key={cmd.ActionId} 
            style={{margin: "1px 10px"}} 
            onClick={() => { this.onCommandClick(cmd);} } 
            type="primary">
                {cmd.Title}
            </Button>);
    }
}