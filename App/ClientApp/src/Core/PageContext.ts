import React from "react";

const PageContext = React.createContext({ navigator: {
    navigate: (url: string) => { },
    reload: () => { }
} });

function createPageContext(windowId: number, history: any) {
    //const { WindowManager } = window;
    
    return {
        navigator: {
            windowType: windowId === 0 ? 2 : 1,
            windowId,
            openerWindowId: 0,
            navigate: (url: string) => {
                history.push(url);
            },
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