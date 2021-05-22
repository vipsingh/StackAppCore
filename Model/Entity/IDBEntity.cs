using System;
using System.Collections.Generic;
using System.Data;
using StackErp.Model.DataList;

namespace StackErp.Model.Entity
{
    public interface IDBEntity
    {
        int MasterId {get;}
        EntityCode EntityId {get;}
        string Name {get;}
        string DBName {get;}
        string Text { get; }
        InvariantDictionary<BaseField> Fields {get;}
        
        string TextField {get;}
         string IDField {get;}

        List<IEntityRelation> Relations {get;}

        int DefaultItemTypeId {get;} 

        List<string> ComputeOrderSeq { get; }

        EntityType EntityType {get;}

        Dictionary<string, BaseField> GetFields();
        BaseField GetFieldSchema(string fieldName);
        BaseField GetFieldSchema(int fieldId);
        //BaseField GetFieldSchemaByViewName(string fieldViewName);
        EntityModelBase GetSingle(StackAppContext appContext, int id);
        EntityModelBase GetDefault(StackAppContext appContext);
        List<EntityModelBase> GetAll(StackAppContext appContext, FilterExpression filter);
        List<EntityModelBase> GetAll(StackAppContext appContext, int[] ids);
        AnyStatus Save(StackAppContext appContext, EntityModelBase model);
        AnyStatus Save(StackAppContext appContext, EntityModelBase model, IDbConnection connection, IDbTransaction transaction);
        AnyStatus OnAfterDbSave(StackAppContext appContext, EntityModelBase model, IDbConnection connection, IDbTransaction transaction);
        AnyStatus OnBeforeDbSave(StackAppContext appContext, EntityModelBase model, IDbConnection connection, IDbTransaction transaction);
        
        AnyStatus DeleteRecord(StackAppContext appContext, int id);
        DBModelBase Read(StackAppContext appContext, int id, List<string> fields);
        List<DBModelBase> ReadAll(StackAppContext appContext, List<string> fields, FilterExpression filter);
        List<int> ReadIds(StackAppContext appContext, FilterExpression filter);

        IDBEntity GetEntity(EntityCode id);

        List<BaseField> GetLayoutFields(EntityLayoutType type);

        Layout.TView GetDefaultLayoutView(EntityLayoutType layoutType);

        EntityListDefinition CreateDefaultListDefn(StackAppContext appContext);
        
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