import React from "react";
import UIView from "../Layout";
import { PageSheet } from "../Layout/UIView";
//import { PlusCircleOutlined } from '@ant-design/icons';
import { PageHeader } from "antd";

export default class PageView extends React.Component<{
    getControl: Function,
    entityModel: any,
    getFormActions: Function
}> {
 
    renderTemplate() {
        const {getControl, entityModel: { Layout, PageTitle }} = this.props;        
        
        return (<div className="page-view">
                    {
                        this.renderHeader(Layout, PageTitle)
                    }
                    <UIView template={Layout} getControl={getControl} />
            </div>
        );
    }

    renderHeader(layout: any, pageTitle: any) {                
        const { getFormActions, getControl } = this.props;

        return (<div className="paper mb-g">
        <PageHeader
            ghost={false}
            title={pageTitle.PageTitle}
            // subTitle="user x"
            // tags={<Tag color="blue">Running</Tag>}
            extra={getFormActions()}
            avatar={{ src: 'https://avatars1.githubusercontent.com/u/8186664?s=460&v=4' }}
        >            
                {!layout.Header || <PageSheet getControl={getControl} sheet={layout.Header} key={"header_0"} />}
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