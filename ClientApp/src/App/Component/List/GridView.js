import React from "react";
import PropTypes from "prop-types";
import { Table } from 'antd';

class GridView extends React.Component {
    static propTypes = {
        Columns: PropTypes.array, 
        Rows: PropTypes.array
    }
    constructor(props) {
        super(props);
        
        this.state = {
            columns: this.prepareAntTableSchema(props.Columns)
        };
    }

    formatCell(val, row) {
        if (this.Format && this.Format.URL) {
            let u = this.Format.URL;
            u = u.replace("$$ObjectId$$", row.RowId);
            return (<a href={u}>{val}</a>);
        }

        return val;
    }

    prepareAntTableSchema(columns) {
        return columns.map(c => {
            return {
                title: c.Caption,
                dataIndex: "Data." + c.Name, 
                key: c.Name,
                render: this.formatCell.bind(c)
            };
        });
    }

    render() {
        const { columns } = this.state;
        const { Rows } = this.props;
 
        return (
            <Table 
                columns={columns}
                dataSource={Rows}
                rowKey={"RowId"}
            />
        );
    }
}

export default GridView;