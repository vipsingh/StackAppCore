using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Newtonsoft.Json;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.Model.Layout;

namespace StackErp.DB.DataList
{
    public class ListDbService
    {
        public static EntityListDefinition GetEntityList(EntityCode entityId)
        {
            var entityList = DBService.Query("select * from t_entitylist where entityid=@entityid", new { entityid = entityId.Code });
            
            if (entityList.Count() > 0)
            {
                var l = new EntityListDefinition();
                var db = entityList.First();
                l.EntityId = db.Get("entityid", 0);
                l.Name = db.Get("name", "");
                l.ItemIdField = db.Get("idfield", "");
                l.ItemViewField = db.Get("viewfield", "");
                l.PageSize = db.Get("recordlimit", 0);

                var orderby = db.Get("orderby", "");
                if(!string.IsNullOrEmpty(orderby))
                {
                    l.OrderByField = orderby.Split(',').ToList();
                }

                var additional = db.Get("additional", "");
                if(!string.IsNullOrEmpty(additional))
                {
                    l.AdditionalFields = additional.Split(',').ToList();
                }

                var layoutxml = db.Get("layoutxml", "");
                if (!string.IsNullOrEmpty(layoutxml))
                {
                    var tlist = TList.ParseFromXml(layoutxml);
                    l.Layout = tlist;
                }
                
                l.DataSource = new FieldDataSource() 
                {
                    Type = DataSourceType.Entity,
                    Entity = l.EntityId
                };

                var filterpolicy = db.Get("filterpolicy", "");
                if(!string.IsNullOrEmpty(filterpolicy))
                {
                    l.FilterPolicy = FilterExpression.BuildFromJson(l.EntityId, filterpolicy);
                }

                return l;
            }

            return null;
        }
    }
}