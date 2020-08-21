import { Layout, Menu, Avatar } from "antd";
import React, { Fragment } from "react";

export default class Header extends React.Component {
    handleClickMenu = (e: any) => {
        //e.key === 'SignOut' && this.props.onSignOut();
    }

    render() {
      const logoUrl = `${_AppSetting.AssetUrl}/img/logo.png`;

        const rightContent = [
            <Menu key="user" mode="horizontal" onClick={this.handleClickMenu}>
              <Menu.SubMenu
                title={
                  <Fragment>
                    <span style={{ color: '#999', marginRight: 4 }}>
                      Hi,
                    </span>
                    <span>{"Alex Don"}</span>
                    <Avatar style={{ marginLeft: 8 }} />
                  </Fragment>
                }
              >
                <Menu.Item key="SignOut">
                  Sign out
                </Menu.Item>
              </Menu.SubMenu>
            </Menu>,
        ];
        
        return (<Layout.Header className="app-header">
                    <div className="logo"><img alt="logo" src={logoUrl} width="100%" style={{ marginTop: "-40px" }}/></div>
                    <div className="right-container">{rightContent}</div>
          </Layout.Header>);
    }
}