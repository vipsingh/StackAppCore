import React from "react";
import { Input, AutoComplete } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import _ from "lodash";

//const Option = AutoComplete.Option;

export class EntityPicker extends React.Component<WidgetInfoProps, {
    IsFetching: boolean,
    DataSource: Array<any>
}> {

    loadedSource: boolean

    constructor(props: any) {
        super(props);

        let source: Array<any> = [];
        if (props.Value && props.Value.Value) {
            source = [props.Value];
        }

        this.state= {
            IsFetching: false,
            DataSource: source
        };

        this.loadedSource = false;
    }

    handleOnFocus = () => {
        if (!this.loadedSource) {
            this.getData();
        }
    }

    getData = (search: string = "") => {
        const {DataActionLink, api, WidgetId} = this.props;
        const { Url } = DataActionLink;
        const req = api.prepareFieldRequest(WidgetId);

        this.setState({ IsFetching: true });

        window._App.Request.getData({
            url: Url,
            type: "POST",
            body: req
        }).then((res: any) => {
            if (res.Data) {
                const ds = _.map(res.Data, d => {
                    return { Value: d.RowId, Text: d[res.ItemViewField].FormatedValue }
                })
                this.setState({DataSource: ds});
            }
            
            this.loadedSource = true;
        }).finally(() => {
            this.setState({IsFetching: false});
        });;
    }

    handleOnChange = (value: any, opt: any) => {
        const {onChange} = this.props;        
        _Debug.log("selected " + opt.id);
        
        onChange(opt.id);
    }

    options() {
        return _.map(this.state.DataSource, (d) => {
            //return (<Option key={d.Value} value={d.Value.toString()} title={d.Text}><span>{d.Text}</span></Option>);
            return {
                value: d.Text,
                id: d.Value,
                label: (<div>{d.Text}</div>)
            }
        });
    }

    render() {
        const {Value, IsReadOnly} = this.props;
        let v = Value;
        if (v && typeof v === "object") {
            v = v.Text;
        }
        
        return(<div className="certain-category-search-wrapper" style={{ width: "100%" }}>
        <AutoComplete
          className="certain-category-search"
          dropdownClassName="certain-category-search-dropdown"
          dropdownMatchSelectWidth={false}
          dropdownStyle={{ width: 300 }}          
          style={{ width: '100%' }}
          options={this.options()}
          placeholder="input here"          
          onSelect={this.handleOnChange}
          defaultValue={v}
          onFocus={this.handleOnFocus}
          disabled={IsReadOnly}
        >
          <Input suffix={<SearchOutlined />} />
        </AutoComplete>
      </div>);
    }
}

// class EntityPickerDialog extends React.PureComponent {
//     constructor(props) {
//         super(props);
//     }

//     render() {
        
//     }
// }