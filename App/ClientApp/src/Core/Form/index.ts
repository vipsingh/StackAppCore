import React, { useState, useEffect, useRef, useCallback } from "react";
import _ from "lodash";
import update from "immutability-helper";
import validationUtil from "./Utils/Validations";
import formFeatures from "./Features";
import { prepareFieldRequest as prepareReq, getFormDataToSubmit } from "./Utils/FormUtils";

export function useForm({ entitySchema, formData, controlFactory }: UseFormOptions): UseFormMethod { 
    const widgetsTempData = useRef<IDictionary<any>>({});

    const [dataModel, setDataModel] = useState(formData);
    const [isSubmitting, setOnSubmitting] = useState<boolean>(false);
    const [errors, setErrors] = useState<IDictionary<any>>({});
    const formRendered = useRef(true);

    useEffect(() => {
        if (formRendered.current) {
            setDataModel(formData);
            setErrors({});
            setOnSubmitting(false);
            //run features
        }
        formRendered.current = false;
      }, [formData]);
    
    const renderField= useCallback((widgetId: string, customProps: any) => {
        const cinfo = entitySchema.getField(widgetId);
        if (!cinfo) return null;

        const {WidgetType, IsViewMode} =  cinfo;    
        
        let IComponent = controlFactory.getComponent(WidgetType, IsViewMode);        
        
        if (!IComponent)
            return React.createElement(controlFactory.BlockElement, { children: "INVALID COMPONENT" });

        if (cinfo.IsHidden) {
            IComponent = controlFactory.HiddenField;
        }

        const widgetModel = dataModel[widgetId];

        if (widgetModel.Invisible) {
            
            return React.createElement(controlFactory.BlockElement);
        }

        return React.createElement(IComponent, {...cinfo, ...customProps, ...widgetModel, onChange: (val: any) => setValue(widgetId, val)});
        //onBlur={this.onBlur.bind(this, ControlId)}
        //api={this.getFormAPI()}
    }, []);

    const getControl= useCallback((widgetId: string, customProps: any) => {
        const inputFieldComponent = renderField(widgetId, customProps);
        if (!inputFieldComponent) return null;
        
        const cinfo = entitySchema.getField(widgetId);   
        const widgetModel = dataModel[widgetId];     

        return inputFieldComponent; //React.createElement(FormField,  { widgetInfo: cinfo, dataModel: widgetModel, api: this.getFormAPI(), otherProps: customProps);
    }, []);

    const setValue = useCallback((widgetId: string, value: any, afterChange?: Function) => {

        const fieldInfo = entitySchema.Widgets[widgetId];
        
        let model = update(dataModel, {[widgetId]: { Value: {$set: value}, IsDirty: {$set: true}}});
        
        const vRes = validationUtil.validateField({...fieldInfo, ...model[widgetId]});        
        model = update(model, {[widgetId]: { HasError: {$set: !vRes.IsValid} }});

        const cErrors = setValidationResult(widgetId, vRes);
        setErrors(cErrors);

        execAllFormFeature(widgetId, model, entitySchema, ["Invisible", "Options", "ReadOnly"], apiProps, (m: IDictionary<IFieldData>) => {
            executeDependecy(widgetId, m, apiProps, (m1: IDictionary<IFieldData>) => {
                setDataModel(m1);
            });            
        });
    }, []);

    useEffect(() => {

    }, [dataModel]);

    const updateField= useCallback((controlId: string, info: any) => {
        let wd = update(dataModel, {[controlId]: { $merge: info }});
        
        //return this.setDataModel(wd, { updateBy: "WIDGET", param: controlId }, afterUpdate); //use useeffect in specific control for afterupdate
        setDataModel(wd);
    }, []);

    const validateField= useCallback((widgetId: string) => {
        const fieldInfo = entitySchema.getField(widgetId);
        const vRes = validationUtil.validateField({...fieldInfo, ...dataModel[widgetId]});                
        
        updateField(widgetId, { HasError: !vRes.IsValid });
        
        const cErrors = setValidationResult(widgetId, vRes);
        setErrors(cErrors);
    }, []);

    const setValidationResult = useCallback((WidgetId: string, result: any) => {
            const cErrors = Object.assign({}, errors, {});
            if (result.IsValid) {
                delete cErrors[WidgetId];
            } else {
                cErrors[WidgetId] = result;
            }
        return cErrors;
    }, []);

    const executeAction= useCallback(() => {

    }, []);

    const prepareFieldRequest= useCallback((widgetId: string) => {
        return prepareReq(entitySchema, dataModel, widgetId);
    }, []);

    const setWidgetTempdata = useCallback((widgetId: string, data: any) => {
        widgetsTempData.current[widgetId] = _.extend(widgetsTempData.current[widgetId], data);
    }, []);

    const getWidgetTempdata = useCallback((widgetId: string) => {

        return widgetsTempData.current[widgetId] ? widgetsTempData.current[widgetId]: {};
    }, []);

    const apiProps = { 
        setValue,
        validateField,
        updateField,
        prepareFieldRequest,
        setWidgetTempdata,
        getWidgetTempdata,
        getEntitySchema: () => { return entitySchema },
        getField: (w: string) => entitySchema.getField(w),
        getErrorResult: () => { }
    }

    return {
        dataModel,
        isSubmitting,                              
        getControl,
        executeAction,
        ...apiProps
    };
}

function execAllFormFeature(controlId: string, dataModel: IDictionary<IFieldData>, entitySchema: IPageInfo, onlyFire: Array<string> | null, api: FormApi, cb: Function) {
    const field = entitySchema.getField(controlId);
    if (!field) return;
    
    const dependents = entitySchema.Dependencies[controlId];
    
    if (dependents && dependents.length > 0) {
        _.each(dependents, (fieldId) => {
            const depField = entitySchema.getField(fieldId);
            dataModel = execWidgetFeature(field, depField, dataModel, entitySchema, api, onlyFire)
        })
    }
    cb(dataModel);
}

function execWidgetFeature(parentField: WidgetInfo, depField: WidgetInfo, dataModel: IDictionary<IFieldData>, entitySchema: IPageInfo, api: FormApi, onlyFire: Array<string> | null) {
    _.forIn(depField.Features, (f, fType) => {
        if (onlyFire && onlyFire.indexOf(fType) < 0) return;
        
        if(fType === "Invisible") {
            dataModel = formFeatures.FieldVisibility.execute(depField, parentField.WidgetId, dataModel, api);
        } else if(fType === "Eval") {
            dataModel = formFeatures.ExpressionEval.execute(depField, dataModel, api);
        } else if(fType === "Options") {
            dataModel = formFeatures.OptionsVisibility.execute(depField, parentField.WidgetId, dataModel, api);
        } else if(fType === "ReadOnly") {
            dataModel = formFeatures.FieldReadOnly.execute(depField, parentField.WidgetId, dataModel, api);
        }
    });

    return dataModel;
}

function executeDependecy(controlId: string, entModel: IDictionary<IFieldData>, api: FormApi, cb: Function) {
    entModel = formFeatures.FieldDependency.execute(controlId, entModel, api);

    cb(entModel);
}


type UseFormOptions = {
    entitySchema: IPageInfo,
    formData: IDictionary<IFieldData>,
    controlFactory: { getComponent: Function, HiddenField: any, BlockElement: any }
}

type UseFormMethod = {
    dataModel: IDictionary<IFieldData>,
    isSubmitting: boolean,
    setValue: Function,
    updateField: Function,
    validateField: Function,
    prepareFieldRequest: Function,
    getControl: Function,
    executeAction: Function
}