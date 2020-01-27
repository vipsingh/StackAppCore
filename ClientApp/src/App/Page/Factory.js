import NewEdit from "./Edit";
import Desk from "./Desk";

const getPage = (type) => {
    let Component = null;
    switch(type) {
        case "FORM":
            Component = NewEdit;
            break;
        case "DESK":
            Component = Desk;
            break;

    }

    return Component;
};

export default {
    getPage
};