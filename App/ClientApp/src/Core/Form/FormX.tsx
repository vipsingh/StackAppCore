import React, { Children } from "react";
import { useForm, UseFormOptions } from "./useForm";
import * as componetFactory from "../ComponentFactory";

const FormContext = React.createContext<any>(null);
FormContext.displayName = 'FormXContext';

export function FormX({ entitySchema, formData, render, onSubmit }: FormXProps) {
    const formObject = useForm({ 
                          entitySchema, 
                          formData
                        });

    return <FormContext.Provider value={formObject}
        >
            {render(formObject)}
    </FormContext.Provider>
}

export function FormXField({ id, customProps }: FormXFieldProps) {
    const formObject= React.useContext(FormContext);
    const { renderField, getField, dataModel, getErrorResult } = formObject;

    const widgetInfo = getField(id);
    const widgetModel = dataModel[id];   

    const properies = { ...widgetInfo, ...customProps };
    const { WidgetId, Caption, IsHidden, CaptionPosition } = properies;

    const formItemLayout = React.useMemo(() => {
        const fItemLayout = {
            labelCol: {
              xs: { span: 24 },
              sm: { span: 8 },
            },
            wrapperCol: {
              xs: { span: 24 },
              sm: { span: 16 },
            },
            labelAlign: "right"
          };
    
        if(!Caption || CaptionPosition === 1) {
            fItemLayout.wrapperCol.sm.span = 24;
        }
    
        if (CaptionPosition === 1) {
            fItemLayout.labelAlign = "left";
        }

        return fItemLayout;
    }, []);

    const inputFieldComponent = renderField(id, customProps);
    if (!inputFieldComponent) return componetFactory.BlockElement();
        
    const { HasError, Invisible } = widgetModel;
    

    let validResult: any;
    if (HasError) {
      validResult = getErrorResult(WidgetId);
    }

    if (IsHidden || Invisible) {
        return inputFieldComponent;
    }

    const FormItem = componetFactory.FormFieldItem();

    return (
          <FormItem formItemLayout={formItemLayout} label={Caption} hasError={HasError} validResult={validResult}>
              <FormItemField widgetInfo={widgetInfo} dataModel={widgetModel}>{inputFieldComponent}</FormItemField>
        </FormItem>
      );
}

function FormItemFieldT({ children }: any) {
  return children;
}

const FormItemField = React.memo(
  FormItemFieldT, 
  // Notice condition is inversed from shouldComponentUpdate
  (prevProps, nextProps) => {
    if (prevProps.widgetInfo.WidgetType === 100) return false;

    return !(nextProps.widgetInfo !== prevProps.widgetInfo || nextProps.dataModel !== prevProps.dataModel);
  }
)

export type FormXProps = {
    entitySchema: IPageInfo,
    formData: IDictionary<IFieldData>,
    render: Function,
    onSubmit: Function,
    onExecuteAction?: Function
}

type FormXFieldProps = {
    id: string,
    customProps?: any
}