import React from 'react';
import Form, { ViewPageInfo } from "../../../Component/Form/Form";
import SimpleLayout from '../../../Component/Layout/SimpleLayout';
import ActionLink from '../../../Component/ActionLink';

export default class EntityPage extends React.Component<any, any> {
    Schema: any
    constructor(p: any) {
        super(p);

        var eModel = new ViewPageInfo({
            Widgets: {
                Name: { WidgetId: "Name", WidgetType: 1, Caption: "Name" },
                Text: { WidgetId: "Text", WidgetType: 1, Caption: "Text" },
                DbStore: { WidgetId: "DbStore", WidgetType: 4, Caption: "DbStore", Value: true }
            }
        })

        this.state = {
            entityModel: eModel
        };
    }

    onModelChange = (model: ViewPageInfo) => {
        this.setState({ entityModel: model });
    }

    handleSubmit = (model: any) => {
        this.props.onChangeView(false);
    }

    render() {

        return(
            <Form
                entityModel={this.state.entityModel}
                onChange={this.onModelChange}
                onSubmit={this.handleSubmit}
                render={
                    (prop: any) => {
                        return (<div>
                           <SimpleLayout 
                               Fields={this.state.entityModel.Widgets}
                               getControl={prop.getControl}
                           />
                           <ActionLink ActionId={"BTN_SUBMIT"} DisplayType={2} Title={"Save"} ActionType={1} onClick={this.handleSubmit} />
                           </div>
                        );
                    }
                }
             />
        )
    }
}