using System;
using System.Collections.Generic;
using StackErp.Model.DataList;

namespace StackErp.Model.Entity
{
    public interface IDBEntity
    {
        EntityCode EntityId {get;}
        string Name {get;}
        string DBName {get;}
        string Text { get; }
        Dictionary<string, BaseField> Fields {get;}
        
        string TextField {get; set;}
         string IDField {get;}

        List<IEntityRelation> Relations {get;}

        Dictionary<string, BaseField> GetFields();
        BaseField GetFieldSchema(string fieldName);
        BaseField GetFieldSchema(int fieldId);
        //BaseField GetFieldSchemaByViewName(string fieldViewName);
        EntityModelBase GetSingle(int id);
        EntityModelBase GetDefault();
        List<EntityModelBase> GetAll(FilterExpression filter);
        List<EntityModelBase> GetAll(int[] ids);
        AnyStatus Save(EntityModelBase model);
        bool Write(int id, DynamicObj model);
        DBModelBase Read(int id, List<string> fields);
        List<DBModelBase> ReadAll(List<string> fields, FilterExpression filter);

        IDBEntity GetEntity(EntityCode id);
        
    }

    public interface IEntityRelation
    {
        EntityRelationType Type {get;}
        EntityCode ParentName {get;}
        EntityCode ChildName {get;}

        BaseField ParentRefField {get;}

        BaseField ChildRefField {get;}

        /*Item1: FIeldName, Item2: Child RefFieldName*/
        List<(string, string)> OtherChildFields {get;}
    }


}