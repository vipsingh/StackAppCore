using System;
using System.IO;

namespace StackErp.Model
{
    public class StackAppContext
    {
        public string AppRoot { set; get; }
        public int MasterId { private set; get; }
        public ApplicationType AppType { set; get; }
        public string ShortDateFormat { set; get; }
        public string LongDateFormat { set; get; }
        public string BaseCurrencyCode { set; get; }
        public int BaseCurrencyId { set; get; }
        public string CurrencyAmountFormat { set; get; }
        public UserContext UserInfo { set; get; }
        public RequestQueryString RequestQuery { set; get; }
        public string NotSpecifiedText { set; get; }
        public string ImageStorePath { set; get; }

        public void Init(AppKeySetting appSetting)
        {
            MasterId = 10;
            AppType = ApplicationType.Web;            
            AppRoot = "/";

            ShortDateFormat = "dd/MMM/yyyy";

            BaseCurrencyCode = "INR";
            BaseCurrencyId = 1;
            CurrencyAmountFormat = "{0:0.00}";

            UserInfo = new UserContext();

            NotSpecifiedText = "";

            ImageStorePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dms");

           AppSetting = appSetting;
        }

        public AppKeySetting AppSetting {private set;get;}

        // public bool IsAllow(ItemType itemType, OperationType operationType)
        // {
        //     var operations = CRMMetaData.GetCategoryOperations(itemType, operationType);

        //     if (operations == null || IsAdministrator || AuthenticationContext.IsAnyAllowed(operations))
        //         return true;

        //     return false;
        // }
    }

    public class UserContext
    {
        public int UnitId { set; get; }
        public int CompanyId { set; get; }
        public int RoleId { set; get; }
        public int UserId { set; get; }
        public string UserName { set; get; }
        public int LoginId { set; get; }
        public int LanguageId { set; get; }
        public int TimeZoneId { set; get; }
        public int TimeZoneOffset { set; get; }
        public bool IsMobile { set; get; }

        public UserContext()
        {
            UserId = 1;
            UserName = "Default User";
            CompanyId = 1;
            RoleId = 1;
            LanguageId = 1;
            TimeZoneOffset = 330;
        }
    }
}
