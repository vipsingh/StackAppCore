using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StackErp.Model.Entity
{
    public class SelectField: BaseField {
        public int LookupId {set;get;}
        public bool IsPickList {set;get;}
        public List<SelectOption> PickList {set;get;}
        public SelectField(): base() {
            Type = FieldType.Select;
            BaseType = TypeCode.Int32;
        }

        public override void OnInit()
        {
            base.OnInit();
            ControlInfo.LookupId = this.LookupId;
            ControlInfo.IsMultiSelect = this.IsMultiSelect;
        }

        public override object ResolveSetValue(object value, out bool isValid)
        {
            isValid = true;
            if (value is Int32 || value is List<Int32>)
                return value;
            else if (value is SelectOption)
            {
                return ((SelectOption)value).Get("Value", 0);
            }
            else if (value is List<SelectOption>)
            {
                return ((List<SelectOption>)value).Select(x => x.Get("Value", 0));
            }
            else
            {
                if (value is IEnumerable) {
                    List<int> data = new List<int>();
                    foreach(var o in (IEnumerable)value) {
                        int number;
                        if(int.TryParse(o.ToString(), out number))
                        {
                            data.Add(number);
                        }
                    }
                    return data;
                } else {
                    int number;
                    if(int.TryParse(value.ToString(), out number))
                    {
                        return number;
                    }
                }
                
                isValid = false;
                return value;
            }
        }

        public override object ResolveDbValue(DbObject db)
        {
            object v = null;
            if(this.IsMultiSelect) {
                var d = db.Get(this.DBName, "");
                if (!string.IsNullOrEmpty(d)) {
                   v = DataHelper.ConvertToIntList(d.Split(','));
                }
            } else {
                v = db.Get(this.DBName, 0);
            }

            return v;
        }
    }

    public class LinkField: SelectField {
        public LinkField(): base() {
            Type = FieldType.ObjectLink;
            BaseType = TypeCode.Int32;
        }
        public override void OnInit()
        {
            base.OnInit();
            ControlInfo.FieldAttribute.RefEntity = this.RefObject;
            BuildPickerDataSource();
        }        

        public override object ResolveDbValue(DbObject db)
        {
            //handle multiselect -> resolve data name
            var v = db.Get(this.DBName, 0);
            var t = db.Get<string>(this.DBName + "__name", null);

            if(v == 0)
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
            this.ControlInfo.DataSource = new PickerDataSource {
                Type = "ENTITY",
                Entity = this.RefObject,
                Fields = new List<string>(){refEntity.TextField},
                IdField = refEntity.IDField,
                SortOnField = "",
                Domain = this.Domain
            };
        }
    }    
}