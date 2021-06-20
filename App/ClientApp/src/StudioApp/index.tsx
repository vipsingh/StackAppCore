import React, { Component } from "react";
import { Layout } from "antd";
import { Route, BrowserRouter as Router, Switch } from "react-router-dom";
import { RouteComponent } from "../Component/PageComponent";
import EntityDesk from "./Pages/EntityDesk";
import EntityList from "./Pages/EntityList";
import PageContext, { createPageContext } from "../Core/PageContext";
import _ from "lodash";

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
      return (<Router>
        <Layout style={{ minHeight: "100vh" }}>
        <Header />
        <Content >
              <div className="page-content-wrapper"
                style={{
                  padding: 10,
                  minHeight: 500
                }}
                >
                    <AppRoutes />
              </div>
              <Footer style={{ textAlign: "center" }}>StackERP ©2021</Footer> 
        </Content>
          </Layout>
          </ Router>
    );
  }
}

function AppRoutes() {
    return (
        <Switch>
            <PageRoute exact path="/entitystudio/entity/:id" component={EntityDesk} />
            <PageRoute exact path="/entitystudio/list" component={EntityList}  />
            <Route path="/:controller/:action/:param?" component={RouteComponent} />
            <Route component={NotFound} />
        </Switch>
    );
}

function PageRoute({ component, ...rest }: any) {
    return (
        <Route
            {...rest}
            render={routeProps => {
                //closeAll();
                window.scrollTo(0, 0);

                return (
                    <LoadWith routeProps={routeProps} component={component} />
                );
               }
            }
        />
    );
}


function LoadWith(props: any) {
    //const [loading, setLoading] = React.useState(true);

    const { routeProps, component: Component } = props;
    const pageContext = createPageContext(0, routeProps.history);

    //React.useEffect(() => {
    //    if (!_UserInfo) {
    //        _App.Request.getAPIData({ url: `api/users/getuser?userid=${_AppSetting.UserId}` }).then((data: any) => {
    //            window._UserInfo = data;
    //        }).finally(() => setLoading(false));
    //    } else {
    //        setLoading(false);
    //    }
    //});

    //if (loading) {
    //    return <PageLoader />;
    //}

    return (<PageContext.Provider value={pageContext}>
        <Component {...routeProps} key={_.uniqueId("x")} />
    </PageContext.Provider>);
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