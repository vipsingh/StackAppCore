import React from "react";
import PageView from "../../../Component/Form/PageView";
import EntityForm from "../../../Component/Form/EntityForm";

class Desk extends React.Component<{
  data: any
}, {
  IsLoading: boolean,
  Schema: any,
  FormData: any
}> {

  constructor(props: any) {
    super(props);

    this.state = {
      IsLoading: false,
      Schema: props.data,
      FormData: null
    };
  }

  shouldComponentUpdate() {
    return true;
  }

  componentWillReceiveProps(nextProps: any) {
    this.setState({ Schema: nextProps.data });
  }

  render() {
    
    return (
        <EntityForm
                Schema={this.state.Schema}
                FormData={this.state.FormData}
                render={
                    (prop: any) => {
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
