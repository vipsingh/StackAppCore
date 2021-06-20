import React from "react";
import { Route, Switch } from "react-router-dom";
import PageFactory from "../Core/PageFactory";
import PageDesigner, {PageX} from "../StudioApp/Pages/Studio/PageDesigner";
import PageContext, { createPageContext } from "../Core/PageContext";
import { closeAll } from "../Component/UI/Dialog";

function AppRoutes() {
  return (
    <Switch>
      <Route exact path="/PageDesigner" component={PageDesigner} />
      <Route exact path="/PageX" component={PageX} />  
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
