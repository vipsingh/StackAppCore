import React, {Component} from 'react';
import { Layout, Menu, Icon } from 'antd';
import {BrowserRouter as Router, Link} from "react-router-dom";
import AppRoutes from './Route';
import NavMenu from './NavMenu';

const { Header, Content, Footer, Sider } = Layout;

export default class MyApp extends Component{
  constructor(props) {
    super(props);
    
    this.state = {
      collapsed: false
    };
  }

  onCollapse = (collapsed) => {
    
    this.setState({ collapsed });
  }

  render() {
    return (
      <Router>
      <Layout style={{ minHeight: '100vh' }}>
        <Sider
          collapsible
          collapsed={this.state.collapsed}
          onCollapse={this.onCollapse}
        >
          <div className="logo" />
          <NavMenu />
          <Link to={"/builder"}>{"Builder"}</Link>
        </Sider>
        <Layout>
          <Header style={{ background: '#fff', padding: 0 }} />
          <Content style={{ margin: '0 16px' }}> 
            <div style={{ padding: 10, background: '#fff', minHeight: 360 }}>          
              <AppRoutes />
            </div>
          </Content>
          <Footer style={{ textAlign: 'center' }}>
            StackERP ©2018
          </Footer>
        </Layout>
      </Layout>
      </Router>
    );
  }
}
