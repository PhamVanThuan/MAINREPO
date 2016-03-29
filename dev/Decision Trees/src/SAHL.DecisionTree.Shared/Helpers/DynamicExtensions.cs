using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace SAHL.DecisionTree.Shared.Helpers
{
    public static class DynamicExtensions
    {
        public static dynamic ToDynamic(this object value)
        {
            if (value.GetType().Equals(typeof(ExpandoObject)))
            {
                return value;
            }
            IDictionary<string, object> expando = new ExpandoObject();

            Type type = value.GetType();
            foreach (var fieldInfo in type.GetFields())
                expando.Add(fieldInfo.Name, fieldInfo.GetValue(value));

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }

        public static T AssignDynamicValuesToObjectProperties<T>(T destination, dynamic valueVariables)
        {
            Type resultType = typeof(T);
            var valueDictionary = DynamicExtensions.ToDynamic(valueVariables) as IDictionary<string, object>;
            foreach (var propInfo in resultType.GetProperties())
            {
                if (propInfo.Name.Equals("GlobalsVersionResultIsBasedOn", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                var value = valueDictionary[propInfo.Name];
                propInfo.SetValue(destination, value, null);
            }

            return destination;
        }
    }
}
