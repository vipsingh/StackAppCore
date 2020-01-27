import React from "react";
import PropTypes from "prop-types";
import { Icon, Input, AutoComplete } from 'antd';
import Request from "~/Core/Utils/Request";
//import {openDialog} from "~/Core/Ui/Dialog";

const Option = AutoComplete.Option;
const OptGroup = AutoComplete.OptGroup;

export class EntityPicker extends React.Component {
    static propTypes = {
        DataUrl: PropTypes.object,
        onChange: PropTypes.func,
        Value: PropTypes.any,
        ControlId: PropTypes.string,
        Disabled: PropTypes.bool,
        api: PropTypes.object
    }

    constructor(props) {
        super(props);

        this.state= {
            IsFetching: false,
            DataSource: []
        };

        if (props.Value && props.Value.Value) {
            this.state.DataSource = [props.Value];
        }

        this.loadedSource = false;
    }

    handleOnFocus() {
        if (!this.loadedSource) {
            this.getData();
        }
    }

    getData = (search) => {
        const {DataUrl, api, ControlId} = this.props;
        const { URL } = DataUrl;
        const req = api.prepareFieldRequest(ControlId);

        this.setState({IsFetching: true});
        window._App.Request.getData({
            url: URL,
            type: "POST",
            body: req
        }).then((res) => {
            this.setState({DataSource: res});
            this.loadedSource = true;
        }).finally(() => {
            this.setState({IsFetching: false});
        });;
    }

    handleOnChange = (value) => {
        const {onChange} = this.props;
        const {Value, ControlId} = this.props;
        _Debug.log("selected " + value);
        if (typeof onChange === "function") {            
            onChange(value);
        }
    }

    options() {
        return _.map(this.state.DataSource, (d) => {
            return (<Option key={d.Value} text={d.Text}><span>{d.Text}</span></Option>);
        });
    }

    render() {
        const {Value, Disabled, ControlId} = this.props;
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

class EntityPickerDialog extends React.PureComponent {
    constructor(props) {
        super(props);
    }

    render() {
        
    }
}