import React from "react";
//import numeral from "numeral";
import { TextBox } from ".";
import { InputNumber } from "antd";

export class NumberBox extends TextBox {

    handleOnChange = (val: any) => {
        const {onChange} = this.props;

        if (typeof onChange === "function") {            
            onChange(val);            
        }
    }

    getMinMax() {
        const { Validation } = this.props;
        let min, max;
        if (Validation && Validation.RANGE) {
            min = Validation.RANGE.Min;
            max = Validation.RANGE.Max;
        }

        return { min, max};
    }

    render() {
        const {Value, IsReadOnly} = this.props;
        const minMax = this.getMinMax();

        return(<InputNumber  
            value={Value}
            disabled={IsReadOnly}
            onChange={this.handleOnChange}
            onBlur={this.handleOnBlur}
            precision={0}
            min={minMax.min}
            max={minMax.max}
        />);
    }
}

export class DecimalBox extends NumberBox {
    formatStr: string
    prefix: string
    constructor(props: any) {
        super(props);

        this.prefix = "";
        this.formatStr = this.getFormatStr();
    }

    getFormatStr() {
        let d = this.getDecimalPlaces();        

        let str = this.prefix + "0,0" + ".0000000000".substr(0, d+1);

        return str;
    }

    handleOnChange = (val: any) => {
        const {onChange} = this.props;

        if (typeof onChange === "function") {            
            onChange(val);            
        }
    }

    formatter  = (value: any) => {        

        return `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
    
    parser = (value: any) => {
       return value.replace(/\$\s?|(,*)/g, '');
    }

    getDecimalPlaces() {
        const {DecimalPlaces, Properties} = this.props;

        let d = 2;
        if (DecimalPlaces) {
            d = DecimalPlaces;
        }
        
        if (Properties && Properties.IsMonatryAmount) {
            d = _AppSetting.DecimalPlaces;            
        }

        return d;
    }

    render() {
        const { Value, IsReadOnly } = this.props;
        const minMax = this.getMinMax();

        return(<InputNumber  
            value={Value}
            disabled={IsReadOnly}
            onChange={this.handleOnChange}
            formatter={this.formatter}
            parser={this.parser}
            style={{ width: "100%" }}
            onBlur={this.handleOnBlur}
            min={minMax.min}
            max={minMax.max}
            precision={this.getDecimalPlaces()}
        />);
    }
}