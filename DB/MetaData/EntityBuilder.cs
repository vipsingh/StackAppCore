using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;

namespace StackErp.DB
{
    public static class EntityBuilder
    {
        private const string CREATE_COLUMN = "alter table {0} add {1} {2};";
        public static bool CreateEntityColumn(IDbConnection connection, IDbTransaction transaction, string entityTable, string columnName, TypeCode baseTypeCode, int length, bool isRequired = false)
        {
            var fieldType = GetFieldType(baseTypeCode, length);
            var sql = String.Format(CREATE_COLUMN, entityTable, columnName, fieldType);

            DBService.ExecuteDMLQuery(sql, null, connection, transaction);

            return true;
        }

        private static string GetFieldType(TypeCode baseTypeCode, int length)
        {
            string baseType = "VARCHAR";            
            switch(baseTypeCode) {
                case TypeCode.Int32:
                    baseType = "INT";
                    break;
                case TypeCode.Int64:
                    baseType = "BIGINT";
                    break;
                case TypeCode.Boolean:
                    baseType = "BOOL";
                    break;
                case TypeCode.DateTime:
                    baseType = "timestamp without time zone";
                    break;
                case TypeCode.Decimal:
                    baseType = "DECIMAL";
                    break;                                                                                       
            }           

            if (length > 0) {
                if (baseType == "VARCHAR") {
                    if (length == -1)
                        return baseType + "(max)";
                    return baseType + "(" + length.ToString() + ")";
                }
                else if (baseType == "DECIMAL") {
                    return baseType + "(20," + length.ToString() + ")";
                }
            }

            return baseType;
        }

        public static bool CreateEntityTable(EntityModelBase model, IDbConnection connection, IDbTransaction transaction, out int defaultItemType)
        {
            try
            {
                var qry = BuildEntityTableQry(model, out defaultItemType);

                DBService.ExecuteDDLQuery(qry, null, connection, transaction);

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string BuildEntityTableQry(DBModelBase model, out int defaultItemType)
        {
            var itemId = model.ID;
            var tablename = "t_" + (string)model.GetValue("name");
            var namefield = (string)model.GetValue("namefield");
            var currDate = DateTime.Now.ToLongDateString();

            var maxItemType = EntityDBService.GetNextEntityDBId(EntityCode.ItemType);
            var maxLayout = EntityDBService.GetNextEntityDBId(EntityCode.EntityLayout);
            var maxSchema = EntityDBService.GetNextEntityDBId(EntityCode.EntitySchema);

            defaultItemType = maxItemType;

            string qry = string.Format(@"CREATE TABLE {0} (
    masterid integer NOT NULL,
    id integer,
    {1} character varying(100)  NOT NULL,
    createdby integer NOT NULL,
    updatedby integer,
    createdon timestamp without time zone NOT NULL,	
    updatedon timestamp without time zone,
    CONSTRAINT {0}_pkey PRIMARY KEY (id));

    INSERT INTO autoid values({2}, 0);

", tablename, namefield, itemId);

            var qItemTyp = string.Format(@"    
    insert into t_entity_itemtype(masterid,id,entityid,name,code) values({0},{2},{1},'Default','0');
    insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutjson)values({0},{3},{1},{2},'','0', null);", model.MasterId, itemId, maxItemType, maxLayout);

            var defaultNameCol = $@"insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
        ,dbname,tablename,viewtype,defaultvalue
        ,createdon,updatedon)
        values({model.MasterId},{maxSchema},{itemId},'{namefield}','{namefield}',1,100,true
        ,'{namefield.ToLower()}','{tablename}',0,null
        ,'{currDate}','{currDate}');";

            return qry + qItemTyp + defaultNameCol;
        }
    }
}