import { Button, notification } from 'antd';

const openNotificationWithIcon = (type, message, op = {}) => {
    notification[type]({
      message: op.title,
      description: message,
    });
  };

export default {
  success: function(message, op) {
    openNotificationWithIcon("success", message, op);
  },
  warn: function(message, op) {
    openNotificationWithIcon("warning", message, op);
  },
  info: function(message, op) {
    openNotificationWithIcon("info", message, op);
  },
  error: function(message, op) {
    openNotificationWithIcon("error", message, op);
  },
  closeAll:function() {
    //Alert.closeAll();
  }
};