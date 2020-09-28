import React from "react";
import Loadable from "react-loadable";
import Loader from "../UI/Loader";

export function getAsyncComponent(Compnent: any) {
    return Loadable({
        loader: Compnent,
        loading() {
            return <Loader />;
        }
    });
}
