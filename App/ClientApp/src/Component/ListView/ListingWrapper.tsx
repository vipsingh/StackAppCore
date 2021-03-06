import React from "react";
import _ from "lodash";
import FilterBox from "../Form/Control/FilterBox";
import { cellRenderer } from "./Helper";
import ActionLink from "../ActionLink";

export default function(ListComp: React.ComponentClass<any,any>) {
    
    return class ListingWrapper extends React.Component<ListingProps, {
        IsFetching: boolean,
        ListInfo: any,
        Pager: {
            Count: number,
            Page: number,
            Size: number
        },
        selectedRowKeys: Array<any>,
        Filters: Array<any>
    }> {

        constructor(props: any) {
            super(props);

            this.state = {
                IsFetching: false,
                ListInfo: null,
                Pager: {
                    Count: 0,
                    Page: 1,
                    Size: 25
                },
                selectedRowKeys: [],
                Filters: []
            };

            this.loadData();
        }

        loadData = (index: number = 1) => {
            const { WidgetId, DataActionLink, ListData, api, IsLocalStore } = this.props;
            if (!DataActionLink) return;

            const { Url } = DataActionLink;

            this.setState({ IsFetching: true });

            _App.Request.getData({
                url: Url,
                type: "POST",
                body: { RequestType: 0 }
            }).then((res: any) => {
                const info = res;
                if (IsLocalStore) {
                    this.setState({ ListInfo: Object.assign({}, this.state.ListInfo || {}, info) });
                } else if (api && api.updateField) {
                    api.updateField(WidgetId, { ListData: Object.assign({}, ListData, info) });
                }
            }).finally(() => {
                this.setState({ IsFetching: false });
            });;
        }

        handleRequestPage = (index: number) => {
            this.loadData(index);
        }

        handleGridSort(sortColumn: string, sortDirection: string) {
            // this.currentSortInfo.field = sortColumn;
            // this.currentSortInfo.dir = sortDirection;
            // this.loadData(1);
        }

        getSelectedRows() {
            const { selectedRowKeys } = this.state;
            const { Data } = this.getListInfo();

            return _.filter(Data, k => {
                return selectedRowKeys.indexOf(k._RowId) >= 0;
            });
        }

        getListInfo() {
            const { ListData, IsLocalStore } = this.props;
            if (IsLocalStore) return this.state.ListInfo;

            return ListData;
        }

        getRowSelection(): any {
            const { SelectionConfig } = this.props;
            if (!SelectionConfig) return;

            return {
                type: (SelectionConfig.IsMultiSelect ? "checkbox" : "radio"),
                onChange: (selectedRowKeys: any, selectedRows: any) => {
                    this.setState({ selectedRowKeys: _.map(selectedRows, x => x._RowId) });
                },
                getCheckboxProps: (record: any) => {
                    return {
                        //disabled: record.name === 'Disabled User'
                        name: record._RowId,
                    };
                },
                selectedRowKeys: this.state.selectedRowKeys
            };
        }

        formatCell = (col: any, val: any, row: any) => {
            if (this.props.formatCell) return this.props.formatCell(col, val, row);

            return cellRenderer(col, val);
        }

        renderAction = (action: ActionInfo) => {
            if (this.props.renderAction) return this.props.renderAction(action);

            return <ActionLink {...action} LinkTarget="POPUP" />;
        }

        renderFilterPanel() {
            const { FilterBox: filterInfo } = this.props;
            const { Filters } = this.state;

            if (!filterInfo) return;

            return (<FilterBox {...filterInfo} onChange={this.onFilterValueChange} Value= {Filters} />);
        }

        onFilterValueChange = (filters: Array<any>) => {
            this.setState({ Filters: filters });
        }

        render() {
            const ListData = this.getListInfo();
            const { Pager, IsFetching } = this.state;
            
            if(IsFetching){
                return (<label>loading..</label>);
            } else if (ListData) {
                return (
                    <div>
                        {
                            this.renderFilterPanel()
                        }
                    
                        <ListComp                         
                            listData={ListData}
                            pager={Pager}
                            rowSelection={this.getRowSelection()}
                            formatCell={this.formatCell}
                            renderAction={this.renderAction}
                        />
                    </div>
                );
            } else {
                return (<label></label>);
            }
        }
    }
}

class FilterBar extends React.Component {

    render() {
        return (<div>

        </div>);
    }
}