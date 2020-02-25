import { Dictionary } from "lodash"

export class CustomRequestInfo {
    ModelInfo!: ObjectModelInfo
    FieldId!: string
    LinkContext: any
    Parameters!: Dictionary<object>
    Properties!: Dictionary<object>

    add(key: string, data: any) {
        this.Parameters[key] = data;
    }
}