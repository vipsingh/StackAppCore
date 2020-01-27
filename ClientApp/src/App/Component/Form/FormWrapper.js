import React from "react";
import PropTypes from "prop-types";
import _ from "lodash";
import ActionBar from "../UIView/ActionBar";
import {getControlComponent} from "./ControlFactory";
import FieldWrapper from "./Control/FieldWrapper";

import BaseForm from "./BaseForm";
import { validateField } from "~/Core/Form/Utils/Validations";
import formFeatures from "~/Core/Form/Features";
import PageWrapper from "../PageWrapper";

export default class FormWrapper extends PageWrapper {
    static contextTypes = {
        router: PropTypes.object
    }

    static propTypes = {
        Schema: PropTypes.object,
        Formdata: PropTypes.object
    }

    constructor(props) {
        super(props);
       
    }
    
    setValue = (controlId, value) => {
        const {Fields} = this.state.entityModel;
        const field = _.assign({}, Fields[controlId], {Value: value, IsDirty: true});
        
        this.setValidationResult(field);

        const entModel = Object.assign({}, this.state.entityModel, {Fields: _.assign({}, Fields, {[controlId]: field}) });

        this.execAllFormFeature(controlId, entModel, (m) => {
            this.setState({entityModel: m});
        });

        
    }

    execAllFormFeature(controlId, entModel, cb) {
        const field = entModel.Fields[controlId];
        if (field.RuleToFire && field.RuleToFire.length > 0) {
            _.each(field.RuleToFire, (rule) => {
                const rFire = _.find(entModel.Rules, { Index: rule.Index });
                if (rFire) {
                    if(rFire.Type === "INVISIBLE") {
                        entModel = formFeatures.FieldVisibility.execute(rFire, field, entModel);
                    }
                }
            });
        }
        cb(entModel);
    }

    execFormFeature(controlId, featureName) {

    }

    validate() {

    }

    validateField(controlId) {
        return null;
    }

    setValidationResult(controlInfo) {
        const ValidateResult = this.validateField(controlInfo, controlInfo.Value);
        Object.assign(controlInfo, { ValidateResult });

        return controlInfo;
    }

    setErrorField(controlId) {

    }

    executeAction = (command) => {
        const { ExecutionType, Url } = command;
        if (ActionType === 10) { //save
            this.performSubmit(command);
        } else {
            super.executeAction(command);
        }

    }

    performSubmit(linkInfo) {
        debugger;
        const { EntityContext } = this.state.entityModel;
        let onlyChanged = true;
        if (!EntityContext.ObjectId || EntityContext.ObjectId <= 0) {
            onlyChanged = false;
        }

        const toSaveModel = this.getFormDataToSubmit(onlyChanged);
        _App.Request.getData({
            url: linkInfo.URL,
            body: toSaveModel
        }).then((result) => {
            _Debug.log(result);
            this.handeleSubmitResponse(result);
        }).catch((ex) => {
            _Debug.error("Submit Error.");
            _Debug.error(ex);
        });
    }

    handeleSubmitResponse(result) {
        const { Status, RedirectUrl, Model, Message } = result;
        if (Status === 0) {
            window._App.Notify.success("Data Saved");
            if (RedirectUrl) {
                _Debug.log("REDIRECT: " + RedirectUrl);
            }
        } else if(Status === 2) {//FAIL
            window._App.Notify.error(Message);
            _Debug.log("FAIL: " + Message);
        }
    }

    getFormDataToSubmit(onlyChanged = true) {
        const { EntityContext, Fields } = this.state.entityModel;
        const model = {EntityContext};
        const f = {};
        _.forIn(Fields, (val, key) => {
            if (!onlyChanged || val.IsDirty) {
                f[key] = {
                    Properties: val.Properties,
                    ControlId: val.ControlId,
                    Value: val.Value
                };
            }
        });

        model.Fields = f;

        return model;
    }

    prepareFieldRequest = (fieldId) => {        
        const f = this.state.entityModel.Fields[fieldId];
        const r = {
            FieldId: fieldId,
            Properties: f.Properties,
            EntityContext: this.state.entityModel.EntityContext,
            FieldValue: f.Value
        };

        if (f.Parents) {
            r.Model = [];
            _.each(f.Parents, (p) => {
                const v = this.state.entityModel.Fields[p.Id];

                r.Model.push({ControlId: v.ControlId, Value: v.Value});
            });
        }

        return r;
    }

    getFormAPI() {
        return {
            setValue: this.setValue,
            prepareFieldRequest: this.prepareFieldRequest
        };
    }

    render() {
        return (
            <div>
                <BaseForm 
                    Schema={this.props.Schema}
                    getControl={this.getControl.bind(this)}
                />
                <ActionBar commands={this.props.Schema.Commands} onCommandClick={this.executeAction} />
            </div>
        );
    }
}