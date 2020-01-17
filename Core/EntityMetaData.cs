using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.DB;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core
{
    public class EntityMetaData
    {
        private static IDictionary<int, DBEntity> entities;

        public static IDictionary<int, DBEntity> Build()
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
                        field.Init();
                        fields.Add(fname.ToUpper(), field);
                    }

                    var e = new DBEntity(entid, name, fields);
                    entities.Add(entid, e);
                }

                foreach (var ent in entities)
                {
                    ent.Value.Init();
                }
            }
            catch (Exception ex)
            {
                throw new EntityException("Error in building entities. " + ex.Message);
            }

            return entities;
        }

        private static BaseField BuildField(string entName, string table, DynamicObj sch, List<DynamicObj> dbentities)
        {            
            var typ = sch.Get("fieldtype", 0);
            var fname = sch.Get("FIELDNAME", "");

            var field = CreateField((FieldType)typ);

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
            field.ControlType = GetDefaultControl(field.Type);

            return field;
        }

        private static BaseField CreateField(FieldType type)
        {
            var field = new BaseField();
            switch(type)
            {
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
                    default:
                    field = new StringField();
                    break;
            }
            return field;
        }
        // public static DBEntity Get(string name)
        // {
        //     if (entities.Keys.Contains(name.ToUpper()))
        //         return entities[name.ToUpper()];

        //     throw new EntityException($"Requested Entity {name} not found.");
        // }

        public static DBEntity Get(EntityCode id)
        {
            if (entities.Keys.Contains(id.Code))
                return entities[id.Code];

            throw new EntityException($"Requested Entity {id.Code} # {id.Name} not found.");
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

            }

            return t;
        }
    }
}
