import React from "react";
import UIView, { PageSheet } from "../Layout";
//import { PlusCircleOutlined } from '@ant-design/icons';
import { PageHeader } from "antd";

export default class PageView extends React.Component<{
    getControl: Function,
    entityModel: any,
    renderActions: Function,
    getFormActions: Function
}> {
 
    renderTemplate() {
        const {getControl, entityModel: { Layout }} = this.props;        
        
        return (<div className="page-view">
                    {
                        this.renderHeader(Layout)
                    }
                    <UIView template={Layout} getControl={getControl} />
            </div>
        );
    }

    renderHeader(layout: any) {
        if(!layout.Header) return;
        
        const { getFormActions, getControl } = this.props;

        return (<div className="paper mb-g">
        <PageHeader
            ghost={false}
            title="User Master"
            subTitle="user x"
            // tags={<Tag color="blue">Running</Tag>}
            extra={getFormActions()}
            avatar={{ src: 'https://avatars1.githubusercontent.com/u/8186664?s=460&v=4' }}
        >            
                <PageSheet getControl={getControl} sheet={layout.Header} key={"header_0"} />
        </PageHeader>
        </div>);

    }    
    
    render() {
        return (
            <div>
                {this.renderTemplate()}
            </div>
        );
    }
}