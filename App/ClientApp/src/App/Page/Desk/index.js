import React from "react";
import PropTypes from "prop-types";
import Form from "../../../Component/Form/Form";
import PageView from "../../../Component/Form/PageView";

class Desk extends React.Component {
  static propTypes = {
    data: PropTypes.object
  };
  constructor(props) {
    super(props);

    this.state = {
      IsLoading: false,
      Schema: props.data,
      Formdata: null
    };
  }

  shouldComponentUpdate() {
    return true;
  }

  componentWillReceiveProps(nextProps) {
    this.setState({ Schema: nextProps.data });
  }

  render() {
    
    return (
        <Form
                Schema={this.state.Schema}
                Formdata={this.state.Formdata}
                render={
                    (prop) => {
                        return (
                           <PageView 
                               {...prop}
                           />  
                        );
                    }
                }
             />
    );
  }
}

export default Desk;
