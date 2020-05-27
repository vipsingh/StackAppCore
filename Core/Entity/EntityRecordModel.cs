using System;
using System.IO;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core
{
    public class EntityRecordModel: EntityModelBase 
    {
        public EntityRecordModel(IDBEntity entity): base(entity) {
            
        }

        internal void ResolveComputedFields()
        {
            var executer =  new StackScript.StackScriptExecuter();
            foreach(var cf in this.Entity.ComputeOrderSeq)
            {
                ComputeFieldData(executer, this.Entity.GetFieldSchema(cf));
            }
        } 

        private void ComputeFieldData(StackScript.StackScriptExecuter executer, BaseField field)
        {
            var exp = field.ComputeExpression;
            var o = executer.ExecuteExpression(exp.Exp, this);
            this.SetValue(field.Name, o);
        }

        internal void PrepareSaveImageField(StackAppContext appContext)
        {
            var dmsPath = appContext.ImageStorePath;            

            foreach (var f in this.Attributes)
            {               
                var val = f.Value;
                if (val.Field.Type == FieldType.Image && val.IsChanged)
                {
                    var value ="";
                    var obj =  (DynamicObj)val.Value;
                    if (obj.Get("IsTemp", false))
                    {
                        var tempFile = obj.Get("FileName", "");
                        var ext = "png";
                        var destFile = $"docimage_{this.EntityId.Code.ToString()}_{this.ID.ToString()}_{val.Field.Name}.{ext}";
                        var destFilePath = Path.Combine(dmsPath, "store_" + appContext.MasterId, destFile);
                        File.Copy(Path.Combine(dmsPath, "temp", tempFile), destFilePath, true);
                        value = destFile;
                    }
                    f.Value.SetValue(value);
                }                
            }
        }
    }
}