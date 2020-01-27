import React, { Component } from "react";
import PropTypes from "prop-types";
import _ from "lodash";
import { Row, Col } from "antd";
import { Form } from "antd";
import { Input, Select, Button } from 'antd';

const FormItem = Form.Item;
const Option = Select.Option;

export default class ObjectBuilder extends Component {
    constructor(p) {
        super(p);

        const obj = {
            Name: "",
            Module: "",
            Fields: []
        };

        this.state = {
            form: obj
        };

        this.fieldTypes = {Text : 1,
            Integer : 2,
            Decimal : 3,
            DateTime : 4,
            Date : 5,
            MonataryAmount : 6,
            Bool : 7,
            Time : 8,
            Select : 9,
            ObjectLink : 10,
            ObjectNumber : 11,
            BigInt : 12,
            LongText : 13,
            Image : 14,
            Email : 15,
            Url : 16,
            Html : 17,
            Xml : 18,
            KeyPair : 19,
            ObjectList : 20,
            Computed : 21,
            Password : 22,
            File : 23,
            Json : 24 
        };

        this.currentFieldId = 1;
    }

    addField = () => {
        const fields = this.state.form.Fields.splice(0);
        fields.push({ID: this.currentFieldId++, Name: "", Type: 0, LinkObject: "", ListItem:"", Params: "{ Text: '', IsRequired: false, IsReadOnly: false, DefaultValue: '' }"});
        this.setState({
            form: Object.assign({}, this.state.form, {Fields: fields})
        });
    }

    fieldValueChange(ev, fName, id) {
        if (fName == "Type") {
            v = ev;
        } else {
            v = ev.target.value;
        }
        const fields = this.state.form.Fields.splice(0);
        const f = _.find(fields, { ID: id });

        Object.assign({},f, {[fName]: v});

        this.setState({form: Object.assign({}, this.state.form, {Fields: fields})});
    }

    submitForm() {

    }

    renderField(f) {

        return (
            <div key={f.ID} style={{border: "1px solid black"}}>
            <Row>
                <Col>
                <FormItem            
                    label={"Name"}>
                        <Input 
                            value={f.Name}
                            onChange={(ev) => { this.fieldValueChange(ev, "Name", f.ID); }}
                        />
                    </FormItem>
                </Col>
                <Col>
                <FormItem            
                    label={"Type"}>
                        <Select 
                            value={f.Type}
                            onChange={(ev) => { this.fieldValueChange(ev, "Type", f.ID); }}
                        >
                        {_.mapValues(this.fieldTypes, (v, k) => { return <Option key={v}>{k}</Option>;})}
                        </Select>
                    </FormItem>
                </Col>
                <Col>
                    <FormItem            
                    label={"Link Object"}>
                        <Input 
                            value={f.LinkObject}
                            disabled={f.Type !== 10}
                            onChange={(ev) => { this.fieldValueChange(ev, "LinkObject", f.ID); }}
                        />
                    </FormItem>
                </Col>
            </Row>
            <Row>
                <Col>
                <FormItem            
                    label={"Params"}>
                        <Input.TextArea 
                            value={f.Params}
                            onChange={(ev) => { this.fieldValueChange(ev, "Params", f.ID); }}
                        />
                    </FormItem>
                </Col>
                <Col>
                    <FormItem            
                    label={"List Item"}>
                        <Input 
                            value={f.ListItem}
                            disabled={f.Type !== 9}
                            onChange={(ev) => { this.fieldValueChange(ev, "ListItem", f.ID); }}
                        />
                    </FormItem>
                </Col>
            </Row>
            </div>
        );
    }

    render() {
        const { form } = this.state;
        return(
            <div>
                <Row>
                    <Col><FormItem            
                    label={"Name"}>
                        <Input 
                            value={form.Name}
                            onChange={(e) => { this.setState({ form: Object.assign({}, form, { Name: e.target.value }) }); }}
                        />
                    </FormItem></Col>
                    <Col>
                        <Button onClick={this.addField} type="primary">{"Add Field"}</Button>
                    </Col>
                </Row>
                {
                    _.map(form.Fields, (f) => {
                        return this.renderField(f);
                    })
                }
            </div>
        );
    }
}