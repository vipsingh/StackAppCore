import React from "react";
import PropTypes from "prop-types";
import _ from "lodash";
import Form, { ViewPageInfo } from "./Form";

export default class EntityForm extends Form {
    static contextTypes = {
        router: PropTypes.object
    }

    constructor(props: any) {
        super(props);
    }
    
    executeAction = (command: any) => {
        const { ExecutionType, Url, ActionType } = command;
        if (ActionType === 10) { //save
            this.performSubmit(command);
        } else {
            super.executeAction(command);
        }

    }

    performSubmit(linkInfo: any) {
        debugger;
        const { EntityInfo } = this.getEntityModel();
        let onlyChanged = true;
        if (!EntityInfo.ObjectId || EntityInfo.ObjectId <= 0) {
            onlyChanged = false;
        }

        const toSaveModel = this.getFormDataToSubmit(onlyChanged);
        _App.Request.getData({
            url: linkInfo.URL,
            body: toSaveModel
        }).then((result: any) => {
            _Debug.log(result);
            this.handeleSubmitResponse(result);
        }).catch((ex: any) => {
            _Debug.error("Submit Error.");
            _Debug.error(ex);
        });
    }

    handeleSubmitResponse(result: any) {
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

    prepareFieldRequest = (fieldId: string) => {        
        const f = this.getEntityModel().Widgets[fieldId];
        const r: any = {
            FieldId: fieldId,
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

    getFormAPI() {
        return Object.assign(super.getFormAPI(), {
            setValue: this.setValue,
            prepareFieldRequest: this.prepareFieldRequest
        });
    }

    render() {
        const { render } = this.props;
        const formProps = {
            Schema: this.props.Schema,
            getControl: this.getControl.bind(this),
            executeAction: this.executeAction
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