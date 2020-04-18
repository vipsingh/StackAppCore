import React from "react";
import { Table } from 'antd';
import _, { Dictionary } from "lodash";
import ListingWrapper from "./ListingWrapper";
import ActionLink from "../ActionLink";

class GridView extends React.Component<{
    listData: any, 
    pager: any
}, {
    columns: Array<any>
}> {
 
    constructor(props: any) {
        super(props);

        const { Fields } = props.listData;
        this.state = {
            columns: this.prepareAntTableSchema(Fields)
        };
    }

    formatCell(val: any, row: any) {        
        const { AdditionalValue, FormatedValue } = val;
        let d = FormatedValue;
        if (typeof FormatedValue === "object"){
            d = FormatedValue.Text;
        } 

        if (AdditionalValue && AdditionalValue.ViewLink) {
            const { ViewLink } = AdditionalValue;
            return (<ActionLink ActionId={"VIEW"} {...ViewLink} Title={d} />);
        }

        return d;
    }

    prepareAntTableSchema(columns: Dictionary<any>) {
        return  _.map(Object.keys(columns), (k: string) => {
            const c =  columns[k];

            return {
                title: c.WidgetId,
                dataIndex: c.WidgetId, 
                key: c.WidgetId,
                render: this.formatCell.bind(c)
            };
        });
    }

    render() {
        const { columns } = this.state;
        const { listData: { Data, IdColumn} } = this.props;
 
        return (
            <Table 
                columns={columns}
                dataSource={Data}
                rowKey={IdColumn}
                bordered
                size="small"
            />
        );
    }
}

export default ListingWrapper(GridView);