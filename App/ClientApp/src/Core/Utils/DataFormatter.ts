import { FormControlType } from "../../Constant";
import moment from "moment";

export function formatValue(widget: WidgetInfo, val: any) {
    const { Value } = val;
    const { FormatInfo } = widget;
    let formatStr = "";
    if (FormatInfo) {
        formatStr = FormatInfo.FormatString;
    }

    if (widget.WidgetType === FormControlType.DatePicker) {
        return moment(Value).format(formatStr || _AppSetting.DateFormat);
    }

    return Value;
}