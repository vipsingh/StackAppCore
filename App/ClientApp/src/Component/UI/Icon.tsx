import React from  "react";
import * as AntdIcons from '@ant-design/icons';

export default (p: string|{ Icon: string, Size: string }|undefined) => {
    if (!p) return undefined;
    
    let i = p;
    if (typeof p == "object") {
        i = p.Icon;
    }
    
    let Comp;
    switch(i) {
        case "edit":
            Comp = AntdIcons.EditOutlined;
            break;
        case "new":
            Comp = AntdIcons.PlusOutlined;
            break;        
        case "save":
            Comp = AntdIcons.SaveOutlined;
            break;        
        case "cancel":
        case "close":
            Comp = AntdIcons.CloseOutlined;
            break;
    }

    return  Comp ? <Comp /> : undefined;
};