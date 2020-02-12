using System;

namespace StackErp.Model
{
    public class StackAppContext
    {
        public int RoleId {set;get;} 
    public int UserId  {set;get;}
    public string AppRoot  {set;get;}
    public int LanguageId  {set;get;}
    public int TimeZoneId  {set;get;}
    public int UnitId  {set;get;}
    public int CompanyId  {set;get;}
    public ApplicationType AppType  {set;get;}
    public string ShortDateFormat  {set;get;}
    public string LongDateFormat  {set;get;}
    public UserContext UserInfo {set;get;}
    public RequestQueryString RequestQuery {set;get;}
    public AppInfo AppInfo {set;get;}

    public void Init()
    {
        AppType =  ApplicationType.Web;
        CompanyId =1;
        RoleId =1;
        LanguageId=1;
        AppRoot ="/";

        ShortDateFormat = "dd/mmm/yyyy";

        UserInfo = new UserContext();
        AppInfo =  new AppInfo();
    }
    }

    public class UserContext {
        public int UserId {set;get;}
        public int UserName {set;get;}
        public int LoginId {set;get;}
    }

    public class AppInfo 
    {
        public AppInfo()
        {
            AppRoot ="/";
        BaseCurrencyCode ="INR";
        BaseCurrencyId = 1;
        }
        public string AppRoot  {set;get;}
        public string BaseCurrencyCode  {set;get;}
        public int BaseCurrencyId  {set;get;}
        public string NotSpecifiedText  {set;get;}
    }
}
