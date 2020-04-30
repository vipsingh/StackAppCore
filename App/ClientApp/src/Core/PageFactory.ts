const appPages: IDictionary<any> = {};
let errorPage: any;

const getPage = (type: number) => {
    const c = appPages[type];
    if (c) return c;

    return errorPage;
}

const addPage = (type: number, pageComponent: any) => {
    appPages[type] = pageComponent;
}

export default {
    getPage,
    addPage,
    errorPage
};