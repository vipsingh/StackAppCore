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
import { SettingOutlined, DeleteOutlined } from '@ant-design/icons';
import { getComponent } from '../../../../Component/WidgetFactory';
import DesignerFieldProp from "./DesignerFieldProp";
import { openDialog } from "../../../../Component/UI/Dialog";
import ActionLink from '../../../../Component/ActionLink';
import LinkProcesser from '../../../../Core/Utils/LinkProcesser';
import { FieldPanel, MoveBox } from "./FieldPanel";
import Pallet from "./Pallet";

export default class DesignerPage extends Component<any,any> {
    designerContext: any
    
    _ix: number
    fieldPropDlg: any
    constructor(props: any) {
        super(props);
        this._ix = 0;

        this.state = this.prepareState(props);                

        this.designerContext = {
            renderGroupSetting: this.renderGroupSetting
        };

    }

    prepareState(props: any) {
      const { LayoutInfo, LayoutFields } = props.data;
      const d = _.cloneDeep(LayoutInfo);
      if (!d.Header) d.Header = {Id: "header", Groups: [{ Id: "gh", Rows: []}]};
      if (!d.Pages || d.Pages.length === 0) d.Pages = [{Id: "p1", Groups: [{ Id: "g1", Rows: []}]}];
      d.Fields = !LayoutFields ? [] : LayoutFields;

      d.fieldContainerLink = {};
      _.each(d.Header.Groups, (g) => {
        this.initFieldContainerLink(g, d.fieldContainerLink);
      });

      _.each(d.Pages, (p) => {
        if (!p.Id) p.Id = `p${this._ix++}`;
        _.each(p.Groups, (g) => {
          this.initFieldContainerLink(g, d.fieldContainerLink);
        });
      });

      return d;
    }

    initFieldContainerLink(g: any, fieldContainerLink: any) {
      if(!g || !g.Rows) return;
      if (!g.Id) g.Id = `g${this._ix++}`;
      
      _.each(g.Rows, (r) => {
        if(!r.Cols) return;
        _.each(r.Cols, (c) => {
          if (c.Id) fieldContainerLink[c.Id] = c.Id;
          else {
            c.Id = `id${this._ix++}`;
            fieldContainerLink[c.Id] = null;
          }
        });
      });
    }

    linkField = (fieldInfo: any, containerId: string) => {
        if (_.find(this.state.Fields, { FieldId: fieldInfo.Name })) {
          this.setState({ fieldContainerLink: Object.assign({}, this.state.fieldContainerLink, { [containerId]: fieldInfo.Name } ) });
        } else {
          this.setState({ Fields : update(this.state.Fields, {$push: [{ FieldId: fieldInfo.Name, WidgetType: fieldInfo.WidgetType, FieldType: fieldInfo.Type }]}) }, () => {
              this.setState({ fieldContainerLink: Object.assign({}, this.state.fieldContainerLink, { [containerId]: fieldInfo.Name } ) });
          });     
        }   
    }
    moveField = (fieldId: string, containerId: string | null) => {        
        let lastCont = "";
        _.forIn(this.state.fieldContainerLink, function(value, key) {
            if (value === fieldId) lastCont = key;
        });
        if (containerId) {
          this.setState({ fieldContainerLink: Object.assign({}, this.state.fieldContainerLink, { [containerId]: fieldId, [lastCont]: undefined } ) });
        } else {
          this.setState({ fieldContainerLink: Object.assign({}, this.state.fieldContainerLink, { [lastCont]: undefined } ) });
        }
    }

    isReadonlyPalletItem = (type: string, id: string) => {
      if (type === "FIELD") {
        return _.values(this.state.fieldContainerLink).indexOf(id) >= 0;
      }
      else if (type === "BUTTON") {
        return !!_.find(this.state.Commands, { Id: id });
      }

      return false;
    }
    
    getControl = (container: string) => {
        let fieldId = this.state.fieldContainerLink[container];

        let IComponent: any;
        let wInfo: any = {};
        if (fieldId) {
          wInfo =  _.find(this.state.Fields, { FieldId: fieldId }) as any;
          if (!wInfo.Text) wInfo.Text = wInfo.FieldId;
        
          IComponent = getComponent(wInfo.WidgetType, false); 
        }
        
        return (<FieldPanel panelType="FIELDBOX" Id={container} FieldId={fieldId}>
                    {!fieldId || 
                        <MoveBox item={{ id: fieldId }} moveField={(id: string, fieldBoxId: string) => { 
                            this.moveField(id, fieldBoxId);
                          }}>
                              <div>
                                  {this.renderFieldSettingIcon(fieldId)}
                                  <FormField widgetInfo={{ WidgetId: fieldId, Caption: wInfo.Text, ...wInfo }} dataModel={{}} >
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

    getLayoutActions = () => {
      const { Commands } = this.state;
      let cmds: any[] = [];
      if (Commands) {
        cmds =  _.map(Commands, (c: any) => {
              return (
                  <ActionLink key={c.Id} Title={c.Text} {...c} DisplayType={2} />
              );
          });        
      }

      cmds.push(<FieldPanel panelType="BUTTONBOX" Id={"buttonContainer"}></FieldPanel>);

      return cmds;
      
  }

  linkButton = (buttonInfo: any, containerId: string) => {

    if (this.state.Commands) {
      this.setState({ Commands: update(this.state.Commands, {$push: [buttonInfo]}) });
    } else {
      this.setState({ Commands: [buttonInfo] });
    }
  }

    renderFieldSettingIcon = (fieldId: string) => {
      if (!fieldId) return;

      return (<div style={{ position: "absolute", zIndex: 1 }}>
                <a href="javascript:void(0)" className="ant-dropdown-link" onClick={e => this.onWidgetSelect(fieldId)}>
                    <SettingOutlined />
                </a>
                <a href="javascript:void(0)" className="ant-dropdown-link" onClick={e => this.onFieldRemove(fieldId)}>
                    <DeleteOutlined />
                </a>
                </div>)
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

    onWidgetSelect = (fieldId: string) => {
      const selectedF = _.find(this.state.Fields, { FieldId: fieldId });

      this.fieldPropDlg = openDialog("Field", () => { return <DesignerFieldProp
          selectedField={selectedF}
          setFieldProp={this.setWidgetProp}
        />; 
      }, { hideCommands: true, size: "lg" });
    }

    onFieldRemove = (fieldId: string) => {
      const { Fields } = this.state;
      
      this.moveField(fieldId, null);
      const ix = _.findIndex(Fields, { FieldId: fieldId });
      
      this.setState({ Fields : update(Fields, {$splice: [[ix, 1]] })});
    }

    setWidgetProp = (toSaveModel: any) => {
      const id = toSaveModel.Widgets["FieldId"].Value;
      const ix = _.findIndex(this.state.Fields, { FieldId: id });
      const selectedF = this.state.Fields[ix];
      
      const blankObj: any = {};
      _.forIn(toSaveModel.Widgets, (v, k) => {
        blankObj[k] = v.Value;
      });

      this.setState({ Fields: update(this.state.Fields, { [ix]: { $set: Object.assign({}, selectedF, blankObj) } }) });   

      this.fieldPropDlg.cleanup();
    }

    addgroupRow = (pageid: string, group: any, fieldCount: number) => {
        const newRow: any = { Cols: [] };
        const span = 24/fieldCount;
        for(let x=0; x < fieldCount; x++) {
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
            const { Fields } = this.state;

            const data = { Layout: this.rectifyDataForSave(), Fields };            

            LinkProcesser.processLink(action, data, this.context.navigator);
        }
    }

    rectifyDataForSave() {
      const { Header, Pages, Commands, fieldContainerLink } = this.state;
      const cHeader = _.cloneDeep(Header);
      const cPages = _.cloneDeep(Pages);

      const setFieldId = (groups: Array<any>) => {
        _.each(groups, (g) => {
          if  (!g.Rows) return;
          _.each(g.Rows, (r) => {
            _.each(r.Cols, (c) => {
              if (c.Id) {
                c.Id = fieldContainerLink[c.Id];
              }
            });
          });
        });
      };

      if (cHeader && cHeader.Groups) {
        setFieldId(cHeader.Groups);
      }

      if (cPages) {
        _.each(cPages, (p) => {
          setFieldId(p.Groups);
        });        
      }

      return  { Header: cHeader, Pages: cPages, Commands };
    }

    getFormActions = () => {
        const { Actions } = this.props.data;

        return  _.map(Actions, (c) => {
                return <ActionLink key={c.ActionId} {...c} DisplayType={2} onClick={this.executeAction} />;
            });        
    }

    render() {
        const { Header, Pages} = this.state;
        const { Pallet: pallet } = this.props.data;

        const entityModel = { Layout: { Header, Pages }, PageTitle: { PageTitle: "Form Name" } };

        return (<div>
            {
                this.getFormActions()
            }
            <DndProvider backend={Backend}>
            <DesignerContext.Provider value={this.designerContext}>
            <Row>
            <Col span={4}>
              <Card size="small" title={"Pallet"}>
                <Pallet {...pallet} linkField={this.linkField} linkButton={this.linkButton} isReadonly={this.isReadonlyPalletItem} />
              </Card>
            </Col>
            <Col span={20}>
                <PageView getControl={this.getControl} entityModel={entityModel} getFormActions={this.getLayoutActions} />
            </Col>
            </Row>
            </DesignerContext.Provider>
        </DndProvider>
        </div>
        );
    }
}