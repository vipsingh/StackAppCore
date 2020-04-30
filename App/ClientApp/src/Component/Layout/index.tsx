import React from "react";
import _ from "lodash";
import { Row, Col } from "antd";

export default class UIView extends React.Component<{
    template: any,
    getControl: Function
  }> {

  render() {
    const { template, getControl } = this.props;
    const { Pages } = template;
    let i = 0;

    return (
        <div className="page-form">            
            <div>
                {
                    _.map(Pages, (s) => {
                        return <div className=" paper"><PageSheet getControl={getControl} sheet={s} key={`page_${i++}`} /></div>;
                    })
                }
            </div>
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

export const PageSheet: React.FC<any> = (props) => {
    const { sheet, key, getControl } =props;
    const { Groups } = sheet;
    let i = 0;

    return (
        <div className="form-ui-sheet">
                {
                    _.map(Groups, (p) => {
                        return <PageGroup getControl={getControl} group={p} key={`${key}_group_${i++}`} /> //this.renderGroup(p, `${key}_group_${i++}`);
                    })
                }
        </div>
    );
  }

export const PageGroup: React.FC<any> = (props) => {
    const { group, getControl } = props;
    const { Rows, Label } = group;
    //let i = 0;

    return (
        <div className="form-ui-group paper-group">
            {!Label || <h3>{Label}</h3>}
        {
            _.map(Rows, (row) => {
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
            })
        }
        </div>
    );
}