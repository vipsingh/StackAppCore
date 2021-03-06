import React from "react";
import EntityForm from "../../../Component/Form/EntityForm";
import PageView from "../../../Component/Form/PageView";


export default class NewEdit extends React.Component <{
    data: any,
    overrideProps?: any
    }, {
        IsLoading: boolean,
        Schema: any,
        FormData: any
    }> 
{

    constructor(props: any) {
        super(props);
    
        this.state = {
          IsLoading: false,
          Schema: props.data,
          FormData: null
        };
      }

    shouldComponentUpdate() {
        return true;
    }

    componentWillReceiveProps(nextProps: any) {
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
                FormData={this.state.FormData}
                overrideProps={this.props.overrideProps}
                render={
                    (prop: any) => {
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