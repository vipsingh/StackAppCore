import React from "react";
import _ from "lodash";
import { Row, Col } from "antd";
// import { Paper } from "~/Core/Ui";

export default class UIView extends React.Component<{
    template: any,
    getControl: Function
  }> {

  render() {
    const { template } = this.props;
    const { Pages } = template;

    return (
        <div className="form-ui">
            {
                _.map(Pages, (s) => {
                    return this.renderSheet(s);
                })
            }
        </div>
    );
  }  

  renderSheet(sheet: any) {
    const { Groups } = sheet;

    return (
        <div className="form-ui-sheet">
            <div>
                {
                    _.map(Groups, (p) => {
                        return this.renderGroup(p);
                    })
                }
            </div>
        </div>
    );
  }

  renderGroup(group: any) {
    const { Rows } = group;

    return (
        <div className="form-ui-group">
        {
            _.map(Rows, (p) => {
                return this.renderRow(p);
            })
        }
        </div>
    );
  }

  renderRow(row: any) {
    const { getControl } = this.props;
    const { Fields } = row;

    return (
        <div className="form-ui-row" >
            <Row>
                {
                    _.map(Fields, (field) => {
                        return (<UIField key={field.FieldId} {...field} getControl={getControl} />);
                    })
                }
            </Row>
        </div>
    );
  }
}


export class UIField extends React.Component<{
    FieldId: string,
    getControl: Function,
    FullRow?: boolean
}> {

  render() {
    const { getControl, FieldId, FullRow } = this.props;
    const span = FullRow ? 24: 12;
    
    return (<Col span={span}>{getControl(FieldId)}</Col>);
  }
}
