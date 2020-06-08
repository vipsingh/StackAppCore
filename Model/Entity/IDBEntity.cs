using System;
using System.Collections.Generic;
using System.Data;
using StackErp.Model.DataList;

namespace StackErp.Model.Entity
{
    public interface IDBEntity
    {
        EntityCode EntityId {get;}
        string Name {get;}
        string DBName {get;}
        string Text { get; }
        InvariantDictionary<BaseField> Fields {get;}
        
        string TextField {get; set;}
         string IDField {get;}

        List<IEntityRelation> Relations {get;}

        List<string> ComputeOrderSeq { get; }

        bool IsChildEntity {get;}
        bool IsTransiant {get;}

        Dictionary<string, BaseField> GetFields();
        BaseField GetFieldSchema(string fieldName);
        BaseField GetFieldSchema(int fieldId);
        //BaseField GetFieldSchemaByViewName(string fieldViewName);
        EntityModelBase GetSingle(int id);
        EntityModelBase GetDefault();
        List<EntityModelBase> GetAll(FilterExpression filter);
        List<EntityModelBase> GetAll(int[] ids);
        AnyStatus Save(StackAppContext appContext, EntityModelBase model);
        AnyStatus Save(StackAppContext appContext, EntityModelBase model, IDbConnection connection, IDbTransaction transaction);
        AnyStatus OnAfterDbSave(StackAppContext appContext, EntityModelBase model, IDbConnection connection, IDbTransaction transaction);
        AnyStatus OnBeforeDbSave(StackAppContext appContext, EntityModelBase model, IDbConnection connection, IDbTransaction transaction);
        
        AnyStatus DeleteRecord(int id);
        DBModelBase Read(int id, List<string> fields);
        List<DBModelBase> ReadAll(List<string> fields, FilterExpression filter);
        List<int> ReadIds(FilterExpression filter);

        IDBEntity GetEntity(EntityCode id);
        
    }

    public interface IEntityRelation
    {
        EntityRelationType Type {get;}
        EntityCode ParentName {get;}
        EntityCode ChildName {get;}

        BaseField ParentRefField {get;}

        BaseField ChildRefField {get;}
        BaseField ChildDisplayField {get;}

        /*Item1: FIeldName, Item2: Child RefFieldName*/
        List<(string, string)> OtherChildFields {get;}
    }

}