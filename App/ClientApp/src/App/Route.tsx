import React from "react";
import { Route, Switch } from "react-router-dom";
import PageDesigner, {PageX} from "../StudioApp/Pages/Studio/PageDesigner";
import { RouteComponent } from "../Component/PageComponent";

function AppRoutes() {
  return (
    <Switch>
      <Route exact path="/PageDesigner" component={PageDesigner} />
      <Route exact path="/PageX" component={PageX} />
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

export default AppRoutes;
