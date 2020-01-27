import React from "react";
import PropTypes from "prop-types";
import _ from "lodash";
import { Row, Col } from "antd";
import { Paper } from "~/Core/Ui";

export default class UIView extends React.Component {
  static propTypes = {
    viewTemplate: PropTypes.object,
    getControl: PropTypes.func
  };

  render() {
    const { viewTemplate } = this.props;
    const { Pages } = viewTemplate;

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

  renderSheet(sheet) {
    const { Groups } = sheet;

    return (
        <div className="form-ui-sheet" key={_.uniqueId("uiview")}>
            <Paper>
                {
                    _.map(Groups, (p) => {
                        return this.renderGroup(p);
                    })
                }
            </Paper>
        </div>
    );
  }

  renderGroup(group) {
    const { Rows } = group;

    return (
        <div className="form-ui-group"  key={_.uniqueId("uiview")}>
        {
            _.map(Rows, (p) => {
                return this.renderRow(p);
            })
        }
        </div>
    );
  }

  renderRow(row) {
    const { getControl } = this.props;
    const { Fields } = row;

    return (
        <div className="form-ui-row"  key={_.uniqueId("uiview")}>
            <Row>
                {
                    _.map(Fields, (field) => {
                        return (<UIField key={_.uniqueId("uifield")} {...field} getControl={getControl} />);
                    })
                }
            </Row>
        </div>
    );
  }
}


class UIField extends React.Component {
  static propTypes = {
      FieldId: PropTypes.string,
      getControl: PropTypes.func,
      FullRow: PropTypes.bool
  };

  render() {
    const { getControl, FieldId, FullRow } = this.props;
    const span = FullRow ? 24: 12;
    
    return (<Col span={span}>{getControl(FieldId)}</Col>);
  }
}
