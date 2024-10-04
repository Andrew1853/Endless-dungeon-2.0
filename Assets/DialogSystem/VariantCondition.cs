using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DialogSystem
{
    public enum ComparisonType
    {
        greaterThan,
        lessThan,
        greaterThanOrEqual,
        lessThanOrEqual,
        equal,
        notEqual
    }
    [Serializable]
    public class VariantCondition
    {
        public IComparer comparer;
        public string variableToComparisonA;
        public string variableToComparisonB;

        public ValueToCompare fixedValueToCompare;
        public ComparisonType comparisonType;
        public bool Try(IDialogContext context)
        {
            var variableA = context.GetValue(variableToComparisonA);
            var variableB = context.GetValue(variableToComparisonB);

            var fxd = fixedValueToCompare.Get();
            if (fxd != null)
            {
                variableB = fxd;
            }
            if (variableA.GetType() == typeof(bool) && variableB.GetType() == typeof(bool))
            {
                if (comparisonType == ComparisonType.equal)
                {
                    return variableA.Equals(variableB);
                }
                else if(comparisonType == ComparisonType.notEqual)
                {
                    return !variableA.Equals(variableB);
                }
                else
                {
                    Debug.Log("Wrong comparison type");
                }
            }
            if (variableA.GetType() == typeof(float) && variableB.GetType() == typeof(float))
            {
                switch (comparisonType)
                {
                    case ComparisonType.greaterThan:
                        return (float)variableA > (float)variableB;
                    case ComparisonType.lessThan:
                        return (float)variableA < (float)variableB;
                    case ComparisonType.greaterThanOrEqual:
                        return (float)variableA >= (float)variableB;
                    case ComparisonType.lessThanOrEqual:
                        return (float)variableA <= (float)variableB;
                    case ComparisonType.equal:
                        return (float)variableA == (float)variableB;
                    case ComparisonType.notEqual:
                        return (float)variableA != (float)variableB;
                }
            }
            if (variableA.GetType() == typeof(string) && variableB.GetType() == typeof(string))
            {
                if (comparisonType == ComparisonType.equal)
                {
                    return variableA.Equals(variableB);
                }
                else if (comparisonType == ComparisonType.notEqual)
                {
                    return !variableA.Equals(variableB);
                }
                else
                {
                    Debug.Log("Wrong comparison type");
                }
            }
            return false;
        }
    }
    public enum ValueType
    {
        None = -1,
        Bool,
        Float,
        String
    }
    [Serializable]
    public class ValueToCompare
    {
        public ValueType valueType;
        public bool boolV;
        public float floatV;
        public string stringV;
        public object Get()
        {
            switch (valueType)
            {
                case ValueType.Bool:
                    return boolV;
                case ValueType.Float:
                    return floatV;
                case ValueType.String:
                    return stringV;
                default:
                    return null;
            }
        }
    }
}