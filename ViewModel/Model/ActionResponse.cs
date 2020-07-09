using System;
using StackErp.Model;

namespace StackErp.ViewModel.Model
{
    public class ActionResponse
    {
        public object Data {set;get;}
        public int ResponseStatus {set;get;}
        public string Message {set;get;}
        public ActionResponse(object data)
        {
            Data = data;
            this.ResponseStatus = 200;
        }
    }

    public class PageResponse: ActionResponse
    {
        public bool IsPage{get;}
        //public AppPageType PageType {get;}
        public PageResponse(object data): base(data)
        {
            this.IsPage = true;
        }
    }

    public class ErrorResponse: ActionResponse
    {
        public ErrorResponse(object data): base(data)
        {
            this.ResponseStatus = 500;
        }
    }

    public class AuthErrorResponse: ErrorResponse
    {
        public AuthErrorResponse(string message): base(message)
        {
            this.ResponseStatus = 401;
        }
    }

    public class SubmitActionResponse: ActionResponse
    {
        public SubmitStatus Status {set;get;}
        public string VerificationToken {set;get;}
        public UIFormModel Model {set;get;}
        public string RedirectUrl {set;get;}
        public SubmitActionResponse(object data): base(data)
        {
        }
    }

    public class FieldActionResponse: ActionResponse
    {
        public SubmitStatus Status {set;get;}
        public string VerificationToken {set;get;}
        public FieldActionResponse(object data): base(data)
        {
        }
    }
}