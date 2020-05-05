import React from "react";
import { Button } from "antd";
//import { Link } from "react-router-dom";
import { ButtonType } from "antd/lib/button";
import { openDialog } from "./UI/Dialog";
import PageComponent from "./PageComponent";
import PageContext from "../Core/PageContext";

export default class ActionLink extends React.Component<ActionInfo> {
    static contextType = PageContext;
    
    onCommandClick = () => {
        const prop = this.props;
        if (prop.onClick) {
            prop.onClick(prop);

            return;
        }
    }

    openInTarget = () => {
        const { Url, Target } = this.props;
        
        if (Target === "POPUP") {
           openDialog("<>", <PageComponent url={Url} history={this.context.navigator.history} popup />, { hideCommands: true, size: "lg" });
        }
    }

    navigate = (event: any) => {
        event.preventDefault();
        this.context.navigator.navigate(this.props.Url);
    }
    
    renderLink() {
        const { Title, ActionId, ExecutionType, Url, OnlyIcon, Target } = this.props;
        const title = Title || ActionId;
        let caption = OnlyIcon ? "" : title;

        if ((!ExecutionType || ExecutionType === 4) && Url) {
            let u = `/${Url}`;
            u = u.replace("//", "/");

            if (Target) {
                return <Button type="link" onClick={this.openInTarget} style={{ height:0, padding:0 }}>{caption}</Button>;
            }

            return <a title={title} href={u} onClick={this.navigate}>{caption}</a>;
        }

        return caption;
    }

    getButtonType(): { type: ButtonType, isDanger: boolean } {
        const { ButtonStyle, ActionType } = this.props;
        const acTyp = ActionType || 0;
        let res: { type: ButtonType, isDanger: boolean } = {type: "default", isDanger: false};
        
        if (ButtonStyle) {

            res.type = ButtonStyle as ButtonType;
        } else if([2,10,11,12].indexOf(acTyp) >= 0) {
            res.type = "primary";
        } else if([5,9,13].indexOf(acTyp) >= 0) {
            res.type = "primary";
            res.isDanger = true;
        }

        return res;
    }

    render() {
        const prop = this.props;
        const type = this.getButtonType();

        if (prop.DisplayType === 2) {
            return (
                <Button 
                    key={prop.ActionId}
                    onClick={prop.ExecutionType === 4 ? undefined : this.onCommandClick}
                    type={type.type}
                    danger={type.isDanger}
                    >
                        {
                            this.renderLink()
                        }
                </Button>
            );
        } else {
            return this.renderLink();
        }
    }
}