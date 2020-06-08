import React, { Component } from 'react';
import { Tabs, PageHeader } from 'antd';
import GridView from "../../../Component/ListView/GridView";
import TextArea from 'antd/lib/input/TextArea';
import _ from 'lodash';
import ActionLink from '../../../Component/ActionLink';

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

    render() {

        return (<div>
            
            {this.renderHeader()}

            <Tabs style={{ marginBottom: 32 }}>
                <TabPane tab="Fields" key="1">
                    {this.renderFieldList()}
                </TabPane>
                <TabPane tab="Layout" key="2">
                    <TextArea autoSize={{ minRows: 20, maxRows: 50 }} />
                </TabPane>
                <TabPane tab="List" key="3">
                    List
                </TabPane>
                <TabPane tab="Actions" key="4">
                    Action
                </TabPane>
            </Tabs>
        </div>);
    }
}