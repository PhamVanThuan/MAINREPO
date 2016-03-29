using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DataGridConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DataGridConfigurationDataModel(string statementName, string columnName, string columnDescription, int sequence, string width, bool visible, bool indexIdentifier, int? formatTypeKey, int? dataGridConfigurationTypeKey)
        {
            this.StatementName = statementName;
            this.ColumnName = columnName;
            this.ColumnDescription = columnDescription;
            this.Sequence = sequence;
            this.Width = width;
            this.Visible = visible;
            this.IndexIdentifier = indexIdentifier;
            this.FormatTypeKey = formatTypeKey;
            this.DataGridConfigurationTypeKey = dataGridConfigurationTypeKey;
		
        }
		[JsonConstructor]
        public DataGridConfigurationDataModel(int dataGridConfigurationKey, string statementName, string columnName, string columnDescription, int sequence, string width, bool visible, bool indexIdentifier, int? formatTypeKey, int? dataGridConfigurationTypeKey)
        {
            this.DataGridConfigurationKey = dataGridConfigurationKey;
            this.StatementName = statementName;
            this.ColumnName = columnName;
            this.ColumnDescription = columnDescription;
            this.Sequence = sequence;
            this.Width = width;
            this.Visible = visible;
            this.IndexIdentifier = indexIdentifier;
            this.FormatTypeKey = formatTypeKey;
            this.DataGridConfigurationTypeKey = dataGridConfigurationTypeKey;
		
        }		

        public int DataGridConfigurationKey { get; set; }

        public string StatementName { get; set; }

        public string ColumnName { get; set; }

        public string ColumnDescription { get; set; }

        public int Sequence { get; set; }

        public string Width { get; set; }

        public bool Visible { get; set; }

        public bool IndexIdentifier { get; set; }

        public int? FormatTypeKey { get; set; }

        public int? DataGridConfigurationTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.DataGridConfigurationKey =  key;
        }
    }
}