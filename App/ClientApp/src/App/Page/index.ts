import { PageType } from "../../Constant";
import NewEdit from "./Edit";
import Detail from "./Detail";
import Desk from "./Desk";
import EntityStudio from "../../StudioApp/Pages/Studio/EntityPage";
import ErrorPage from "./Error";
import PageFactory from "../../Core/PageFactory";
import FormDesigner from "../../StudioApp/Pages/Studio/Designer/FormDesigner";

export default function() {
    PageFactory.errorPage = ErrorPage;

    PageFactory.addPage(PageType.Desk, Desk);
    PageFactory.addPage(PageType.Edit, NewEdit);
    PageFactory.addPage(PageType.Detail, Detail);
    PageFactory.addPage(PageType.AppStudio, EntityStudio);
    PageFactory.addPage(PageType.Designer, FormDesigner);
    PageFactory.addPage(PageType.Error, ErrorPage);    
}