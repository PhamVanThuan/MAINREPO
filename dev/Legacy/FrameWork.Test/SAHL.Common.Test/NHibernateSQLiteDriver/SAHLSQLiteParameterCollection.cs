using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SAHL.Common.Test.NHibernateSQLiteDriver
{
    public class SAHLSQLiteParameterCollection : DbParameterCollection
    {
        private readonly SQLiteParameterCollection parameters;
        private readonly List<SAHLSQLiteParameter> thisParameters = new List<SAHLSQLiteParameter>();

        public SAHLSQLiteParameterCollection(SQLiteParameterCollection parameters)
        {
            this.parameters = parameters;
        }

        private SAHLSQLiteParameter Cast(object value)
        {
            return (SAHLSQLiteParameter)value;
        }

        public override int Add(object value)
        {
            thisParameters.Add(Cast(value));
            return parameters.Add(GetInner(value));
        }

        public override bool Contains(object value)
        {
            return thisParameters.Contains(Cast(value));
        }

        public override void Clear()
        {
            thisParameters.Clear();
            parameters.Clear();
        }

        public override int IndexOf(object value)
        {
            return thisParameters.IndexOf(Cast(value));
        }

        public override void Insert(int index, object value)
        {
            parameters.Insert(index, value);
            thisParameters.Insert(index, Cast(value));
        }

        public override void Remove(object value)
        {
            parameters.Remove(value);
            thisParameters.Remove(Cast(value));
        }

        public override void RemoveAt(int index)
        {
            parameters.RemoveAt(index);
            thisParameters.RemoveAt(index);
        }

        public override void RemoveAt(string parameterName)
        {
            foreach (var parameter in thisParameters)
            {
                if (parameter.ParameterName == parameterName)
                {
                    thisParameters.Remove(parameter);
                }
            }
            parameters.RemoveAt(parameterName);
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            parameters[index] = Cast(value).InnerParameter;
            thisParameters[index] = Cast(value);
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            var parameter = Cast(value);
            for (int i = 0; i < thisParameters.Count; i++)
            {
                if (thisParameters[i].ParameterName == parameterName)
                {
                    thisParameters[i] = parameter;
                    parameters[i] = parameter.InnerParameter;
                    return;
                }
            }

            thisParameters.Add(parameter);
            parameters.Add(parameter.InnerParameter);
        }

        public override int Count
        {
            get { return parameters.Count; }
        }

        public override object SyncRoot
        {
            get { return parameters.SyncRoot; }
        }

        public override bool IsFixedSize
        {
            get { return parameters.IsFixedSize; }
        }

        public override bool IsReadOnly
        {
            get { return parameters.IsReadOnly; }
        }

        public override bool IsSynchronized
        {
            get { return parameters.IsSynchronized; }
        }

        public override int IndexOf(string parameterName)
        {
            return parameters.IndexOf(parameterName);
        }

        public override IEnumerator GetEnumerator()
        {
            return thisParameters.GetEnumerator();
        }

        protected override DbParameter GetParameter(int index)
        {
            return thisParameters[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            foreach (var parameter in thisParameters)
            {
                if (parameter.ParameterName == parameterName)
                    return parameter;
            }
            return null;
        }

        public override bool Contains(string value)
        {
            return GetParameter(value) != null;
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override void AddRange(Array values)
        {
            foreach (var value in values)
            {
                Add(value);
            }
        }

        private SQLiteParameter GetInner(object value)
        {
            return ((SAHLSQLiteParameter)value).InnerParameter;
        }
    }
}