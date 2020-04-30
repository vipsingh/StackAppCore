import React from "react";

const PageContext = React.createContext({ popup: false, navigator: {
    navigate: (url: string, replace: boolean = false) => { },
    reload: () => { }, 
    history: null
} });

function createPageContext(windowId: number, history: any, popup: boolean = false) {
    //const { WindowManager } = window;
    
    return {
        popup,
        navigator: {
            windowType: windowId === 0 ? 2 : 1,
            windowId,
            openerWindowId: 0,
            navigate: (url: string, replace: boolean = false) => {
                if (replace)
                    history.replace(url);
                else
                    history.push(url);
            },
            history
            // navigateInCurrent: WindowManager.navigateInCurrent,
            // reload: (isFullReload) => { return WindowManager.reload(this.windowId, isFullReload); },
            // close: () => { return WindowManager.closeWindow(this.windowId); },
            // reloadParent: WindowManager.reloadParent,
            // navigateOpener: WindowManager.navigateOpener
        }
    };
}

export { createPageContext };

export default PageContext;