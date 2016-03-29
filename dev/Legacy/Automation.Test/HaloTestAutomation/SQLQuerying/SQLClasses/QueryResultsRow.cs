using System;
using System.Collections.Generic;

namespace Automation.DataAccess
{
    public sealed class QueryResultsRow
    {
        private List<QueryResultsColumn> columns = new List<QueryResultsColumn>();
        internal void AddColumn(QueryResultsColumn Column)
        {
            this.columns.Add(Column);
            this.Count = columns.Count;
        }
        public QueryResultsColumn Column(string columnName)
        {
            for (int colIndex = 0; colIndex < this.columns.Count; colIndex++)
                if (this.columns[colIndex].Name.ToLower() == columnName.ToLower())
                    return this.columns[colIndex];
            throw new Exception("Could not locate Column: " + columnName);
        }
        public int Count { get; private set; }
        public QueryResultsColumn Column(int columnIndex)
        {
            return this.columns[columnIndex];
        }
        public QueryResultsColumn[] Columns { get { return this.columns.ToArray(); } }
    }
}