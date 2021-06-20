import React, { Component, useCallback } from 'react';
import { Tabs, PageHeader } from 'antd';
import GridView from "../../Component/ListView/GridView";
import _ from 'lodash';
import ActionLink from '../../Component/ActionLink';
import { cellRenderer } from "../../Component/ListView/Helper";
import LinkProcesser from '../../Core/Utils/LinkProcesser';

const { TabPane } = Tabs;

export default class EntityDesk extends Component<{ data: any }, {
    IsLoading: boolean,
    Schema: any,
    entityName: string
}> {

    constructor(props: any) {
        super(props);

        const { match } = props;
        this.state = {
            IsLoading: false,
            Schema: props.data,
            entityName: match.params.id
        };
    }

    componentDidMount() {
        this.fetchPage();
    }

    fetchPage() {
        const url = `${window._AppSetting.AppRoot}Studio/Studio/${this.state.entityName}`;

        window._App.Request.getData({
            url: url,
            type: "GET",
        })
            .then((data: any) => {
                this.setState({ Schema: data });
            })
            .catch((err: any) => {
                _Debug.error(`PageLoad: ${err}`);
                this.setState({ IsLoading: false });
            });
    }

    renderFieldList() {
        const { Widgets } = this.state.Schema;
       // const link = { Url: `/studio/GetFieldList?${CurrentQuery}` };
        const feldListWIdget = Widgets.FieldList;

        return (<GridView
            IsLocalStore={true}
            DataActionLink={feldListWIdget.DataActionLink}
            onChange={() => { }}
            WidgetId="FieldList"
            WidgetType={100}
            formatCell={this.formatListCell}
            renderAction={this.renderAction}
            />);
    }

    formatListCell = (col: any, val: any, row: any) => {
        //if (col.WidgetId === "name" && val.AdditionalValue) {
        //    const { ViewLink } = val.AdditionalValue;
        //    if (ViewLink) val.AdditionalValue.ViewLink = `${window._AppSetting.AppRoot}entitystudio/entity/${row.name.FormatedValue}`;
        //}

        return cellRenderer(col, val);
    }

    renderAction = (action: any) => {

        return <ActionLink {...action} LinkTarget="POPUP" overrideProps={{ handeleSubmitResponse: this.handeleSubmitResponse }} />;
    }

    renderHeader() {
        const { Actions, PageTitle } = this.state.Schema;

        return (<div className="paper mb-g">
            <PageHeader
                ghost={false}
                onBack={() => window.history.back()}
                title={PageTitle.Title}
                extra={_.map(Actions, (c) => {
                    return <ActionLink key={c.ActionId} {...c} DisplayType={2} />;
                })}
            >
            </PageHeader>
        </div>);

    }

    handeleSubmitResponse = (response: any, navigator: any) => {
        const { Status } = response;

        if (Status === 0) {
            window._App.Notify.success("Data Saved 1");
            if (navigator.opener) {
                navigator.close();
                navigator.opener.reload();
            }
            //if (gridRef && gridRef.current) (gridRef.current as any).loadData();
        } else {
            LinkProcesser.handeleResponse(response, navigator);
        }
    }

    render() {
        const { Schema } = this.state;

        if (!Schema) return <div>error</div>;

        return <div>
            {this.renderHeader()}
            <Tabs style={{ marginBottom: 32 }}>
                <TabPane tab="Fields" key="1">
                    {this.renderFieldList()}
                </TabPane>
                <TabPane tab="Views" key="2">
                    
                </TabPane>
                <TabPane tab="Data" key="3">
                    
                </TabPane>
            </Tabs>
        </div>;
    }
}