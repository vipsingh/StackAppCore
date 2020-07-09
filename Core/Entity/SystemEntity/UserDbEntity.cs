using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core.Entity
{
    public class UserDbEntity : DBEntity
    {
        public UserDbEntity(int id, string name, Dictionary<string, BaseField> fields, string tableName) : base(id, name, fields, tableName)
        {
            this.Fields.Add("LOGINID", new StringField()
            {
                Type = FieldType.Text,
                Name = "Loginid",
                Text = "Loginid",
                DBName = "loginid",
                IsRequired = true,
                Copy = false,
                IsDbStore = true,
                ViewId = 0
            });

            this.Fields.Add("LOGINTYPE", new IntegerField()
            {
                Type = FieldType.Integer,
                Name = "Logintype",
                Text = "Login Type",
                DBName = "logintype",
                IsRequired = true,                
                IsDbStore = true,
                ViewId = 0
            });

            this.Fields.Add("TYPE", new IntegerField()
            {
                Type = FieldType.Integer,
                Name = "Type",
                Text = "Type",
                DBName = "type",
                IsRequired = true,                
                IsDbStore = true,
                ViewId = 0
            });

            this.Fields.Add("PASSWORD", new PasswordField()
            {
                Type = FieldType.Password,
                Name = "Password",
                Text = "Password",
                DBName = "password",
                IsRequired = true,
                Copy = false,
                IsDbStore = true,
                ViewId = 0
            });

            this.Fields.Add("ISACTIVE", new BoolField()
            {
                Type = FieldType.Bool,
                Name = "IsActive",
                Text = "Active",
                DBName = "isactive",
                IsRequired = false,                
                IsDbStore = true,
                ViewId = 0
            });
        }

        public override AnyStatus Save(StackAppContext appContext, EntityModelBase model)
        {
            //encrypt pwd if new
            //if edit do not change **
            if (model.IsNew)
            {
                model.SetValue("Password", Encrypt(model.GetValue("Password").ToString()));
            }

            return base.Save(appContext, model);
        }

        public int AuthenticateUser(string loginId, string pwd, out string email)
        {
            var exp = new FilterExpression(this.EntityId);
            exp.Add(new FilterExpField("loginid", FilterOperationType.Equal, loginId));
            var data = ReadAll(new List<string>(){ "password", "emailid" }, exp);

            email = null;
            
            if (data.Count > 0)
            {
                var d = data.First();                
                email = d.GetValue("emailid").ToString();

                var userpwd = Decrypt(d.GetValue("Password").ToString());

                if (pwd == userpwd)
                {
                    return d.ID;
                }
            }

            return -1;
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

        static string _EncyKey = "stackerp_userpwd";
    }
}
