import { Dictionary } from "lodash";

const globalResources: Dictionary<string> = {
    "Error": "Error"
};

const localResources: Dictionary<string> = {

};
export function getResource(key: string, def?: string) {
    let d = localResources[key];
    if (!d) {
        d = globalResources[key];
    }
    if (!d)
        d = def || "";

    return d || key;
}

export function loadLocalResources(res: any) {

}
