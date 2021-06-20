import React from "react";
import Form from "../../Form";
import ViewPageInfo from "../../../../Core/Models/ViewPageInfo";
import _ from "lodash";
import { Button, Table, Space } from "antd";
import update from "immutability-helper";
//import { getFormDataToSubmit } from "../../../../Core/Form/Utils/FormUtils";
import { getComponent } from "../../../../Core/ComponentFactory";
import { EditFilled, SaveFilled, DeleteFilled } from '@ant-design/icons';

export interface ListFormProps extends WidgetInfoProps {
    FormPage: any
}

export default class ListForm extends React.Component<ListFormProps, 
    { 
        activeKey: string,
        activeModel: IDictionary<IFieldData>,
        IsUnCommitedData: boolean
    }
> {
    schema: ViewPageInfo
    constructor(props: any, cntx: any) {
        super(props, cntx);

        this.schema = this.mergeFields(this.props.FormPage, null);
        
        this.state = {
            activeKey: "",
            IsUnCommitedData: false,
            activeModel: {} 
        };
    }

    componentDidMount() {
        const { DataLink, onChange } = this.props as any;

        window._App.Request.getData({
            url: DataLink.Url,
            type: "GET"
        }).then((res: any) => {
            var data: Array<IDictionary<IFieldData>> = [];
            
            _.each(res, (r: any) => {
                var m = this.schema.getDataModel();                
                m._EntityInfo = r.EntityInfo;
                _.forIn(r.Widgets, (w, k) => {
                    m[k] = w;
                });
                data.push(m);
            });

            onChange(data);

        }).finally(() => {
            //this.setState({IsFetching: false});
        });
    }

    mergeFields(schema: any, formData: any) {        
        const entityModel: ViewPageInfo = new ViewPageInfo(this.props.FormPage, formData);
        return entityModel;
    }  
    
    addItem = () => {
        const { onChange, Value } = this.props;
        const d = this.schema.getDataModel();
        
        let vals = Value;
        if (!vals) vals = [];
        onChange(update(vals, {$push: [d] }));

        this.setState({ activeModel: d, activeKey: d._UniqueId.Value, IsUnCommitedData: true});
    }
    
    editItem = (formId: string) => {
        const data = this.getModel(formId);
        this.setState({ activeKey: data._UniqueId.Value, activeModel: data});
    }

    deleteRowItem = (formId: string) => {
        const { onChange, Value } = this.props;
        const ix = _.findIndex(Value, (o: any) => o._UniqueId.Value === formId);
        onChange(update(Value, { $splice: [[ix, 1]] }));

        this.setState({ activeKey: "", IsUnCommitedData: false});
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
        const { onChange, Value } = this.props;

        const ix = _.findIndex(Value, (o: any) => o._UniqueId.Value === model._UniqueId.Value);
        onChange(update(Value, {[ix]: { $set: model }}));
        this.setState({ IsUnCommitedData: true, activeModel: model});
    }

    saveItem = (toSaveModel: any) => {
        // const { onChange, Value } = this.props;

        // const ix = _.findIndex(Value, (o: any) => o.UniqueId === this.state.activeKey);        
        this.setState({ activeKey: "", IsUnCommitedData: false });
    }

    renderList(prop: any) {

        return (<div></div>);
    }

    getModel(key: string): IDictionary<IFieldData> {
        const { Value } = this.props;
        const v = _.filter(Value, x => x._UniqueId.Value === key);

        return v[0] as IDictionary<IFieldData>;
    }
    
    render() {
        const { Value, IsViewMode } = this.props;
        const { activeKey, activeModel } = this.state;        

        return(
            <div>
                <div>
                    {IsViewMode || <Button disabled={ !!activeKey } onClick={this.addItem}>Add</Button>}
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
                                listData={Value}
                                editRowItem={this.editItem}
                                deleteRowItem={this.deleteRowItem}
                                cancelRowEdit={this.cancelEdit}
                                isViewMode={IsViewMode}
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
        const { entityModel: { Widgets }, isViewMode } = this.props;

        const cols: Array<any> = [];

        _.forIn(Widgets, (v: WidgetInfo) => {
            if (v.WidgetType === 0) return;

            cols.push({
                title: v.Caption,
                key: v.WidgetId,
                render: this.renderCell.bind(this, v.WidgetId),
                width: 150
            })
        });

        if (!isViewMode) {
            cols.push({
                title: 'Action',
                dataIndex: '',
                key: '_action_',
                render: this.renderRowAction,
                width: 60
            });
        }

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
