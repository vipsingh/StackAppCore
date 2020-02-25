import React, {Component} from 'react';
import Form, { ViewPageInfo } from '../../../Component/Form/Form';
import SimpleLayout from '../../../Component/Layout/SimpleLayout';
import ActionLink from '../../../Component/ActionLink';

export default class FieldProps extends Component<{
    setFieldProp: Function,
    selectedField: any
  }, any> {
    
    constructor(props: any) {
      super(props);
      var eModel = new ViewPageInfo({
        Widgets: {
            Name: { WidgetId: "Name", WidgetType: 1, Caption: "Name" },
            Label: { WidgetId: "Label", WidgetType: 1, Caption: "Label" },
            Type: { WidgetId: "Type", WidgetType: 1, Caption: "Type", IsDisabled: true },
            IsRequired: { WidgetId: "IsRequired", WidgetType: 4, Caption: "IsRequired" },
            RefObject: { WidgetId: "RefObject", WidgetType: 1, Caption: "RefObject" }
        }
    })

    this.state = {
        entityModel: eModel
    };      
    }
    // /visible: () => this.props.selectedField.Type === 10

    // fChange = (f: any, v: any) => {
    //     if (f.IsDisabled)
    //         return;
    //     this.props.setFieldProp(f.Name, v);
    // }

    onModelChange = (model: ViewPageInfo) => {
        this.setState({ entityModel: model });
    }

    handleSubmit = (model: any) => {
        
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
    // getFieldValue(x: string) {
    //     const form = this.props.selectedField;
    //     return ( form[x + "__text"]? form[x + "__text"]: form[x] );
    // }

    // renderF(field: any) {
    //     const form = this.props.selectedField;
    //     if (!form)
    //         return;

    //     if(field.Type === 1){
    //         return (<Input value={this.getFieldValue(field.Name)} onChange={(e) => { this.fChange(field, e.target.value); }} /> );
    //     } else if(field.Type === 3){
    //         return (<Checkbox checked={!!form[field.Name]} onChange={(e) => { this.fChange(field, e.target.checked); }} /> );
    //     }
    // }
}