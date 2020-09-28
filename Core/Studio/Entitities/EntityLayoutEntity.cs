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
        public EntityLayoutEntity(int id, string name, Dictionary<string, BaseField> fields,EntityType entityType, DbObject entDbo) : base(id, name, fields,entityType, entDbo)
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

            this.Fields.Add("LAYOUTJSON", new JsonField()
            {
                Name = "layoutjson",
                Text = "layoutjson",
                DBName = "layoutjson",
                IsRequired = true,
                Copy = false,
                IsDbStore = true,
                ViewId = 0
            });

            TextField = "id";
        }

        public override AnyStatus Save(StackAppContext appContext, EntityModelBase model)
        {
            if (model.IsNew)
            {
                var entityId = model.GetValue<int>("entityid", 0);
                var itemType = this.GetEntity(entityId).DefaultItemTypeId;
                model.SetValue("itemType", itemType);
                model.SetValue("viewType", 0);
            }

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

        protected override bool Validate(EntityModelBase model)
		{

			return base.Validate(model);
		}

        public override Model.Layout.TView GetDefaultLayoutView(EntityLayoutType layoutType)
        {
           var view = base.GetDefaultLayoutView(layoutType);          
           view.Commands = new List<Model.Layout.TCommand>();
           view.Commands.Add(new Model.Layout.TCommand() { Id = 12, Text = "Designer" });
            
            return view;
        }
    }
}
