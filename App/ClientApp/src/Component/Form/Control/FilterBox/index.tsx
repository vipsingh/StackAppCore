import React from "react";
import { Button, Row, Col } from "antd";
import update from "immutability-helper";
import Form from "../../Form";
import ViewPageInfo from "../../../../Core/Models/ViewPageInfo";
import _ from "lodash";
import { EntityPicker } from "../input";
import { prepareWidgetRequest } from "../../../../Core/Form/Utils/FormUtils";

export default class FilterBox extends React.Component<WidgetInfoProps> {

    componentDidMount() {
        const { DataActionLink, api, WidgetId, Value } = this.props as any;
        if (Value && !_.isArray(Value) && DataActionLink) {
            let req;
            if (api) req = api.prepareFieldRequest(WidgetId);
            else req = prepareWidgetRequest(this.props);

            window._App.Request.getData({
                url: DataActionLink.Url,
                type: "POST",
                body: req
            }).then((res: any) => {
                this.addFilter(res, []);
            }).finally(() => {
                //this.setState({IsFetching: false});
            });
        }
    }

    onFieldSelect = (val: any) => {
        const { FilterFormLink, api, WidgetId, Value } = this.props as any;
        if (FilterFormLink) {
            let req;
            if (api) req = api.prepareFieldRequest(WidgetId);
            else req = prepareWidgetRequest(this.props);

            req.Value = Object.assign({}, val);

            window._App.Request.getData({
                url: FilterFormLink.Url,
                type: "POST",
                body: req
            }).then((res: any) => {
                this.addFilter(res, Value? Value : []);
            }).finally(() => {
                //this.setState({IsFetching: false});
            });
        }
    }

    addFilter = (response: any, Value: any[]) => {
        if (response.Filters && response.Filters.length > 0) {
            let values = Value;
            _.each(response.Filters, filter => {
                const entityModel: ViewPageInfo = new ViewPageInfo(filter, null);
                let val = { _model: entityModel.getDataModel(), _schema: entityModel };
                values = update(values, {
                    $push: [val]
                })
            });

            this.props.onChange(values);
        }
    }

    removeRowItem = (formId: string) => {
        const { Value } = this.props;
        const ix = _.findIndex(Value, (o: any) => o._model._UniqueId.Value === formId);
        this.props.onChange(update(Value, { $splice: [[ix, 1]] }));
    }

    onModelChange = (model: IDictionary<IFieldData>) => {
        const { Value } = this.props;
        const ix = _.findIndex(Value, (o: any) => o._model._UniqueId.Value === model._UniqueId.Value);
        const val = Value[ix];

        this.props.onChange(update(Value, {
            [ix]: { $set: { _model: model , _schema: val._schema } }
        }));
    }

    renderFilterRow(schema: ViewPageInfo, model: IDictionary<IFieldData>) {
        if (!model) return;

        return(
            <Form
                    entityModel={schema}
                    dataModel={model}
                    onFormUpdate={this.onModelChange}
                    onSubmit={() => {}}                    
                    render={
                        ({ dataModel, getControl }: any) => {
                            return <Row>
                                <Col span={8}>{getControl("Name", { Caption: null })}</Col>
                                <Col span={6}>{getControl("OP", { Caption: null })}</Col>
                                <Col span={7}>{getControl("Field", { Caption: null })}</Col>
                                <Col span={3}>{<Button type="link" onClick={this.removeRowItem.bind(this, dataModel._UniqueId.Value)}>Remove</Button>}</Col>
                            </Row>;
                        }
                    }
                />
        );
    }

    render() {
        const { Value } = this.props;
        const { SourcePicker } = this.props as any;

        return (<div>
            <EntityPicker {...SourcePicker} Value={null} onChange={this.onFieldSelect} />
            {
                _.map(Value, f => {
                    return this.renderFilterRow(f._schema, f._model);
                })
            }
            </div>);
    }
}
