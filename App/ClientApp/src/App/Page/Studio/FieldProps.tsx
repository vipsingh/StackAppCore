import React, { Component } from 'react';
import Form from '../../../Component/Form/Form';
import SimpleLayout from '../../../Component/Layout/SimpleLayout';
import _ from "lodash";
import { fieldTypes } from "./helper";
import ViewPageInfo from '../../../Core/Models/ViewPageInfo';
import ListForm from '../../../Component/Form/Control/ListForm';
//import FilterBox from '../../../Component/Form/Control/FilterBox';

export default class FieldProps extends Component<{
    setFieldProp: Function,
    selectedField: any
}, any> {

    schema: any
    constructor(props: any) {
        super(props);
        this.schema = {
            Widgets: {
                Id: { WidgetId: "Id", WidgetType: 10, Caption: "Id" },
                FieldName: { WidgetId: "FieldName", WidgetType: 1, Caption: "Name", IsRequired: true },
                Label: { WidgetId: "Label", WidgetType: 1, Caption: "Label" },
                FieldType: { WidgetId: "FieldType", WidgetType: 6, Caption: "Type", Options: fieldTypes, RuleToFire: [1] },
                Length: { WidgetId: "Length", WidgetType: 1, Caption: "Length" },
                IsRequired: { WidgetId: "IsRequired", WidgetType: 4, Caption: "IsRequired" },
                LinkEntity: { WidgetId: "LinkEntity", WidgetType: 1, Caption: "LinkEntity" },
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
        //{ Index: 2, Type: "OPTIONS", Criteria: [{ FieldName: "IsRequired", Op: 0, Value: true }], Field: "FieldType", Options: [3,4,7,8] }

        var eModel = new ViewPageInfo(this.schema);

        const { selectedField } = this.props;
        const { Widgets } = eModel;            
        _.forIn(Widgets, (v, k) => {
            v.Value = selectedField[k];
        });
            
        this.state = {
            entityModel: eModel,
            dataModel: eModel.getDataModel()
        };
    }

    onModelChange = (model: IDictionary<IFieldData>, params: { updateBy: string, param?: string }) => {
        this.setState({ dataModel: model });
    }

    handleSubmit = (toSaveModel: any) => {
        this.props.setFieldProp(toSaveModel);
    }

    render() {

        //return (<FilterBox />);
        //return (<ListForm Schema={this.schema}  />);

        return (
            <Form
                entityModel={this.state.entityModel}
                dataModel={this.state.dataModel}
                onFormUpdate={this.onModelChange}
                onSubmit={this.handleSubmit}
                render={
                    (prop: any) => {
                        return (<div>
                            <SimpleLayout
                                Fields={this.state.entityModel.Widgets}
                                getControl={prop.getControl}
                            />
                            {prop.renderActions()}
                        </div>
                        );
                    }
                }
            />
        )
    }    
}