import React from "react";

export default function(ListComp: React.ComponentClass<any,any>) {
    
    return class ListingWrapper extends React.Component<ListingProps, {
                IsFetching: boolean,            
                Rows: Array<any>,
                Pager: {
                    Count: number,
                    Page: number,
                    Size: number
                }
        }> {
    
        constructor(props: any) {
            super(props);

            this.state = {
                IsFetching: true,            
                Rows: [],
                Pager: {
                    Count: 0,
                    Page: 1,
                    Size: 25
                }
            };

            this.loadData();
        }

        loadData = (index: number = 1) => {
            const { WidgetId, DataActionLink, ListData, api } = this.props; 
            const { Url } = DataActionLink;      

            _App.Request.getData({
                url: Url,
                type: "POST",
                body: {RequestType: 0}
            }).then((res: any) => {
                const info = res;

                api.updateField(WidgetId,  { ListData: Object.assign({}, ListData, info) });
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
                    />
                );
            }
        }
    }
}