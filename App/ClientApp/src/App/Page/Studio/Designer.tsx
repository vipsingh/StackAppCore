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
import { getComponent } from '../../../Component/WidgetFactory';
import DesignerFieldProp from "./DesignerFieldProp";
import { openDialog } from "../../../Component/UI/Dialog";

export default class DesignerPage extends Component<any,any> {
    designerContext: any
    fields: Array<any>
    _ix: number
    fieldPropDlg: any
    constructor(props: any) {
        super(props);

        this.state = {
            Header: {
                Id: "header",
                Text: "",
                Groups: [{ Id: "gh", Rows: [ { Fields: [ ] } ] }]
            },
            Pages: [{ Id: "page1",
                Groups: [{ Id: "g1", Rows: [ { Fields: [ ] }] }]
            }],
            fieldContainerLink: {

            }
        }
        this.fields = [
          {WidgetId: "Field1", WidgetType: 1}, {WidgetId: "Field2", WidgetType: 1}, {WidgetId: "Field3", WidgetType: 1}, {WidgetId: "Field4", WidgetType: 4}, {WidgetId: "Field5", WidgetType: 2}
      ];

        this._ix = 0;

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

        let IComponent: any;
        let wInfo: any = {};
        if (fieldId) {
          wInfo =  _.find(this.fields, { WidgetId: fieldId }) as any;
          if (!wInfo.Caption) wInfo.Caption = wInfo.WidgetId;
        
          IComponent = getComponent(wInfo.WidgetType, false); 
        }
        
        return (<FieldPanel Id={container} FieldId={fieldId}>
                    {!fieldId || 
                        <MoveBox item={{ id: fieldId }} moveField={(id: string, fieldBoxId: string) => { 
                            this.moveField(id, fieldBoxId);
                          }}>
                              <div>
                                  {this.renderFieldSettingIcon(fieldId)}
                              <FormField WidgetId={fieldId} {...wInfo} >
                                    {!IComponent || <IComponent 
                                      {...wInfo}
                                      api={this.getFormAPI()}
                                      onChange={() => { }}
                                    />}
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
            {_.map(this.fields, (item) => {
                const isReadOnly = _.values(this.state.fieldContainerLink).indexOf(item.WidgetId) >= 0;
              return (<Box field={item} readOnly={isReadOnly} addField={(fieldId: string, boxId: string) => {
                  this.linkField(fieldId, boxId);
                }} />
              );
            })}
          </div>
        );
    }

    renderFieldSettingIcon = (fieldId: string) => {
      if (!fieldId) return;

      return (<div style={{ position: "absolute", zIndex: 1 }}>
                <a href="javascript:void(0)" className="ant-dropdown-link" onClick={e => this.onWidgetSelect(fieldId)}>
                    <SettingOutlined />
                </a></div>)
    }

    renderGroupSetting = (pageid: string, group: any) => {
        var pageMenues = [];
        pageMenues.push(<Menu.Item>
          <a href="javascript:void(0)" onClick={() => this.addgroupRow(pageid, group)}> Add Row</a>
      </Menu.Item>);
      if (pageid !== "header") {
        pageMenues.push(<Menu.Item>
          <a href="javascript:void(0)" onClick={() => this.editgroup(pageid, group)}> Edit Group</a>
        </Menu.Item>);
        pageMenues.push(<Menu.Item>
          <a href="javascript:void(0)" onClick={() => this.addgroup(pageid)}> Add Group</a>
        </Menu.Item>);
      }
        var menu = (
            <Menu>
              {pageMenues}
            </Menu>
        );

        //style={{ position: "absolute", right: "5px" }}
        return (<div>
            <Dropdown overlay={menu}>
                <a className="ant-dropdown-link" onClick={e => e.preventDefault()}>
                    <SettingOutlined />
                </a>
            </Dropdown>
        </div>)
    }

    onWidgetSelect = (widgetId: string) => {
      const selectedF = _.find(this.fields, { WidgetId: widgetId });

      this.fieldPropDlg = openDialog("Field", () => { return <DesignerFieldProp
          selectedField={selectedF}
          setFieldProp={this.setWidgetProp}
        />; 
      }, { hideCommands: true, size: "lg" });
    }

    setWidgetProp = (toSaveModel: any) => {
      this.fieldPropDlg.cleanup();
    }

    addgroupRow = (pageid: string, group: any) => {
        const newRow = { Fields: [ { FieldId: `id${this._ix++}` }, { FieldId: `id${this._ix++}` } ] };
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

    editgroup = (pageid: string, group: any) => {

    }

    addgroup = (pageid: string) => {
      const newRow = { Id: `g${this._ix++}`, Label: "Group", Rows: [ { Fields: [ ] } ] };
      const pageix = _.findIndex(this.state.Pages, { Id: pageid });

      const h = update(this.state.Pages[pageix], { Groups: { $push: [newRow] } });
      this.setState({ Pages: update(this.state.Pages, { [pageix]: { $set: h } }) });      
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
      item: { name: field.WidgetId, type: "FIELDBOX" },
  
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
            {field.Caption || field.WidgetId}
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
      <div ref={drag} style={{ opacity }}>
        {children}
      </div>
    )
  }