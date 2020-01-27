import React from "react";
import PropTypes from "prop-types";
import { Spin } from "antd";
import PageWrapper from "../../Component/PageWrapper";

class Desk extends React.Component {
  static propTypes = {
    Data: PropTypes.object
  };
  constructor(props) {
    super(props);

    this.state = {
      IsLoading: false,
      Schema: props.Data,
      Formdata: null
    };
  }

  shouldComponentUpdate() {
    return true;
  }

  componentWillReceiveProps(nextProps) {
    this.setState({ Schema: nextProps.Data });
  }

  render() {
    return (
      <PageWrapper Schema={this.state.Schema} Formdata={this.state.Formdata} />
    );
  }
}

export default Desk;
