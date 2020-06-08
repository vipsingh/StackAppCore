import React, { Component } from 'react';
import { Tabs, Row, Col, Card, Button } from 'antd';
import { Menu, Dropdown } from 'antd';
import PageView from '../../../Component/Form/PageView';
import FormField from '../../../Component/Form/FormField';
import ViewPageInfo from '../../../Core/Models/ViewPageInfo';
import { fieldTypes } from "./helper";
import update from "immutability-helper";
import { DndProvider, useDrag, DragSourceMonitor, useDrop } from 'react-dnd'
import Backend from 'react-dnd-html5-backend'
import _ from 'lodash';
import { DesignerContext } from '../../../Core/Studio';
import { SettingOutlined } from '@ant-design/icons';

const { TabPane } = Tabs;

var fields = [
    {Id: "Field1"}, {Id: "Field2"}, {Id: "Field3"}, {Id: "Field4"}, {Id: "Field5"}
];

export default class DesignerPage extends Component<any,any> {
    designerContext: any
    constructor(props: any) {
        super(props);

        this.state = {
            Header: {
                Id: "header",
                Text: "",
                Groups: [{ Id: "gh", Rows: [ { Fields: [ { FieldId: "id1" } ] } ] }]
            },
            Pages: [{ Id: "page1",
                Groups: [{ Id: "g1", Rows: [ { Fields: [ { FieldId: "id11" }, { FieldId: "id12" } ] }] }]
            }],
            fieldContainerLink: {

            }
        }

        this.designerContext = {
            renderGroupSetting: this.renderGroupSetting
        };

    }

    linkField = (fieldId: string, containerId: string) => {
        this.setState({ fieldContainerLink: Object.assign({}, this.state.fieldContainerLink, { [containerId]: fieldId } ) });
    }
    moveField = (fieldId: string, containerId: string) => {        
        let lastCont = "";
        _.forIn(this.state.fieldContainerLink, function(value, key) {
            if (value === fieldId) lastCont = key;
        });

        this.setState({ fieldContainerLink: Object.assign({}, this.state.fieldContainerLink, { [containerId]: fieldId, [lastCont]: undefined } ) });
    }
    
    getControl = (container: string) => {
        let fieldId = this.state.fieldContainerLink[container];
        
        return (<FieldPanel Id={container} FieldId={fieldId}>
                    {!fieldId || 
                        <MoveBox item={{ id: fieldId }} moveField={(id: string, fieldBoxId: string) => { 
                            this.moveField(id, fieldBoxId);
                          }}>
                              <div>
                                  {this.renderFieldSettingIcon()}
                              <FormField WidgetId={fieldId} WidgetType={1} Caption="Field" onChange={()=>{}} api={this.getFormAPI()}>
                                    <label>{fieldId}</label>
                                </FormField>
                                </div> 
                        </MoveBox>
                    }
            </FieldPanel>);
    }

    getFormAPI(): FormApi {
        return {
            getEntitySchema: () => { return new ViewPageInfo(null); },
            setValue: () => { },
            updateField: () => { },
            getField: () => { return { WidgetId: "", WidgetType: 1 }; },
            getErrorResult: () => { },
            validateField: () => { },
            prepareFieldRequest: () => { },
            setWidgetTempdata: () => { },
            getWidgetTempdata: () => { }
        };
    }

    renderFieldTypes() {
        return (
          <div>
            {_.map(fields, (item) => {
                const isReadOnly = _.values(this.state.fieldContainerLink).indexOf(item.Id) >= 0;
              return (<Box field={item} readOnly={isReadOnly} addField={(fieldId: string, boxId: string) => {
                  this.linkField(fieldId, boxId);
                }} />
              );
            })}
          </div>
        );
    }

    renderFieldSettingIcon = () => {
        return (<div style={{ position: "absolute" }}>
                <a className="ant-dropdown-link" onClick={e => e.preventDefault()}>
                    <SettingOutlined />
                </a></div>)
    }

    renderGroupSetting = (pageid: string, group: any) => {
        var menu = (
            <Menu>
            <Menu.Item>
                <a href="javascript:void(0)" onClick={() => this.addgroupRow(pageid, group)}>
                Add Row
                </a>
            </Menu.Item>
            </Menu>
        );
        
        return (<div style={{ position: "absolute", right: "5px" }}>
            <Dropdown overlay={menu}>
                <a className="ant-dropdown-link" onClick={e => e.preventDefault()}>
                    <SettingOutlined />
                </a>
            </Dropdown>
        </div>)
    }
    addgroupRow = (pageid: string, group: any) => {
        const newRow = { Fields: [ { FieldId: "id21" }, { FieldId: "id22" } ] };
        if (pageid === "header") {
            const ix = _.findIndex(this.state.Header.Groups, { Id: group.Id });
            const g = this.state.Header.Groups[ix];
            const h = update(this.state.Header, { Groups: { [ix]: { $set: Object.assign({}, g, { Rows: update(g.Rows, {$push: [newRow]}) }) } } });
            this.setState({ Header: h });
        } else {
            const pageix = _.findIndex(this.state.Pages, { Id: pageid });
            const ix = _.findIndex(this.state.Pages[pageix].Groups, { Id: group.Id });
            const g = this.state.Pages[pageix].Groups[ix];
            const h = update(this.state.Pages[pageix], { Groups: { [ix]: { $set: Object.assign({}, g, { Rows: update(g.Rows, {$push: [newRow]}) }) } } });
            this.setState({ Pages: update(this.state.Pages, { [pageix]: { $set: h } }) });
        }
    }

    render() {
        const { Header, Pages} = this.state;

        const entityModel = { Layout: { Header, Pages }, PageTitle: { PageTitle: "Form Name" } };

        return (<DndProvider backend={Backend}>
            <DesignerContext.Provider value={this.designerContext}>
            <Row>
            <Col span={4}>
              <Card size="small" title={"Fields"}>
                {this.renderFieldTypes()}
              </Card>
            </Col>
            <Col span={20}>
                <PageView getControl={this.getControl} entityModel={entityModel} getFormActions={() => { }} />
            </Col>
            </Row>
            </DesignerContext.Provider>
        </DndProvider>
        );
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////

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
    field: any,
    addField: Function,
    readOnly: boolean
  }> = ({ field, addField, readOnly }) => {
    const [{ isDragging }, drag] = useDrag({
      item: { name: field.Id, type: "FIELDBOX" },
  
      end: (item: { name: string } | undefined, monitor: DragSourceMonitor) => {
        const dropResult = monitor.getDropResult()
        if (item && dropResult && !dropResult.fieldId) {
          addField(item.name, dropResult.name);
        }
      },
      
      collect: (monitor) => ({
        isDragging: monitor.isDragging(),
      }),

      canDrag: !readOnly
    })
    const opacity = isDragging || readOnly ? 0.4 : 1
  
    return (
      <div ref={drag} style={{ ...styleBox, opacity }}>
            {field.Id}
      </div>
    )
  }

const style: React.CSSProperties = {
    minHeight: '2.5rem',
    width: '15rem',
    margin: '0.5rem',
    float: 'left',
    border: "1px dashed gray"
  }
  
  const FieldPanel: React.FC<any> = ({Id, FieldId, children}) => {
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
  
    return (<div ref={drop} style={{ ...style, backgroundColor }}>{children}</div>);
  }

  const MoveBox: React.FC<{
    item: any,
    moveField: Function,
    children: any
  }> = ({ item, moveField, children }) => {
    
    const [{ isDragging }, drag] = useDrag({
      item: { name: item.id.toString(), type: "FIELDBOX" },
      
      end: (item: { name: string } | undefined, monitor: DragSourceMonitor) => {
        const dropResult = monitor.getDropResult();
        if (item && dropResult && !dropResult.fieldId) {
          moveField(item.name, dropResult.name);
        }
      },
      
      collect: (monitor) => ({
        isDragging: monitor.isDragging(),
      }),
    })
    const opacity = isDragging ? 0.4 : 1
  
    return (
      <div ref={drag} style={{ border: '1px dashed gray', opacity }}>
        {children}
      </div>
    )
  }