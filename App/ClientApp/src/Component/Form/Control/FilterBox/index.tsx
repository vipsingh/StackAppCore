// import React from "react";
// import { Button, Row, Col } from "antd";
// import update from "immutability-helper";
// import Form from "../../Form";
// import ViewPageInfo from "../../../../Core/Models/ViewPageInfo";
// import _ from "lodash";

// export default class FilterBox extends React.Component<any, {
//     Filters: Array<ViewPageInfo>
// }> {
//     schema: any
//     constructor(p: any) {
//         super(p);

//         this.state = {
//             Filters: []
//         }

//         this.schema = {
//             Widgets: {                
//                 Field: { WidgetId: "Field", WidgetType: 1, Caption: "" },                
//                 Op: { WidgetId: "Op", WidgetType: 6, Caption: "", Options: [] },
//                 Value: { WidgetId: "Value", WidgetType: 1, Caption: "" }
//             },
//             Rules: [
//                 //{ Index: 1, Type: "INVISIBLE", Criteria: [{ FieldName: "FieldType", Op: 8, Value: [10,20] }], Fields: ["LinkEntity"] }                
//             ],
//             EntityInfo: { ObjectId: 0 }
//         };
//     }

//     mergeFields(schema: any, formData: any) {        
//         const entityModel: ViewPageInfo = new ViewPageInfo(this.schema, formData);
//         return entityModel;
//     }  

//     addFilter = () => {
//         const m = this.mergeFields(this.props.Schema, null);                
//         this.setState({ Filters: update(this.state.Filters, {$push: [m] })});
//     }

//     removeRowItem = (formId: string) => {

//     }

//     onModelChange = (model: ViewPageInfo) => {
//         const ix = _.findIndex(this.state.Filters, { UniqueId: model.UniqueId });
//         this.setState({ Filters: update(this.state.Filters, {[ix]: { $set: model } })});
//     }

//     renderFilterRow(model: ViewPageInfo) {
//         if (!model) return;

//         return(
//             <Form
//                     entityModel={model}
//                     onFormUpdate={this.onModelChange}
//                     onSubmit={() => {}}                    
//                     render={
//                         ({ entityModel, getControl }: any) => {
//                             return <Row>
//                                 <Col span={8}>{getControl("Field")}</Col>
//                                 <Col span={6}>{getControl("Op")}</Col>
//                                 <Col span={7}>{getControl("Value")}</Col>
//                                 <Col span={3}>{<Button type="link" onClick={this.removeRowItem.bind(this, entityModel.UniqueId)}>Remove</Button>}</Col>
//                             </Row>;
//                         }
//                     }
//                 />
//         );
//     }

//     render() {

//         return (<div>
//             <Button onClick={this.addFilter}>Add</Button>
//             {
//                 _.map(this.state.Filters, f => {
//                     return this.renderFilterRow(f);
//                 })
//             }
//         </div>);
//     }
// }
export const x = "";