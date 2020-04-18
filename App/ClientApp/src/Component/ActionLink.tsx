import React from "react";
import PropTypes from "prop-types";
import { Button } from "antd";
import { Link } from "react-router-dom";

export default class ActionLink extends React.Component<ActionInfo> {
    static contextTypes = {
        router: PropTypes.object
    }
    
    onCommandClick = () => {
        const prop = this.props;
        if (prop.onClick) {
            prop.onClick(prop);

            return;
        }
    }

    render() {
        const prop = this.props;

        if (prop.DisplayType === 2) {
            return (
                <Button 
                    key={prop.ActionId}
                    style={{ margin: "1px 10px" }}
                    onClick={prop.ExecutionType === 4 ? undefined : this.onCommandClick}
                    type="primary">
                        {
                            this.renderLink()
                        }
                </Button>
            );
        } else {
            return this.renderLink();
        }
    }

    renderLink() {
        const prop = this.props;
        const title = prop.Title || prop.ActionId;

        if (prop.ExecutionType === 4 && this.props.Url) {
            let u = `/web/${prop.Url}`;
            u = u.replace("//", "/");

            return <Link to={u}>{title}</Link>;
        }

        return title;
    }
}