import StatusCode from "../../Constant/StatusCodes";

function handeleResponse(result: RequestResultInfo, navigator: any) {
    const { Status, RedirectUrl, Message } = result;

    if (Status === StatusCode.Success) {
        window._App.Notify.success("Data Saved");
        if (RedirectUrl) {            
            navigator.navigate(RedirectUrl);
        }
    } else if(Status === StatusCode.Fail) {//FAIL
        window._App.Notify.error(Message);
        _Debug.log("FAIL: " + Message);
    }
}

function openDrawer(linkInfo: any) {
    
}

const LinkProcesser = {
    processLink: (action: ActionInfo, payload: {}) => {
        _App.Request.getData({
            url: action.Url,
            body: payload
        }).then((result: any) => {
            _Debug.log(result);
            //this.handeleSubmitResponse(result);
        }).catch((ex: any) => {
            _Debug.error("Submit Error.");
            _Debug.error(ex);
        });
    },

    handeleResponse
};

export default LinkProcesser;