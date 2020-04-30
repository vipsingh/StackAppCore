import axios from "axios";
//import { entity_detail, entity_edit, widget_data } from "../../mockupdata";


function processRequest(resolve: Function, reject: Function, params: RequestParam = { type: "POST", body: null, contentType: "", url: "" }) {
    const myHeaders = new Headers();
    let requestBody = params.body;
    const requestType = (typeof params.type === "string") ? params.type.toUpperCase() : "POST";
    let contentType = params.contentType ? params.contentType : null;
    let url = params.url;
    //const _w: any = window;
    url = "/" + url;
    url = `${url.replace("//","/")}${url.indexOf("?") >= 0 ? "&": "?"}_ajax=1&_x=${new Date().getTime()}`;
    
    //console.log(url);

    contentType = "application/json";
    if (contentType) {
        myHeaders.append("Content-Type", contentType);
        //myHeaders.append("X-PAjax", "true");
    }   

    let f = null;
    const instance = axios.create({
        headers: myHeaders
    });

    if (requestType === "POST") {        
        f = instance({
            method: 'post',
            url: url,
            data: requestBody
          });       
    } else {
        f = instance({
            method: 'get',
            url: url
          });     
    }

    f.then(({data}) => {
        if (data.ResponseStatus === 200) {
            resolve(data.Data);
        } else {
            reject(data.Data);
        }
    }).catch(function (error) {
        // handle error
        console.log(error);
        reject(error);
      });
}

function getData(params: RequestParam) {
    return new Promise(function (resolve, reject) {        
            //getMockupData(resolve, reject, params);
        
            processRequest(resolve, reject, params);        
    });
}

// function getMockupData(resolve: Function, reject: Function, params: any) {
//     const url = params.url;
//     if (url.indexOf("entity/detail") >= 0) {
//         resolve(entity_detail);
//     } else  if (url.indexOf("entity/edit") >= 0) {
//         resolve(entity_edit);
//     } else  if (url.toLowerCase().indexOf("widget/getpickerdata") >= 0) {
//         resolve(widget_data);
//     }
// }

var Request = {
    getData
};

export default Request;

interface RequestParam { type: "POST"|"GET", url: string, body: any|undefined, contentType: string }