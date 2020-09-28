import React, { Component } from 'react';
import { Row, Col, Card, Button, Menu, Dropdown } from 'antd';
import PageView from '../../../../Component/Form/PageView';
import FormField from '../../../../Component/Form/FormField';
import ViewPageInfo from '../../../../Core/Models/ViewPageInfo';
import update from "immutability-helper";
import { DndProvider } from 'react-dnd'
import Backend from 'react-dnd-html5-backend'
import _ from 'lodash';
import { DesignerContext } from '../../../../Core/Studio';
import { SettingOutlined } from '@ant-design/icons';
import { getComponent } from '../../../../Component/WidgetFactory';
import DesignerFieldProp from "./DesignerFieldProp";
import { openDialog } from "../../../../Component/UI/Dialog";
import ActionLink from '../../../../Component/ActionLink';
import LinkProcesser from '../../../../Core/Utils/LinkProcesser';
import { FieldPanel, DropBox, MoveBox } from "./FieldPanel";

export default class DesignerPage extends Component<any,any> {
    designerContext: any
    
    _ix: number
    fieldPropDlg: any
    constructor(props: any) {
        super(props);

        this.state = {
            Header: {
                Id: "header",
                Text: "",
                Groups: [{ Id: "gh", Rows: [ { Cols: [ ] } ] }]
            },
            Pages: [{ Id: "page1",
                Groups: [{ Id: "g1", Rows: [ { Cols: [ ] }] }]
            }],
            fieldContainerLink: {

            },

          fields: []
        }

        this._ix = 0;

        this.designerContext = {
            renderGroupSetting: this.renderGroupSetting
        };

    }

    linkField = (fieldId: string, containerId: string) => {
        const { Fields: entityFields } = this.props.data;

        const fieldInfo = _.find(entityFields, { Name: fieldId });
        this.setState({ fields : update(this.state.fields, {$push: [{ WidgetId: fieldId, WidgetType: fieldInfo.WidgetType, FieldType: fieldInfo.Type }]}) }, () => {
            this.setState({ fieldContainerLink: Object.assign({}, this.state.fieldContainerLink, { [containerId]: fieldId } ) });
        });        
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
          wInfo =  _.find(this.state.fields, { WidgetId: fieldId }) as any;
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
                                  <FormField widgetInfo={{ WidgetId: fieldId, ...wInfo }} dataModel={{}} >
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
        if (!this.props.data) return;

        const { Fields } = this.props.data;
        return (
          <div>
            {_.map(Fields, (item) => {
                const isReadOnly = _.values(this.state.fieldContainerLink).indexOf(item.Name) >= 0;

                return (<DropBox field={item} readOnly={isReadOnly} addField={(fieldId: string, boxId: string) => {
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
          <a href="javascript:void(0)" onClick={() => this.addgroupRow(pageid, group, 2)}> Add Row</a>
       </Menu.Item>);
        pageMenues.push(<Menu.Item>
          <a href="javascript:void(0)" onClick={() => this.addgroupRow(pageid, group, 3)}> Add 3 Fields Row</a>
       </Menu.Item>);
        pageMenues.push(<Menu.Item>
          <a href="javascript:void(0)" onClick={() => this.addgroupRow(pageid, group, 1)}> Add 1 Field Row</a>
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
      const selectedF = _.find(this.state.fields, { WidgetId: widgetId });

      this.fieldPropDlg = openDialog("Field", () => { return <DesignerFieldProp
          selectedField={selectedF}
          setFieldProp={this.setWidgetProp}
        />; 
      }, { hideCommands: true, size: "lg" });
    }

    setWidgetProp = (toSaveModel: any) => {
      const id = toSaveModel.Widgets["WidgetId"].Value;
      const ix = _.findIndex(this.state.fields, { WidgetId: id });
      const selectedF = this.state.fields[ix];
      
      const blankObj: any = {};
      _.forIn(toSaveModel.Widgets, (v, k) => {
        blankObj[k] = v.Value;
      });

      this.setState({ fields: update(this.state.fields, { [ix]: { $set: Object.assign({}, selectedF, blankObj) } }) });   

      this.fieldPropDlg.cleanup();
    }

    addgroupRow = (pageid: string, group: any, fieldCount: number) => {
        const newRow: any = { Cols: [] };
        const span = 24/fieldCount;
        for(let x=0;x<fieldCount;x++) {
          newRow.Cols.push({ Id: `id${this._ix++}`, Span: span });
        }

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
      const newRow = { Id: `g${this._ix++}`, Label: "Group", Rows: [ { Cols: [ ] } ] };
      const pageix = _.findIndex(this.state.Pages, { Id: pageid });

      const h = update(this.state.Pages[pageix], { Groups: { $push: [newRow] } });
      this.setState({ Pages: update(this.state.Pages, { [pageix]: { $set: h } }) });      
    }

    executeAction = (action: ActionInfo) => {
        const { ActionId } = action;
        if (ActionId === "BTN_SAVE") {
            const { Header, Pages, fields } = this.state;
            const data = { Layouts: { Header, Pages }, Fields: fields };
            //prepare data. adjust fieldContainerLink

            LinkProcesser.processLink(action, data, this.context.navigator);
        }
    }

    getFormActions = () => {
        const { Actions } = this.props.data;

        return  _.map(Actions, (c) => {
                return <ActionLink key={c.ActionId} {...c} DisplayType={2} onClick={this.executeAction} />;
            });        
    }

    render() {
        const { Header, Pages} = this.state;

        const entityModel = { Layout: { Header, Pages }, PageTitle: { PageTitle: "Form Name" } };

        return (<div>
            {
                this.getFormActions()
            }
            <DndProvider backend={Backend}>
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
        </div>
        );
    }
}