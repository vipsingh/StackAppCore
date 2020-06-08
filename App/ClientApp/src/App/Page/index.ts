import { PageType } from "../../Constant";
import NewEdit from "./Edit";
import Detail from "./Detail";
import Desk from "./Desk";
import EntityStudio from "./Studio/EntityPage";
import ErrorPage from "./Error";
import PageFactory from "../../Core/PageFactory";



export default function() {
    PageFactory.errorPage = ErrorPage;

    PageFactory.addPage(PageType.Desk, Desk);
    PageFactory.addPage(PageType.Edit, NewEdit);
    PageFactory.addPage(PageType.Detail, Detail);
    PageFactory.addPage(PageType.AppStudio, EntityStudio);
    PageFactory.addPage(PageType.Error, ErrorPage);
}

// const getPage = (type: number) => {
//     let Component = null;
//     switch(type) {
//         case PageType.Edit:
//             Component = NewEdit;
//             break;
//         case PageType.Detail:
//             Component = Detail;
//             break;
//         case PageType.Desk:
//             Component = Desk;
//             break;
//         default:
//             Component = ErrorPage;
//             break;

//     }

//     return Component;
// };

// export default {
//     getPage
// };