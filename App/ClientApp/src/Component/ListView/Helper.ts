export var x: string = "";
// import React, {Component,PropTypes} from 'react';
// import moment from "moment";
// import numeral from "numeral";
// import { Checkbox } from 'antd';

// export const cellFormatter = {
//     date: (p) => {
//       return (<div>{moment(p.Value).format('DD-MMM-YYYY')}</div>);
//     },
//     decimal: (p) => {
//       return (<div style={{textAlign:'right'}}>{p.Value && numeral(p.Value).format('0,0.000')}</div>);
//     },
//     monetary: (p) => {
//       return (<div style={{textAlign:'right'}}>{p.Value && numeral(p.Value).format('0,0.00')}</div>);
//     },
//     boolean: (p) => {
//       return (<div style={{textAlign:'center'}}><Checkbox disabled defaultChecked={p.Value}/></div>);
//     },
//     // select: (p) => {
//     //   let d = _.find(p.dependentValues.col.ListValues, {Value: p.value });
//     //   d = (d)?d.Text:'';
//     //   return (<div>{d}</div>);
//     // },
//     //link field
//     link: (p) => {
//       let d = (p.Value) ? p.Value.Text : null;
//       return (<div>{d}</div>);
//     },
//     ////////
//     // title: (p) => {
//     //   return (<a href={"#/object/form/"+p.objectName+"?id="+p.dependentValues.data.id}>{p.value}</a>)
//     // }
//     //ObjectNumber, ObjectStatus
//   };