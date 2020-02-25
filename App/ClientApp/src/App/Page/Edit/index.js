import React from "react";
import PropTypes from "prop-types";
import EntityForm from "../../../Component/Form/EntityForm";
import PageView from "../../../Component/Form/PageView";


export default class NewEdit extends React.Component {
    static propTypes = {
        data: PropTypes.object
    }
    constructor(props) {
        super(props);

        this.state = {
            IsLoading: false,
            Schema: props.data,
            Formdata: null
        };
    }

    shouldComponentUpdate() {
        return true;
    }

    componentWillReceiveProps(nextProps) {
        this.setState({Schema: nextProps.data});
    }

    componentDidMount() {
        //this.setState({IsLoading: true});
        //this.fetchSchema();
    }

    render() {

        return (
            <EntityForm
                Schema={this.state.Schema}
                Formdata={this.state.Formdata}
                render={
                    (prop) => {
                        return (
                           <PageView
                               {...prop}
                           />  
                        );
                    }
                }
             />             
        );
    }
}