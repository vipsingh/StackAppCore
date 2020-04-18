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
    const { Pages, Header } = template;
    let i = 0;

    return (
        <div className="page-form">
            {
                this.renderHeader(Header)
            }            
            {
                _.map(Pages, (s) => {
                    return this.renderSheet(s, `page_${i++}`);
                })
            }
        </div>
    );
  }  

  renderHeader(header: any) {
    if(!header) return;

    return(
        <div className={"page-form-header"}>
            {this.renderSheet(header, `header_0`)}
        </div>
    );
  }

  renderSheet(sheet: any, key: string) {
    const { Groups } = sheet;
    let i = 0;

    return (
        <div className="form-ui-sheet" key={key}>
            <div>
                {
                    _.map(Groups, (p) => {
                        return this.renderGroup(p, `${key}_group_${i++}`);
                    })
                }
            </div>
        </div>
    );
  }

  renderGroup(group: any, key: string) {
    const { Rows } = group;
    let i = 0;

    return (
        <div className="form-ui-group">
        {
            _.map(Rows, (p) => {
                return this.renderRow(p, `${key}_row_${i++}`);
            })
        }
        </div>
    );
  }

  renderRow(row: any, key: string) {
    const { getControl } = this.props;
    const { Fields } = row;
    let i = 0;

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
