import React from "react";
import PropTypes from "prop-types";
import { Icon, Input, AutoComplete } from 'antd';
import _ from "lodash";

const Option = AutoComplete.Option;
const OptGroup = AutoComplete.OptGroup;

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
            this.setState({DataSource: res});
            this.loadedSource = true;
        }).finally(() => {
            this.setState({IsFetching: false});
        });;
    }

    handleOnChange = (value: any) => {
        const {onChange} = this.props;        
        _Debug.log("selected " + value);
        
        onChange(value);        
    }

    options() {
        return _.map(this.state.DataSource, (d) => {
            return (<Option key={d.Value} title={d.Text}><span>{d.Text}</span></Option>);
        });
    }

    render() {
        const {Value} = this.props;
        let v = Value;
        if (v && typeof v === "object") {
            v = v.Value.toString();
        }
        
        return(<div className="certain-category-search-wrapper" style={{ width: "100%" }}>
        <AutoComplete
          className="certain-category-search"
          dropdownClassName="certain-category-search-dropdown"
          dropdownMatchSelectWidth={false}
          dropdownStyle={{ width: 300 }}          
          style={{ width: '100%' }}
          dataSource={this.options()}
          placeholder="input here"          
          onSelect={this.handleOnChange}
          optionLabelProp="text"
          value={v}
          onFocus={this.handleOnFocus}
        >
          <Input suffix={<Icon type="search" className="certain-category-icon" />} />
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