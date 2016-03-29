using System;
using System.Collections.Generic;
using System.Linq;

namespace Automation.DataModels
{
    internal static class Helpers
    {
        internal static string GetDelimitedString<InputType>(InputType[] inputlist, string delimiter)
        {
            Converter<InputType, string> converter = new Converter<InputType, string>(Converter);
            string[] stringArray = Array.ConvertAll<InputType, string>(inputlist, converter);
            return String.Join(delimiter, stringArray);
        }

        private static string Converter<T>(T parameter)
        {
            return parameter.ToString();
        }

        /// <summary>
        ///   Format the input string for use in a sql query statement.  Enclose the string in single quotes or set the return string
        ///   to "null" if the string is null or blank
        /// </summary>
        /// <param name = "InputSting"></param>
        /// <returns></returns>
        internal static string FormatStringForSQL(string InputSting)
        {
            string ReturnString;

            if (!string.IsNullOrEmpty(InputSting) && InputSting.ToLower() != "null")
            {
                ReturnString = @"'" + InputSting + @"'";
            }
            else
            {
                ReturnString = "null";
            }
            return ReturnString;
        }

        /// <summary>
        /// Sets the values of the properties of the outInstance param to the values of the properties in the inInstance param.
        /// </summary>
        /// <typeparam name="outT"></typeparam>
        /// <typeparam name="intT"></typeparam>
        /// <param name="outInstance"></param>
        /// <param name="inInstance"></param>
        internal static void SetProperties<outT, intT>(ref outT outInstance, intT inInstance)
        {
            //input instance properties
            var inputInstanceProperties = new Dictionary<string, Type>();
            var outputInstanceProperties = new Dictionary<string, Type>();

            var propertyNames = new List<string>();

            //get all the proeprty names inInstance
            foreach (var prop in inInstance.GetType().GetProperties())
                propertyNames.Add(prop.Name);
            //get all the proeprty names inInstance
            foreach (var prop in outInstance.GetType().GetProperties())
                propertyNames.Add(prop.Name);

            foreach (var name in propertyNames)
            {
                var propToGet = (from p in inInstance.GetType().GetProperties()
                                 where p.Name == name
                                 select p).FirstOrDefault();
                var propToSet = (from p in outInstance.GetType().GetProperties()
                                 where p.Name == name
                                 select p).FirstOrDefault();
                if (propToSet != null && propToGet != null)
                {
                    var value = propToGet.GetValue(inInstance, null);
                    propToSet.SetValue(outInstance, value, null);
                }
            }
        }

        internal static object IfZeroReturnNULL(int value)
        {
            if (value == 0)
                return "NULL";
            else
                return value;
        }
    }
}