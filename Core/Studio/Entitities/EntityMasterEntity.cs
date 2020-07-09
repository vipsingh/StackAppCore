using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core.Entity
{
    public class EntityMasterEntity : DBEntity
    {
        public EntityMasterEntity(int id, string name, Dictionary<string, BaseField> fields, string tableName) : base(id, name, fields, tableName)
        {
            
        }

        public override AnyStatus Save(StackAppContext appContext, EntityModelBase model)
        {
            return base.Save(appContext, model);
        }

        public override AnyStatus OnAfterDbSave(StackAppContext appContext, EntityModelBase model, IDbConnection connection, IDbTransaction transaction)
        {
            AnyStatus sts = base.OnAfterDbSave(appContext, model, connection, transaction);
            if (sts == AnyStatus.Success)
            {
                if (model.IsNew)
                {
                    
                }
            }
            return sts;
        }
    }
}
