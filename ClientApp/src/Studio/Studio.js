import React, { Component } from "react";
import PropTypes, { object } from "prop-types";
import _ from "lodash";
import { Layout, Menu, Icon, Row, Col, List, Button, Input, Form } from "antd";
import FieldProps from "./FieldProps";

const { Header, Content, Footer, Sider } = Layout;

export default class Studio extends Component {
    constructor(props) {
        super(props);
        this.state = {
            form: {
                Id: 0,
                Name: "",
                Text: "",
                Fields: []
            },
            selectedField: -1
        };

        this.fieldsType = [];
        this.nextId = 0;
    }

    renderFields() {}

    addF = typ => {
        var fields = this.state.form.Fields.splice(0);

        var f = {
            Id: this.nextId++,
            Name: "",
            Label: "",
            Type: typ,
            Type__text: _.findKey(fieldTypes, function(o) {
                return o === typ;
            }),
            IsRequired: false,
            RefObject: ""
        };

        fields.push(f);

        this.setState({
            form: Object.assign({}, this.state.form, { Fields: fields }),
            selectedField: f.Id
        });
    };

    onFieldSelect = f => {
        this.setState({ selectedField: f.Id });
    };

    setFieldProp = (field, value) => {
        const { form, selectedField } = this.state;
        var fields = form.Fields.splice(0);

        var f = _.find(fields, { Id: selectedField });
        f = Object.assign(f, { [field]: value });

        this.setState({
            form: Object.assign({}, this.state.form, { Fields: fields })
        });
    };

    saveEntity() {

    }

    validate() {

    }

    render() {
        const { form, selectedField } = this.state;
        const selectedF = _.find(form.Fields, { Id: selectedField });
        const formItemLayout = {
            labelCol: { span: 4 },
            wrapperCol: { span: 14 },
          };
        const panelStyle = {background: '#fff',
        padding: 12,
        margin: 0,
        minHeight: 280};

        return (
            <Layout className="layout">
                <Header>
                    <div className="logo" />
                    <Menu
                        theme="dark"
                        mode="horizontal"
                        defaultSelectedKeys={["2"]}
                        style={{ lineHeight: "64px" }}
                    >
                        <Menu.Item key="1">nav 1</Menu.Item>
                    </Menu>
                </Header>
                <Content>
                    <Row>
                        <Col span={4}>
                            <div style={panelStyle}>
                            <List
                                bordered
                                dataSource={Object.keys(fieldTypes)}
                                renderItem={item => (
                                    <List.Item>
                                        <Button
                                            type="primary"
                                            icon="list"
                                            block
                                            onClick={() => {
                                                this.addF(fieldTypes[item]);
                                            }}
                                        >
                                            {item}
                                        </Button>
                                    </List.Item>
                                )}
                            />
                            </div>
                        </Col>
                        <Col span={14}>
                            <div  style={panelStyle}>
                            <div>
                                <Form layout={"horizontal"}>
                                    <Form.Item label={"Entity Name"} {...formItemLayout}>
                                        <Input />
                                    </Form.Item>
                                </Form>
                            </div>
                            <Form>
                            {
                                _.map(form.Fields, (f) => {
                                    return (
                                        <Form.Item label={"Field " + f.Name} {...formItemLayout}>
                                            <Input addonAfter={
                                                <a
                                                href="#"
                                                onClick={() => {
                                                    this.onFieldSelect(f);
                                                }}
                                            >                                                
                                                {"###"}
                                            </a>
                                            } placeholder={f.Name} disabled />
                                        </Form.Item>
                                    );
                                })
                            }
                            </Form>
                            <Button type="primary">{"Save"}</Button>
                            </div>
                        </Col>
                        <Col span={6}>
                            <div style={panelStyle}>
                            <FieldProps
                                selectedField={selectedF}
                                setFieldProp={this.setFieldProp}
                            />
                            </div>
                        </Col>
                    </Row>
                </Content>
            </Layout>
        );
    }

}

var fieldTypes = {
    Text: 1,
    Integer: 2,
    Decimal: 3,
    DateTime: 4,
    Date: 5,
    MonataryAmount: 6,
    Bool: 7,
    Time: 8,
    Select: 9,
    ObjectLink: 10,
    ObjectNumber: 11,
    BigInt: 12,
    LongText: 13,
    Image: 14,
    Email: 15,
    Url: 16,
    Html: 17,
    Xml: 18,
    KeyPair: 19,
    ObjectList: 20,
    Computed: 21,
    Password: 22,
    File: 23,
    Json: 24
};
