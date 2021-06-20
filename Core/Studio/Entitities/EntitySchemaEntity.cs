using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Layout;
using StackErp.DB;

namespace StackErp.Core.Entity
{
    public class EntitySchemaEntity : DBEntity
    {
        public EntitySchemaEntity(int id, string name, Dictionary<string, BaseField> fields,EntityType entityType, DbObject entDbo) : base(0, id, name, fields, entityType, entDbo)
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
			if (model.IsNew)
            {
				var entityId = model.GetValue<int>("entityid", 0);
				var length = model.GetValue<int>("length", 0);
				var type = model.GetValue<int>("fieldtype", 0);
				var fieldInfo = EntityMetaData.CreateField((FieldType)type);
				var baseType = fieldInfo.BaseType;
				model.SetTempInfo("basetypecode", baseType);

				if (length == 0) {
					if (baseType == TypeCode.String) {
						length = GetDefaultVarCharLength((FieldType)type);
					} else if (baseType == TypeCode.Decimal) {
						length = 6;                
					}

					model.SetValue("length", length);
				}	

				var fieldName = model.GetValue("fieldname", "");
				var tableName = this.GetEntity(entityId).DBName;
				model.SetValue("tablename", tableName);
				model.SetValue("dbname", fieldName);
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
                    CreateTableColumn(model, connection, transaction);
                }
            }
            return sts;
        }

        private void CreateTableColumn(EntityModelBase model, IDbConnection connection, IDbTransaction transaction)
        {
			var fieldName = model.GetValue("dbname", "");
			//var type = model.GetValue("fieldtype", 0);
			var baseType = model.GetTempInfo("basetypecode", 0);
			var table = model.GetValue("tablename", "");
			var length = model.GetValue("length", 0);

			EntityBuilder.CreateEntityColumn(connection, transaction, table, fieldName, (TypeCode)baseType, length);
        }

		protected override bool Validate(EntityModelBase model)
		{
			return base.Validate(model);
		}

		private static int GetDefaultVarCharLength(FieldType type)
        {
            int length = 100;
            switch(type){
                case FieldType.Html:
                case FieldType.Xml:
                    length = -1;
                    break;
                case FieldType.Json:
                case FieldType.LongText:
                case FieldType.StackScript:
                    length = 4000;
                    break;                               
            }
        
            return length;
        }

        public override Model.Layout.TView GetDefaultLayoutView(EntityLayoutType layoutType)
        {
            //var view  = TView.ParseFromXml(_LayoutXml);
            //var fields = view.GetAllFields();

			var view1 = new TView() { Header = new TPage(), Fields = new List<TField>() };
			view1.Fields = new List<TField>() {
				new TField() { FieldId = "entityid", ReadOnly = "1" },
				new TField() { FieldId = "fieldname", ReadOnly = "2" },
				new TField() { FieldId = "fieldtype", ReadOnly = "2" },
				new TField() { FieldId = "label" },
				new TField() { FieldId = "isrequired" },
				new TField() { FieldId = "defaultvalue" },
				new TField() { FieldId = "ismultiselect", ReadOnly = "2" },
				new TField() { FieldId = "computeexpression" },
				new TField() { FieldId = "uisetting" },
				new TField() { FieldId = "linkentity" , ReadOnly = "2", Invisible = @"[{""fieldtype"":[8, ""10,11,20,21""]}]"},
				new TField() { FieldId = "linkentity_domain", Invisible = @"[{""fieldtype"":[8, ""10,11,20,21""]}]" },
				new TField() { FieldId = "linkentity_field", ReadOnly = "2", Invisible = @"[{""fieldtype"":[8, ""10,11,20,21""]}]" },
				new TField() { FieldId = "relatedexp", ReadOnly = "2" },
				new TField() { FieldId = "collectionid", ReadOnly = "2", Invisible = @"[{""fieldtype"":[8, ""9,30""]}]" }				
			};

            view1.Header.Groups.Add(new TGroup() {
				Rows =  new List<TRow>() {
					new TRow() {
						Cols = new List<TCol>() { new TCol("entityid") }
					},
					new TRow() {
						Cols = new List<TCol>() { new TCol("fieldname"), new TCol("fieldtype") }
					}
				}
			});

			view1.Pages.Add(new TPage() {
				Id = "page_1",
				Text = "General",
				Groups = new List<TGroup>() {
					new TGroup() {
						Rows =  new List<TRow>() {
							new TRow() {
								Cols = new List<TCol>() { new TCol("label"), new TCol("isrequired") }
							},
							new TRow() {
								Cols = new List<TCol>() { new TCol("defaultvalue"), new TCol("ismultiselect") }
							},
							new TRow() {
								Cols = new List<TCol>() { new TCol("computeexpression") }
							}
						}
					},

					new TGroup() {
						Text = "Relation",
						Rows =  new List<TRow>() {
							new TRow() {
								Cols = new List<TCol>() { new TCol("linkentity"), new TCol("linkentity_domain") }
							},
							new TRow() {
								Cols = new List<TCol>() { new TCol("linkentity_field"), new TCol("relatedexp") }
							},
							new TRow() {
								Cols = new List<TCol>() { new TCol("collectionid") }
							}
						}
					} 
				}
			});

			view1.Pages.Add(new TPage()
			{
				Id = "page_2",
				Text = "UI Setting",
				Groups = new List<TGroup>() {
					new TGroup() {
						Rows =  new List<TRow>() {
							new TRow() {
								Cols = new List<TCol>() { new TCol("uisetting") { Span = 2 } }
							},
						}
					}
				}
			});


			return view1;
        }        
    
		public override Model.DataList.EntityListDefinition CreateDefaultListDefn(StackAppContext appContext)
        {
            var defn = PrepareEntityListDefin();
            
            var layoutF = new List<string>() { "fieldname", "fieldtype", "label", "linkentity", "collectionid", "computeexpression" };
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
