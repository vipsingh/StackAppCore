using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Core.Entity;
using StackErp.DB;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core
{
    public class EntityMetaData
    {
        public static IDictionary<int, DBEntity> entities;

        public static void Build()
        {
            entities = new Dictionary<int, DBEntity>();
            EntityCode.AllEntities = new Dictionary<string, int>();

            try
            {
                var dbentities = DB.EntityDBService.GetEntities();
                var dbentitiesScbhemas = DB.EntityDBService.GetEntitySchemas();
                List<string> avlEntitiesName = new List<string>();
                foreach (var ent in dbentities)
                {
                    var entid = ent.Get("id", 0);
                    var name = ent.Get("name", "");
                    EntityCode.AllEntities.Add(name.ToUpper(), entid);

                    if (avlEntitiesName.Contains(name.ToUpper()))
                        throw new AppException($"Entity with same name {name} found in system");

                    avlEntitiesName.Add(name.ToUpper());

                    var table = ent.Get("tablename", "");
                    Dictionary<string, BaseField> fields = new Dictionary<string, BaseField>();
                    var schemas = dbentitiesScbhemas.Where(x => x.Get("entityid", 0) == entid);
                    foreach (var sch in schemas)
                    {
                        var fname = sch.Get("FIELDNAME", "");

                        if (fields.Keys.Contains(fname.ToUpper()))
                            throw new AppException($"Field with same name <{fname}> in entity <{name}> found in system.");

                        var field = BuildField(name, table, sch, dbentities);
                        fields.Add(fname.ToUpper(), field);
                    }

                    entities.Add(entid, GetDBEntity(entid, name, fields));
                }

                Metadata.FixedEntities.BuildSchema(ref entities);

                foreach (var entK in entities)
                {
                    var ent = entK.Value;
                    ent.Init();
                    foreach (var fieldK in ent.Fields)
                    {
                        fieldK.Value.Init();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new EntityException("Error in building entities. " + ex.Message);
            }

        }

        internal static BaseField BuildField(string entName, string table, DbObject sch, List<DbObject> dbentities)
        {
            var fieldId = sch.Get("id", 0);
            var typ = sch.Get("fieldtype", 0);
            var fname = sch.Get("FIELDNAME", "");

            var field = CreateField((FieldType)typ);

            field.FieldId = fieldId;
            field.Name = fname;
            field.Type = (FieldType)typ;
            field.DBName = fname;
            field.IsDbStore = true;
            field.TableName = table;

            if (field.Type == FieldType.ObjectLink)
            {
                var linkEnt = sch.Get("linkentity", 0);
                var linkDbEnt = dbentities.Where(x => x.Get("id", 0) == linkEnt);
                if (linkDbEnt.Count() > 0)
                {
                    field.RefObject = linkEnt;
                }

                //field.DBName = fname + "__id";
            }
            field.IsRequired = sch.Get("isrequired", false);

            if (field is SelectField)
            {
                var lookupid = sch.Get("lookupid", 0);
                ((SelectField)field).LookupId = lookupid;
            }      
            field.IsMultiSelect = sch.Get("ismultiselect", false);

            field.ControlType = GetDefaultControl(field.Type);

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
                case FieldType.MonataryAmount:
                    field = new DecimalField();
                    break;
                case FieldType.ObjectLink:
                    field = new LinkField();
                    break;
                case FieldType.Select:
                    field = new SelectField();
                    break;
                case FieldType.FilterField:
                    field = new FilterField();
                    break;
                default:
                    field = new StringField();
                    break;
            }
            return field;
        }

        public static T GetAs<T>(EntityCode id) where T: DBEntity
        {
            if (entities.Keys.Contains(id.Code))
                return (T)entities[id.Code];

            throw new EntityException($"Requested Entity {id.Code} # {id.Name} not found.");
        }

        public static DBEntity Get(EntityCode id)
        {
            return GetAs<DBEntity>(id);
        }

        static FormControlType GetDefaultControl(FieldType type)
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
                case FieldType.ObjectLink:
                    t = FormControlType.EntityPicker;
                    break;
                case FieldType.MonataryAmount:
                    t = FormControlType.DecimalBox;
                    break;
                case FieldType.Select:
                    t = FormControlType.Dropdown;
                    break;
                case FieldType.Image:
                    t = FormControlType.Image;
                    break;
                case FieldType.FilterField:
                    t = FormControlType.EntityFilter;
                    break;

            }

            return t;
        }

        private static DBEntity GetDBEntity(int entid, string name, Dictionary<string, BaseField> fields)
        {
            DBEntity e;
            if (entid == 1) {
                e = new UserDbEntity(entid, name, fields);
            } else {
                e = new DBEntity(entid, name, fields);
            }

            return e;
        }
    }
}
