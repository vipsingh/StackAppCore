using System;

namespace StackErp.Model
{
    public class StackAppContext
    {
        public int RoleId {set;get;} 
    public int UserId  {set;get;}
    public string AppRoot  {set;get;}
    public int LanguageId  {set;get;}
    public string BaseCurrencyCode  {set;get;}
    public int BaseCurrencyId  {set;get;}
    public int TimeZoneId  {set;get;}
    public int UnitId  {set;get;}
    public int CompanyId  {set;get;}
    public ApplicationType AppType  {set;get;}
    public string ShortDateFormat  {set;get;}
    public string LongDateFormat  {set;get;}
    public UserContext UserInfo {set;get;}
    public RequestQueryString RequestQuery {set;get;}

    public void Init()
    {
        AppType =  ApplicationType.Web;
        CompanyId =1;
        RoleId =1;
        LanguageId=1;
        AppRoot ="/";
        BaseCurrencyCode ="INR";
        BaseCurrencyId = 1;

        ShortDateFormat = "dd/mmm/yyyy";

        UserInfo = new UserContext();

    }
    }

    public class UserContext {
        public int UserId {set;get;}
        public int UserName {set;get;}
        public int LoginId {set;get;}
    }
}
