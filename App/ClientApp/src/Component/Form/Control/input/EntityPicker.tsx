import React from "react";
//import { AutoComplete } from 'antd';
import _ from "lodash";
import GridView from "../../../ListView/GridView";
import { Button, Modal, Input, Row, Col } from "antd";
import { SearchOutlined } from '@ant-design/icons';
import { prepareWidgetRequest } from "../../../../Core/Form/Utils/FormUtils";

//const Option = AutoComplete.Option;

export class EntityPicker extends React.Component<WidgetInfoProps, {
    visible: boolean
}> {
    selectionConfig: any
    constructor(props: WidgetInfoProps) {
        super(props);

        // let source: Array<any> = [];
        // if (props.Value && props.Value.Value) {
        //     source = [props.Value];
        // }

        this.state= {
            visible: false
        };
   
        this.selectionConfig = {
            IsMultiSelect: props.IsMultiSelect
        }   
    }

    handleOnChange = (value: any, opt: any) => {
        const {onChange} = this.props;        
        _Debug.log("selected " + opt.id);
        
        onChange(opt.id);
    }

    // options() {
    //     return _.map(this.state.Options, (d) => {
    //         //return (<Option key={d.Value} value={d.Value.toString()} title={d.Text}><span>{d.Text}</span></Option>);
    //         return d;
    //     });
    // }

    showPicker = () => {
        this.setState({
            visible: true,
        });
    }

    handleOk = (val: any = null) => {
        this.props.onChange(val);
        
        this.setState({
            visible: false,
        });
    }    

    handleTextChange = (ev: any) => {
        console.log("s");
    }

    handleCancel = () => {
        this.setState({
            visible: false,
          });
    }

    getFieldRequest = () => {
        const { api,WidgetId } = this.props;
        let req;
        if (api) req = api.prepareFieldRequest(WidgetId);
        else req = prepareWidgetRequest(this.props);

        return req;
    }

    render() {
        const { Value, DataActionLink, WidgetId, api } = this.props; 
        let text = "";
        if (_.isArray(Value)) {
            text = _.map(Value, v => v.Text).join(", ");
        } else {
            text = Value ? Value.Text: "";
        }
                
        return (<div>            
            <Row>
                <Col span="20"><Input value={text} onChange={this.handleTextChange} /></Col>
                <Col span="4"><Button icon={<SearchOutlined />} onClick={this.showPicker} /></Col>
            </Row>
            {!this.state.visible || <EntityPickerView 
                DataActionLink={DataActionLink} 
                WidgetId={WidgetId} 
                SelectionConfig={this.selectionConfig} 
                getFieldRequest={this.getFieldRequest}
                onSelect={this.handleOk}
                onCancel={this.handleCancel} />}
            </div>);        
    }
}

class EntityPickerView extends React.PureComponent<{
    SelectionConfig: any, 
    onSelect: Function,
    onCancel: Function,
    DataActionLink: any, 
    WidgetId: string,
    getFieldRequest: Function
}, any> {
    gridRef: any
    loadedSource: boolean
    constructor(props: any) {
        super(props);

        this.state = {
            IsFetching: false,
            DataSource: null
        }
        this.gridRef = null;

        this.getData();
        this.loadedSource = false;  
    }

    getData = (search: string = "") => {
        // const tdata = this.props.api.getWidgetTempdata(this.props.WidgetId);
        
        // if (!(tdata.isReloadRequired || !this.loadedSource))
        //     return;

        const {DataActionLink, WidgetId, getFieldRequest} = this.props;
        const { Url } = DataActionLink;
        const req = getFieldRequest(WidgetId);        

        this.setState({ IsFetching: true });

        window._App.Request.getData({
            url: Url,
            type: "POST",
            body: {RequestType: 0, RequestInfo: req}
        }).then((res: any) => {
            if (res.Data) {
                // if dropdown type
                // const ds = _.map(res.Data, d => {
                //     return { Value: d.RowId, Text: d[res.ItemViewField].FormatedValue }
                // })
                this.setState({DataSource: res});
            }
            
            this.loadedSource = true;
            //api.setWidgetTempdata(WidgetId, { isReloadRequired: false });
        }).finally(() => {
            this.setState({IsFetching: false});
        });;
    }

    handleOk = () => {
        const { ItemViewField } = this.state.DataSource;

        const rows = this.gridRef.getSelectedRows();
        if (!rows || rows.length === 0) this.props.onSelect();

        const vals = _.map(rows, r => {
            return { Text: r[ItemViewField].FormatedValue, Value: r.RowId };
        })

        if (this.props.SelectionConfig.IsMultiSelect) {
            this.props.onSelect(vals);
        } else {
            this.props.onSelect(vals[0]);
        }
    }

    handleCancel = () => {
        this.props.onCancel();
    }

    render() {
        const { DataSource } = this.state;
        const { SelectionConfig } = this.props;

        return (<div>
                <Modal
                    title={__L("Select Data")}
                    visible={true}
                    onOk={this.handleOk}
                    onCancel={this.handleCancel}
                    >
                        <div>
                            <GridView 
                                ref={(node) => { this.gridRef = node; }}
                                ListData={DataSource}
                                WidgetId="sddd" 
                                WidgetType={100}
                                onChange={() => { }}
                                SelectionConfig={SelectionConfig}
                            />
                        </div>
                </Modal>                
        </div>);        
    }
}