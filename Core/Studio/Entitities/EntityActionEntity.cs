using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core.Entity
{
    public class EntityActionEntity : DBEntity
    {
        public EntityActionEntity(int id, string name, Dictionary<string, BaseField> fields,EntityType entityType, string tableName) : base(id, name, fields,entityType, tableName)
        {
        }

        public override AnyStatus Save(StackAppContext appContext, EntityModelBase model)
        {
            return base.Save(appContext, model);
        }
    }
}