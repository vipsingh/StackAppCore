import { PageType } from "../../Constant";
import NewEdit from "./Edit";
import Desk from "./Desk";

const getPage = (type) => {
    let Component = null;
    switch(type) {
        case PageType.Edit:
            Component = NewEdit;
            break;
        case PageType.Desk:
            Component = Desk;
            break;

    }

    return Component;
};

export default {
    getPage
};