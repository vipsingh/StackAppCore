import React, { Component } from 'react';
import { Tabs, PageHeader } from 'antd';
import GridView from "../../../Component/ListView/GridView";
import _ from 'lodash';
import ActionLink from '../../../Component/ActionLink';
import EntityForm from '../../../Component/Form/EntityForm';

const { TabPane } = Tabs;

export default class EntityPage extends Component<{ data: any }, {
  IsLoading: boolean,
  Schema: any
}> {

    constructor(props: any) {
        super(props);
    
        this.state = {
          IsLoading: false,
          Schema: props.data
        };
      }
     
    componentWillReceiveProps(nextProps: any) {
        this.setState({ Schema: nextProps.data });
    } 

    renderFieldList() {
        const { CurrentQuery } = this.state.Schema;
        const link = { Url: `/studio/GetFieldList?${CurrentQuery}` };

        return (<GridView 
                IsLocalStore={true} 
                DataActionLink={link}
                onChange={()=>{}} 
                WidgetId="FiledList" 
                WidgetType={100} />);
    }

    executeAction = () => {

    }

    renderHeader() {                
        const { Actions } = this.state.Schema;

        return (<div className="paper mb-g">
            <PageHeader
                ghost={false}
                onBack={() => window.history.back()}
                title={"Entity"}
                extra={_.map(Actions, (c) => {
                    return <ActionLink key={c.ActionId} {...c} DisplayType={2} onClick={this.executeAction} />;
                    }) }                
            >                            
            </PageHeader>
        </div>);

    }

    renderLayoutList() {

    }

    renderPage = (props: any) => {
        return (<div>
                    
            {this.renderHeader()}

            <Tabs style={{ marginBottom: 32 }}>
                <TabPane tab="Fields" key="1">
                    {this.renderFieldList()}
                </TabPane>
                <TabPane tab="Layout" key="2">
                    {props.getControl("LayoutList")}
                </TabPane>
                <TabPane tab="List" key="3">
                    {props.getControl("EntityListList")}
                </TabPane>
                <TabPane tab="Actions" key="4">
                    {props.getControl("EntityActionList")}
                </TabPane>
            </Tabs>
        </div>);
    }

    render() {

        return (<EntityForm
            Schema={this.state.Schema}
            FormData={null}
            render={
                this.renderPage
            }
         />   );        
    }
}