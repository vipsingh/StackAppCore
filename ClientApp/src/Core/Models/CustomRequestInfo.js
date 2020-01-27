
export class CustomRequestInfo {
    constructor() {
        this.EntityContext = null;
        this.FieldId = "";
        this.LinkContext = null;
        this.Parameters = {};
        this.Properties = {};
    }

    add(key, data) {
        this.Parameters[key] = data;
    }
}
