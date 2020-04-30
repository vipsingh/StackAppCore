import { notification } from 'antd';

interface NotifyArgs{
  title: string
}

const openNotificationWithIcon = (type: string, message: string, op: NotifyArgs = { title: "" }) => {
  const n: any = notification;
    n[type]({
      message: op.title,
      description: message,
    });
};

export default {
  success: function(message: string, op: NotifyArgs) {
    openNotificationWithIcon("success", message, op);
  },
  warn: function(message: string, op: NotifyArgs) {
    openNotificationWithIcon("warning", message, op);
  },
  info: function(message: string, op: NotifyArgs) {
    openNotificationWithIcon("info", message, op);
  },
  error: function(message: string, op: NotifyArgs) {
    openNotificationWithIcon("error", message, op);
  },
  closeAll:function() {
    //Alert.closeAll();
  }
};