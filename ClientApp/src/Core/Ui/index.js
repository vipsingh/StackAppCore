import React from "react";
import PropTypes from "prop-types";

export function Paper(p) {
    return (<div className="paper">
            {p.children}
        </div>);
};

const sty = {
  bar: {
    boxSizing:'border-box',
    height: 32,
    padding: '5px 5px',
    display: 'flex',
    justifyContent: 'space-between',
    borderTop: '2px solid'
  },
  body:{

  },
  ul_style: {position: 'absolute',
      right: 0,
      top: 0,
      zIndex: 9,
      listStyle: 'none',
      padding: 0,
      margin: 0,
      height: '100%'}
};

export class Panel extends React.PureComponent{
  propTypes = {
    style: PropTypes.object,
    title: PropTypes.string,
    children: PropTypes.any
  }

  render(){
    let paper_styl = Object.assign({},this.props.style);
    return(
      <div style={{width:'100%'}}>
        <Paper style={paper_styl}>
          <div style={sty.bar}>
            <div>
              <span style={{fontWeight: 500, fontSize: 14}}>{this.props.title}</span>
            </div>
            <div style={{position: 'relative', padding: 0, boxSizing: 'border-box', marginRight: 10}}>
              <ul style={sty.ul_style}>
                <li style={{display:'inline-block', float: 'left'}}><a><i className='fa fa-times'></i></a></li>
              </ul>
            </div>
          </div>
          <div style={sty.body}>
            {this.props.children}
          </div>
        </Paper>
      </div>
    );
  }
}
