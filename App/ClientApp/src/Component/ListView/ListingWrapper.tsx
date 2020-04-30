import React from "react";
import _ from "lodash";

export default function(ListComp: React.ComponentClass<any,any>) {
    
    return class ListingWrapper extends React.Component<ListingProps, {
                IsFetching: boolean,            
                Rows: Array<any>,
                Pager: {
                    Count: number,
                    Page: number,
                    Size: number
                },
                selectedRowKeys: Array<any>
        }> {
    
        constructor(props: any) {
            super(props);

            this.state = {
                IsFetching: false,
                Rows: [],
                Pager: {
                    Count: 0,
                    Page: 1,
                    Size: 25
                },
                selectedRowKeys: []
            };
            
            this.loadData();
        }

        loadData = (index: number = 1) => {
            const { WidgetId, DataActionLink, ListData, api } = this.props; 
            if (!DataActionLink) return;
            
            const { Url } = DataActionLink;      
            
            this.setState({IsFetching: true});

            _App.Request.getData({
                url: Url,
                type: "POST",
                body: {RequestType: 0}
            }).then((res: any) => {
                const info = res;
                if (api && api.updateField) {
                    api.updateField(WidgetId,  { ListData: Object.assign({}, ListData, info) });
                }
            }).finally(() => {
                this.setState({IsFetching: false});
            });;
        }

        handleRequestPage = (index: number)=>{
            this.loadData(index);
        }

        handleGridSort(sortColumn: string, sortDirection: string){
            // this.currentSortInfo.field = sortColumn;
            // this.currentSortInfo.dir = sortDirection;
            // this.loadData(1);
        }

        getSelectedRows() {
            const { selectedRowKeys } =this.state;
            const { ListData: { Data } } = this.props;

            return _.filter(Data, k => {
                return  selectedRowKeys.indexOf(k.RowId) >= 0;
            });
        }

        getRowSelection(): any {
            const { SelectionConfig } = this.props;
            if (!SelectionConfig) return;

            return {
                type: (SelectionConfig.IsMultiSelect ? "checkbox" : "radio"),
                onChange: (selectedRowKeys: any, selectedRows: any) => {                
                    this.setState({ selectedRowKeys: _.map(selectedRows, x => x.RowId) });
                },
                getCheckboxProps: (record: any) => {
                    return {
                        //disabled: record.name === 'Disabled User'
                        name: record.RowId,
                    };
                },
                selectedRowKeys: this.state.selectedRowKeys
              };
        }

        render() {
            const { ListData } = this.props;
            const { Pager } = this.state;
            
            if(this.state.IsFetching){
                return (<label>loading..</label>);
            } else if (!ListData) {
                return (<label>something went wrong..</label>);
            } else {
                return (
                    <ListComp                         
                        listData={ListData} 
                        pager={Pager}   
                        rowSelection={this.getRowSelection()}                        
                    />
                );
            }
        }
    }
}