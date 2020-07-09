using System;
using System.Collections.Generic;
using System.IO;

namespace StackErp.Model
{
    public class StackAppContext
    {
        public string AppRoot { set; get; }
        public int MasterId { get { return UserInfo.MasterId; } }
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

    [Serializable]
    public class UserContext
    {
        public int MasterId { private set; get; }
        public int UnitId { set; get; }
        public int CompanyId { set; get; }
        public int RoleId { set; get; }
        public int UserId { set; get; }
        public string UserName { set; get; }
        public string LoginId { set; get; }
        public int LanguageId { set; get; }
        public int TimeZoneId { set; get; }
        public int TimeZoneOffset { set; get; }
        public bool IsMobileUser { set; get; }

        public List<DynamicObj> EntityAccessData { set; get; } 

        public UserContext()
        {
            MasterId = 10;
            UserId = 1;
            UserName = "Default User";
            CompanyId = 1;
            RoleId = 1;
            LanguageId = 1;
            TimeZoneOffset = 330;
        }

        public bool HasAccess(EntityCode entityId, AccessType type)
        {
            if (this.UserId == 1) return true; //super user

            var d = EntityAccessData.Find(x => x.Get("entityid", 0) == entityId.Code && (x.Get("operation", 0) == (int)type || x.Get("operation", 0) == 0));
            if (d != null)
            {
                var op = d.Get("invoke", true);
                
                return op;
            }

            return false;
        }
    }
}
