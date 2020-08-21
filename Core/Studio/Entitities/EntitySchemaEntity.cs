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
    public class EntitySchemaEntity : DBEntity
    {
        public EntitySchemaEntity(int id, string name, Dictionary<string, BaseField> fields,EntityType entityType, string tableName) : base(id, name, fields, entityType, tableName)
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
                    CreateTableColumn();
                }
            }
            return sts;
        }

        private void CreateTableColumn()
        {

        }
    }
}
