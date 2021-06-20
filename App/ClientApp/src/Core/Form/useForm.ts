import React, { useState, useEffect, useRef, useCallback } from "react";
import _ from "lodash";
import update from "immutability-helper";
import validationUtil from "./Utils/Validations";
import formFeatures from "./Features";
import { prepareFieldRequest as prepareReq, getFormDataToSubmit } from "./Utils/FormUtils";
import * as componentFactory from "../ComponentFactory";
import LinkProcesser from "../Utils/LinkProcesser";

export function useForm({ entitySchema, formData }: UseFormOptions): UseFormMethod { 
    const widgetsTempData = useRef<IDictionary<any>>({});

    const [dataModel, setDataModel] = useState(formData);
    const [formState, setFormState] = useState<IDictionary<any>>({ isSubmitting: false, errors: {}});

    const formRendered = useRef(true);
    const dataModelRef = useRef(dataModel);

    useEffect(() => {
        if (formRendered.current) {
            intDefaultFeatures(formData);
            // setErrors({});
            // setOnSubmitting(false);
            //run features
        }
        formRendered.current = false;
      }, [formData]);

    const intDefaultFeatures = useCallback((model: any) => {
            const keys = Object.keys(model);
            var ix = 0;
    
            const next = (m: any) => {
                if (ix === keys.length) {
                    dataModelRef.current = m;
                    setDataModel(m);
                } else
                    execAllFormFeature(keys[ix++], m, entitySchema, ["Invisible", "Options", "ReadOnly"], apiProps, next);
            };
    
            if (!_.isEmpty(model)) next(model);
    }, []);
    
    const renderField = useCallback((widgetId: string, customProps: any) => {
        const cinfo = entitySchema.getField(widgetId);
        if (!cinfo) return null;

        const {WidgetType, IsViewMode} =  cinfo;    
        
        let IComponent = componentFactory.getComponent(WidgetType, IsViewMode);        
        
        if (!IComponent)
            return React.createElement(componentFactory.BlockElement(), { children: "INVALID COMPONENT" });

        if (cinfo.IsHidden) {
            IComponent = componentFactory.HiddenField();
        }

        const widgetModel = dataModelRef.current[widgetId];

        if (widgetModel.Invisible) {
            
            return React.createElement(componentFactory.BlockElement());
        }

        return React.createElement(IComponent, 
            {
                ...cinfo, 
                ...customProps, 
                ...widgetModel, 
                ...apiProps,
                onChange: (val: any) => setValue(widgetId, val),
                onBlur:(widgetId: any) => onBlur(widgetId)
            });
        //onBlur={this.onBlur.bind(this, ControlId)}
        //api={this.getFormAPI()}
    }, []);

    const getControl= useCallback((widgetId: string, customProps: any) => {
        const inputFieldComponent = renderField(widgetId, customProps);
        if (!inputFieldComponent) return null;
        
        const cinfo = entitySchema.getField(widgetId);   
        const widgetModel = dataModelRef.current[widgetId];     

        return inputFieldComponent; //React.createElement(FormField,  { widgetInfo: cinfo, dataModel: widgetModel, api: this.getFormAPI(), otherProps: customProps);
    }, []);

    const setValue = useCallback((widgetId: string, value: any, afterChange?: Function) => {

        const fieldInfo = entitySchema.Widgets[widgetId];
        
        let model = update(dataModelRef.current, {[widgetId]: { Value: {$set: value}, IsDirty: {$set: true}}});
        
        const vRes = validationUtil.validateField({...fieldInfo, ...model[widgetId]});        
        model = update(model, {[widgetId]: { HasError: {$set: !vRes.IsValid} }});

        const cErrors = setValidationResult(widgetId, vRes);
        setFormState({ errors: cErrors });

        execAllFormFeature(widgetId, model, entitySchema, ["Invisible", "Options", "ReadOnly"], apiProps, (m: IDictionary<IFieldData>) => {
            executeDependecy(widgetId, m, apiProps, (m1: IDictionary<IFieldData>) => {
                dataModelRef.current = m1;
                setDataModel(m1);
            });            
        });
    }, []);

    const onBlur = useCallback((widgetId: string) => {
        execAllFormFeature(widgetId, dataModelRef.current, entitySchema, ["Eval"], apiProps, (m: IDictionary<IFieldData>) => {
            dataModelRef.current = m;
            setDataModel(m);
        });
    }, []);

    useEffect(() => {

    }, []);

    const updateField= useCallback((controlId: string, info: any) => {
        let wd = update(dataModelRef.current, {[controlId]: { $merge: info }});
        
        //return this.setDataModel(wd, { updateBy: "WIDGET", param: controlId }, afterUpdate); //use useeffect in specific control for afterupdate
        dataModelRef.current = wd;
        setDataModel(wd);
    }, []);

    const validateField= useCallback((widgetId: string) => {
        const fieldInfo = entitySchema.getField(widgetId);
        const vRes = validationUtil.validateField({...fieldInfo, ...dataModelRef.current[widgetId]});                
        
        updateField(widgetId, { HasError: !vRes.IsValid });
        
        const cErrors = setValidationResult(widgetId, vRes);
        setFormState({ errors: cErrors });
    }, []);

    const setValidationResult = useCallback((WidgetId: string, result: any) => {
            const cErrors = Object.assign({}, formState.errors, {});
            if (result.IsValid) {
                delete cErrors[WidgetId];
            } else {
                cErrors[WidgetId] = result;
            }
        return cErrors;
    }, [formState.errors]);

    const validate = useCallback(() => {
        return new Promise((resolve, reject) => {
            validationUtil.validateForm(entitySchema, dataModelRef.current).then(errors => {
                const dataModel = dataModelRef.current;
                
                let model = dataModel;
                _.forIn(entitySchema.Widgets, (v, k) => {
                    model = update(model, {[k]: { HasError: {$set: errors[k]? !errors[k].IsValid: false}}});                    
                });
                
                setFormState({ errors });
                dataModelRef.current = model;
                setDataModel(model);
                
                if (Object.keys(errors).length > 0) {
                    //const msg: any = _.map(errors, v => (<span>{v.Message}<br/></span>));
                    //notification.error({ message: __L("Error"), description: <div>{msg}</div> });

                    resolve([false, errors]);
                } else {
                    resolve([true, null]);
                }
            });
        });
    }, []);

    const executeAction= useCallback((action: ActionInfo) => {
        const { ExecutionType } = action; //ActionType
        if (ExecutionType === 1) { //submit
            return handleSubmit();
        } else {
            // if (onExecuteAction) {
            //     onExecuteAction(action);
            // } 
            return LinkProcesser.processLink(action, {}, null); //this.context.navigator
        }        
    }, []);

    const prepareFieldRequest= useCallback((widgetId: string) => {
        return prepareReq(entitySchema, dataModelRef.current, widgetId);
    }, []);

    const setWidgetTempdata = useCallback((widgetId: string, data: any) => {
        widgetsTempData.current[widgetId] = _.extend(widgetsTempData.current[widgetId], data);
    }, []);

    const getWidgetTempdata = useCallback((widgetId: string) => {

        return widgetsTempData.current[widgetId] ? widgetsTempData.current[widgetId]: {};
    }, []);

    const handleSubmit = useCallback(() => {
        const dataModel = dataModelRef.current;
        //const { onSubmit } = restProps;
        validate().then((validObj: any) => {
            if (!validObj.isValid) {  
                //onFormError(validObj.errors);                  
                return;
            }

            const { EntityInfo } = entitySchema;
            let onlyChanged = true;
            if (!EntityInfo.ObjectId || EntityInfo.ObjectId <= 0) {
                onlyChanged = false;
            }
    
            const toSaveModel = getFormDataToSubmit(entitySchema, dataModel, onlyChanged);
    
            // if (onSubmit) {
            //     onSubmit(toSaveModel);
            // }
        });
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
        isSubmitting: formState.isSubmitting,
        formState,                              
        getControl,
        renderField,
        executeAction,
        handleSubmit,
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


export type UseFormOptions = {
    entitySchema: IPageInfo,
    formData: IDictionary<IFieldData>
}

export type UseFormMethod = {
    dataModel: IDictionary<IFieldData>,
    isSubmitting: boolean,
    formState: any,
    setValue: Function,
    updateField: Function,
    validateField: Function,
    prepareFieldRequest: Function,
    getControl: Function,
    renderField: Function,
    executeAction: Function,
    handleSubmit: Function
}