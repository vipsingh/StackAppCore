import React from "react";
import PageContext, { createPageContext } from "../Core/PageContext";
import PageFactory from "../Core/PageFactory";
import { close } from "../Component/UI/Dialog";

export default class PageComponent extends React.Component<
{
  url: any;
  openerNavigator?: any,
  popup?: any
},
{
  loaded: boolean;
  pageType: number;
  data: any;
}
> 
{
pageContext: any  

constructor(props: any) {
  super(props);
  this.state = {
    loaded: false,
    pageType: 10,
    data: null,
  };

  this.pageContext = createPageContext(0, this.props.openerNavigator.history, this.props.popup);
  if (this.props.popup) {
    this.pageContext.navigator.windowType = "POPUP" 
    this.pageContext.navigator.close = () => {
        const dlg = this.props.popup as any
        close(dlg.dialogId);
    }

    this.pageContext.navigator.opener = this.props.openerNavigator;
  }
}

componentDidMount() {
  this.loadPageData(this.props.url);
}

loadPageData(url: any) {
  this.setState({ loaded: false });

  window._App.Request.getData({
    url: url,
    type: "GET",
  })
    .then((data: any) => {
      const schema = data;
      const pageType = schema.PageType;
      this.setState({ pageType, data: data, loaded: true });
    })
    .catch((err: any) => {
      _Debug.error(`PageLoad: ${err}`);
      this.setState({ pageType: 10, data: err, loaded: true });
    });
}

componentWillReceiveProps(nextProps: any) {
  if (nextProps.url !== this.props.url) {
    this.loadPageData(nextProps.location);
  }
}

render() {
  const { pageType, data } = this.state;

  if (this.state.loaded && data) {
    const Component: any = PageFactory.getPage(pageType);
    return <PageContext.Provider value={this.pageContext}><Component data={data} /></PageContext.Provider>;
  } else {
    return <label>routing</label>;
  }
}
}