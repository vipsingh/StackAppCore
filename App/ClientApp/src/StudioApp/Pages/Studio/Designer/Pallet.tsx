import React, { Component } from 'react';
import _ from "lodash";
import { DropBox } from "./FieldPanel";
import { Divider, List } from 'antd';

export default class DesignerFieldProp extends Component<any, any> {
    
    render() {

        const { Fields, Buttons, linkField, linkButton, isReadonly } = this.props;

        return (<div>
            <Divider orientation="left">Field</Divider>
            <List
                size="small"
                bordered
                dataSource={Fields}
                renderItem={(item: any) => (<DropBox type="FIELDBOX" field={item} readOnly={isReadonly("FIELD", item.Name)} addField={(item: any, boxId: string) => {
                    linkField(item, boxId);
                }} />)}
            />            
            <Divider orientation="left">Button</Divider>
            <List
                size="small"
                bordered
                dataSource={Buttons}
                renderItem={(item: any) => (<DropBox type="BUTTONBOX" field={item} readOnly={isReadonly("BUTTON", item.Id)} addField={(item: any, boxId: string) => {
                    linkButton(item, boxId);
                }} />)}
            />
          </div>
        );
    }
}