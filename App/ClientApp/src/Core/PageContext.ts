import React from "react";

const PageContext = React.createContext({ popup: false, navigator: {
    navigate: (url: string, replace: boolean = false) => { },
    reload: () => { }, 
    history: null
} });

function createPageContext(windowId: number, history: any, popup: any = null) {
    //const { WindowManager } = window;
    
    return {
        popup,
        navigator: {
            windowType: "", // "","POPUP","TAB"
            windowId,
            openerWindowId: 0,
            navigate: (url: string, replace: boolean = false) => {
                if (replace)
                    history.replace(url);
                else
                    history.push(url);
            },
            history,
            opener: null,
            // navigateInCurrent: WindowManager.navigateInCurrent,
            reload: () => {
                const path =  `${history.location.pathname}${history.location.search}`;
                history.push("/reload");//temp path
                history.replace(path);
            },
            close: () => { 

            },
            // reloadParent: WindowManager.reloadParent,
            // navigateOpener: WindowManager.navigateOpener
        }
    };
}

export { createPageContext };

export default PageContext;