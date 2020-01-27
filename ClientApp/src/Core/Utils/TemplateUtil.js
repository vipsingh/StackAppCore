import _ from "lodash";

export function CreateTwoColTemplate(schema) {
    const {ViewFields} = schema.ViewTemplate;
    const sections = [{Id: "section_1", Rows: []}];
    const section = sections[0];
    let start_new_r = true, col_r = [];
    _.each(ViewFields, (f, idx) => {
        const f_schema = _.find(schema.Controls, (fs) => {
            return fs.ControlId === f;
        });
        if(f_schema.isObjectListType){
            if(col_r.length == 1){
                section.Rows.push(col_r);
                col_r = [];
            }
            col_r.push(f);
            section.Rows.push(col_r);
            col_r = [];
        } else {
            if(col_r.length == 1)
              start_new_r = true;
            else
              start_new_r = false;
            col_r.push(f);
            if(start_new_r){
                section.Rows.push(col_r);
                col_r=[];
            }
            else if(ViewFields.length -1 == idx){
                section.Rows.push(col_r);
            }
        }
    });

    return sections;
}