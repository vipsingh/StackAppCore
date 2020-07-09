import React, { Component } from 'react';
import { Tabs, Row, Col, Card, Button } from 'antd';
import { Menu, Dropdown } from 'antd';
import update from "immutability-helper";
import { DndProvider, useDrag, DragSourceMonitor, useDrop } from 'react-dnd'
import Backend from 'react-dnd-html5-backend'
import _ from 'lodash';
import { openDialog } from "../../../Component/UI/Dialog";

var blockItems = [];
export default class DesignerPage extends Component<any,any> {
    designerContext: any
    _ix: number
    fieldPropDlg: any
    constructor(props: any) {
        super(props);

        this.state = {

        }

        this._ix = 0;
    }

    renderBlockItems() {
        // return (
        //   <div>
        //     {_.map(fields, (item) => {
        //         const isReadOnly = _.values(this.state.fieldContainerLink).indexOf(item.WidgetId) >= 0;
        //       return (<Box field={item} readOnly={isReadOnly} addField={(fieldId: string, boxId: string) => {
        //           this.linkField(fieldId, boxId);
        //         }} />
        //       );
        //     })}
        //   </div>
        // );
    }

    render() {
    
        return (<DndProvider backend={Backend}>
            <Row>
            <Col span={4}>
              <Card size="small" title={"Fields"}>
                
              </Card>
            </Col>
            <Col span={20}>
            <div style={{ width: "100%", minHeight: "100px" }}>

            </div>
            </Col>
            </Row>

        </DndProvider>);
    }
}