import { PageType } from "../../Constant";
import NewEdit from "./Edit";
import Detail from "./Detail";
import Desk from "./Desk";

const getPage = (type) => {
    let Component = null;
    switch(type) {
        case PageType.Edit:
            Component = NewEdit;
            break;
        case PageType.Detail:
            Component = Detail;
            break;
        case PageType.Desk:
            Component = Desk;
            break;
        default:
            Component = null;
            break;

    }

    return Component;
};

export default {
    getPage
};