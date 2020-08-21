using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StackErp.Model.Entity
{
    public class SelectField: BaseField {
        public int CollectionId {set;get;}
        public bool IsPickList {set;get;}
        public List<SelectOption> PickList {set;get;}
        public SelectField(): base() {
            Type = FieldType.Select;
            BaseType = TypeCode.Int32;
        }

        public override void OnInit()
        {
            base.OnInit();
            ControlInfo.CollectionId = this.CollectionId;
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
                if(int.TryParse(value.ToString(), out number))
                {
                    return number;
                }
                
                isValid = false;
                return value;
            }
        }

        public override object ResolveDbValue(DbObject db)
        {
            var v = db.Get(this.Name, 0);
            var t = db.Get<string>(this.Name + "__name", null);

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
    }

    public class MultiSelectField: SelectField 
    {
        public MultiSelectField(): base() {
            Type = FieldType.MultiSelect;
            BaseType = TypeCode.Int32;
            IsArrayData = true;
        }

        public override void OnInit()
        {
            base.OnInit();
            ControlInfo.CollectionId = this.CollectionId;
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
                return ((List<SelectOption>)value).Select(x => x.Get("Value", 0));
            }
            else
            {
                List<int> data = new List<int>();
                foreach(var o in (IEnumerable)value) 
                {
                    int number;
                    if(int.TryParse(o.ToString(), out number))
                    {
                        data.Add(number);
                    }
                    else 
                    {
                        isValid = false;
                        break;
                    }
                }

                return data;
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

            return v;
        }
    }
}