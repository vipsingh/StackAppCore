import React from "react";
import { Result, Button } from 'antd';

export default class ErrorPage extends React.Component <{
    data: any
    }, {
        Schema: any        
    }> 
{

    constructor(props: any) {
        super(props);
    
        this.state = {
          Schema: props.data,
        };
      }

    shouldComponentUpdate() {
        return true;
    }

    componentWillReceiveProps(nextProps: any) {
        this.setState({Schema: nextProps.data});
    }

    render() {

        return (
            <div>
                <Result
                    status="500"
                    title="500"
                    subTitle="Sorry, the server is wrong."
                    extra={<Button type="primary">Back</Button>}
                />
            </div>           
        );
    }
}