import React from "react";
import ReactDOM, { render } from "react-dom";
import { Modal, Button } from "antd";

function defer() {
  var resolve, reject;
  var promise = new Promise(function() {
    resolve = arguments[0];
    reject = arguments[1];
  });
  return {
    resolve: resolve,
    reject: reject,
    promise: promise
  };
}

class DialogBox extends React.Component<{options: any, title: string, children: any}> {  
  displayName: string
  promise: any

  constructor(props: any) {
    super(props);
    this.displayName = "ConfirmBox";    
  }

  abort = () => {
    return this.promise.reject({});
  };

  confirm = () => {
    return this.promise.resolve();
  };

  componentDidMount() {    
    this.promise = defer();
  }

  render() {
    let btn_op = {
      confirmLabel: "OK",
      abortLabel: "Cancel"
    };
    if (this.props.options.buttonType == "YESNO") {
      btn_op.confirmLabel = "Yes";
      btn_op.abortLabel = "No";
    }
    let options = Object.assign({}, this.props.options, btn_op);

    const action_abort = (
      <Button type="primary" onClick={this.abort}>
        {options.abortLabel}
      </Button>
    );

    const action_confirm = (
      <Button type="primary" onClick={this.confirm}>
        {options.confirmLabel}
      </Button>
    );

    const actions = [];
    if (this.props.options.buttonType == "OK") {
      actions.push(action_confirm);
    } else {
      actions.push(action_abort);
      actions.push(action_confirm);
    }

    //actions={actions}
    return (
      <Modal
        title={this.props.title}        
        visible={true}
        onCancel={this.abort}
      >
        {this.props.children}
      </Modal>
    );
  }
}

export const openDialog = function(title: string, children: any, options: any) {
  var cleanup, component: any, props, wrapper: HTMLDivElement;
  if (options == null) {
    options = {};
  }
  title = title || "Message";
  props = { title: title, options: options, children };
  wrapper = document.body.appendChild(document.createElement("div"));
  component = render(<DialogBox {...props} />, wrapper);
  cleanup = function() {
    ReactDOM.unmountComponentAtNode(wrapper);
    return setTimeout(function() {
      return wrapper.remove();
    });
  };
  return component.promise.promise.finally(cleanup);
};
