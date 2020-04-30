import React from "react";
import { Table } from 'antd';
import _, { Dictionary } from "lodash";
import ListingWrapper from "./ListingWrapper";
import { cellRenderer } from "./Helper";

class GridView extends React.Component<{
    listData: any, 
    pager?: any,
    rowSelection?: any
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

    formatCell(col: any, val: any, row: any) {        
        return cellRenderer(col, val);
    }

    prepareAntTableSchema(columns: Dictionary<any>) {
        return  _.map(Object.keys(columns), (k: string) => {
            const c =  columns[k];

            return {
                title: c.WidgetId,
                dataIndex: c.WidgetId, 
                key: c.WidgetId,
                render: this.formatCell.bind(this, c)
            };
        });
    }    

    render() {
        const { columns } = this.state;
        const { listData: { Data} } = this.props;
 
        return (
            <Table 
                columns={columns}
                dataSource={Data}
                rowKey={"RowId"}
                bordered
                size="small"
                rowSelection={this.props.rowSelection}
            />
        );
    }
}

export default ListingWrapper(GridView);