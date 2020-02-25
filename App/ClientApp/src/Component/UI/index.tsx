import React from "react";

export function Paper(p: any) {
    return (<div className="paper">
            {p.children}
        </div>);
};

const sty = {
  bar: {
    boxSizing :'border-box',
    height: 32,
    padding: '5px 5px',
    display: 'flex',
    justifyContent: 'space-between',
    borderTop: '2px solid'
  },
  body:{

  },
  ul_style: {position: "absolute",
      right: 0,
      top: 0,
      zIndex: 9,
      listStyle: 'none',
      padding: 0,
      margin: 0,
      height: '100%'}
};

export class Panel extends React.PureComponent<{
  style: any,
  title: string,
  children: any
}>{

  render(){
    let paper_styl = Object.assign({},this.props.style);
    return(
      <div style={{width:'100%'}}>
        <Paper style={paper_styl}>
          <div style={{
    boxSizing :'border-box',
    height: 32,
    padding: '5px 5px',
    display: 'flex',
    justifyContent: 'space-between',
    borderTop: '2px solid'
  }}>
            <div>
              <span style={{fontWeight: 500, fontSize: 14}}>{this.props.title}</span>
            </div>
            <div style={{position: 'relative', padding: 0, boxSizing: 'border-box', marginRight: 10}}>
              <ul style={{position: "absolute",
      right: 0,
      top: 0,
      zIndex: 9,
      listStyle: 'none',
      padding: 0,
      margin: 0,
      height: '100%'}}>
                <li style={{display:'inline-block', float: 'left'}}><a href="#"><i className='fa fa-times'></i></a></li>
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
