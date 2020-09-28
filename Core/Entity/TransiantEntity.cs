using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using StackErp.Core.Form;
using StackErp.DB;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;

namespace StackErp.Core
{
    //No db store OR temporary store as json (auto deleted frequently)
    public class TransiantEntity : DBEntity
    {
        public bool IsShortTimeDbSave {private set; get;}
        public TransiantEntity(int id, string name, Dictionary<string, BaseField> fields): base(id,name,fields, EntityType.Transiant, new DbObject())
        {
            this.IsShortTimeDbSave = false;
        }

        public override AnyStatus Save(StackAppContext appContext, EntityModelBase model)
        {
            AnyStatus status = AnyStatus.NotInitialized;

            return status;
        }
    }
}