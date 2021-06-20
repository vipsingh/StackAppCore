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
        public EntityMasterEntity(int id, string name, Dictionary<string, BaseField> fields,EntityType entityType, DbObject entDbo) : base(0, id, name, fields, entityType, entDbo)
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
            int defaultItemType = 0;
            if (sts == AnyStatus.Success)
            {
                if (model.IsNew)
                {
                    EntityBuilder.CreateEntityTable(model, connection, transaction, out defaultItemType);
                }
                
            }
            return sts;
        }

        public override AnyStatus OnSaveComplete(StackAppContext appContext, EntityModelBase model)
        {
            EntityMetaData.Build();

            return base.OnSaveComplete(appContext, model);
        }

        protected override bool Validate(EntityModelBase model)
        {
            var isvalid = base.Validate(model);

            return isvalid;
        }

        public override Model.DataList.EntityListDefinition CreateDefaultListDefn(StackAppContext appContext)
        {
            var defn = PrepareEntityListDefin();
            
            var layoutF = new List<string>() { "name", "text", "tablename", "namefield" };
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
