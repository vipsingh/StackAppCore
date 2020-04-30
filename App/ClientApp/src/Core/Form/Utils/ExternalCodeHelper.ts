
export default class ExternalCodeExecution {
    execute() {

    }

    getScriptContext = (jsonData: any): any => {
        const context = {
            getValue: (widgetId: string) => {
                //const field = this.props.formApi.getField(controlKey);

                //return field.Value;
            },
            setValue: (widgetId: string, value: any) => {
                //this.props.formApi.setValue(controlKey, value);
            },
            data: jsonData
        };
        context.getValue.bind(this);
        context.setValue.bind(this);

        return context;
    }

    evalLocalScript = (script: string, jsonData: any) => {
        function strict(that: any) {
            try {
                const setFormValue = that.setValue;//this variable is using in external script.
                const getFormValue = that.getValue;//this variable is using in external script.
                const data = that.data;
                eval(that.scriptt);
            } catch (err) {
                _Debug.error(err);
            }
        }
        const stricW = Function(`return (${strict})(this)`);
        const cntxt = this.getScriptContext(jsonData);
        cntxt.scriptt = script;
        stricW.call(cntxt);
    }
}

export function validateJsScript(script: string) {
    if (script.match(/function|document|<script/g))
        return false;

    return true;
}
