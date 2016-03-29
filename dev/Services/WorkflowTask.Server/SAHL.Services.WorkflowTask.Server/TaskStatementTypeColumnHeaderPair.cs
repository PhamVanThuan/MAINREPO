using System;

namespace SAHL.Services.WorkflowTask.Server
{
    public class TaskStatementTypeColumnHeaderPair
    {
        public string TaskStatementFullName { get; private set; }
        public string ColumnHeaderWithFormatString { get; private set; }

        public TaskStatementTypeColumnHeaderPair(string taskStatementFullName, string columnHeaderWithFormatString)
        {
            this.TaskStatementFullName = taskStatementFullName ?? string.Empty;
            this.ColumnHeaderWithFormatString = columnHeaderWithFormatString ?? string.Empty;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}]", this.TaskStatementFullName, this.ColumnHeaderWithFormatString);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 29 + this.TaskStatementFullName.GetHashCode();
                hash = hash * 29 + this.ColumnHeaderWithFormatString.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            var castObj = (TaskStatementTypeColumnHeaderPair) obj;
            if (castObj == null)
            {
                return false;
            }

            var matches = this.ColumnHeaderWithFormatString.Equals(castObj.ColumnHeaderWithFormatString, StringComparison.OrdinalIgnoreCase)
                && this.TaskStatementFullName.Equals(castObj.TaskStatementFullName, StringComparison.OrdinalIgnoreCase);

            return matches;
        }
    }
}