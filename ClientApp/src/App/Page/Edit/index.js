import React from "react";
import PropTypes from "prop-types";
import {Spin} from "antd";
import FormWrapper from "../../Component/Form/FormWrapper";


export default class NewEdit extends React.Component {
    static propTypes = {
        Data: PropTypes.object
    }
    constructor(props) {
        super(props);

        this.state = {
            IsLoading: false,
            Schema: props.Data,
            Formdata: null
        };
    }

    shouldComponentUpdate() {
        return true;
    }

    componentWillReceiveProps(nextProps) {
        this.setState({Schema: nextProps.Data});
    }

    componentDidMount() {
        //this.setState({IsLoading: true});
        //this.fetchSchema();
    }

    // fetchSchema = () => {
    //     window._App.Request.getData({
    //         url: this.props.routeParam.url,
    //         type: "GET"
    //     }).then((data) => {
    //         debugger;
    //         const schema = data;
    //         this.setState({Schema: schema, Formdata: null, IsLoading: false});
    //     }).catch((err) => {
    //         console.error(err);
    //     });
    //     // const schemaT = `{"EntityFields":[{"ControlInfo":{"Properties":[],"Caption":"ID","ControlId":"ID","ControlType":0,"IsReadOnly":true,"IsRequired":false}},{"ControlInfo":{"Properties":[],"Caption":"CreatedOn","ControlId":"CreatedOn","ControlType":2,"IsReadOnly":true,"IsRequired":false}},{"ControlInfo":{"Properties":[],"Caption":"UpdatedOn","ControlId":"UpdatedOn","ControlType":2,"IsReadOnly":true,"IsRequired":false}},{"ControlInfo":{"Properties":[],"Caption":"AdmissionNo","ControlId":"AdmissionNo","ControlType":1,"IsReadOnly":false,"IsRequired":true}},{"ControlInfo":{"Properties":[],"Caption":"ClassSection","ControlId":"ClassSection","ControlType":7,"IsReadOnly":false,"IsRequired":true,"DataUrl":"//entity/getsimplelist?entity=ClassSection"}},{"ControlInfo":{"Properties":[],"Caption":"RegDate","ControlId":"RegDate","ControlType":2,"IsReadOnly":false,"IsRequired":false}},{"ControlInfo":{"Properties":[],"Caption":"Gender","ControlId":"Gender","ControlType":6,"IsReadOnly":false,"IsRequired":false,"ListValues":[{"Text":"Male","Value":1},{"Text":"Female","Value":2}]}},{"ControlInfo":{"Properties":[],"Caption":"DOB","ControlId":"DOB","ControlType":2,"IsReadOnly":false,"IsRequired":false}},{"ControlInfo":{"Properties":[],"Caption":"FatherName","ControlId":"FatherName","ControlType":1,"IsReadOnly":false,"IsRequired":false}},{"ControlInfo":{"Properties":[],"Caption":"MotherName","ControlId":"MotherName","ControlType":1,"IsReadOnly":false,"IsRequired":false}},{"ControlInfo":{"Properties":[],"Caption":"Status","ControlId":"Status","ControlType":6,"IsReadOnly":false,"IsRequired":false,"ListValues":[{"Text":"Studying","Value":1},{"Text":"Not Studying","Value":2}]}},{"ControlInfo":{"Properties":[],"Caption":"ContactNo","ControlId":"ContactNo","ControlType":1,"IsReadOnly":false,"IsRequired":false}},{"ControlInfo":{"Properties":[],"Caption":"Name","ControlId":"Name","ControlType":1,"IsReadOnly":false,"IsRequired":true}}],"ViewTemplate":{"ViewFields":["Name","AdmissionNo","ClassSection","RegDate","Gender","DOB","Status","ContactNo"]}}`;
    //     // const schema = JSON.parse(schemaT);
    //     // setTimeout(() => {
    //     //     this.setState({Schema: schema, Formdata: null, IsLoading: false});
    //     // }, 1000);
    // }

    render() {

        return (
            <FormWrapper
                Schema={this.state.Schema}
                Formdata={this.state.Formdata}
             />
        );
    }
}