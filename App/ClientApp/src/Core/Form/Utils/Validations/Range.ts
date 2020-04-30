
function rangeDecimal(min: any, max: any, fieldValue: any) {
    const minVal = Number.parseFloat(min);
    const maxVal = Number.parseFloat(max);

    if (minVal !== 0 || maxVal !== 0) {
        if (fieldValue >= minVal && fieldValue <= maxVal) {
            return true;
        }

        return false;
    } else if (minVal >= 0 && fieldValue < minVal) {
        return false;
    } else if (maxVal !== 0 && fieldValue <= maxVal) {
        return true;
    }

    return false;
}

function rangeInteger(min: any, max: any, fieldValue: any) {
    fieldValue = Number.parseInt(fieldValue);
    const minVal = Number.parseInt(min);
    const maxVal = Number.parseInt(max);

    if (minVal !== 0 || maxVal !== 0) {
        if (fieldValue >= minVal && fieldValue <= maxVal) {
            return true;
        }

        return false;
    }

    if (minVal !== 0 && fieldValue >= minVal) {
        return true;
    } else if (maxVal !== 0 && fieldValue <= maxVal) {
        return true;
    }

    return false;
}

export function rangeAdaptor(widgetInfo: WidgetInfoProps, validationInfo: any = {}, dataModel?: IDictionary<IFieldData>) {
    const value = widgetInfo.Value;
    if (!value || value === "" || isNaN(value)) {
        return;
    }
    let minV, maxV;
    // if (validationInfo.MinRef) {
    //     const minRefField = this.getField(rangeInfo.MinRef);
    //     if (minRefField) { minValue = minRefField.Value; }
    // }
    // if (validationInfo.MaxRef) {
    //     const maxRefField = this.getField(rangeInfo.MaxRef);
    //     if (maxRefField) { maxValue = maxRefField.Value; }
    // }

    minV = validationInfo.Min;
    maxV = validationInfo.Max;

    let isValid = true;

    if (validationInfo.ValueType === "DECIMAL") {
        isValid = rangeDecimal(minV, maxV, value);
    } else {
        isValid = rangeInteger(minV, maxV, value);
    }

    let errorMessage = validationInfo.Msg;
    if (!isValid) {
        errorMessage = _App.FormatString(errorMessage, { min: minV, max: maxV });
    }

    return {
        IsValid: isValid,
        Message: errorMessage
    };
}
