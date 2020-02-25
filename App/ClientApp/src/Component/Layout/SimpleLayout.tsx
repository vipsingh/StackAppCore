import React from "react";
import _ from "lodash";
import { Row, Col } from "antd";
import { UIField } from ".";
// import { Paper } from "~/Core/Ui";

export default class SimpleLayout extends React.Component<{
    Fields: Array<any>,
    getControl: Function
  }> {

  render() {
    const { Fields, getControl } = this.props;    

    return (
        <div>
            {
                _.map(Fields, (s) => {
                    return (<Row>
                        <Col>
                            <UIField key={s.WidgetId} FieldId={s.WidgetId} getControl={getControl} />
                        </Col>
                    </Row>);
                })
            }
        </div>
    );
  }  
}