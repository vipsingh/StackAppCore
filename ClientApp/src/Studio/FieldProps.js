import React, {Component} from 'react';
import PropTypes from "prop-types";
import { Icon, Row, Col, List, Button, Input, Checkbox, Form } from 'antd';

export default class FieldProps extends Component{
    static propTypes = {
        setFieldProp: PropTypes.func,
        selectedField: PropTypes.any
      };

    constructor(props) {
      super(props);      
        this.formFields = [
            {Name: "Name", Type: 1 },
            {Name: "Label", Type: 1 },
            {Name: "Type", Type: 1, IsDisabled: true },
            {Name: "IsRequired", Type: 3 },
            {Name: "RefObject", Type: 1, visible: (typ) => typ === 10 }
        ];       
    }
    // /visible: () => this.props.selectedField.Type === 10

    fChange = (f, v) => {
        if (f.IsDisabled)
            return;
        this.props.setFieldProp(f.Name, v);
    }

    render(){
        const form = this.props.selectedField;
        if (!form)
            return (<div />);
            
        return(
            <Form layout={"horizontal"}>
                {
                    this.formFields.map((x) =>{
                        if (x.visible && !x.visible(form.Type))
                            return;

                        return (<Form.Item label={x.Name}>
                                {this.renderF(x)}
                            </Form.Item>);
                    })
                }                                         
            </Form>
        );
    }
    getFieldValue(x){
        const form = this.props.selectedField;
        return ( form[x + "__text"]? form[x + "__text"]: form[x] );
    }

    renderF(field){
        const form = this.props.selectedField;
        if (!form)
            return;
            
        if(field.Type === 1){
            return (<Input value={this.getFieldValue(field.Name)} onChange={(e) => { this.fChange(field, e.target.value); }} /> );
        } else if(field.Type === 3){
            return (<Checkbox checked={!!form[field.Name]} onChange={(e) => { this.fChange(field, e.target.checked); }} /> );
        }
    }
}