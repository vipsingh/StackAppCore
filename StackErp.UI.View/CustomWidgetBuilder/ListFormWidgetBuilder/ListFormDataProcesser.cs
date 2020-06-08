using System;
using System.Collections;
using System.Collections.Generic;
using StackErp.Core;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.Model.Layout;
using StackErp.UI.View.PageGenerator;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.Model.ViewResponse;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.CustomWidgetBuilder
{
    public class ListFormDataProcesser
    {
        public static List<ViewPageDataOnly> GetData(StackAppContext appContext, RequestQueryString requestQuery)
        {
            var baseEntity = EntityMetaData.Get(requestQuery.EntityId);
            var baseField = baseEntity.GetFieldSchema(requestQuery.FieldName);
            var childEntity = EntityMetaData.Get(baseField.RefObject);

            var ids = FetchDataModel(requestQuery.ItemId, baseField, childEntity);
            var pages = new List<ViewPageDataOnly>();
                        
            var lConext = new LayoutContext(appContext, 0, baseField.RefObject);
            lConext.Build();

            foreach(var id in ids)
            {
                var query = new RequestQueryString(){ EntityId = baseField.RefObject, ItemId = id };

                var context = new EditFormContext(appContext, baseField.RefObject, query);
                context.Build();
                context.CreateDataModel();
                
                context.LayoutContext = lConext;

                var renderer = new EditFormRenderer(context);
                renderer.Generate(lConext);

                pages.Add(renderer.GetViewPageOnlyData());
            }

            return pages;
        }

        private static List<int> FetchDataModel(int parentId, BaseField parentField, IDBEntity entity)
        {
            var filterExp = new FilterExpression(entity.EntityId);
            var relField = (OneToManyField)parentField;
            filterExp.Add(new FilterExpField(relField.RefFieldName, FilterOperationType.Equal, parentId));
            var ids = entity.ReadIds(filterExp);

            return ids;
        }
    }
}