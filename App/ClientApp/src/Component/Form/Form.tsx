import React from "react";
import update from "immutability-helper";
import _ from "lodash";
import { notification } from 'antd';
import { getComponent } from "../WidgetFactory";
import FormField from "./FormField";
import HiddenField from "./Control/HiddenField";
import formFeatures from "../../Core/Form/Features";
import LinkProcesser from "../../Core/Utils/LinkProcesser";
import validationUtil from "../../Core/Form/Utils/Validations";
import ActionBar from "../Layout/ActionBar";
import ActionLink from "../ActionLink";
import { prepareFieldRequest, getFormDataToSubmit } from "../../Core/Form/Utils/FormUtils";

export interface FormProps {    
    render: Function,
    entityModel: IPageInfo,
    dataModel: IDictionary<IFieldData>
    onSubmit: Function,
    onFormUpdate: Function,
    onExecuteAction?: Function
}

export default class Form extends React.Component<FormProps, {    
    errors: IDictionary<{ IsValid: boolean, Message?: string }>
}> {    
    WidgetsTempData: any
    constructor(props: FormProps, ctx: any) {
        super(props, ctx);        

        this.state = {
            errors: {}
        }

        this.WidgetsTempData = {};
    }

    componentWillReceiveProps(nextProps: any) {
        if (nextProps.entityModel) {

        }
    }

    componentDidMount() {
        let model = this.getDataModel();
        const keys = Object.keys(model);
        var ix = 0;

        const next = (m: any) => {
            if (ix === keys.length) {
                this.setDataModel(m, { updateBy: "FORM" });
            } else
                this.execAllFormFeature(keys[ix++], m, this.getEntitySchema(), [], next); 
        };
        next(model);
    }

    renderField(ControlId: string, customProps: any) {
        const cinfo = this.getField(ControlId);
        if (!cinfo) return null;

        const {WidgetType, IsViewMode} =  cinfo;    
        
        let IComponent = getComponent(WidgetType, IsViewMode);        
        
        if (!IComponent)
            return <div>INVALID COMPONENT</div>;

        if (cinfo.IsHidden) {
            IComponent = HiddenField;
        }

        const dataModel = this.getDataModel();
        const widgetModel = dataModel[ControlId];

        if (widgetModel.Invisible) {
            
            return (<div></div>);
        }

        return (<IComponent 
            {...cinfo}
            {...customProps}
            {...widgetModel}
            api={this.getFormAPI()}
            onChange={this.setValue.bind(this, ControlId)}
            onBlur={this.onBlur.bind(this, ControlId)}
        />);
    }

    public getControl(ControlId: string, customProps: any) {
        const inputFieldComponent = this.renderField(ControlId, customProps);
        if (!inputFieldComponent) return null;
        
        const cinfo = this.getField(ControlId);   
        const dataModel = this.getDataModel(); 
        const widgetModel = dataModel[ControlId];

        return (<FormField {...cinfo} {...customProps} {...widgetModel} api={this.getFormAPI()}>
            {inputFieldComponent}
        </FormField>);
    }

    getEntitySchema() {
        return this.props.entityModel;        
    }

    setDataModel(model: IDictionary<any>, updateBy: { updateBy: string, param?: string }, afterUpdate?: Function) {
        if (this.props.onFormUpdate) {
            this.props.onFormUpdate(model, updateBy, afterUpdate);
        }

        //this.setState({ dataModel: model });
    }

    getDataModel() {
        return this.props.dataModel;
    }

    setValue = (controlId: string, value: any, afterChange?: Function) => {
        let dataModel = this.getDataModel();
        const entitySchema = this.getEntitySchema();

        const fieldInfo = this.getField(controlId);
        
        let model = update(dataModel, {[controlId]: { Value: {$set: value}, IsDirty: {$set: true}}});
        
        const vRes = validationUtil.validateField({...fieldInfo, ...model[controlId]});        
        model = update(model, {[controlId]: { HasError: {$set: !vRes.IsValid} }});

        const cErrors = this.setValidationResult(controlId, vRes);
        this.setState({errors: cErrors});

        this.execAllFormFeature(controlId, model, entitySchema, ["HIDDEN", "OPTIONS"], (m: IDictionary<IFieldData>) => {
            this.setDataModel(m, { updateBy: "WIDGET", param: controlId }, afterChange);
        });
        
        if (fieldInfo.Dependency) {
            this.executeDependecy(controlId, model, (m: IDictionary<IFieldData>) => {
                this.setDataModel(m, { updateBy: "WIDGET", param: controlId });
            });
        }
    }

    execAllFormFeature(controlId: string, dataModel: IDictionary<IFieldData>, entitySchema: IPageInfo, onlyFire: Array<string>, cb: Function) {
        const field = entitySchema.getField(controlId);
        
        if (field.RuleToFire && field.RuleToFire.length > 0) {
            _.each(field.RuleToFire, (rule) => {                
                const rFire = _.find(entitySchema.FormRules, { Index: rule });
                if (rFire) {
                    if (onlyFire.length > 0 && onlyFire.indexOf(rFire.Type.toUpperCase()) >= 0) {
                        dataModel = this.execFeature(rFire, field, dataModel);
                    } else if (onlyFire.length === 0) {
                        dataModel = this.execFeature(rFire, field, dataModel);
                    }
                }
            });
        }
        cb(dataModel);
    }

    execFeature(rFire: any, field: WidgetInfo, dataModel: IDictionary<IFieldData>): IDictionary<IFieldData> {
        const typ = rFire.Type.toUpperCase();
        if(typ === "HIDDEN") {
            dataModel = formFeatures.FieldVisibility.execute(rFire, field.WidgetId, dataModel, this.getFormAPI());
        } else if(typ === "OPTIONS") {
            dataModel = formFeatures.OptionsVisibility.execute(rFire, field.WidgetId, dataModel, this.getFormAPI());
        } else if(typ === "EVAL") {
            dataModel = formFeatures.ExpressionEval.execute(rFire, dataModel, this.getFormAPI());
        }

        return dataModel;
    }

    executeDependecy(controlId: string, entModel: IDictionary<IFieldData>, cb: Function) {
        entModel = formFeatures.FieldDependency.execute(controlId, entModel, this.getFormAPI());

        cb(entModel);
    }

    onBlur = (widgetId: string) => {
        const model = this.getDataModel();
        this.execAllFormFeature(widgetId, model, this.getEntitySchema(), ["EVAL"], (m: IDictionary<IFieldData>) => {
            this.setDataModel(m, { updateBy: "WIDGET", param: widgetId });
        });
    }

    validate(): PromiseLike<boolean> {
        return new Promise((resolve, reject) => {
            validationUtil.validateForm(this.getEntitySchema(), this.getDataModel()).then(errors => {
                let entitySchema = this.getEntitySchema();
                const dataModel = this.getDataModel();
                
                let model = dataModel;
                _.forIn(entitySchema.Widgets, (v, k) => {
                    model = update(model, {[k]: { HasError: {$set: errors[k]? !errors[k].IsValid: false}}});                    
                });

                this.setDataModel(model, { updateBy: "VALIDATE" });
                this.setState({ errors })
                if (Object.keys(errors).length > 0) {
                    const msg = _.map(errors, v => (<span>{v.Message}<br/></span>));
                    notification.error({ message: __L("Error"), description: <div>{msg}</div> });

                    resolve(false);
                } else {
                    resolve(true);
                }
            });
        });
    }

    validateField(widgetId: string) {
        const dataModel = this.getDataModel();
        let entitySchema = this.getEntitySchema();
        const fieldInfo = entitySchema.getField(widgetId);
        const vRes = validationUtil.validateField({...fieldInfo, ...dataModel[widgetId]});                
        
        this.updateField(widgetId, { HasError: !vRes.IsValid });
        
        const cErrors = this.setValidationResult(widgetId, vRes);
        this.setState({errors: cErrors});
    }

    setValidationResult(WidgetId: string, result: any) {
        const errors = this.state.errors;
            const cErrors = Object.assign({}, errors, {});
            if (result.IsValid) {
                delete cErrors[WidgetId];
            } else {
                cErrors[WidgetId] = result;
            }
        return cErrors;
    }

    getErrorResult = (widgetId: string) => {
        const errors = this.state.errors;
        if (errors)
            return errors[widgetId];
        else
            return null;
    }

    public executeAction = (action: any) => {
        const { ExecutionType } = action; //ActionType
        if (ExecutionType === 1) { //submit
            return this.handleSubmit();
        } else {
            // if (onExecuteAction) {
            //     onExecuteAction(action);
            // } 
            return LinkProcesser.processLink(action, {});
        }        
    }

    public updateField = (controlId: string, info: any, afterUpdate?: Function) => {
        const dataModel = this.getDataModel();
        
        let wd = update(dataModel, {[controlId]: { $merge: info }});
        
        return this.setDataModel(wd, { updateBy: "WIDGET", param: controlId }, afterUpdate);
    }

    handleSubmit = () => {
        const dataModel = this.getDataModel();
        const entitySchema = this.getEntitySchema();
        const { onSubmit } = this.props;
        this.validate().then((isValid) => {
            if (!isValid) {                    
                return;
            }

            const { EntityInfo } = entitySchema;
            let onlyChanged = true;
            if (!EntityInfo.ObjectId || EntityInfo.ObjectId <= 0) {
                onlyChanged = false;
            }
    
            const toSaveModel = getFormDataToSubmit(entitySchema, dataModel, onlyChanged);
    
            if (onSubmit) {
                onSubmit(toSaveModel);
            }
        });
    }    

    prepareFieldRequest = (widgetId: string) => {
        return prepareFieldRequest(this.getEntitySchema(), this.getDataModel(), widgetId);
    }

    renderActions = () => {
        const { Actions } = this.getEntitySchema();
        
        return <ActionBar commands={Actions} onCommandClick={this.executeAction} />
    }

    getFormActions = () => {
        const { Actions } = this.getEntitySchema();

        return  _.map(Actions, (c) => {
                return <ActionLink key={c.ActionId} {...c} DisplayType={2} onClick={this.executeAction} />;
            });        
    }

    getField = (widgetId: string): WidgetInfoProps => {
        return this.getEntitySchema().Widgets[widgetId];
    }

    setWidgetTempdata = (widgetId: string, data: any) => {
        this.WidgetsTempData[widgetId] = _.extend(this.WidgetsTempData[widgetId], data);
    }

    getWidgetTempdata = (widgetId: string) => {

        return this.WidgetsTempData[widgetId] ? this.WidgetsTempData[widgetId]: {};
    }

    getFormAPI(): FormApi {
        return {
            getEntitySchema: () => { return this.getEntitySchema(); },
            setValue: this.setValue,
            updateField: this.updateField,
            getField: this.getField,
            getErrorResult: this.getErrorResult,
            validateField: this.validateField.bind(this),
            prepareFieldRequest: this.prepareFieldRequest,
            setWidgetTempdata: this.setWidgetTempdata,
            getWidgetTempdata: this.getWidgetTempdata
        };
    }

    render() {
        const { render } = this.props;
        const formProps = {
            entityModel: this.getEntitySchema(),
            getControl: this.getControl.bind(this),
            executeAction: this.executeAction,
            handleSubmit: this.handleSubmit,
            renderActions: this.renderActions,
            getFormActions: this.getFormActions
        };

        return (
            <React.Fragment>
                {
                    render(formProps)
                }
            </React.Fragment>
        );        
    }
}