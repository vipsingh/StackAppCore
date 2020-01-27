import React from "react";
import PropTypes from "prop-types";
import _ from "lodash";
import ActionBar from "./UIView/ActionBar";
import {getWidgetComponent} from "./WidgetFactory";
import {getControlComponent} from "./Form/ControlFactory";
import FieldWrapper from "./Form/Control/FieldWrapper";
import PageView from "./PageView";
import HiddenField from "./Form/Control/HiddenField";

export default class PageWrapper extends React.Component {
    static contextTypes = {
        router: PropTypes.object
    }

    static propTypes = {
        Schema: PropTypes.object,
        Formdata: PropTypes.object
    }

    constructor(props) {
        super(props);

        this.state = {
            entityModel : this.mergeFields()
        };
    }

    mergeFields() {
        const {Controls, Commands, EntityContext, Rules} = this.props.Schema;
        const entityModel = {Fields: {}, Commands, EntityContext, Rules };
        _.each(Controls, (f) => {
            entityModel.Fields[f.ControlId] = _.assign({}, f);
        });

        entityModel.getField = function(controlId) {
            const f = this.Fields[controlId];

            return f;
        };
        entityModel.getField.bind(entityModel);
        

        return entityModel;
    }

    renderField(ControlId) {
        const cinfo = this.state.entityModel.Fields[ControlId];
        const {ControlType, IsViewMode} =  cinfo;    
        
        let IComponent = getWidgetComponent(ControlType, IsViewMode);
        if (!IComponent) {
            IComponent = getControlComponent(ControlType, IsViewMode);        
        }
        
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

    getControl(ControlId, customProps) {
        const inputFieldComponent = this.renderField(ControlId);
        const cinfo = this.state.entityModel.Fields[ControlId];

        return (<FieldWrapper {...cinfo} Label={cinfo.Caption} {...customProps}>
            {inputFieldComponent}
        </FieldWrapper>);
    }

    setValue = (controlId, value) => {

    }

    executeAction = (command) => {
        const { ExecutionType, Url } = command;

        this.context.router.history.push(Url);

    }

    getFormAPI() {
        return {
            
        };
    }

    render() {
        return (
            <div>
                <PageView 
                    Schema={this.props.Schema}
                    getControl={this.getControl.bind(this)}
                />
                <ActionBar commands={this.props.Schema.Commands} onCommandClick={this.executeAction} />
            </div>
        );
    }
}