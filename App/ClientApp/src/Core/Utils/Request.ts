import axios from "axios";
//import request from "request";

function processRequest(resolve: Function, reject: Function, params: RequestParam = { type: "POST", body: null, contentType: "", url: "" }) {
    const myHeaders = new Headers();
    let requestBody = params.body;
    const requestType = (typeof params.type === "string") ? params.type.toUpperCase() : "POST";
    let contentType = params.contentType ? params.contentType : null;
    let url = params.url;
    //const _w: any = window;
    url = "/" + url;
    url = `${url.replace("//","/")}&_x=${new Date().getTime()}`;
    
    //console.log(url);

    contentType = "application/json";
    if (contentType) {
        myHeaders.append("Content-Type", contentType);
    }   

    let f = null;
    if (requestType === "POST") {
        const instance = axios.create({
            headers: myHeaders
          });

        f = instance({
            method: 'post',
            url: url,
            data: requestBody
          });       
    } else {
        f = axios.get(url);
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

// function processRequest1(resolve, reject, params) {
//     const myHeaders = new Headers();

//     let requestBody;
//     const requestType = (typeof params.type === "string") ? params.type.toUpperCase() : "POST";
//     let contentType = params.contentType ? params.contentType : null;

//     contentType = "application/json";
//     if (contentType) {
//         myHeaders.append("Content-Type", contentType);
//     }
//     const myInit = {
//         method: requestType,
//         //mode: "cors",
//         cache: "default",
//         //credentials: "same-origin",
//         headers: myHeaders,
//         body: requestBody
//     };

//     fetch(`${_AppSetting.ApiUrl}${params.url}`, myInit)
//             .then(function (response) {
//                 if (response.ResponseStatus === 200) {
//                     resolve(response.Data);
//                 } else {
//                     reject(response.Data);
//                 }
//             })
//             .catch(function (resError) {
                
//                 //__debug.errorLog("Http request failed.", params.url, myInit, resError);

//                 resError.message = "OOPS!!! Something went wrong. Seems like you are not connected to internet.";
//                 reject(resError);
//             });
// }

// const convertToFormData = function (rawObject) {
//     const formData = new FormData();
//     for (const key in rawObject) {
//         formData.append(key, encodeURIComponent(JSON.stringify(rawObject[key])));
//     }

//     return formData;
// };

// const convertToUrlEncodedData = function (rawObject) {
//     const keys = Object.keys(rawObject);
//     let requestString = "";
//     keys.forEach((key, index) => {
//         requestString += `${key}=${encodeURIComponent(JSON.stringify(rawObject[key]))}`;
//         if (index < keys.length - 1) {
//             requestString += "&";
//         }
//     });

//     return requestString;
// };

function getData(params: RequestParam) {
    return new Promise(function (resolve, reject) {
        processRequest(resolve, reject, params);
    });
}

var Request = {
    getData
};

export default Request;

interface RequestParam { type: "POST"|"GET", url: string, body: any|undefined, contentType: string }