import React, { Component } from "react";
import _ from "lodash";
import { Layout, Row, Col, Button, Input, Form, Card } from "antd";
import FieldProps from "./FieldProps";
import { fieldTypes } from "./helper";
import { openDialog } from "../../../Component/UI/Dialog";
import update from "immutability-helper";
import { DndProvider, useDrag, DragSourceMonitor, useDrop } from 'react-dnd'
import Backend from 'react-dnd-html5-backend'

const { Content } = Layout;

export default class Studio extends Component<
  any,
  { form: any; selectedField: number, FieldBox: Array<{ Id: number, FieldId: number }> }
  > {
  fieldsType: Array<any>
  nextId: number
  nextBoxId: number
  fieldPropDlg: any
  constructor(props: any) {
    super(props);
    this.state = {
      form: {
        Id: 0,
        Name: "",
        Text: "",
        Fields: []        
      },
      FieldBox: [{Id: 1, FieldId: -1}],
      selectedField: -1,
    };
    
    this.fieldsType = [];
    this.nextId = 1;
    this.nextBoxId = 2;

  }

  addFieldBox() {
    this.setState({ FieldBox: update(this.state.FieldBox, { $push: [{Id: this.nextBoxId++, FieldId: -1}]}) });
  }

  renderFields() { }

  addF = (item: any, fieldBoxId: number) => {
    var fields = this.state.form.Fields.splice(0);
    const id = this.nextId++;

    var f = {
      Id: id,
      FieldName: "Field " + id,
      Label: "",
      FieldType: item.Value,
      Length: 0,
      IsRequired: false,
      LinkEntity: 0,
      OtherSetting: ""
    };    

    fields.push(f);

    this.setState({
      form: Object.assign({}, this.state.form, { Fields: fields }),
      selectedField: f.Id,
    });

    let fieldBox = update(this.state.FieldBox, { $push: [{Id: this.nextBoxId++, FieldId: -1}]});
    const ix = _.findIndex(fieldBox, { Id: fieldBoxId });
    fieldBox = update(fieldBox, {[ix]: { $set: Object.assign({}, fieldBox[ix], { FieldId: f.Id })} });
    this.setState({ FieldBox: fieldBox });    

    setTimeout(() => {
      this.onFieldSelect(f);
    }, 100);
  };

  validateFieldForm() {
    
  }

  onFieldSelect = (f: any) => {
    //this.setState({ selectedField: f.Id });
    const selectedF = f;//_.find(this.state.form.Fields, { Id: f.Id });
    this.fieldPropDlg = openDialog("Field", (<FieldProps
      selectedField={selectedF}
      setFieldProp={this.setFieldProp}
    />), { hideCommands: true, size: "lg" });
  };

  setFieldProp = (toSaveModel: any) => {
    const { form, selectedField } = this.state;
    var fields = form.Fields.splice(0);
    var f = _.find(fields, { Id: selectedField });
    _.forIn(toSaveModel.Widgets, (w, k) => {
      Object.assign(f, { [k]: w.Value });
    });    

    this.setState({
      form: Object.assign({}, this.state.form, { Fields: fields }),
    });

    this.fieldPropDlg.cleanup();
  };

  saveEntity() { }

  validate() { }

  renderFieldTypes() {
    return (
      <div>
        {_.map(fieldTypes, (item) => {
          return (<Box item={item} addField={(typeId: string, fieldBoxId: string) => {
              this.addF(_.find(fieldTypes, {Value: parseInt(typeId)}), parseInt(fieldBoxId));
            }} />
          );
        })}
      </div>
    );
  }

  renderFieldBox = (fieldId: number) => {
    let fieldItem = _.find(this.state.form.Fields, { Id: fieldId });
    return (
      <Form.Item label={fieldItem.FieldName} labelCol={{span: 6}} wrapperCol={{span: 18}}>
      <Input
        addonAfter={
          <Button
            type="link"
            onClick={() => {
              this.onFieldSelect(fieldItem);
            }}
          >
            {"###"}
          </Button>
        }
        placeholder={fieldItem.FieldName}
        disabled
      />
    </Form.Item>
    );
  }

  render() {
    const panelStyle = {
      background: "#fff",
      padding: 12,
      margin: 0,
      minHeight: 280,
    };

    return (
      <Layout className="layout">
        <Content>
        <DndProvider backend={Backend}>
          <Row>
            <Col span={4}>
              <Card size="small" title={"Fields"} style={panelStyle}>
                {this.renderFieldTypes()}
              </Card>
            </Col>
            <Col span={20}>
              <div style={panelStyle}>
                <Form>
                {
                  _.map(this.state.FieldBox, (b) => {
                    return <FieldPanel 
                      {...b} 
                      render={this.renderFieldBox}/>
                  })
                }
                <Button type="primary">{"Save"}</Button>
                </Form>
              </div>
            </Col>
          </Row>
          </DndProvider>
        </Content>
      </Layout>
    );
  }
}

const styleBox: React.CSSProperties = {
  border: '1px dashed gray',
  backgroundColor: 'white',
  padding: '0.5rem',
  marginRight: '1rem',
  marginBottom: '0.5rem',
  cursor: 'move',
  float: 'left',
}

interface BoxProps {
  name: string
}

const Box: React.FC<{
  item: any,
  addField: Function
}> = ({ item, addField }) => {
  const [{ isDragging }, drag] = useDrag({
    item: { name: item.Value.toString(), type: "FIELDBOX" },
    end: (item: { name: string } | undefined, monitor: DragSourceMonitor) => {
      const dropResult = monitor.getDropResult()
      if (item && dropResult && dropResult.fieldId <= 0) {
        addField(item.name, dropResult.name);
      }
    },
    collect: (monitor) => ({
      isDragging: monitor.isDragging(),
    }),
  })
  const opacity = isDragging ? 0.4 : 1

  return (
    <div ref={drag} style={{ ...styleBox, opacity }}>
      {item.Text}
    </div>
  )
}

const style: React.CSSProperties = {
  height: '2.5rem',
  width: '15rem',
  margin: '0.5rem',
  float: 'left',
  border: "1px solid black"
}

const FieldPanel: React.FC<any> = ({Id, FieldId, render}) => {
  const [{ canDrop, isOver }, drop] = useDrop({
    accept: "FIELDBOX",
    drop: () => ({ name: Id, fieldId: FieldId }),
    collect: (monitor) => ({
      isOver: monitor.isOver(),
      canDrop: monitor.canDrop(),
    }),
  })

  const isActive = canDrop && isOver
  let backgroundColor = '#fff'
  if (isActive) {
    backgroundColor = 'darkgreen'
  } else if (canDrop) {
    backgroundColor = 'darkkhaki'
  }

  return (<Row>
        <Col span={24}>
          <div ref={drop} style={{ ...style, backgroundColor }}>
            {FieldId < 0 || render(FieldId)}
          </div>
        </Col>
    </Row>
  );
}
