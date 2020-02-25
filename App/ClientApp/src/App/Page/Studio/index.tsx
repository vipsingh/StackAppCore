import React, { Component } from "react";
import _ from "lodash";
import { Layout, Menu, Icon, Row, Col, List, Button, Input, Form } from "antd";
import FieldProps from "./FieldProps";
import EntityPage from "./EntityPage";

const { Header, Content, Footer, Sider } = Layout;

export default class Studio extends Component<any, { form: any, selectedField: number, isEntityView: boolean }> {
  fieldsType: Array<any>
  nextId: number
  constructor(props: any) {
    super(props);
    this.state = {
      form: {
        Id: 0,
        Name: "",
        Text: "",
        Fields: []
      },
      selectedField: -1,
      isEntityView: true
    };

    this.fieldsType = [];
    this.nextId = 0;
  }

  renderFields() {}

  addF = (item: any) => {
    var fields = this.state.form.Fields.splice(0);

    var f = {
      Id: this.nextId++,
      Name: "",
      Label: "",
      Type: item.Id,
      Type__text: item.Text,
      IsRequired: false,
      RefObject: ""
    };

    fields.push(f);

    this.setState({
      form: Object.assign({}, this.state.form, { Fields: fields }),
      selectedField: f.Id
    });
  };

  onFieldSelect = (f: any) => {
    this.setState({ selectedField: f.Id });
  };

  setFieldProp = (field: string, value: any) => {
    const { form, selectedField } = this.state;
    var fields = form.Fields.splice(0);

    var f = _.find(fields, { Id: selectedField });
    f = Object.assign(f, { [field]: value });

    this.setState({
      form: Object.assign({}, this.state.form, { Fields: fields })
    });
  };

  changeView = (isEntityView: boolean) => {
    this.setState({isEntityView});
  }

  saveEntity() {}

  validate() {}

  renderFieldTypes() {
    return (
      <List
        bordered
        dataSource={fieldTypes}
        renderItem={item => (
          <List.Item>
            <Button
              type="default"
              icon="list"
              block
              onClick={() => {
                this.addF(item);
              }}
            >
              {item.Text}
            </Button>
          </List.Item>
        )}
      />
    );
  }

  render() {
    const { form, selectedField } = this.state;
    const selectedF = _.find(form.Fields, { Id: selectedField });
    const formItemLayout = {
      labelCol: { span: 4 },
      wrapperCol: { span: 14 }
    };
    const panelStyle = {
      background: "#fff",
      padding: 12,
      margin: 0,
      minHeight: 280
    };

    if (this.state.isEntityView) {
      return (<EntityPage onChangeView={this.changeView} />);
    }

    return (
      <Layout className="layout">
        <Content>
          <Row>
            <Col span={4}>
              <div style={panelStyle}>{this.renderFieldTypes()}</div>
            </Col>
            <Col span={14}>
              <div style={panelStyle}>                
                <Form>
                  {_.map(form.Fields, f => {
                    return (
                      <Form.Item label={"Field " + f.Name} {...formItemLayout}>
                        <Input
                          addonAfter={
                            <a
                              href="#"
                              onClick={() => {
                                this.onFieldSelect(f);
                              }}
                            >
                              {"###"}
                            </a>
                          }
                          placeholder={f.Name}
                          disabled
                        />
                      </Form.Item>
                    );
                  })}
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

var fieldTypes = [
  { Id: 1, Text: "Text", Icon: "edit" },
  { Id: 2, Text: "Integer", Icon: "" },
  { Id: 3, Text: "Decimal", Icon: "" },
  { Id: 4, Text: "DateTime", Icon: "" },
  { Id: 5, Text: "Date", Icon: "" },
  { Id: 6, Text: "MonataryAmount", Icon: "" },
  { Id: 7, Text: "Bool", Icon: "" },
  { Id: 8, Text: "Time", Icon: "" },
  { Id: 9, Text: "Select", Icon: "" },
  { Id: 10, Text: "ObjectLink", Icon: "" },
  { Id: 11, Text: "ObjectNumber", Icon: "" },
  { Id: 12, Text: "BigInt", Icon: "" },
  { Id: 13, Text: "LongText", Icon: "" },
  { Id: 14, Text: "Image", Icon: "" },
  { Id: 15, Text: "Email", Icon: "" },
  { Id: 16, Text: "Url", Icon: "" },
  { Id: 17, Text: "Html", Icon: "" },
  { Id: 18, Text: "Xml", Icon: "" },
  { Id: 19, Text: "KeyPair", Icon: "" },
  { Id: 20, Text: "ObjectList", Icon: "" },
  { Id: 21, Text: "Computed", Icon: "" },
  { Id: 22, Text: "Password", Icon: "" },
  { Id: 23, Text: "File", Icon: "" },
  { Id: 24, Text: "Json", Icon: "" }
];
