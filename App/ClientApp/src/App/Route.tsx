import React from "react";
import { Route, Switch } from "react-router-dom";
import PageFactory from "../Core/PageFactory";
import Studio from "./Page/Studio/Designer/FormDesigner";
import PageDesigner, {PageX} from "./Page/Studio/PageDesigner";
import PageContext, { createPageContext } from "../Core/PageContext";
import { closeAll } from "../Component/UI/Dialog";

function AppRoutes() {
  return (
    <Switch>
      <Route exact path="/Studio" component={Studio} />
      <Route exact path="/PageDesigner" component={PageDesigner} />
      <Route path="/:controller/:action/:param?" component={RouteComponent} />      
      <Route component={NotFound} />
    </Switch>
  );
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

class RouteComponent extends React.Component<
  {
    match: any;
    location: any;
    history: any
  },
  {
    loaded: boolean;
    pageType: number;
    data: any;
  }
  > 
{
  pageContext: any  
  
  constructor(props: any) {
    super(props);
    this.state = {
      loaded: false,
      pageType: 10,
      data: null,
    };

    this.pageContext = createPageContext(0, this.props.history);
  }

  componentDidMount() {
    this.loadPageData(this.props.location);    
  }

  loadPageData(location: any) {
    closeAll();
    
    this.setState({ loaded: false });

    let path = location.pathname;
    //path = path.replace("web/", "");

    const q = location.search;

    const url = `${path}${q}`;

    window._App.Request.getData({
      url: url,
      type: "GET",
    })
      .then((data: any) => {
        const schema = data;
        const pageType = schema.PageType;
        this.setState({ pageType, data: data, loaded: true });
      })
      .catch((err: any) => {
        _Debug.error(`PageLoad: ${err}`);
        this.setState({ pageType: 10, data: err, loaded: true });
      });
  }

  componentWillReceiveProps(nextProps: any) {
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
      return <PageContext.Provider value={this.pageContext}><Component data={data} /></PageContext.Provider>;
    } else {
      return <label>routing</label>;
    }
  }
}

export default AppRoutes;
