import React, { Component } from 'react';
import Form from '../../../../Component/Form/Form';
import SimpleLayout from '../../../../Component/Layout/SimpleLayout';
import _ from "lodash";
import ViewPageInfo from '../../../../Core/Models/ViewPageInfo';
import { FormControlType } from '../../../../Constant';

export default class DesignerFieldProp extends Component<{
    setFieldProp: Function,
    selectedField: any
}, any> {
    schema: any
    constructor(props: any) {
        super(props);
        this.schema = {
            Widgets: {
                WidgetId: { WidgetId: "WidgetId", WidgetType: FormControlType.Label, Caption: "WidgetId", IsHidden: true },
                Caption: { WidgetId: "Caption", WidgetType: FormControlType.TextBox, Caption: "Caption" },
                IsMandatory: { WidgetId: "IsMandatory", WidgetType: FormControlType.CheckBox, Caption: "IsMandatory" },
                WidgetStyle: { WidgetId: "WidgetStyle", WidgetType: FormControlType.LongText, Caption: "WidgetStyle" }
            },
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