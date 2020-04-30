import React from "react";
import ReactDOM, { render } from "react-dom";
import { Modal, Button } from "antd";
import _ from "lodash";

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
    const { options } = this.props;
    let btn_op = {
      confirmLabel: "OK",
      abortLabel: "Cancel"
    };
    if (options.buttonType === "YESNO") {
      btn_op.confirmLabel = "Yes";
      btn_op.abortLabel = "No";
    }
    let op = Object.assign({}, options, btn_op);

    const action_abort = (
      <Button key="back" onClick={this.abort}>
        {op.abortLabel}
      </Button>
    );

    const action_confirm = (
      <Button key="submit" type="primary" onClick={this.confirm}>
        {op.confirmLabel}
      </Button>
    );

    const actions = [];
    if (!options.hideCommands) {
      if (options.buttonType === "OK") {
        actions.push(action_confirm);
      } else {
        actions.push(action_abort);
        actions.push(action_confirm);
      }
    }

    let width;
    if (options.size === "lg")
      width = "85%";

    //actions={actions}
    return (
      <Modal
        title={this.props.title}        
        visible={true}
        onCancel={this.abort}
        footer={actions}
        maskClosable={false}
        width={width}
        style={{ top: options.size === "lg"? 20: undefined }}
      >
        {this.props.children}
      </Modal>
    );
  }
}

export const openDialog = function(title: string, children: any, options: any = {}): { promise: Promise<any>, cleanup: Function } {
  var cleanup, component: any, props, wrapper: HTMLDivElement;

  title = title || "Message";
  props = { title: title, options: options, children };
  wrapper = document.body.appendChild(document.createElement("div"));
  wrapper.id = _.uniqueId("dialog");
  openedDialogs.push(wrapper.id);
  component = render(<DialogBox {...props} />, wrapper);
  cleanup = function() {
    ReactDOM.unmountComponentAtNode(wrapper);
    return setTimeout(function() {
      openedDialogs = _.remove(openedDialogs, a => a === wrapper.id);
      return wrapper.remove();
    });
  };
  return { promise: component.promise.promise.finally(cleanup), cleanup };
};
let openedDialogs: Array<any> = [];

export function closeAll() {
  _.forEach(openedDialogs, d => {
      const wrapper = document.getElementById(d);
      if (wrapper) {
        ReactDOM.unmountComponentAtNode(wrapper);
        return setTimeout(function() {
          return wrapper.remove();
        });
      }
  });

  openedDialogs = [];
}
