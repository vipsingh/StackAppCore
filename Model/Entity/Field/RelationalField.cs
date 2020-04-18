using System;
using System.Collections.Generic;

namespace StackErp.Model.Entity
{
    public class LinkField: BaseField {
        public LinkField(): base() {
            Type = FieldType.ObjectLink;
            BaseType = BaseTypeCode.Int32;
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
            if (value is Int32 )
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
            this.ControlInfo.DataSource = new PickerDataSource {
                Type = "ENTITY",
                Entity = this.RefObject,
                Fields = new List<string>(){this.Entity.TextField},
                IdField = this.Entity.IDField,
                SortOnField = "",
                Domain = this.Domain
            };
        }
    }

    public class SelectField: BaseField {
        public int LookupId {set;get;}
        public bool IsPickList {set;get;}
        public List<SelectOption> PickList {set;get;}
        public SelectField(): base() {
            Type = FieldType.Select;
            BaseType = BaseTypeCode.Int32;
        }

        public override void OnInit()
        {
            base.OnInit();
            ControlInfo.LookupId = this.LookupId;
        }

        public override object ResolveSetValue(object value, out bool isValid)
        {
            isValid = true;
            if (value is Int32 )
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
            var v = db.Get(this.DBName, 0);

            return v;
        }
    }
}