using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Core.Entity;
using StackErp.DB;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core
{
    public partial class EntityMetaData
    {
        
        internal static BaseField BuildField(string entName, string table, DbObject sch, List<DbObject> dbentities)
        {
            var fieldId = sch.Get("id", 0);
            var typ = sch.Get("fieldtype", 0);
            var fname = sch.Get("FIELDNAME", "");
            var dbname = sch.Get("dbname", "");

            var field = CreateField((FieldType)typ);

            field.FieldId = fieldId;
            field.Name = fname;
            field.Type = (FieldType)typ;
            field.DBName = dbname;
            field.ViewId = sch.Get<short>("viewtype", 0);
            field.IsDbStore = !String.IsNullOrEmpty(dbname);
            field.TableName = table;
            field._TempSchemaData = new DynamicObj();
            field.DefaultValue = sch.Get("defaultvalue", String.Empty);
            field.ViewOrder = sch.Get("vieworder", 10000);

            field.IsRequired = sch.Get("isrequired", false);
            

            var computeexpression = sch.Get("computeexpression", "");
            if (!string.IsNullOrEmpty(computeexpression))
            {
                field.IsComputed = true;
                field.ComputeExpression = new Model.Form.EvalExpression(computeexpression);
            }

            var relatedexp = sch.Get("relatedexp", String.Empty);
            if (!string.IsNullOrEmpty(relatedexp))
            {
                field.Related = new Model.Form.FieldPathExpression(relatedexp);
            }
            var linkEnt = sch.Get("linkentity", 0);

            if (field.Type == FieldType.ObjectLink || field.Type == FieldType.MultiObjectLink)
            {                
                var linkDbEnt = dbentities.Where(x => x.Get("id", 0) == linkEnt);
                if (linkDbEnt.Count() > 0)
                {
                    field.RefObject = linkEnt;
                }

                field._TempSchemaData.Add("linkentity_domain", sch.Get("linkentity_domain", ""));                             
            }            

            if (field is SelectField)
            {
                var collectionid = sch.Get("collectionid", 0);
                ((SelectField)field).CollectionId = collectionid;
            }


            field.ControlType = GetDefaultControl(field.Type);

            if (field is OneToManyField)
            {
                var rField = (OneToManyField)field;
                var linkDbEnt = dbentities.Where(x => x.Get("id", 0) == linkEnt);
                if (linkDbEnt.Count() > 0)
                {
                    rField.RefObject = linkEnt;
                }

                var linkentity_field = sch.Get("linkentity_field", "");
                rField.SetRelationShipInfo(rField.RefObject, linkentity_field);
            }

            return field;
        }

        internal static BaseField CreateField(FieldType type)
        {
            BaseField field = null;
            switch (type)
            {
                case FieldType.ObjectKey:
                    field = new ObjectKeyField();
                    break;
                case FieldType.BigInt:
                    field = new BigIntField();
                    break;
                case FieldType.Bool:
                    field = new BoolField();
                    break;
                case FieldType.Date:
                    field = new DateField();
                    break;
                case FieldType.DateTime:
                    field = new DateTimeField();
                    break;
                case FieldType.Decimal:
                    field = new DecimalField();
                    break;
                case FieldType.Image:
                    field = new ImageField();
                    break;
                case FieldType.Integer:
                    field = new IntegerField();
                    break;
                case FieldType.LongText:
                    field = new LongTextField();
                    break;
                case FieldType.Password:
                    field = new PasswordField();
                    break;
                case FieldType.MonataryAmount:
                    field = new DecimalField();
                    break;
                case FieldType.Html:
                    field = new HtmlField();
                    break;
                case FieldType.Email:
                    field = new EmailField();
                    break;
                case FieldType.Phone:
                    field = new PhoneField();
                    break;
                case FieldType.ObjectLink:
                    field = new ObjectLinkField();
                    break;
                case FieldType.MultiObjectLink:
                    field = new MultiObjectLinkField();
                    break;
                case FieldType.Select:
                    field = new SelectField();
                    break;
                case FieldType.MultiSelect:
                    field = new MultiSelectField();
                    break;
                case FieldType.FilterField:
                    field = new FilterField();
                    break;
                case FieldType.OneToMany:
                    field = new OneToManyField();
                    break;
                default:
                    field = new StringField();
                    break;
            }
            return field;
        }

        private static FormControlType GetDefaultControl(FieldType type)
        {
            FormControlType t = FormControlType.TextBox;
            switch (type)
            {
                case FieldType.BigInt:
                    t = FormControlType.NumberBox;
                    break;
                case FieldType.Bool:
                    t = FormControlType.CheckBox;
                    break;
                case FieldType.Date:
                    t = FormControlType.DatePicker;
                    break;
                case FieldType.DateTime:
                    t = FormControlType.DateTimePicker;
                    break;
                case FieldType.Decimal:
                    t = FormControlType.DecimalBox;
                    break;
                case FieldType.Integer:
                    t = FormControlType.NumberBox;
                    break;
                case FieldType.LongText:
                    t = FormControlType.LongText;
                    break;
                case FieldType.Password:
                    t = FormControlType.Password;
                    break;
                case FieldType.ObjectLink:
                case FieldType.MultiObjectLink:
                    t = FormControlType.EntityPicker;
                    break;
                case FieldType.MonataryAmount:
                    t = FormControlType.DecimalBox;
                    break;
                case FieldType.Select:
                case FieldType.MultiSelect:
                    t = FormControlType.Dropdown;
                    break;
                case FieldType.Image:
                    t = FormControlType.Image;
                    break;
                case FieldType.Html:
                    t = FormControlType.HtmlText;
                    break;
                case FieldType.Email:
                    t = FormControlType.Email;
                    break;
                case FieldType.Phone:
                    t = FormControlType.Phone;
                    break;
                case FieldType.FilterField:
                    t = FormControlType.EntityFilter;
                    break;
                case FieldType.OneToMany:
                    t = FormControlType.ListForm;
                    break;

            }

            return t;
        }

    }
}