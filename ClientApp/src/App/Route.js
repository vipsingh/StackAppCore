import React, { Component } from "react";
import PropTypes from "prop-types";
import { Route, Link, Switch, Redirect } from "react-router-dom";
import PageFactory from "./Page/Factory";
import ObjectBuilder from "./Page/ObjectBuilder";

function AppRoutes() {
  return (
    <Switch>
      <Route exact path="/" component={NotFound} />
      <Route exact path="/builder" component={ObjectBuilder} />
      <Route path="/web/entity/:type" component={RouteComponent} />
      <Route component={NotFound} />
    </Switch>
  );
}

function redirectToBase() {
  return <Redirect to={window._CURRENT_URL_} />;
}

class NotFound extends React.Component {
  render() {
    return (
      <div>
        <label>{"Page is not available"}</label>
      </div>
    );
  }
}

class RouteComponent extends React.Component {
  static propTypes = {
    match: PropTypes.object,
    location: PropTypes.object
  };

  constructor(props) {
    super(props);
    this.state = {
      loaded: false,
      pageType: ""
    };
  }

  componentDidMount() {
    this.loadPageData(this.props.location, d => {});
  }

  loadPageData(location) {
    this.setState({ loaded: false });

    let path = location.pathname;
    path = path.replace("web/", "");

    const q = location.search;

    const url = `${path}${q}`;

    window._App.Request.getData({
      url: url,
      type: "GET"
    })
      .then(data => {        
        const schema = data;
        const pageType = schema.PageType;
        this.setState({ pageType, Data: data, loaded: true });
      })
      .catch(err => {
        console.error(err);
        this.setState({ Data: null, loaded: true });
      });
  }

  componentWillReceiveProps(nextProps) {
    var that = this;

    const type = this.props.location.pathname;
    const q = this.props.location.search;

    const n_type = nextProps.location.pathname;
    const n_q = nextProps.location.search;

    if (type !== n_type || q !== n_q) {
      this.loadPageData(nextProps.location, d => {});
    }
  }

  render() {
    const { pageType, Data } = this.state;

    if (this.state.loaded && Data) {
      const Component = PageFactory.getPage(pageType.toUpperCase());
      return <Component Data={Data} />;
    } else {
      return <label>routing</label>;
    }
  }
}

export default AppRoutes;
