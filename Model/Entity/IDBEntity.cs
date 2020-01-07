using System;
using System.Collections.Generic;
using StackErp.Model.DataList;

namespace StackErp.Model.Entity
{
    public interface IDBEntity
    {
        string Name {get;}
        string DBName {get;}
        Dictionary<string, BaseField> Fields {get;}
        
        string TextField {get; set;}
         string IDField {get;}

        List<IEntityRelation> Relations {get;}

        Dictionary<string, BaseField> GetFields();
        BaseField GetFieldSchema(string fieldName);
        EntityModelBase GetSingle(int id);
        EntityModelBase GetAll(FilterExpression filter);
        EntityModelBase GetAll(int[] ids);
        bool Save(EntityModelBase model);
        bool Write(int id, DynamicObj model);
        DBModelBase Read(int id, List<string> fields);
        List<DBModelBase> ReadAll(List<string> fields, FilterExpression filter);

        IDBEntity GetEntity(string name);
        
    }

    public interface IEntityRelation
    {
        EntityRelationType Type {get;}
        string ParentName {get;}
        string ChildName {get;}

        BaseField ParentRefField {get;}

        BaseField ChildRefField {get;}

        /*Item1: FIeldName, Item2: Child RefFieldName*/
        List<(string, string)> OtherChildFields {get;}
    }


}