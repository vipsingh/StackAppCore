import React from "react";
import PropTypes from "prop-types";
import ListingWrapper from "./ListingWrapper";

class EntityList extends React.Component {
    static propTypes = {
        DataUrl: PropTypes.object,
        ListSchema: PropTypes.object
    }

    constructor(props) {
        super(props);

        //this.fetchData();
    }

    fetchData() {
        const { Url } = this.props.DataUrl;
        _App.Request.getData({
            url: Url
        }).then((res) => {
            //this.setState({listingSchema: JSON.parse(`{"Id":"ENTITYLIST_StudentMaster","Caption":"StudentMaster","IdColumn":"ID","Columns":[{"Name":"Name","Caption":"Name","Type":1},{"Name":"AdmissionNo","Caption":"AdmissionNo","Type":1},{"Name":"ClassSection","Caption":"ClassSection","Type":10},{"Name":"Gender","Caption":"Gender","Type":9},{"Name":"DOB","Caption":"DOB","Type":5},{"Name":"Status","Caption":"Status","Type":9}]}`)});
        }).finally(() => {
            //this.setState({IsFetching: false});
        });
        //this.setState({listingSchema: JSON.parse(`{"Id":"ENTITYLIST_StudentMaster","Caption":"StudentMaster","IdColumn":"ID","Columns":[{"Name":"Name","Caption":"Name","Type":1},{"Name":"AdmissionNo","Caption":"AdmissionNo","Type":1},{"Name":"ClassSection","Caption":"ClassSection","Type":10},{"Name":"Gender","Caption":"Gender","Type":9},{"Name":"DOB","Caption":"DOB","Type":5},{"Name":"Status","Caption":"Status","Type":9}]}`)});
    }    

    render() {

        if (this.props.ListSchema) {
            return (<ListingWrapper 
                listingSchema={this.props.ListSchema}
                dataUrl={this.props.DataUrl}
            />);
        } else {
            return (<div></div>);
        }
    }
}

export default EntityList;