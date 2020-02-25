
import PropTypes from 'prop-types';
import React from 'react';
import {Link as RouterLink} from 'react-router-dom';


export default class GlobalLink extends React.Component<{ to: string|object, children: any }> {  
    static contextTypes = {
      location: PropTypes.object,
    };
  
    render() {
      const {location} = this.context;

      const query = location.query;//extractSelectionParameters(location.query);

      if (location) {
        const hasQuery = Object.keys(query).length > 0;
  
        let { to } = this.props;
  
        if (hasQuery) {
          if (typeof to === 'string') {
            to = {pathname: to, query};
          }
        }
  
        const routerProps = to ? {...this.props, to} : {...this.props};
  
        return <RouterLink {...routerProps}>{this.props.children}</RouterLink>;
      } else {
        const {to, ...props} = this.props;
        return (
          <a {...props} href={to.toString()}>
            {this.props.children}
          </a>
        );
      }
    }
}