using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using StackErp.DB;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Layout;

namespace StackErp.Core.Entity
{
    public class EntityMasterEntity : DBEntity
    {
        public EntityMasterEntity(int id, string name, Dictionary<string, BaseField> fields,EntityType entityType, DbObject entDbo) : base(id, name, fields, entityType, entDbo)
        {
        }

        protected override void BuildDefaultQueries()
        {
            var qBuilder = new EntityQueryBuilder(this);
            
            _detailQry = qBuilder.BuildDetailQry(true);

            BuildRelatedFIeldQueries(qBuilder);
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

        public override Model.DataList.EntityListDefinition CreateDefaultListDefn(StackAppContext appContext)
        {
            var defn = PrepareEntityListDefin();
            
            var layoutF = new List<string>() { "name", "text", "tablename", "primaryfield", "namefield" };
            var tlist = new TList();
            foreach (var f in layoutF)
            {
                tlist.Fields.Add(new TListField() { FieldId = f });
            }
            defn.Layout = tlist;

            defn.IncludeGlobalMasterId = true;
            
            return defn;
        }
    }
}
