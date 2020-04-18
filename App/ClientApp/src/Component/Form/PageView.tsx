import React from "react";
import _ from "lodash";
import UIView from "../Layout";
import ActionBar from "../Layout/ActionBar";

export default class PageView extends React.Component<{
    getControl: Function,
    Schema: any,
    executeAction: Function
}> {
 
    renderTemplate() {
        const {getControl, Schema} = this.props;
        const { Layout } = Schema;
        
        return (<div className="page-view">
            <UIView template={Layout} getControl={getControl} />
            {this.renderActions()}
            </div>
        );
    }

    renderActions = () => {
        const { Schema: { Actions }, executeAction } = this.props;
        
        return <ActionBar commands={Actions} onCommandClick={executeAction} />
    }
    
    render() {
        return (
            <div>
                {this.renderTemplate()}
            </div>
        );
    }
}