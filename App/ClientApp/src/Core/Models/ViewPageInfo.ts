import _ from "lodash"

export default class ViewPageInfo implements IPageInfo {    
    Widgets: IDictionary<WidgetInfoProps>
    Actions!: IDictionary<any>
    EntityInfo: ObjectModelInfo
    ErrorMessage!: string
    Layout!: any
    PostUrl: string|undefined
    FormRules!: any
    Errors!: IDictionary<{ IsValid: boolean, Message?: string }>
    Dependencies: IDictionary<Array<any>>

    constructor(schema: any, data: any = null) {
        this.Widgets = {};
        this.Errors = {};
        this.EntityInfo = { ObjectId: -1 };        

        if (schema) {
            _.extend(this, {...schema});

            if (data && data.Widgets) {
                this.Widgets = _.cloneDeepWith(schema.Widgets, (v: any) => {
                    if (v.WidgetId) {
                        return _.extend({}, v, data.Widgets[v.WidgetId]);
                    }                    
                });
            } else
                this.Widgets = _.cloneDeep(schema.Widgets);
        }
        
        this.Dependencies = { };
        _.forIn(this.Widgets, (w, wk) => {
            if (w.Features) {
                _.forIn(w.Features, (f, fk) => {

                    if (f.Depends) {
                        _.each(f.Depends, (d) => {
                            if (!this.Dependencies[d]) this.Dependencies[d] = [];

                            this.Dependencies[d] = _.union(this.Dependencies[d], [wk]);
                        });
                    }
                });
            }
        });

        const wd: any = this.Widgets;
        wd["_UniqueId"] = { WidgetId: "_UniqueId", WidgetType: 0 };
        wd["_EntityInfo"] = { WidgetId: "_EntityInfo", WidgetType: 0 };
    }

    getField(widgetId: string) {
        return this.Widgets[widgetId];
    }

    getDataModel(): IDictionary<IFieldData> {
        const dataModel: IDictionary<any> = {};
        _.forIn(this.Widgets, w => {
            dataModel[w.WidgetId] = {Value: w.Value };
        });

        dataModel._UniqueId = { Value: _.uniqueId("v_form") };
        dataModel._EntityInfo = { Value: this.EntityInfo };

        return dataModel;
    }

}