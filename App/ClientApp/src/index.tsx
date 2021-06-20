import React from 'react';
import ReactDOM from 'react-dom';
import { RegisterAllComponents } from "./Component/WidgetFactory";
import registerPages from "./App/Page";
import MyApp from './App/MyApp';
import StudioApp from './StudioApp';
import Notify from "./Core/Utils/Notify";
import Request from "./Core/Utils/Request";
import DebugTrace from "./Core/Utils/DebugTrace";
import { getResource } from "./Core/Utils/Localization";
import * as serviceWorker from './serviceWorker';
import './index.css';
import _ from 'lodash';
//import "./Core/Utils/ScriptParser";


window._App = {
    Notify,
    Request,
    FormatString: (str: string, data: any): string => {
      var compiled = _.template(str);
      return compiled(data);
    }
  };
  
window._Debug = DebugTrace;
window.__L = getResource;

//Default for dev
if (!window._AppSetting) {
  window._AppSetting = {
    UserId: 1,
    RoleId: 1,
    Language: "en-us",
    DateFormat: "DD-MM-YYYY",
    BaseUrl: "/",
      ApiUrl: "/",
      AssetUrl: "/",
      AppRoot: "/",
    BaseCurrency: 1,
    BaseCurrencySymbol: "$",
    DecimalPlaces: 3
  };
}
//

RegisterAllComponents();
registerPages();

const StartApp = () => {

    if (window._isEntityStudioApp) {
        return <StudioApp />;
    } else {
        return <MyApp />;
    } 
};

ReactDOM.render(<StartApp />, document.getElementById('app'));


// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
