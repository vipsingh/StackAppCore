import React from "react";
import Form from "../../Form";
import ViewPageInfo from "../../../../Core/Models/ViewPageInfo";
import _ from "lodash";
import { Button, Table, Space } from "antd";
import update from "immutability-helper";
//import { getFormDataToSubmit } from "../../../../Core/Form/Utils/FormUtils";
import { getComponent } from "../../../WidgetFactory";
import { EditFilled, SaveFilled, DeleteFilled } from '@ant-design/icons';

export interface ListFormProps {
    Schema: any
}

export default class ListForm extends React.Component<ListFormProps, 
    { 
        data: Array<IDictionary<IFieldData>>,
        activeKey: string,
        activeModel: IDictionary<IFieldData>,
        IsUnCommitedData: boolean
    }
> {
    schema: ViewPageInfo
    constructor(props: any, cntx: any) {
        super(props, cntx);

        this.schema = this.mergeFields(this.props.Schema, null);
        
        this.state = {
            data: [],
            activeKey: "",
            IsUnCommitedData: false,
            activeModel: this.schema.getDataModel() 
        };
    }

    mergeFields(schema: any, formData: any) {        
        const entityModel: ViewPageInfo = new ViewPageInfo(this.props.Schema, formData);
        return entityModel;
    }  
    
    addItem = () => {
        const d = this.schema.getDataModel();
        this.setState({ data: update(this.state.data, {$push: [d] }), activeModel: d, activeKey: d._UniqueId.Value, IsUnCommitedData: true});
    }
    
    editItem = (formId: string) => {
        const data = this.getModel(formId);
        this.setState({ activeKey: data._UniqueId.Value, activeModel: data});
    }

    deleteRowItem = (formId: string) => {
        const ix = _.findIndex(this.state.data, o => o._UniqueId.Value === formId);
        this.setState({ data: update(this.state.data, { $splice: [[ix, 1]] }), activeKey: "", IsUnCommitedData: false});
    }

    cancelEdit = () => {
        const { activeKey, IsUnCommitedData } = this.state;        
        if (IsUnCommitedData) {
            this.deleteRowItem(activeKey);
        } else {
            this.setState({ activeKey: "", IsUnCommitedData: false });
        }
    }

    onModelChange = (model: IDictionary<IFieldData>, params: { updateBy: string, param?: string }) => {  
        const ix = _.findIndex(this.state.data, o => o._UniqueId.Value === model._UniqueId.Value);
        this.setState({ data: update(this.state.data, {[ix]: { $set: model }}), IsUnCommitedData: true});        
        this.setState({ activeModel: model });
    }

    saveItem = (toSaveModel: any) => {
        const ix = _.findIndex(this.state.data, o => o.UniqueId === this.state.activeKey);        
        this.setState({ activeKey: "", IsUnCommitedData: false });
    }

    renderList(prop: any) {

        return (<div></div>);
    }

    getModel(key: string): IDictionary<IFieldData> {
        const v = _.filter(this.state.data, x => x._UniqueId.Value === key);

        return v[0] as IDictionary<IFieldData>;
    }
    
    render() {
        const { activeKey, data, activeModel } = this.state;        

        return(
            <div>
                <div>
                    <Button disabled={ !!activeKey } onClick={this.addItem}>Add</Button>
                </div>
                <Form
                    entityModel={this.schema}
                    onFormUpdate={this.onModelChange}
                    dataModel={activeModel}
                    onSubmit={this.saveItem}
                    render={
                        (prop: any) => {
                            return <ListFormGrid 
                                {...prop}  
                                activeFormId={activeKey}
                                listData={data}
                                editRowItem={this.editItem}
                                deleteRowItem={this.deleteRowItem}
                                cancelRowEdit={this.cancelEdit}
                                />;
                        }
                    }
                />
             </div>
        );
    }

    static validate = (widgetInfo: WidgetInfoProps) => {
        
    }
}


class ListFormGrid extends React.Component<any, any> {
    constructor(props: any) {
        super(props);

        this.state = {
            columns: this.prepareColumns()
        }
    }

    saveRow = () => {
        const { handleSubmit } = this.props;

        handleSubmit();
    }
    editRow(formId: string) {
        this.props.editRowItem(formId);
    }
    deleteRow = (formId: string) => {
        this.props.deleteRowItem(formId);
    }
    cancelRow = () => {
        this.props.cancelRowEdit();
    }

    prepareColumns() {
        const { entityModel: { Widgets } } = this.props;
        const cols: Array<any> = [];
        _.forIn(Widgets, (v: WidgetInfo) => {
            if (v.WidgetId === "_UniqueId") return;

            cols.push({
                title: v.Caption,
                key: v.WidgetId,
                render: this.renderCell.bind(this, v.WidgetId),
                width: 150
            })
        });

        cols.push({
            title: 'Action',
            dataIndex: '',
            key: '_action_',
            render: this.renderRowAction,
            width: 60
        });

        return cols;
    }

    renderCell = (widgetId: string, text: any, record: IDictionary<IFieldData>) => {
        if (this.props.activeFormId === record._UniqueId.Value) {
            return this.props.getControl(widgetId, { Caption: "" });
        } else {
            const wd = this.props.entityModel.Widgets[widgetId];
            const IComponent = getComponent(wd.WidgetType, true);
            const fieldData = record[widgetId];
            return <IComponent 
                        {...wd}
                        {...fieldData}
                        IsViewMode={true}
                        api={{}}
                        onChange={() => {}}
                    />
        }
        
    };

    renderRowAction = (text: any, record: IDictionary<IFieldData>) => {
        if (record._UniqueId.Value === this.props.activeFormId) {
            return (
                <Space size="small">
                    <Button type="link" onClick={this.saveRow} icon={<SaveFilled/>}></Button>
                    <Button type="link" onClick={this.cancelRow} icon={<DeleteFilled/>}></Button>
                </Space>
            );
        }
        
        return <Space size="small">
            <Button type="link" onClick={this.editRow.bind(this, record._UniqueId.Value)} icon={<EditFilled/>}></Button>
            <Button type="link" onClick={this.deleteRow.bind(this, record._UniqueId.Value)} icon={<DeleteFilled/>}></Button>
        </Space>
    }

    render() {
        const { columns } = this.state;
        const { listData } = this.props;
 
        return (<div>
            <Table 
                columns={columns}
                dataSource={listData}
                rowKey={"UniqueId"}
                bordered
                size="small"
            />            
            </div>
        );
    }
}
