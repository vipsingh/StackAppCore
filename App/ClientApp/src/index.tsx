import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
//import App from './App';
import MyApp from './App/MyApp';
import Notify from "./Core/Utils/Notify";
import Request from "./Core/Utils/Request";
import DebugTrace from "./Core/Utils/DebugTrace";
import * as serviceWorker from './serviceWorker';

window._App = {
    Notify,
    Request  
  };
  
  window._Debug = DebugTrace;

ReactDOM.render(<MyApp />, document.getElementById('app'));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
