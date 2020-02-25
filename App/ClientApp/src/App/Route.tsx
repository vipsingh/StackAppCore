
import React from "react";
import { Route, Switch, Redirect } from "react-router-dom";
import PageFactory from "./Page/Factory";
import Studio from "./Page/Studio";

function AppRoutes() {
  
  return (
    <Switch>
      <Route exact path="/" component={NotFound} />
      {/* <Route exact path="/builder" component={ObjectBuilder} /> */}
      <Route path="/web/:controller/:action" component={RouteComponent} />
      <Route exact path="/Studio" component={Studio} />
      <Route component={NotFound} />
    </Switch>
  );
}

// function redirectToBase() {
//   return <Redirect to={window._CURRENT_URL_} />;
// }

class NotFound extends React.Component {
  render() {
    return (
      <div>
        <label>{"Page is not available"}</label>
      </div>
    );
  }
}

class RouteComponent extends React.Component<{
  match: any,
  location: any
}, {
  loaded: boolean,
  pageType: string,
  data: any
}> {
  
  constructor(props: any) {
    super(props);
    this.state = {
      loaded: false,
      pageType: "",
      data: null
    };
  }

  componentDidMount() {
    this.loadPageData(this.props.location);
  }

  loadPageData(location: any) {
    this.setState({ loaded: false });

    let path = location.pathname;
    path = path.replace("web/", "");

    const q = location.search;

    const url = `${path}${q}`;

    window._App.Request.getData({
      url: url,
      type: "GET"
    })
      .then((data: any) => {        
        const schema = data;
        const pageType = schema.PageType;
        this.setState({ pageType, data: data, loaded: true });
      })
      .catch((err: any) => {
        console.error(err);
        this.setState({ data: null, loaded: true });
      });
  }

  componentWillReceiveProps(nextProps: any) {
    var that = this;

    const type = this.props.location.pathname;
    const q = this.props.location.search;

    const n_type = nextProps.location.pathname;
    const n_q = nextProps.location.search;

    if (type !== n_type || q !== n_q) {
      this.loadPageData(nextProps.location);
    }
  }

  render() {
    const { pageType, data } = this.state;

    if (this.state.loaded && data) {
      const Component: any = PageFactory.getPage(pageType);
      return <Component data={data} />;
    } else {
      return <label>routing</label>;
    }
  }
}

export default AppRoutes;
