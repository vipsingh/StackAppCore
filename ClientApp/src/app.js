import React from 'react';
import { render } from 'react-dom';
import MyApp from './App/MyApp';
import Studio from './Studio/Studio';

import Notify from "./Core/Utils/Notify";
import Request from "./Core/Utils/Request";
import DebugTrace from "./Core/Utils/DebugTrace";

// window._AppSetting = {
//   UserId: 1,
//   RoleId: 1,
//   DateFormat: "DD-MM-YYYY",
//   BaseUrl: "/",
//   ApiUrl: "http://localhost:3300"
// };

window._App = {
  Notify,
  Request  
};

window._Debug = DebugTrace;

render(
    <Studio/>
  ,
  document.getElementById('app')
);