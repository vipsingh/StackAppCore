import React from 'react';
import ReactDOM from 'react-dom';
import registerPages from "./App/Page";
import MyApp from './App/MyApp';
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
window._AppSetting = {
  UserId: 1,
  RoleId: 1,
  Language: "en-us",
  DateFormat: "DD-MM-YYYY",
  BaseUrl: "/",
  ApiUrl: "@ViewBag.Host",
  AssetUrl: "@ViewBag.Host",
  BaseCurrency: 1,
  BaseCurrencySymbol: "$",
  DecimalPlaces: 2
};
//

registerPages();

ReactDOM.render(<MyApp />, document.getElementById('app'));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
