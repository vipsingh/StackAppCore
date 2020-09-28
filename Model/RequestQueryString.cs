using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StackErp.Model
{
    public class RequestQueryString : InvariantDictionary<string>
    {
        //private string _UrlReferer;

        public int ItemId
        {
            get { return GetInt("ItemId"); }
            set { this["ItemId"] = value.ToString(); }
        } //ObjectId, ReportId ...
        public int ViewType
        {
            get { return GetInt("ViewType"); }
            set { this["ViewType"] = value.ToString(); }
        }

        public EntityCode EntityId
        {
            get { return GetInt("EntityId"); }
            set { this["EntityId"] = value.Code.ToString(); }
        }
        public EntityCode RelatedEntityId
        {
            get { return GetInt("RelatedEntityId"); }
            set { this["RelatedEntityId"] = value.Code.ToString(); }
        }
        public int ItemTypeId
        {
            get { return GetInt("ItemTypeId"); }
            set { this["ItemTypeId"] = value.ToString(); }
        }

        public int LayoutId
        {
            get { return GetInt("LayoutId"); }
            set { this["LayoutId"] = value.ToString(); }
        }
        public int LayoutType
        {
            get { return GetInt("LayoutType"); }
            set { this["LayoutType"] = value.ToString(); }
        }

        public int RelatedObjectId
        {
            get { return GetInt("RelatedObjectId"); }
            set { this["RelatedObjectId"] = value.ToString(); }
        }
        public string RelatedEntityName
        {
            get { return GetString("RelatedEntityName"); }
            set { this["RelatedEntityName"] = value.ToString(); }
        }

        public ListingType ListingType
        {
            get { return (ListingType)GetInt("ListingType"); }
            set { this["ListingType"] = value.ToString(); }
        }
        public int ListingId
        {
            get { return GetInt("ListingId"); }
            set { this["ListingId"] = value.ToString(); }
        }
        public int RowId
        {
            get { return GetInt("RowId"); }
            set { this["RowId"] = value.ToString(); }
        }

        public bool IsAjax
        {
            get { return GetBool("IsAjax"); }
            set { this["IsAjax"] = value.ToString(); }
        }

        public int FieldId
        {
            get { return GetInt("FieldId"); }
            set { this["FieldId"] = value.ToString(); }
        }
        public string FieldName
        {
            get { return GetString("FieldName"); }
            set { this["FieldName"] = value.ToString(); }
        }

        public string RelationField
        {
            get { return GetString("RelationField"); }
            set { this["RelationField"] = value.ToString(); }
        }
        
        public string WidgetId
        {
            get { return GetString("WidgetId"); }
            set { this["WidgetId"] = value.ToString(); }
        }
        public string Name
        {
            get { return GetString("Name"); }
            set { this["Name"] = value.ToString(); }
        }
        private int GetInt(string param)
        {
            string val = GetData(param);

            if (val != null)
            {
                var propertyValue = Convert.ToInt32(val);
                return propertyValue;
            }
            else
            {
                return 0;
            }
        }
        private bool GetBool(string param)
        {
            string val = GetData(param);

            if (val != null)
            {
                var propertyValue = Convert.ToBoolean(val);
                return propertyValue;
            }
            else
            {
                return false;
            }
        }
        private string GetString(string param)
        {
            string val = GetData(param);

            if (val != null)
            {
                var propertyValue = Convert.ToString(val);
                return propertyValue;
            }
            else
            {
                return "";
            }
        }
        public RequestQueryString()
        {
            
        }
        public string GetData(string key)
        {
            if(this.ContainsKey(key))
                return this[key];

            return null;
        }

        public void Load(string query)
        {
            var s = Decrypt(query);
            var d = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(s);

            foreach (var k in d)
            {
                if (k.Value != null)
                {
                    this[k.Key] = k.Value.ToString();
                }
            }
        }
        public void LoadNonEncrypt(string query)
        {            
            var d = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(query);

            foreach (var k in d)
            {
                if (k.Value != null)
                {
                    this[k.Key] = k.Value.ToString();
                }
            }
        }

        public RequestQueryString Clone()
        {
            return (RequestQueryString)MemberwiseClone();
        }

        public string ToQueryString()
        {
            var jObj = new JObject();
            foreach(var key in this.Keys)
            {
                jObj.Add(key, this[key].ToString());
            }
            var j = JsonConvert.SerializeObject(jObj);
            var enc = Encrypt(j);

            return "q=" + enc;
        }

        private string Encrypt(string encryptString)
        {
            string EncryptionKey = _EncyKey;
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = _EncyKey;
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        static string _EncyKey = "stackapp_x";

    }

    internal static class ReqQueryKeyConstant {
        internal const string ItemId = "a";
        internal const string ViewType = "b";
        internal const string EntityId = "c";
        internal const string RelatedEntityId = "d";
        internal const string ItemTypeId = "e";
    }
}
