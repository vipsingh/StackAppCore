import React from "react";
import GridView from "./GridView";

class ListView extends React.Component<ListingProps> {

    getListComponent() {
        
        return GridView;
    }
        
    render() {
        const ListComp =  this.getListComponent();

        return (<div>
                <ListComp {...this.props} />
            </div>);
    }
}

export default ListView;