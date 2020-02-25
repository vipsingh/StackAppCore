import React from "react";
import PropTypes from "prop-types";
import _, { Dictionary } from "lodash";
import { getComponent } from "../WidgetFactory";
import FormField from "./FormField";
import HiddenField from "./Control/HiddenField";
import formFeatures from "../../Core/Form/Features";

export default class Form extends React.Component<{
            Schema?: any,
            Formdata?: any,
            render: Function,
            entityModel?: ViewPageInfo,
            onChange?: Function,
            onSubmit?: Function
        }, {
            entityModel: ViewPageInfo
        }> 
{
    static contextTypes = {
        router: PropTypes.object
    }

    constructor(props: any) {
        super(props);

        this.state = {
            entityModel : this.mergeFields(props)
        };
    }

    componentWillReceiveProps(nextProps: any) {
        if (nextProps.entityModel) {

        }
    }

    mergeFields(model: ViewPageInfo| null = null) {
        if (model) {
            return model;
        }

        const entityModel: ViewPageInfo = new ViewPageInfo(this.props.Schema);
        return entityModel;
    }

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

        return (<FormField {...cinfo} {...customProps}>
            {inputFieldComponent}
        </FormField>);
    }

    getEntityModel() {
        if(this.props.entityModel) {
            return this.props.entityModel;
        }

        return this.state.entityModel;
    }

    setEntityModel(model: ViewPageInfo) {
        if (this.props.onChange) {
            this.props.onChange(model);
        } else {
            this.setState({entityModel: model});
        }
    }

    setValue = (controlId: string, value: any) => {
        const {Widgets} = this.getEntityModel();
        const field = _.assign({}, Widgets[controlId], {Value: value, IsDirty: true});
        
        this.setValidationResult(field);

        const entModel = Object.assign({}, this.getEntityModel(), {Widgets: _.assign({}, Widgets, {[controlId]: field}) });

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
            resolve(true);
        });
    }

    validateField(controlInfo: WidgetInfoProps, value: any) {
        return null;
    }

    setValidationResult(controlInfo: WidgetInfoProps) {
        const ValidateResult = this.validateField(controlInfo, controlInfo.Value);
        Object.assign(controlInfo, { ValidateResult });

        return controlInfo;
    }

    setErrorField(controlId: string) {

    }



    public executeAction(action: any) {
        const { Url } = action;

        this.context.router.history.push(Url);

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
                    Value: val.Value
                };
            }
        });

        model.Widgets = f;

        return model;
    }

    getFormAPI() {
        return {
            updateField: this.updateField
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

    constructor(schema: any) {
        this.Widgets = {};

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