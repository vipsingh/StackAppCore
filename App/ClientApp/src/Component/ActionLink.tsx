import React from "react";
import { Button } from "antd";
//import { Link } from "react-router-dom";
import { ButtonType } from "antd/lib/button";
import { openDialog } from "./UI/Dialog";
import { openDrawer } from "./UI/Drawer";
import PageComponent from "./PageComponent";
import PageContext from "../Core/PageContext";
import fIcon from "./UI/Icon";

interface ActionLinkProps extends ActionInfo {
    overrideProps?: any
}

export default class ActionLink extends React.Component<ActionLinkProps> {
    static contextType = PageContext;
    
    onCommandClick = () => {
        const prop = this.props;
        if (prop.onClick) {
            prop.onClick(prop);

            return;
        }
    }

    openInTarget = () => {
        const { Url: url, LinkTarget } = this.props;
        if (!url) return;

        const asurl = new URL(url.toLowerCase().indexOf("http") === 0 ? url: `http:/${url}`);
        let target = LinkTarget;
        if (asurl.searchParams) {
            const t = asurl.searchParams.get('target');
            if (t) target = t;
        }
        
        if (target === "POPUP") {
            openDialog("<>", (dlgProps: any) => {
                return <PageComponent url={url} openerNavigator={this.context.navigator} popup={dlgProps} overrideProps={this.props.overrideProps} />
            }, { hideCommands: true, size: "lg" });
        } else if (target === "DRAWER") {
            openDrawer("<>", <PageComponent url={url} openerNavigator={this.context.navigator} popup overrideProps={this.props.overrideProps} />, { hideCommands: true });
        } else {
            this.context.navigator.navigate(url);
        }
    }

    navigate = (event: any) => {
        event.preventDefault();
        //const { LinkTarget } = this.props;
        //if (LinkTarget) {
        //    this.openInTarget();
        //}
        this.openInTarget();
        //this.context.navigator.navigate(this.props.Url);
    }
    
    renderLink() {
        const { Title, ActionId, Url, Attributes, Icon, DisplayType } = this.props;
        const title = Title || ActionId;
        let caption = Attributes && Attributes.OnlyIcon ? "" : title;

        if (DisplayType !== 2) {
            let u = `/${Url}`;
            u = u.replace("//", "/");

            return <a title={title} href={u} onClick={this.navigate}>{caption}</a>;
        }

        return caption;
    }

    getButtonType(): { type: ButtonType, isDanger: boolean } {
        const { Attributes, ActionType } = this.props;
        const acTyp = ActionType || 0;
        let res: { type: ButtonType, isDanger: boolean } = {type: "default", isDanger: false};
        
        if (Attributes && Attributes.ButtonStyle) {

            res.type = Attributes.ButtonStyle as ButtonType;
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
                    onClick={prop.ExecutionType === 4 ? this.navigate : this.onCommandClick}
                    type={type.type}
                    danger={type.isDanger}
                    icon={fIcon(prop.Icon)}
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