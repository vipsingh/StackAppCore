using System;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core.Rules
{
    public class RuleEvaluator
    {
        private EntityCode _EntityId;
        private EntityModelBase _DataModel;
        public RuleEvaluator(StackAppContext appContext, EntityCode entityId)
        {
            this._EntityId = entityId;
        }

        public RuleEvaluator(StackAppContext appContext, EntityModelBase model): this(appContext, model.EntityId)
        {
            this._DataModel = model;
        }

        public bool EvaluateExpression(FilterExpression expcriteria)
        {
            return true;
        }

        public bool EvaluateFieldExpression(FilterExpField exp, object leftVal = null) 
        {
            return true;
        }

        private bool GetRightValue(FilterExpField criteria, out object rVal, ref FieldType rFieldType)
        {
            bool isVaild = true;

            if (criteria.ValueType == FilterValueType.EntityField)
            {
                rVal = _DataModel.GetValue(criteria.FieldName);
            } 
            else
            {
                rVal = criteria.Value;
            }

            return isVaild;
        }

        public static int[] GetUserFieldValue(int LoginUserid, int CurrentRoleId, string fieldName, ref bool isKeyField)
        {
            int[] list = null;
            switch (fieldName.ToLower())
            {
                case "userroles":
                case "userproducts":
                case "secondaryproductid":
                case "primaryproductid":
                    isKeyField = true;
                    //var authorization = new Authorization();

                    if (fieldName.ToLower() == "userroles")
                    {
                        //list = authorization.GetUserRoleIds(OwnerID, LoginUserid);
                    }
                    else
                    {
                        //var productItemType = fieldName.ToLower() == "primaryproductid" ? ItemType.PrimaryProduct : fieldName.ToLower() == "secondaryproductid" ? ItemType.SecondaryProduct : ItemType.Product;
                        //list = authorization.GetUserProducts(OwnerID, LoginUserid, productItemType);
                    }

                    break;
                case "userteams":
                    isKeyField = true;
                    //list = new Team().GetUserTeams(OwnerID, LoginUserid);
                    break;
                case "userterritories":
                    //list = new Region().GetTerritoryIds(OwnerID, LoginUserid);
                    isKeyField = true;
                    break;
                case "userpincodes":
                    //list = new User().GetPinCodes(OwnerID, LoginUserid);
                    isKeyField = true;
                    break;
                case "currentrole":
                    isKeyField = true;
                    list = new int[] { CurrentRoleId };
                    break;


                //break;
            }

            return list;
        }
        
        public bool EvaluateFieldExpression(FilterExpField criteria, object lVal, object rVal, FieldType rFieldType = FieldType.None)
        {
            var field = criteria.Field;
            object[] inSet = null;
            if (lVal is DateTime || rVal is DateTime || (field != null && field.BaseType == TypeCode.DateTime))
            {
                //return DateCompare(criteria, lVal, rVal, rFieldType);
            }
            if (lVal is Boolean && rVal is string)
            {
                rVal = (bool)(rVal.ToString() == "1" || rVal.ToString().ToLower() == "true");
            }
            if (criteria.Op == FilterOperationType.IsSpecified || criteria.Op == FilterOperationType.NotSpecified)
            {
                //return GetSpecifiedNotSpecifiedResult(criteria.Operation, field, lVal);
            }
            int compResult = 0;

            if (rVal != null && criteria.Op == FilterOperationType.In || criteria.Op == FilterOperationType.NotIn)
            {
                inSet = FilterExpField.GetValueSet(rVal.ToString(), lVal.GetType());
            }
            else
            {
                rVal = Convert.ChangeType(rVal, lVal.GetType());

                if (rVal is string)
                {
                    compResult = ((IComparable)lVal.ToString().ToLower()).CompareTo(rVal.ToString().ToLower());
                }
                else
                    compResult = ((IComparable)lVal).CompareTo(rVal);
            }

             bool result = false;
            switch (criteria.Op)
            {
                case FilterOperationType.AnyOf:
                case FilterOperationType.In:
                    result = IsInSet(lVal, inSet);
                    break;
                case FilterOperationType.NotIn:
                    result = IsNotInSet(lVal, inSet);
                    break;
                case FilterOperationType.Equal:
                    result = compResult == 0;                    
                    break;
                case FilterOperationType.GreaterThan:
                    result = compResult > 0;
                    break;
                case FilterOperationType.GreaterThanEqual:
                    result = compResult >= 0;
                    break;
                case FilterOperationType.LessThan:
                    result = compResult < 0;
                    break;
                case FilterOperationType.LessThanEqual:
                    result = compResult <= 0;
                    break;
                case FilterOperationType.NotEqual:
                    result = compResult != 0;                    
                    break;
                case FilterOperationType.StartWith:
                    result = StartWith(lVal, rVal);
                    break;
                case FilterOperationType.Like:
                    result = IsLike(lVal, rVal);
                    break;
            }

            return result;
        }

        private static bool IsInSet(object val, object[] Set)
        {
            if (val is string)
            {
                for (int i = 0; i < Set.Length; i++)
                {
                    if (Set[i].ToString().Equals(val.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Set.Length; i++)
                {
                    if (Set[i].Equals(val))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsInSet(int[] lVal, object[] rVal)
        {
            if (rVal != null)
            {
                for (int i = 0; i < lVal.Length; i++)
                {
                    for (int j = 0; j < rVal.Length; j++)
                    {
                        if (lVal[i] == (int)rVal[j])
                            return true;
                    }
                }
            }

            return false;
        }

        private bool IsNotInSet(int[] lVal, object[] rVal)
        {
            if (rVal != null)
            {
                for (int i = 0; i < lVal.Length; i++)
                {
                    for (int j = 0; j < rVal.Length; j++)
                    {
                        if (lVal[i] == (int)rVal[j])
                            return false;
                    }
                }
            }

            return true;
        }

        private static bool IsNotInSet(object val, object[] Set)
        {
            bool result = false;
            if (val is string)
            {
                for (int i = 0; i < Set.Length; i++)
                {
                    if (Set[i].ToString().Equals(val.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Set.Length; i++)
                {
                    if (Set[i].Equals(val))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return !result;
        }

        private bool IsLike(object lVal, object rVal)
        {
            bool result = false;
            string rval = ((string)rVal).ToLower();
            string lval = ((string)lVal).ToLower();

            if (rval.Contains(","))
            {
                string[] rvals = rval.Split(',');
                for (int index = 0; index < rvals.Length; index++)
                {
                    if (lval.IndexOf(rvals[index].Trim(), StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
                result = lval.IndexOf(rval, StringComparison.OrdinalIgnoreCase) != -1;

            return result;
        }


        private bool StartWith(object lVal, object rVal)
        {
            bool result = false;
            string rval = ((string)rVal).ToLower();
            string lval = ((string)lVal).ToLower();

            if (rval.Contains(","))
            {
                string[] rvals = rval.Split(',');
                for (int index = 0; index < rvals.Length; index++)
                {
                    if (lval.StartsWith(rvals[index].Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
                result = lval.StartsWith(rval, StringComparison.OrdinalIgnoreCase);

            return result;
        }
        
    }
}
