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
    public class EntityLayoutEntity : DBEntity
    {
        public EntityLayoutEntity(int id, string name, Dictionary<string, BaseField> fields,EntityType entityType, string tableName) : base(id, name, fields,entityType, tableName)
        {
            // this.Fields.Add("ITEMTYPEID", new IntegerField()
            // {
            //     Name = "itemtypeid",
            //     Text = "ItemType",
            //     DBName = "itemtype",
            //     IsRequired = true,
            //     Copy = false,
            //     IsDbStore = true,
            //     ViewId = -1
            // });

            this.Fields.Add("ENTITYID", new IntegerField()
            {
                Name = "entityid",
                Text = "entityid",
                DBName = "entityid",
                IsRequired = true,
                Copy = false,
                IsDbStore = true,
                ViewId = -1
            });

            this.Fields.Add("VIEWTYPE", new IntegerField()
            {
                Name = "viewtype",
                Text = "viewtype",
                DBName = "viewtype",
                IsRequired = false,
                Copy = false,
                IsDbStore = true,
                ViewId = 0,
                DefaultValue = 0
            });

            this.Fields.Add("LAYOUTXML", new XmlField()
            {
                Name = "layoutxml",
                Text = "layoutxml",
                DBName = "layoutxml",
                IsRequired = true,
                Copy = false,
                IsDbStore = true,
                ViewId = 0
            });
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
