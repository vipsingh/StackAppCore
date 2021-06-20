import React, { useEffect, useState, useRef, useCallback } from 'react';
import _ from 'lodash';
import { Tabs, PageHeader } from 'antd';
import GridView from "../../Component/ListView/GridView";
import { cellRenderer } from "../../Component/ListView/Helper";
import ActionLink from '../../Component/ActionLink';
import LinkProcesser from '../../Core/Utils/LinkProcesser';

export default function EntityList() {
    const [state, setState] = useState<any>({ isLoading: false });
    const gridRef = useRef(null);

    useEffect(() => {
        const url = `${window._AppSetting.AppRoot}Studio/EntityList`;

        window._App.Request.getData({
            url: url,
            type: "GET",
        })
            .then((data: any) => {
                //const { Widgets } = data;
                //const feldListWIdget = Widgets.LayoutList;

                setState({ schema: data });
            })
            .catch((err: any) => {
                setState({ isLoading: false });
            });
    }, []);

    const handeleSubmitResponse = React.useCallback((response, navigator) => {
        const { Status } = response;

        if (Status === 0) {
            window._App.Notify.success("Data Saved");
            if (gridRef && gridRef.current) (gridRef.current as any).loadData();
        } else {
            LinkProcesser.handeleResponse(response, navigator);
        }
    }, []);

    const formatListCell = useCallback((col: any, val: any, row: any) => {
        if (col.WidgetId === "name" && val.AdditionalValue) {
            const { ViewLink } = val.AdditionalValue;
            if (ViewLink) val.AdditionalValue.ViewLink = `${window._AppSetting.AppRoot}entitystudio/entity/${row.name.FormatedValue}`;
        }

        return cellRenderer(col, val);
    }, []);

    if (!state.schema) return <div>error</div>;

    const { Widgets, Actions } = state.schema;
    const feldListWIdget = Widgets.LayoutList;

    return (<div>
        <div className="paper mb-g">
            <PageHeader
                ghost={false}
                onBack={() => window.history.back()}
                title={"Entities"}
                extra={_.map(Actions, (c) => {
                    return <ActionLink key={c.ActionId} {...c} DisplayType={2} overrideProps={{ handeleSubmitResponse }} />;
                })}
            >
            </PageHeader>
        </div>
        <GridView
            ref={gridRef}
            IsLocalStore={true}
            DataActionLink={feldListWIdget.DataActionLink}
            onChange={() => { }}
            WidgetId="FieldList"
            WidgetType={100}
            formatCell={formatListCell}
            />
    </div>);
}