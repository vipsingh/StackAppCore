using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Core.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.DataList
{
    public class DataListBuilder
    {
        public AnyStatus Status = AnyStatus.NotInitialized;
        List<DynamicObj> Data;
        protected virtual DataListDefinition GetSourceDefinition(DataListContext context)
        {
            return null;
        }

        public virtual DataListResponse GetResponse(DataListContext context)
        {
            var resp = new DataListResponse()
            {                
                Data = Data,

            };
            if (context.ListRequest.RequestType == DataListRequestType.SchemaWithData)
            {
                resp.Fields = context.Fields;
                resp.IdColumn = context.IdColumn;
                resp.ItemViewField = context.ItemViewField;
            }
            return resp;
        }

        public virtual void Build(DataListContext context)
        {
            var defn = GetSourceDefinition(context);
            try {
                OnBuildInit(context, defn);
                Compile(context, defn);
                ExecutePrepareData(context, defn);
            }
            catch(Exception ex)
            {
                Status = AnyStatus.InvalidData;
                Status.Message = ex.Message;
                return;
            }
        }

        protected virtual void Compile(DataListContext context, DataListDefinition defn)
        {
            this.BuildHeader(context, defn);
        }
                
        protected virtual void PrepareLinkDefinition(DataListContext context, DataListDefinition defn)
        {
            
        }

        protected virtual void BuildHeader(DataListContext context, DataListDefinition defn)
        {
            
        }

        public virtual void OnBuildInit(DataListContext context, DataListDefinition defn)
        {
            context.BuildSource(defn);
        }

        protected virtual void BeforePrepareData(DataListContext context, DataListDefinition defn)
        {
        }

        protected virtual void ExecutePrepareData(DataListContext context, DataListDefinition defn)
        {
            this.BeforePrepareData(context, defn);

            EntityListService service = new EntityListService();
            var data = service.ExecuteData(context.DbQuery);

            var records = new List<DynamicObj>();
            foreach(var dataRow in data)
            {
                records.Add(PrepareRowData(dataRow, context, defn));
            }

            this.Data = records;
        }
        
        protected DynamicObj PrepareRowData(DbObject dataRow, DataListContext context, DataListDefinition defn)
        {
            var row = new DynamicObj();
            row.Add("RowId", dataRow.Get(context.DbQuery.ItemIdField, 0));
            foreach(var field in context.DbQuery.Fields)
            {
                var val = field.Field.ResolveDbValue(dataRow);
                if (context.Fields.Keys.Contains(field.FieldName))
                {
                    row.Add(field.FieldName, OnPrepareCell(context, defn, field.FieldName, val, row), true);
                } 
                else 
                {
                    row.Add(field.FieldName, val);
                }

                OnPrepareRow(context, defn, row, dataRow);
            }

            return row;
        }
        
        protected virtual void OnPrepareRow(DataListContext context, DataListDefinition defn, DynamicObj row, DbObject dataRow)
        {
            BuildDataRowActions(context, row);
        }


        protected virtual object OnPrepareCell(DataListContext context, DataListDefinition defn, string fieldName, object fieldValue, DynamicObj row)
        {
            if (!context.Fields.ContainsKey(fieldName))
                return new { Value = fieldValue };

            context.Fields[fieldName].ClearValue();

            context.Fields[fieldName].SetValue(fieldValue);
            //SetEntityOpenLink(context, defn, row, context.Fields[fieldName]);
                
            return new { FormatedValue = context.Fields[fieldName].FormatedValue, AdditionalValue = context.Fields[fieldName].AdditionalValue == null? null: context.Fields[fieldName].AdditionalValue.CloneData() };
        }

        protected virtual void BuildDataRowActions(DataListContext context, DynamicObj row)
        {

        }
    }
}
