import React from "react";
import _ from "lodash";
import { Row, Col, Tabs } from "antd";
import { DesignerContext } from "../../Core/Studio";

const { TabPane } = Tabs;

export default class UIView extends React.Component<{
    template: any,
    getControl: Function
  }> {

    renderSheet(sheet: any, ix: number) {
        const { getControl } = this.props;

        return (
            <PageSheet getControl={getControl} sheet={sheet} uKey={`page_${ix}`} />
        );
    }

  render() {
    const { template } = this.props;
    const { Pages } = template;
    let i = 0;

    return (
        <div className="page-form">            
                <div className="paper" key={`page_${i++}`}>
                    <Tabs style={{ marginBottom: 32 }}>
                    {
                        _.map(Pages, (s, ix) => {
                            return (<TabPane tab={s.Text || "Default"} key={`page_${ix}`}>
                                {this.renderSheet(s, i)}
                            </TabPane>)
                        })
                    }
                    </Tabs>
                </div>
        </div>
    );
  }    
}


export class UIField extends React.Component<{
    Id: string,
    getControl: Function,
    FullRow?: boolean,
    Span?: number
}> {

  render() {
    const { getControl, Id, FullRow, Span } = this.props;    
    let span1 = Span;
    if (!Span) { 
        span1 = FullRow? 24: 12;
    }


    return (<Col span={span1}>{getControl(Id)}</Col>);
  }
}

export const PageSheet: React.FC<any> = (props) => {
    const { sheet, uKey, getControl } = props;
    const { Groups } = sheet;
    let i = 0;

    return (
        <div className="form-ui-sheet">
                {
                    _.map(Groups, (p) => {
                        return <PageGroup sheetId={sheet.Id} getControl={getControl} group={p} key={`${uKey}_group_${i++}`} /> //this.renderGroup(p, `${key}_group_${i++}`);
                    })
                }
        </div>
    );
  }

export const PageGroup: React.FC<any> = (props) => {
    const { group, getControl, sheetId } = props;
    const { Rows, Text } = group;
    //let i = 0;
    const renderDesigner = (api: any) => {
        if (!api) return;
        
        return api.renderGroupSetting(sheetId, group);
    }

    return (
        <div className="form-ui-group paper-group">
            {!Text || <h3>{Text}</h3>}
            <DesignerContext.Consumer>
                {(api) => renderDesigner(api)}
            </DesignerContext.Consumer>
        {
            _.map(Rows, (row, ix) => {
                const { Cols } = row;

                return (
                    <div className="form-ui-row" key={`row_${ix}`}>
                        <Row>
                            {
                                _.map(Cols, (col) => {
                                    return (<UIField key={col.Id} {...col} getControl={getControl} />);
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