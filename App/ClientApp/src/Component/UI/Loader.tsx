import React from  "react";

const comp: React.FC<{ position?: any, message?: string }> = ({ position, message }) => {
    
    return (<div style={{ position: position || "absolute" }}>
        loading..
    </div>);
}
export default comp;