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

        if (prop.Url) {
            this.context.router.history.push(prop.Url);
        }
    }

    render() {
        const prop = this.props;

        if (prop.DisplayType === 2) {
            return (
                <Button 
                    key={prop.ActionId}
                    style={{ margin: "1px 10px" }}
                    onClick={prop.Url ? undefined : this.onCommandClick}
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

        if (this.props.Url) {
            return <Link to={`/web/${prop.Url}`}>{title}</Link>;
        }

        return title;
    }
}