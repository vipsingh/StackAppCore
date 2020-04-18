import React from "react";
import _ from "lodash";
import Form, { ViewPageInfo } from "./Form";
import PageContext from "../../Core/PageContext";
import StatusCode from "../../Constant/StatusCodes";

export interface EntityFormProps {
    Schema?: any,
    FormData?: any,
    render: Function,
    entityModel?: ViewPageInfo
}

export default class EntityForm extends React.Component<EntityFormProps, {
    entityModel: ViewPageInfo
}> {
    static contextType = PageContext;
    //declare context: React.ContextType<typeof PageContext>

    constructor(props: any, cntx: any) {
        super(props, cntx);

        this.state = {
            entityModel : this.mergeFields(props)
        };
    }

    getEntityModel() {
        if(this.props.entityModel) {
            return this.props.entityModel;
        }

        return this.state.entityModel;
    }

    mergeFields(model: ViewPageInfo| null = null) {
        if (model && model instanceof  ViewPageInfo) {
            return model;
        }

        const entityModel: ViewPageInfo = new ViewPageInfo(this.props.Schema);
        return entityModel;
    }    

    performSubmit = (linkInfo: any, toSaveModel: any) => {        
        _App.Request.getData({
            url: linkInfo.Url,
            body: toSaveModel
        }).then((result: any) => {
            _Debug.log(result);
            this.handeleSubmitResponse(result);
        }).catch((ex: any) => {
            _Debug.error("Submit Error.");
            _Debug.error(ex);
        });
    }

    handeleSubmitResponse(result: RequestResultInfo) {
        const { Status, RedirectUrl, Message } = result;
        if (Status === StatusCode.Success) {
            window._App.Notify.success("Data Saved");
            if (RedirectUrl) {
                _Debug.log("REDIRECT: " + RedirectUrl);
                this.context.navigator.navigate(RedirectUrl);
            }
        } else if(Status === StatusCode.Fail) {//FAIL
            window._App.Notify.error(Message);
            _Debug.log("FAIL: " + Message);
        }
    }

    updateForm = (model: ViewPageInfo) => {
        this.setState({entityModel: model});
    }

    render() {

        return (
            <Form 
                {...this.props}
                entityModel={this.state.entityModel}
                onSubmit={this.performSubmit}
                onFormUpdate={this.updateForm}
            />
        )
    }
}
//onExecuteAction={this.executeAction}