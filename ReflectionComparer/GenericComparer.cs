using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflectionComparer
{
    public class GenericComparer
    {
        public bool Comparer<T>(T leftObj, T rightObj)
        {
            var equalsResult = true;

            if (leftObj == null || rightObj == null)
            {
                throw new ArgumentNullException($"Argument was null.");
            }

            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var propertyInfo in properties)
            {
                if (!propertyInfo.CanRead)
                {
                    continue;
                }

                var valueLeftObj = propertyInfo.GetValue(leftObj, null);
                var valueRightObj = propertyInfo.GetValue(rightObj, null);

                if (IsAssignableFrom(propertyInfo.PropertyType)|| IsPrimitiveType(propertyInfo.PropertyType) || IsValueType(propertyInfo.PropertyType))
                {
                    if (!CompareValues(valueLeftObj, valueRightObj))
                    {
                        equalsResult = false;
                    }
                }
                else if (IsEnumerableType(propertyInfo.PropertyType))
                {
                    if (!CompareEnumerations(valueLeftObj, valueRightObj))
                    {
                        equalsResult = false;
                    }
                }
                else if (propertyInfo.PropertyType.IsClass)
                {
                    if (!Comparer(propertyInfo.GetValue(valueLeftObj,null),propertyInfo.GetValue(valueRightObj,null)))
                    {
                        equalsResult = false;
                    }
                }
                else
                {
                    equalsResult = false;
                }
            }

            return equalsResult;
        }

        private bool CompareEnumerations(object valueLeftObj, object valueRightObj)
        {
            if (valueLeftObj == null && valueRightObj != null || valueLeftObj != null && valueRightObj == null)
                return false;
            else if (valueLeftObj != null && valueRightObj != null)
            {
                var leftObjEnum = ((IEnumerable)valueLeftObj).Cast<object>();
                var rightObjEnum = ((IEnumerable)valueRightObj).Cast<object>();
                
                if (leftObjEnum.Count() != rightObjEnum.Count())
                    return false;
                else
                {
                    for (int itemIndex = 0; itemIndex < leftObjEnum.Count(); itemIndex++)
                    {
                        var leftObjEnumItem = leftObjEnum.ElementAt(itemIndex);
                        var rightObjItem = rightObjEnum.ElementAt(itemIndex);
                        var leftObjItemType = leftObjEnumItem.GetType();
                        if (IsAssignableFrom(leftObjItemType) || IsPrimitiveType(leftObjItemType) || IsValueType(leftObjItemType))
                        {
                            if (!CompareValues(leftObjEnumItem, rightObjItem))
                                return false;
                        }
                        else if (!Comparer(leftObjEnumItem, rightObjItem))
                            return false;
                    }
                }
            }
            return true;
        }

        private bool IsEnumerableType(Type propertyType)
        {
            return typeof(IEnumerable).IsAssignableFrom(propertyType);
        }

        private bool CompareValues(object valueLeftObj, object valueRightObj)
        {
            bool areValuesEqual = true;
            IComparable selfValueComparer = valueLeftObj as IComparable;
          
            if (valueLeftObj == null && valueRightObj != null || valueLeftObj != null && valueRightObj == null)
                areValuesEqual = false;
            else if (selfValueComparer != null && selfValueComparer.CompareTo(valueRightObj) != 0)
                areValuesEqual = false;
            else if (!object.Equals(valueLeftObj, valueRightObj))
                areValuesEqual = false;

            return areValuesEqual;

        }

        private bool IsValueType(Type propertyType)
        {
            return propertyType.IsValueType;
        }

        private bool IsPrimitiveType(Type propertyType)
        {
            return propertyType.IsPrimitive;
        }

        private bool IsAssignableFrom(Type propertyType)
        {
            return typeof(IComparable).IsAssignableFrom(propertyType);
        }
    }
}