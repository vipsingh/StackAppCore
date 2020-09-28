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
    public class EntityBuilder
    {
        private const string CREATE_COLUMN = "alter table {0} add {1} {2};";
        public static bool CreateEntityColumn(IDbTransaction transaction, string entityTable, string columnName, TypeCode baseTypeCode, int length, bool isRequired = false)
        {
            var fieldType = GetFieldType(baseTypeCode, length);
            var sql = String.Format(CREATE_COLUMN, entityTable, columnName, fieldType);

            DBService.ExecuteDMLQuery(sql, null, transaction);

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

       
    }
}