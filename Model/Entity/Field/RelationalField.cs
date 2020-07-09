using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StackErp.Model.Entity
{
    public abstract class RelationalField : BaseField
    {
        public bool IsLazyLoad { set; get; }
        public override void OnInit()
        {
            base.OnInit();
            ControlInfo.FieldAttribute.RefEntity = this.RefObject;
        }
    }

    public class ObjectLinkField : RelationalField
    {
        public bool IsPickList { set; get; }
        public ObjectLinkField() : base()
        {
            Type = FieldType.ObjectLink;
            BaseType = TypeCode.Int32;
        }
        public override void OnInit()
        {
            base.OnInit();
            ControlInfo.FieldAttribute.RefEntity = this.RefObject;
            BuildPickerDataSource();
        }

        public override object ResolveSetValue(object value, out bool isValid)
        {
            isValid = true;
            if (value == null) return null;

            if (value is Int32)
                return value;
            else if (value is SelectOption)
            {
                return ((SelectOption)value).Get("Value", 0);
            }
            else
            {

                int number;
                if (int.TryParse(value.ToString(), out number))
                {
                    return number;
                }


                isValid = false;
                return value;
            }
        }

        public override object ResolveDbValue(DbObject db)
        {
            var v = db.Get(this.DBName, 0);
            var t = db.Get<string>(this.DBName + "__name", null);

            if (v == 0)
                return null;
            else
            {
                var option = new SelectOption();
                option.Add("Value", v);
                option.Add("Text", t);

                return option;
            }
        }

        private void BuildPickerDataSource()
          {
            var refEntity = this.Entity.GetEntity(this.RefObject);
            var filterDomain = this._TempSchemaData.Get("linkentity_domain", "");
            this.ControlInfo.DataSource = new FieldDataSource
            {
                Type = DataSourceType.Entity,
                Entity = this.RefObject,
                Domain = FilterExpression.BuildFromJson(this.RefObject, filterDomain)
            };
        }
    }

    public class MultiObjectLinkField : ObjectLinkField
    {
        public MultiObjectLinkField() : base()
        {
            Type = FieldType.MultiObjectLink;
            BaseType = TypeCode.Int32;
            IsArrayData = true;
        }
        public override void OnInit()
        {
            base.OnInit();
            ControlInfo.IsMultiSelect = true;
        }

        public override object ResolveSetValue(object value, out bool isValid)
        {
            isValid = true;
            if (value == null) return null;

            if (value is List<Int32>)
                return value;
            else if (value is List<SelectOption>)
            {
                return ((List<SelectOption>)value).Select(x => x.Value);
            }
            else
            {
                if (value is IEnumerable<int> || value is IEnumerable<string>)
                {
                    List<int> data = new List<int>();
                    foreach (var o in (IEnumerable)value)
                    {
                        int number;
                        if (int.TryParse(o.ToString(), out number))
                        {
                            data.Add(number);
                        }
                    }
                    return data;
                }

                isValid = false;
                return value;
            }
        }

        public override object ResolveDbValue(DbObject db)
        {
            List<SelectOption> v = null;
            var d = db.Get<int[]>(this.DBName, null);
            var reldata = db.Get<string[]>(this.DBName + "__data", null);
            if (d != null)
            {
                v = new List<SelectOption>();
                for(int i=0;i<d.Length;i++)
                {
                    var option = new SelectOption();
                    option.Add("Value", d[i]);
                    option.Add("Text", reldata[i]);
                    v.Add(option);
                }
            }
            // var relData = db.Get<IEnumerable<DbObject>>(this.DBName + "__data", null);

            // if (relData != null)
            // {
            //     v = new List<SelectOption>();
            //     foreach(DbObject obj in relData)
            //     {
            //         var option = new SelectOption();
            //         option.Add("Value", obj.Get("ID", 0));
            //         option.Add("Text", obj.Get("NAME", ""));
            //         v.Add(option);
            //     }
            // }

            return v;
        }
    }
}