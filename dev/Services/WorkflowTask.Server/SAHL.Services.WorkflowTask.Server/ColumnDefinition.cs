namespace SAHL.Services.WorkflowTask.Server
{
    public class ColumnDefinition
    {
        public string ColumnName { get; private set; }
        public string FormatString { get; private set; }

        public ColumnDefinition(string columnName, string formatString)
        {
            this.ColumnName = columnName;
            this.FormatString = formatString;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}]", this.ColumnName, this.FormatString);
        }
    }
}