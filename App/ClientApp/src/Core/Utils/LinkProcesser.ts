
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
    }
};

export default LinkProcesser;