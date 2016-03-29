using System;
using System.Collections.Generic;
using System.Reflection;

namespace SAHL.Core.Testing
{
    public class GenericEqualityComparer<T> : IEqualityComparer<T> where T : class
    {
        public bool Equals(T x, T y)
        {
            Type type = typeof(T);
            if (x == null || y == null)
            {
                return false;
            }

            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.Name == "ExtensionData")
                {
                    continue;
                }
                string xValue = string.Empty;
                string yValue = string.Empty;
                if (type.GetProperty(property.Name).GetValue(x, null) != null)
                {
                    xValue = type.GetProperty(property.Name).GetValue(x, null).ToString();
                }
                if (type.GetProperty(property.Name).GetValue(y, null) != null)
                {
                    yValue = type.GetProperty(property.Name).GetValue(y, null).ToString();
                }
                if (xValue.Trim() != yValue.Trim())
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}