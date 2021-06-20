import React from "react";
import { Table, Dropdown, Menu } from 'antd';
import _, { Dictionary } from "lodash";
import ListingWrapper from "./ListingWrapper";
import { DashOutlined } from '@ant-design/icons';
import ActionLink from "../ActionLink";

class GridView extends React.Component<{
    listData: any,
    formatCell: Function,
    renderAction: Function,
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

    prepareAntTableSchema(columns: Dictionary<any>) {
        const cols =  _.reduce(Object.keys(columns), (result: Array<any>, value: any) => {
            const c =  columns[value];
            if (c.IsHidden) return result;

            result.push({
                title: c.Caption,
                dataIndex: c.WidgetId, 
                key: c.WidgetId,
                render: this.props.formatCell.bind(this, c)
            });

            return result;
        }, []);

        cols.push({key: "_RowActions", 
            dataIndex: "_RowActions",
            fixed: "right",
            width: 30,
            render: (col: any) => { 
                if (!col || col.length === 0) return;

                var menu = (
                    <Menu>
                        {_.map(col, (a) => {
                            return (<Menu.Item key={a.ActionId}>
                                {this.props.renderAction(a)}
                            </Menu.Item>)
                        })}
                    </Menu>
                );
                
                return (<Dropdown overlay={menu}>
                        <a className="ant-dropdown-link" onClick={e => e.preventDefault()}>
                            <DashOutlined />
                    </a>
                </Dropdown>);
            }
        });

        return cols;
    }    

    render() {
        const { columns } = this.state;
        const { listData: { Data} } = this.props;
 
        return (
            <Table 
                columns={columns}
                dataSource={Data}
                rowKey={"_RowId"}
                bordered
                size="small"
                rowSelection={this.props.rowSelection}
            />
        );
    }
}

export default ListingWrapper(GridView);