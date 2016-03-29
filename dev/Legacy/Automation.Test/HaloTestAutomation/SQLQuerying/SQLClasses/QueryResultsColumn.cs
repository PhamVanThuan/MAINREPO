using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Automation.DataAccess
{
    public sealed class QueryResultsColumn
    {
        public QueryResultsColumn(string columnName, object value)
        {
            this.Name = columnName;
            if (value == DBNull.Value)
                this.Value = null;
            else
                this.Value = value.ToString();
        }
        public string Name { get; private set; }
        public string Value { get; private set; }

        /// <summary>
        /// Will get a value as the specified type (T) and return
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValueAs<T>()
        {
            try
            {
                if (this.Value == null)
                    return default(T);
                if (this.Value is string)
                {
                    if (this.Value == "")
                        return default(T);
                    else
                    {
                        var result = 0.0;
                        //if it is a double and want to convert to int
                        if (Double.TryParse(this.Value, out result) && typeof(T) == typeof(int))
                        {
                            var valSplit = this.Value.Split('.').FirstOrDefault();
                            return this.ChangeType<T>(valSplit);
                        }
                    }
                }
                return this.ChangeType<T>(this.Value);
            }
            catch
            {
                throw;
            }
        }

        #region Helpers
        private T ChangeType<T>(object value)
        {
            return (T)ChangeType(typeof(T), value);
        }
        private object ChangeType(Type t, object value)
        {
            if (t.IsEnum)
                return Enum.Parse(t, this.Value);
            return Convert.ChangeType(value, t);
        }
        #endregion Helpers
    }
}