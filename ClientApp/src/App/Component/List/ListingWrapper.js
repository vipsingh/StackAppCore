import React from "react";
import PropTypes from "prop-types";
import GridView from "./GridView";

export default class ListingWrapper extends React.Component {
    static propTypes = {
        listingSchema: PropTypes.object,
        dataUrl: PropTypes.object
    }

    constructor(props) {
        super(props);

        this.state = {
            IsFetching: true,            
            Rows: [],
            Pager: {
                Count: 0,
                Page: 1,
                Size: 25
            }
        };

        this.loadData();
    }

    loadData = () => {
        const { listingSchema, dataUrl } = this.props; 
        const { URL } = dataUrl;      

        _App.Request.getData({
            url: URL,
            type: "POST"
        }).then((res) => {
            const { Data } = res;

            this.setState({ Rows: Data });
        }).finally(() => {
            this.setState({IsFetching: false});
        });;
    }

    handleRequestPage = (index)=>{
        this.loadData(index);
    }

    handleGridSort(sortColumn, sortDirection){
        // this.currentSortInfo.field = sortColumn;
        // this.currentSortInfo.dir = sortDirection;
        // this.loadData(1);
    }

    render() {
        const { listingSchema } = this.props;
        const { Rows } = this.state;
        
        if(this.state.IsFetching){
            return (<label>loading..</label>);
        } else {
            return (
                <GridView 
                    Columns={listingSchema.Columns}
                    Rows={Rows}
                />
            );
        }
    }
}