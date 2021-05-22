import React, { Component } from "react";
import { Layout } from "antd";

const { Content, Footer, Header } = Layout;

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
        <Layout style={{ minHeight: "100vh" }}>
        <Header />
        <Content >
              <div className="page-content-wrapper"
                style={{
                  padding: 10,
                  minHeight: 500
                }}
              >
              </div>
              <Footer style={{ textAlign: "center" }}>StackERP ©2021</Footer> 
        </Content>
        </Layout>
    );
  }
}
