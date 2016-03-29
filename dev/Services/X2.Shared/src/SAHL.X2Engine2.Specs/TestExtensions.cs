using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2.Specs
{
    public static class Extensions
    {
        //public static bool RouteComparer(this IX2Route route, IX2Route routeToCompare)
        //{
        //    return route.RouteEndpoint.ExchangeName == routeToCompare.RouteEndpoint.ExchangeName &&
        //        route.RouteEndpoint.QueueName == routeToCompare.RouteEndpoint.QueueName;
        //}

        public static bool CheckValue(this object objectToCheck, Dictionary<string, object> values)
        {
            foreach (var kvp in values)
            {
                var property = objectToCheck.GetType().GetProperty(kvp.Key);
                var value = property.GetValue(objectToCheck, null);

                var value1 = Convert.ChangeType(kvp.Value, property.PropertyType);
                var value2 = Convert.ChangeType(value, property.PropertyType);

                if (value1.GetType().IsValueType && value2.GetType().IsValueType)
                {
                    if (value1.ToString() != value2.ToString())
                        return false;
                }
                else
                {
                    if (value1 != value2)
                        return false;
                }
            }
            return true;
        }

        public static bool AreEqual(this string[] sourceList, string[] listToCompareTo)
        {
            return sourceList.Except(listToCompareTo).Count() == 0;
        }
    }
}