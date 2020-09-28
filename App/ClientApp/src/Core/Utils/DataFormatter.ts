import { FormControlType } from "../../Constant";
import moment from "moment";
import numeral from "numeral";

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

    if (FormatInfo && FormatInfo.Type) {
        if (FormatInfo.Type === "DECIMAL") {
            return numeral(Value).format(getNumberFormatStr(formatStr));
        } else if (FormatInfo.Type === "DATETIME") {
            return moment(Value).format(formatStr || _AppSetting.DateFormat);
        }
    }

    return Value;
}

function getNumberFormatStr(serverString: string) {
    if (serverString) return serverString.replace("{0:","").replace("}","");

    return "0,0" + ".0000000000".substr(0, _AppSetting.DecimalPlaces + 1);
}