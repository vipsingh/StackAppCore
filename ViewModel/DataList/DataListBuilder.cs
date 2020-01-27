using System;
using StackErp.Core.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.DataList
{
    public class DataListBuilder
    {
        public AnyStatus Status = AnyStatus.NotInitialized;
        protected virtual DataListDefinition GetSourceDefinition(DataListContext context)
        {
            return null;
        }

        public virtual DataListResponse GetResponse(DataListContext context)
        {
            var resp = new DataListResponse()
            {
                Fields = context.Fields
            };
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
            //build header

        }

        public virtual void OnBuildInit(DataListContext context, DataListDefinition defn)
        {
            context.BuildSource(defn);
        }

        protected virtual void ExecutePrepareData(DataListContext context, DataListDefinition defn)
        {

        }
    }
}
