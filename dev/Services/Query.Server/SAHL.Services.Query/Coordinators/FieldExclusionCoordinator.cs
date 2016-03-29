using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Query.Coordinators
{
    public class FieldExclusionCoordinator<T>
    {

        public T MarkFieldsAsNull(T dataModel, IEnumerable<string> fields)
        {
            
            if (CanApplyNulls(fields))
            {
                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (ShouldApplyNullToField(propertyInfo.Name, fields))
                    {
                        propertyInfo.SetValue(dataModel, null);
                    }
                }
            }
            
            return dataModel;

        }

        public IEnumerable<T> MarkListItemsWithNull(IEnumerable<T> datalist, IEnumerable<string> fields)
        {

            if (CanApplyNulls(fields))
            {
                foreach (T dataModel in datalist)
                {
                    MarkFieldsAsNull(dataModel, fields);
                }
            }

            return datalist;

        }

        private bool ShouldApplyNullToField(string propertyName, IEnumerable<string> fields)
        {

            if (propertyName.ToLower() == "aliaselookup" || propertyName.ToLower() == "relationships" || propertyName.ToLower() == "id")
            {
                return false;
            }

            return !fields.Contains(propertyName);

        }

        private bool CanApplyNulls(IEnumerable<string> fields)
        {
            return fields.Count() > 0;
        }
    }
}