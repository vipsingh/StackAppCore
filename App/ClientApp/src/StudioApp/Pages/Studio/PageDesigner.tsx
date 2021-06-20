import React, { Component, useState, useEffect } from 'react';
import { Row, Col, Card } from 'antd';
import update from "immutability-helper";
import { DndProvider, useDrag, DragSourceMonitor, useDrop } from 'react-dnd'
import Backend from 'react-dnd-html5-backend'
import _ from 'lodash';
//import { openDialog } from "../../../Component/UI/Dialog";
import { LayoutView } from "../../../Component/Layout/LayoutView";
import { useForm, FormX, FormXField } from "../../../Core/Form";
import ViewPageInfo from '../../../Core/Models/ViewPageInfo';
import { fieldTypes } from "./helper";
import Form from '../../../Component/Form/Form';
import SimpleLayout from '../../../Component/Layout/SimpleLayout';

const _layout = {
    id: "div_1",
    tagName: "div",
    class: "",
    style: {},
    attrs: {},
    components: [
        {tagName: "h1", class: "c-1", text: "header"},
        {tagName: "br"},
        {tagName: "div", class: "c-2", components: [
            {tagName: "label", class: "c-1", text: "hello"}
        ] }
    ]    
};

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
                <LayoutView layout={_layout} />
            </div>
            </Col>
            </Row>

        </DndProvider>);
    }
}

export const PageX: React.FC<any> = () => {

  const schema = {
    Widgets: {
        Id: { WidgetId: "Id", WidgetType: 10, Caption: "Id" },
        FieldName: { WidgetId: "FieldName", WidgetType: 1, Caption: "Name", IsRequired: true },
        Label: { WidgetId: "Label", WidgetType: 1, Caption: "Label" },
        FieldType: { WidgetId: "FieldType", WidgetType: 6, Caption: "Type", Options: fieldTypes, RuleToFire: [1] },
        Length: { WidgetId: "Length", WidgetType: 1, Caption: "Length" },
        IsRequired: { WidgetId: "IsRequired", WidgetType: 4, Caption: "IsRequired" },
        LinkEntity: { WidgetId: "LinkEntity", WidgetType: 1, Caption: "LinkEntity", Features: {
          "Invisible": {
              "Criteria": {
                  "$and": [
                      {"FieldType": [8, [10,20]]}
                  ]
              },
              "Depends": ["FieldType"]
          }
      }, Hidden_Exp: { Index: 1, Type: "HIDDEN", Criteria: [{ FieldName: "FieldType", Op: 8, Value: [10,20] }], Fields: ["LinkEntity"] } },
        OtherSetting: { WidgetId: "OtherSetting", WidgetType: 1, Caption: "OtherSetting" }
    },
    FormRules: [
        { Index: 1, Type: "HIDDEN", Criteria: [{ FieldName: "FieldType", Op: 8, Value: [10,20] }], Fields: ["LinkEntity"] },
    ],
    Actions: {
        SAVE: {
            ActionId: "SAVE",
            DisplayType: 2,
            ExecutionType: 1,
            Title: "Save"
        }
    },
    EntityInfo: { ObjectId: 0 }
};

  var entitySchema = new ViewPageInfo(schema);
  const [state, setState] = React.useState({ dataModel: entitySchema.getDataModel() });
  const onSubmit = (data: any) => {

  }

  return <FormX 
          entitySchema={entitySchema} 
          formData={state.dataModel}
          onSubmit={onSubmit}
          render={
            (formO: any) => {
                return (<div>
                  <div><FormXField id="FieldType" /></div>
                  <div><FormXField id="LinkEntity" /></div>
                  {/* <button title="Submit" onClick={formO.handleSubmit(onSubmit)} /> */}
                  {JSON.stringify(formO.dataModel)}
                </div>);
            }
          } 
          />
}

const useFormX = () => {

}


export const insert = (
    arrayLike: ArrayLike<any>,
    index: number,
    value: any
  ) => {
    const copy = copyArrayLike(arrayLike);
    copy.splice(index, 0, value);
    return copy;
  };

  export const replace = (
    arrayLike: ArrayLike<any>,
    index: number,
    value: any
  ) => {
    const copy = copyArrayLike(arrayLike);
    copy[index] = value;
    return copy;
  };

  export const move = (array: any[], from: number, to: number) => {
    const copy = copyArrayLike(array);
    const value = copy[from];
    copy.splice(from, 1);
    copy.splice(to, 0, value);
    return copy;
  };
  
  export const swap = (
    arrayLike: ArrayLike<any>,
    indexA: number,
    indexB: number
  ) => {
    const copy = copyArrayLike(arrayLike);
    const a = copy[indexA];
    copy[indexA] = copy[indexB];
    copy[indexB] = a;
    return copy;
  };

  const copyArrayLike = (arrayLike: ArrayLike<any>) => {
    if (!arrayLike) {
      return [];
    } else if (Array.isArray(arrayLike)) {
      return [...arrayLike];
    } else {
      const maxIndex = Object.keys(arrayLike)
        .map(key => parseInt(key))
        .reduce((max, el) => (el > max ? el : max), 0);
      return Array.from({ ...arrayLike, length: maxIndex + 1 });
    }
  }; 


  const ArrayHelper = {
    insert,
    replace,
    move,
    swap,
    copyArrayLike
}
