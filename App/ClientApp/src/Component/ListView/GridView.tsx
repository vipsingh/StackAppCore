import React from "react";
import { Table } from 'antd';
import _, { Dictionary } from "lodash";
import ListingWrapper from "./ListingWrapper";

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
        // if (this.Format && this.Format.URL) {
        //     let u = this.Format.URL;
        //     u = u.replace("$$ObjectId$$", row.RowId);
        //     return (<a href={u}>{val}</a>);
        // }
        if (typeof val === "object" && val.Text){
            val = val.Text;
        }

        return val;
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
            />
        );
    }
}

export default ListingWrapper(GridView);