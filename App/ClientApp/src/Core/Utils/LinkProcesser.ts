import StatusCode from "../../Constant/StatusCodes";

function handeleResponse(result: RequestResultInfo, navigator: any, handler: any = {}) {
    const { Status, RedirectUrl, Message } = result;

    if (Status === StatusCode.Success) {
        if (handler.onSuccessOverride) {
            return handler.onSuccess(result);
        }
        window._App.Notify.success("Data Saved");
        if (navigator.opener) {
            navigator.close();
            navigator.opener.reload();
        } else if (RedirectUrl) {            
            navigator.navigate(RedirectUrl);
        }
    } else if(Status === StatusCode.Fail) {//FAIL
        window._App.Notify.error(Message);
        _Debug.log("FAIL: " + Message);
    }
}

const LinkProcesser = {
    processLink: (action: ActionInfo, payload: {}, navigator: any) => {
        _App.Request.getData({
            url: action.Url,
            body: payload
        }).then((result: any) => {            
            handeleResponse(result, navigator);
        }).catch((ex: any) => {
            _Debug.error(".processLink Error.");
            _Debug.error(ex);
        });
    },

    handeleResponse
};

export default LinkProcesser;