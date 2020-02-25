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
          <Sider
            collapsible
            collapsed={this.state.collapsed}
            onCollapse={this.onCollapse}
          >
            <div className="logo" />
            <NavMenu />            
          </Sider>
          <Layout>
            <Header />
            <Content >
              <div
                style={{
                  padding: 10,
                  minHeight: 360
                }}
              >
                <AppRoutes />
              </div>
            </Content>
            <Footer style={{ textAlign: "center" }}>StackERP ©2018</Footer>
          </Layout>
        </Layout>
      </Router>
    );
  }
}
