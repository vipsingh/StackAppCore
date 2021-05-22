using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using StackErp.DB;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core.Entity
{
    public class EntityActionEntity : DBEntity
    {
        public EntityActionEntity(int id, string name, Dictionary<string, BaseField> fields,EntityType entityType, DbObject entDbo) : base(0, id, name, fields,entityType, entDbo)
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

        public override Model.DataList.EntityListDefinition CreateDefaultListDefn(StackAppContext appContext)
        {
            var defn = base.CreateDefaultListDefn(appContext);
            defn.IncludeGlobalMasterId = true;
            
            return defn;
        }
    }
}
