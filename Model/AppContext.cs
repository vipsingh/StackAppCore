using System;

namespace Model
{
    public class AppContext
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
    public int AppType  {set;get;}
    public string ShortDateFormat  {set;get;}
    public string LongDateFormat  {set;get;}
    public UserContext UserInfo {set;get;}
    public RequestQueryString QS {set;get;}
    }

    public class UserContext {
        public int UserId {set;get;}
        public int UserName {set;get;}
        public int LoginId {set;get;}
    }
}
