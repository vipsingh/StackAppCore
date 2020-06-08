import React from "react";
import Form from "./Form";
import PageContext from "../../Core/PageContext";
//import StatusCode from "../../Constant/StatusCodes";
import ViewPageInfo from "../../Core/Models/ViewPageInfo";
import LinkProcesser from "../../Core/Utils/LinkProcesser";
//import _ from "lodash";

export interface EntityFormProps {
    Schema: any,
    FormData?: any,
    render: Function,
    entityModel?: ViewPageInfo
}

export default class EntityForm extends React.Component<EntityFormProps, {
    entityModel: ViewPageInfo,
    dataModel: IDictionary<IFieldData>
}> {
    static contextType = PageContext;
    //declare context: React.ContextType<typeof PageContext>

    constructor(props: any, cntx: any) {
        super(props, cntx);

        const { entityModel, dataModel }: any = this.mergeFields(props);

        this.state = {
            entityModel,
            dataModel
        };
    }

    componentWillReceiveProps(nextProps: EntityFormProps) {
        if (nextProps.FormData !== this.props.FormData) {
            const { entityModel, dataModel }: any = this.mergeFields(nextProps);
            this.setState({ entityModel, dataModel })
        }
    }

    getEntityModel() {
        if(this.props.entityModel) {
            return this.props.entityModel;
        }

        return this.state.entityModel;
    }

    mergeFields(model: ViewPageInfo| EntityFormProps) {
        if (model && model instanceof  ViewPageInfo) {
            return model;
        }

        const entityModel: ViewPageInfo = new ViewPageInfo(this.props.Schema);
        return { entityModel, dataModel: entityModel.getDataModel() };
    }    

    performSubmit = (toSaveModel: any) => {      
        const { PostUrl } = this.getEntityModel();  
        
        _App.Request.getData({
            url: PostUrl,
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
        LinkProcesser.handeleResponse(result, this.context.navigator);
    }

    updateForm = (model: IDictionary<IFieldData>, updateBy: { updateBy: string, param?: string }, afterChange: () => void = () => {}) => {
        this.setState({dataModel: model}, afterChange);
    }

    render() {

        return (
            <Form 
                {...this.props}
                entityModel={this.state.entityModel}
                dataModel={this.state.dataModel}
                onSubmit={this.performSubmit}
                onFormUpdate={this.updateForm}
            />
        )
    }
}
//onExecuteAction={this.executeAction}