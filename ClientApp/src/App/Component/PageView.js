import React from "react";
import PropTypes from "prop-types";
import _ from "lodash";
import UIView from "./UIView";

export default class PageView extends React.Component {
    static propTypes = {
        getControl: PropTypes.func,
        Schema: PropTypes.object
    }

    constructor(props) {
        super(props);
    }

    renderTemplate() {
        const {getControl, Schema} = this.props;
        const { ViewTemplate } = Schema;
        
        return (
            <UIView viewTemplate={ViewTemplate} getControl={getControl} />
        );
    }

    
    render() {
        return (
            <div>
                {this.renderTemplate()}
            </div>
        );
    }
}