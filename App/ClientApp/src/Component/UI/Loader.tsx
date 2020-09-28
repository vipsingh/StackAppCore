import React from  "react";
import { PositionProperty } from "csstype";

const comp: React.FC<{ position?: PositionProperty, message?: string }> = ({ position, message }) => {
    
    return (<div style={{ position: position || "absolute" }}>
        loading..
    </div>);
}
export default comp;