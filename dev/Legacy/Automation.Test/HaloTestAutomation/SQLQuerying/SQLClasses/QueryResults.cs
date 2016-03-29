using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Automation.DataAccess
{
    public sealed class QueryResults : IQueryable<Automation.DataAccess.QueryResultsRow>
    {
        #region InternalMethods

        //internal void Fill(SqlDataReader tableReader)
        //{
        //    this.RowList = new List<QueryResultsRow>();
        //    this.HasResults = tableReader.HasRows;
        //    //Read through every row in the table
        //    while (tableReader.Read())
        //    {
        //        //Instantiate a new row to use.
        //        var row = new QueryResultsRow();
        //        for (int ColumnIndex = 0; ColumnIndex < tableReader.FieldCount; ColumnIndex++)
        //        {
        //            //Instantiate a new column.
        //            var newColumn = new QueryResultsColumn
        //                (
        //                    tableReader.GetName(ColumnIndex),
        //                    tableReader.GetValue(ColumnIndex)
        //                );
        //            row.AddColumn(newColumn);
        //        }

        //        //Add the new row.
        //        this.RowList.Add(row);
        //    }
        //    //this.RowList.Count = this.RowList.Count;
        //    GC.KeepAlive(this.RowList);
        //}

        //internal void Fill(object sqlScalarValue)
        //{
        //    if (sqlScalarValue == null || sqlScalarValue.ToString().Equals(""))
        //        return;
        //    this.SQLScalarValue = sqlScalarValue.ToString();
        //    this.HasResults = true;
        //}

        internal void Fill(IEnumerable dapperResults)
        {
            this.RowList = new List<QueryResultsRow>();
            foreach (var row in dapperResults)
            {
                QueryResultsRow queryResultsRow = new QueryResultsRow();
                IDictionary<string, object> rowDictionary = row as IDictionary<string, object>;
                foreach (var item in rowDictionary)
                {
                    QueryResultsColumn queryResultsColumn = new QueryResultsColumn(
                            item.Key,
                            item.Value == null ? String.Empty : item.Value
                        );
                    queryResultsRow.AddColumn(queryResultsColumn);
                }
                this.RowList.Add(queryResultsRow);
                this.HasResults = true;
            }
        }
        public QueryResults Filter<ExpressionType>(string columnName, params object[] expressions)
        {
            QueryResults results = new QueryResults
                {
                    RowList = this.FindRowByExpression<ExpressionType>(columnName, true, expressions)
                };
            return results;
        }

        #endregion InternalMethods

        #region PublicMethods

        public QueryResultsRow Rows(int RowIndex)
        {
            if (this.HasResults)
                return this.RowList[RowIndex];
            else
                throw new ArgumentException("The sql statement that was executed returned no rows");
        }

        public void Dispose()
        {
            this.RowList = null;
            GC.Collect();
        }

        /// <summary>
        /// This will find a row based on the column and string expression(s) provided.
        /// </summary>
        /// <param name="ColumnName">Name of the column to look in.</param>
        /// <param name="equalTo">Set to false for great than equal to!</param>
        /// <param name="expression">Expression(s) to compare against. At least one expression must be true</param>
        /// <returns>return one or more row that match the expression(s)</returns>
        public List<QueryResultsRow> FindRowByExpression<ExpressionType>
            (
                string ColumnName,
                bool equalTo,
                params object[] expression
            )
        {
            var matchingRows = new List<QueryResultsRow>();
            var row = new QueryResultsRow();
            string type = typeof(ExpressionType).ToString();
            foreach (QueryResultsRow r in this.RowList)
            {
                switch (type)
                {
                    case "System.Int32":
                        {
                            int value = r.Column(ColumnName).GetValueAs<int>();
                            foreach (int exp in expression)
                            {
                                if (!equalTo && exp >= value)
                                    matchingRows.Add(r);
                                else if (exp == value)
                                    matchingRows.Add(r);
                                break;
                            }
                            break;
                        }
                    case "System.String":
                        {
                            string value = r.Column(ColumnName).GetValueAs<string>();
                            foreach (string exp in expression)
                            {
                                if (String.IsNullOrEmpty(value) && String.IsNullOrEmpty(exp))
                                    matchingRows.Add(r);
                                else if (!String.IsNullOrEmpty(value) && exp.Contains(value))
                                    matchingRows.Add(r);
                            }
                            break;
                        }
                    case "System.DateTime":
                        {
                            DateTime value = r.Column(ColumnName).GetValueAs<DateTime>();
                            foreach (DateTime exp in expression)
                                if (!equalTo && exp >= value)
                                    matchingRows.Add(r);
                                else if (exp == value)
                                    matchingRows.Add(r);
                            break;
                        }
                    case "System.Boolean":
                        {
                            bool value = r.Column(ColumnName).GetValueAs<bool>();
                            foreach (bool exp in expression)
                                if (!equalTo && exp == value)
                                    matchingRows.Add(r);
                                else if (exp == value)
                                    matchingRows.Add(r);
                            break;
                        }
                }
            }
            return matchingRows;
        }

        #endregion PublicMethods

        #region PublicProperties

        public string SQLScalarValue { get; internal set; }

        public bool HasResults { get; private set; }

        public List<QueryResultsRow> RowList { get; set; }

        //public int RowList.Count {get;private set;}

        #endregion PublicProperties

        #region PublicGenericMethods

        public List<ValueType> GetColumnValueList<ValueType>(string columnName)
        {
            List<ValueType> valueList = new List<ValueType>();
            foreach (QueryResultsRow row in this.RowList)
            {
                ValueType value = row.Column(columnName).GetValueAs<ValueType>();
                valueList.Add(value);
            }
            return valueList;
        }

        public IEnumerator<QueryResultsRow> GetEnumerator()
        {
            return this.RowList.GetEnumerator();
        }

        #endregion PublicGenericMethods

        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new List<QueryResultsRow>.Enumerator();
        }

        /// <summary>
        /// Element Type
        /// </summary>
        public Type ElementType
        {
            get { return typeof(QueryResultsRow); }
        }

        /// <summary>
        /// Expression
        /// </summary>
        public System.Linq.Expressions.Expression Expression
        {
            get { return RowList.AsQueryable().Expression; }
        }

        /// <summary>
        /// Provider
        /// </summary>
        public IQueryProvider Provider
        {
            get { return RowList.AsQueryable().Provider; }
        }
    }
}