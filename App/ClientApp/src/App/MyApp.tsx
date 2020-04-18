import React, { Component } from "react";
import { Layout } from "antd";
import { BrowserRouter as Router } from "react-router-dom";
import AppRoutes from "./Route";
import NavMenu from "./NavMenu";

const { Header, Content, Footer, Sider } = Layout;

export default class MyApp extends Component<{}, {collapsed: boolean}> {
  constructor(props: any) {
    super(props);

    this.state = {
      collapsed: false
    };
  }

  onCollapse = (collapsed: boolean) => {
    this.setState({ collapsed });
  };

  render() {
    return (
      <Router>
        <Layout style={{ minHeight: "100vh" }}>
        <Header className="site-layout-background">
        <div className="logo"></div>
          </Header>          
          <Layout className="site-layout">            
          <Sider
            collapsible
            collapsed={this.state.collapsed}
            onCollapse={this.onCollapse}
          >            
            <NavMenu />            
          </Sider>
            <Content >
              <div className="site-layout-background"
                style={{
                  padding: 10,
                  minHeight: 360
                }}
              >
                <AppRoutes />
              </div>
              <Footer style={{ textAlign: "center" }}>StackERP ©2018</Footer> 
            </Content>
            
          </Layout>
          
        </Layout>
      </Router>
    );
  }
}
