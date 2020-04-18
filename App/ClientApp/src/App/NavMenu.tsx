import React from "react";
import _ from "lodash";
import { Menu } from "antd";
import { HomeOutlined } from '@ant-design/icons';
import { Link } from "react-router-dom";
import ActionButton from "../Component/ActionLink";

const SubMenu = Menu.SubMenu;
const MenuItem = Menu.Item;

export default class NavMenu extends React.PureComponent {
  renderAppMenues() {
    if (window._PAGE_DATA_) {
      const menues = window._PAGE_DATA_.SideMenues;

      return _.map(menues, (m) => {
        return this.renderItem(m);
      });
    }
  }

  renderItem(action: any) {

    return (
      <MenuItem key={_.uniqueId("menu")}>
            <ActionButton {...action} />
      </MenuItem>
    );
  }

  render() {
    return (
      <Menu theme="dark" defaultSelectedKeys={["1"]} mode="inline">
        <Menu.Item key="1">
          <HomeOutlined />
          <span>Home</span>
        </Menu.Item>

        {this.renderAppMenues()}

        <Menu.Item key={_.uniqueId("menu")}>
          <Link to={"/Studio"}>{"Builder"}</Link>
        </Menu.Item>

      </Menu>
    );
  }
}
