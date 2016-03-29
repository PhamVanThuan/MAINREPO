using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Web.Test.AJAX
{
    /// <summary>
    /// Class containing static helpers for AJAX method calls.
    /// </summary>
    public class AjaxHelpers
    {

        /// <summary>
        /// Converts a string value into a dictionary key/value pair that is expected by Ajax methods.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ConvertToDictionaryEntry(object key)
        {
            return ConvertToDictionaryEntry(key, "undefined");
        }

        /// <summary>
        /// Converts a string value into a dictionary key/value pair that is expected by Ajax methods.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertToDictionaryEntry(object key, string value)
        {
            return value + ":" + key.ToString() + ";";
        }

    }
}
