using System;
using System.Data;

namespace StackErp.DB
{
    public class DynamicDbParam
    {
        public string Name {get;}
        public object Value {get;}
        public DbType? Type {get;}
        public ParameterDirection Direction {get;}

        public DynamicDbParam(string name, object value, DbType? type = null, ParameterDirection direction = ParameterDirection.Input)
        {
            Name=  name;
            Value=value;
            Type= type;
            Direction = direction;
        }
    }
}

namespace Dapper
{
    public static class DapperExt
    {
        public static void AddParam(this DynamicParameters parameters, StackErp.DB.DynamicDbParam param)
        {
            if (param.Type.HasValue)
                parameters.Add(param.Name, param.Value, param.Type.Value, param.Direction);
            else
                parameters.Add(param.Name, param.Value);
        }
    }
}
