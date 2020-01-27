import React from "react";
import _ from "lodash";
import { Menu, Icon } from "antd";
import { Link } from "react-router-dom";

const SubMenu = Menu.SubMenu;

export default class NavMenu extends React.PureComponent {
  constructor(props) {
    super(props);
  }

  renderAppMenues() {
    const menues = window._PAGE_DATA_.Menues;

    return _.map(menues, (m) => {
      return this.renderItem(m);
    });
  }

  renderItem(action) {

    return (
      <Menu.Item key={_.uniqueId("menu")}>
            <Link to={action.Url}>{action.Title}</Link>
      </Menu.Item>
    );
  }

  render() {
    return (
      <Menu theme="dark" defaultSelectedKeys={["1"]} mode="inline">
        <Menu.Item key="1">
          <Icon type="home" />
          <span>Home</span>
        </Menu.Item>

        {this.renderAppMenues()}

      </Menu>
    );
  }
}
