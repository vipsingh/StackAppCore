import React from "react";
import PropTypes from "prop-types";
import _, { Dictionary } from "lodash";
import { getComponent } from "../WidgetFactory";
import FormField from "./FormField";
import HiddenField from "./Control/HiddenField";
import formFeatures from "../../Core/Form/Features";
import LinkProcesser from "../../Core/Utils/LinkProcesser";
import validationUtil from "../../Core/Form/Utils/Validations";

export interface FormProps {
    Schema?: any,
    FormData?: any,
    render: Function,
    entityModel: ViewPageInfo,
    onSubmit: Function,
    onFormUpdate: Function,
    onExecuteAction?: Function
}

export default class Form extends React.Component<FormProps> 
{
    static contextTypes = {
        router: PropTypes.object
    }

    // constructor(props: any) {
    //     super(props);

    //     this.state = {
    //         entityModel : this.mergeFields(props)
    //     };
    // }

    componentWillReceiveProps(nextProps: any) {
        if (nextProps.entityModel) {

        }
    }

    // mergeFields(model: ViewPageInfo| null = null) {
    //     if (model && model instanceof  ViewPageInfo) {
    //         return model;
    //     }

    //     const entityModel: ViewPageInfo = new ViewPageInfo(this.props.Schema);
    //     return entityModel;
    // }

    renderField(ControlId: string) {
        const cinfo = this.getEntityModel().Widgets[ControlId];
        if (!cinfo) return null;

        const {WidgetType, IsViewMode} =  cinfo;    
        
        let IComponent = getComponent(WidgetType, IsViewMode);        
        
        if (!IComponent)
            return <div>INVALID COMPONENT</div>;

        if (cinfo.IsHidden) {
            IComponent = HiddenField;
        }

        return (<IComponent 
            {...cinfo}
            api={this.getFormAPI()}
            onChange={this.setValue.bind(this, ControlId)}
        />);
    }

    public getControl(ControlId: string, customProps: any) {
        const inputFieldComponent = this.renderField(ControlId);
        if (!inputFieldComponent) return null;
        
        const cinfo = this.getEntityModel().Widgets[ControlId];

        return (<FormField {...cinfo} {...customProps} api={this.getFormAPI()}>
            {inputFieldComponent}
        </FormField>);
    }

    getEntityModel() {
        return this.props.entityModel;
        // if(this.props.entityModel) {
        //     return this.props.entityModel;
        // }

        // return this.state.entityModel;
    }

    setEntityModel(model: ViewPageInfo) {
        if (this.props.onFormUpdate) {
            this.props.onFormUpdate(model);
        }
    }

    setValue = (controlId: string, value: any) => {
        const {Widgets} = this.getEntityModel();
        const field = _.assign({}, Widgets[controlId], {Value: value, IsDirty: true});
        
        const vRes = validationUtil.validateField(field);
        field.HasError = !vRes.IsValid;
        const cErrors = this.setValidationResult(field.WidgetId, vRes);

        const entModel = Object.assign({}, this.getEntityModel(), {Widgets: _.assign({}, Widgets, {[controlId]: field}), Errors: cErrors });

        this.execAllFormFeature(controlId, entModel, (m: ViewPageInfo) => {
            this.setEntityModel(m);
        });

        
    }

    execAllFormFeature(controlId: string, entModel: ViewPageInfo, cb: Function) {
        const field = entModel.Widgets[controlId];
        if (field.RuleToFire && field.RuleToFire.length > 0) {
            _.each(field.RuleToFire, (rule) => {
                const rFire = _.find(entModel.Rules, { Index: rule.Index });
                if (rFire) {
                    if(rFire.Type === "INVISIBLE") {
                        entModel = formFeatures.FieldVisibility.execute(rFire, field.WidgetId, entModel);
                    }
                }
            });
        }
        cb(entModel);
    }

    execFormFeature(controlId: string, featureName: string) {

    }

    validate(): PromiseLike<boolean> {
        return new Promise((resolve, reject) => {
            validationUtil.validateForm(this.getEntityModel()).then(errors => {
                const { Widgets } = this.getEntityModel();
                const wgt: Dictionary<WidgetInfoProps> = {};
                _.forIn(Widgets, (v, k) => {
                    const field = _.assign({}, v, { HasError: errors[k]? !errors[k].IsValid: false });                    
                    wgt[k] = field;
                })
                const entModel = Object.assign({}, this.getEntityModel(), { Errors: errors, Widgets: Object.assign({}, Widgets, wgt) });
                
                this.setEntityModel(entModel);
                if (Object.keys(errors).length > 0) {
                    resolve(false);
                } else {
                    resolve(true);
                }
            });
        });
    }

    validateField(controlInfo: WidgetInfoProps) {
        const vRes = validationUtil.validateField(controlInfo);                
        
            this.updateField(controlInfo.WidgetId, { HasError: !vRes.IsValid });
            const cErrors = this.setValidationResult(controlInfo.WidgetId, vRes);
        
            const entModel = Object.assign({}, this.getEntityModel(), { Errors: cErrors });
            this.setEntityModel(entModel);
    }

    setValidationResult(WidgetId: string, result: any) {
        const {Errors} = this.getEntityModel();
            const cErrors = Object.assign({}, Errors, {});
            if (result.IsValid) {
                delete cErrors[WidgetId];
            } else {
                cErrors[WidgetId] = result;
            }
        return cErrors;
    }

    setErrorField(controlId: string) {

    }

    getErrorResult = (widgetId: string) => {
        return this.getEntityModel().Errors[widgetId];
    }

    public executeAction = (action: any) => {
        const { onSubmit, onExecuteAction } = this.props;
        const { ExecutionType } = action; //ActionType
        if (ExecutionType === 1) { //submit
            this.validate().then((isValid) => {
                const { EntityInfo } = this.getEntityModel();
                let onlyChanged = true;
                if (!EntityInfo.ObjectId || EntityInfo.ObjectId <= 0) {
                    onlyChanged = false;
                }
        
                const toSaveModel = this.getFormDataToSubmit(onlyChanged);
        
                if (onSubmit) {
                    onSubmit(action, toSaveModel);
                }
            });
        } else {
            // if (onExecuteAction) {
            //     onExecuteAction(action);
            // } 
            return LinkProcesser.processLink(action, {});
        }        
    }

    public updateField = (controlId: string, info: any) => {
        const {Widgets} = this.getEntityModel();
        const field = _.assign({}, Widgets[controlId], info);
        
        const entModel = Object.assign({}, this.getEntityModel(), {Widgets: _.assign({}, Widgets, {[controlId]: field}) });
        
        this.setEntityModel(entModel);
    }

    handleSubmit = () => {
        this.validate().then((isValid) => {
            if (isValid) {
                if (this.props.onSubmit) {
                    this.props.onSubmit(this.getFormDataToSubmit());
                }
            }
        })
    }

    getFormDataToSubmit(onlyChanged = true) {
        const { EntityInfo, Widgets } = this.getEntityModel();
        const model: any = {EntityInfo};
        const f: any = {};
        _.forIn(Widgets, (val, key) => {
            if (!onlyChanged || val.IsDirty) {
                f[key] = {
                    Properties: val.Properties,
                    WidgetId: val.WidgetId,
                    Value: val.Value,
                    WidgetType: val.WidgetType
                };
            }
        });

        model.Widgets = f;

        return model;
    }

    prepareFieldRequest = (widgetId: string) => {
        const f = this.getEntityModel().Widgets[widgetId];
        const r: any = {
            FieldId: widgetId,
            Properties: f.Properties,
            EntityInfo: this.getEntityModel().EntityInfo,
            FieldValue: f.Value
        };

        if (f.Parents) {
            r.Model = [];
            _.each(f.Parents, (p) => {
                const v = this.getEntityModel().Widgets[p.Id];

                r.Model.push({WidgetId: v.WidgetId, Value: v.Value});
            });
        }

        return r;
    }

    getFormAPI(): FormApi {
        return {
            setValue: this.setValue,
            updateField: this.updateField,
            getErrorResult: this.getErrorResult,
            validateField: this.validateField.bind(this),
            prepareFieldRequest: this.prepareFieldRequest
        };
    }

    render() {
        const { render } = this.props;
        const formProps = {
            Schema: this.props.Schema,
            getControl: this.getControl.bind(this),
            executeAction: this.executeAction,
            handleSubmit: this.handleSubmit
        };

        return (
            <div>
                {
                    render(formProps)
                }
            </div>
        );        
    }
}

export class ViewPageInfo {
    Widgets!: Dictionary<WidgetInfoProps>
    Actions!: Dictionary<any>
    EntityInfo!: ObjectModelInfo
    ErrorMessage!: string
    Layout!: any
    PostUrl!: string|undefined
    Rules!: any
    Errors!: Dictionary<{ IsValid: boolean, Message?: string }>

    constructor(schema: any) {
        this.Widgets = {};
        this.Errors = {};

        if (schema) {
            _.extend(this, {...schema});            

            _.each(schema.Widgets, (f) => {
                this.Widgets[f.WidgetId] = _.assign({}, f);
            });
        }
    }
    
    getField(controlId: string) {
        const f = this.Widgets[controlId];

        return f;
    }

}