﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.312
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591

namespace SAHL.X2.Framework.DataSets {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.ComponentModel.ToolboxItem(true)]
    [System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedDataSetSchema")]
    [System.Xml.Serialization.XmlRootAttribute("Metrics")]
    [System.ComponentModel.Design.HelpKeywordAttribute("vs.data.DataSet")]
    public partial class Metrics : System.Data.DataSet {
        
        private MetricsDataTable tableMetrics;
        
        private System.Data.SchemaSerializationMode _schemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public Metrics() {
            this.BeginInit();
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            base.Relations.CollectionChanged += schemaChangedHandler;
            this.EndInit();
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected Metrics(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context, false) {
            if ((this.IsBinarySerialized(info, context) == true)) {
                this.InitVars(false);
                System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler1 = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                this.Tables.CollectionChanged += schemaChangedHandler1;
                this.Relations.CollectionChanged += schemaChangedHandler1;
                return;
            }
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((this.DetermineSchemaSerializationMode(info, context) == System.Data.SchemaSerializationMode.IncludeSchema)) {
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.ReadXmlSchema(new System.Xml.XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["Metrics"] != null)) {
                    base.Tables.Add(new MetricsDataTable(ds.Tables["Metrics"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.ReadXmlSchema(new System.Xml.XmlTextReader(new System.IO.StringReader(strSchema)));
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public MetricsDataTable _Metrics {
            get {
                return this.tableMetrics;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.BrowsableAttribute(true)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public override System.Data.SchemaSerializationMode SchemaSerializationMode {
            get {
                return this._schemaSerializationMode;
            }
            set {
                this._schemaSerializationMode = value;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new System.Data.DataTableCollection Tables {
            get {
                return base.Tables;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new System.Data.DataRelationCollection Relations {
            get {
                return base.Relations;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void InitializeDerivedDataSet() {
            this.BeginInit();
            this.InitClass();
            this.EndInit();
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public override System.Data.DataSet Clone() {
            Metrics cln = ((Metrics)(base.Clone()));
            cln.InitVars();
            cln.SchemaSerializationMode = this.SchemaSerializationMode;
            return cln;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void ReadXmlSerializable(System.Xml.XmlReader reader) {
            if ((this.DetermineSchemaSerializationMode(reader) == System.Data.SchemaSerializationMode.IncludeSchema)) {
                this.Reset();
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.ReadXml(reader);
                if ((ds.Tables["Metrics"] != null)) {
                    base.Tables.Add(new MetricsDataTable(ds.Tables["Metrics"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.ReadXml(reader);
                this.InitVars();
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new System.Xml.XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new System.Xml.XmlTextReader(stream), null);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars() {
            this.InitVars(true);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars(bool initTable) {
            this.tableMetrics = ((MetricsDataTable)(base.Tables["Metrics"]));
            if ((initTable == true)) {
                if ((this.tableMetrics != null)) {
                    this.tableMetrics.InitVars();
                }
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void InitClass() {
            this.DataSetName = "Metrics";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/Metrics.xsd";
            this.EnforceConstraints = true;
            this.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            this.tableMetrics = new MetricsDataTable();
            base.Tables.Add(this.tableMetrics);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private bool ShouldSerialize_Metrics() {
            return false;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static System.Xml.Schema.XmlSchemaComplexType GetTypedDataSetSchema(System.Xml.Schema.XmlSchemaSet xs) {
            Metrics ds = new Metrics();
            System.Xml.Schema.XmlSchemaComplexType type = new System.Xml.Schema.XmlSchemaComplexType();
            System.Xml.Schema.XmlSchemaSequence sequence = new System.Xml.Schema.XmlSchemaSequence();
            xs.Add(ds.GetSchemaSerializable());
            System.Xml.Schema.XmlSchemaAny any = new System.Xml.Schema.XmlSchemaAny();
            any.Namespace = ds.Namespace;
            sequence.Items.Add(any);
            type.Particle = sequence;
            return type;
        }
        
        public delegate void MetricsRowChangeEventHandler(object sender, MetricsRowChangeEvent e);
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        [System.Serializable()]
        [System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
        public partial class MetricsDataTable : System.Data.DataTable, System.Collections.IEnumerable {
            
            private System.Data.DataColumn columnMetricKey;
            
            private System.Data.DataColumn columnMetricDate;
            
            private System.Data.DataColumn columnApplicationName;
            
            private System.Data.DataColumn columnHostName;
            
            private System.Data.DataColumn columnWorkStationID;
            
            private System.Data.DataColumn columnWindowsLogon;
            
            private System.Data.DataColumn columnFormName;
            
            private System.Data.DataColumn columnMetricFrom;
            
            private System.Data.DataColumn columnMetricTo;
            
            private System.Data.DataColumn columnMetricAction;
            
            private System.Data.DataColumn columnMetricInformation;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public MetricsDataTable() {
                this.TableName = "Metrics";
                this.BeginInit();
                this.InitClass();
                this.EndInit();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal MetricsDataTable(System.Data.DataTable table) {
                this.TableName = table.TableName;
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected MetricsDataTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                    base(info, context) {
                this.InitVars();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn MetricKeyColumn {
                get {
                    return this.columnMetricKey;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn MetricDateColumn {
                get {
                    return this.columnMetricDate;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn ApplicationNameColumn {
                get {
                    return this.columnApplicationName;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn HostNameColumn {
                get {
                    return this.columnHostName;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn WorkStationIDColumn {
                get {
                    return this.columnWorkStationID;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn WindowsLogonColumn {
                get {
                    return this.columnWindowsLogon;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn FormNameColumn {
                get {
                    return this.columnFormName;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn MetricFromColumn {
                get {
                    return this.columnMetricFrom;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn MetricToColumn {
                get {
                    return this.columnMetricTo;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn MetricActionColumn {
                get {
                    return this.columnMetricAction;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn MetricInformationColumn {
                get {
                    return this.columnMetricInformation;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public MetricsRow this[int index] {
                get {
                    return ((MetricsRow)(this.Rows[index]));
                }
            }
            
            public event MetricsRowChangeEventHandler MetricsRowChanging;
            
            public event MetricsRowChangeEventHandler MetricsRowChanged;
            
            public event MetricsRowChangeEventHandler MetricsRowDeleting;
            
            public event MetricsRowChangeEventHandler MetricsRowDeleted;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void AddMetricsRow(MetricsRow row) {
                this.Rows.Add(row);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public MetricsRow AddMetricsRow(System.DateTime MetricDate, string ApplicationName, string HostName, string WorkStationID, string WindowsLogon, string FormName, string MetricFrom, string MetricTo, string MetricAction, string MetricInformation) {
                MetricsRow rowMetricsRow = ((MetricsRow)(this.NewRow()));
                rowMetricsRow.ItemArray = new object[] {
                        null,
                        MetricDate,
                        ApplicationName,
                        HostName,
                        WorkStationID,
                        WindowsLogon,
                        FormName,
                        MetricFrom,
                        MetricTo,
                        MetricAction,
                        MetricInformation};
                this.Rows.Add(rowMetricsRow);
                return rowMetricsRow;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public MetricsRow FindByMetricKey(int MetricKey) {
                return ((MetricsRow)(this.Rows.Find(new object[] {
                            MetricKey})));
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public virtual System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public override System.Data.DataTable Clone() {
                MetricsDataTable cln = ((MetricsDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override System.Data.DataTable CreateInstance() {
                return new MetricsDataTable();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal void InitVars() {
                this.columnMetricKey = base.Columns["MetricKey"];
                this.columnMetricDate = base.Columns["MetricDate"];
                this.columnApplicationName = base.Columns["ApplicationName"];
                this.columnHostName = base.Columns["HostName"];
                this.columnWorkStationID = base.Columns["WorkStationID"];
                this.columnWindowsLogon = base.Columns["WindowsLogon"];
                this.columnFormName = base.Columns["FormName"];
                this.columnMetricFrom = base.Columns["MetricFrom"];
                this.columnMetricTo = base.Columns["MetricTo"];
                this.columnMetricAction = base.Columns["MetricAction"];
                this.columnMetricInformation = base.Columns["MetricInformation"];
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private void InitClass() {
                this.columnMetricKey = new System.Data.DataColumn("MetricKey", typeof(int), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnMetricKey);
                this.columnMetricDate = new System.Data.DataColumn("MetricDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnMetricDate);
                this.columnApplicationName = new System.Data.DataColumn("ApplicationName", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnApplicationName);
                this.columnHostName = new System.Data.DataColumn("HostName", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnHostName);
                this.columnWorkStationID = new System.Data.DataColumn("WorkStationID", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnWorkStationID);
                this.columnWindowsLogon = new System.Data.DataColumn("WindowsLogon", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnWindowsLogon);
                this.columnFormName = new System.Data.DataColumn("FormName", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnFormName);
                this.columnMetricFrom = new System.Data.DataColumn("MetricFrom", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnMetricFrom);
                this.columnMetricTo = new System.Data.DataColumn("MetricTo", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnMetricTo);
                this.columnMetricAction = new System.Data.DataColumn("MetricAction", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnMetricAction);
                this.columnMetricInformation = new System.Data.DataColumn("MetricInformation", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnMetricInformation);
                this.Constraints.Add(new System.Data.UniqueConstraint("Constraint1", new System.Data.DataColumn[] {
                                this.columnMetricKey}, true));
                this.columnMetricKey.AutoIncrement = true;
                this.columnMetricKey.AutoIncrementSeed = -1;
                this.columnMetricKey.AutoIncrementStep = -1;
                this.columnMetricKey.AllowDBNull = false;
                this.columnMetricKey.ReadOnly = true;
                this.columnMetricKey.Unique = true;
                this.columnApplicationName.MaxLength = 50;
                this.columnHostName.MaxLength = 50;
                this.columnWorkStationID.MaxLength = 50;
                this.columnWindowsLogon.MaxLength = 50;
                this.columnFormName.MaxLength = 50;
                this.columnMetricFrom.MaxLength = 50;
                this.columnMetricTo.MaxLength = 50;
                this.columnMetricAction.MaxLength = 255;
                this.columnMetricInformation.MaxLength = 50;
                this.ExtendedProperties.Add("Generator_TablePropName", "_Metrics");
                this.ExtendedProperties.Add("Generator_UserTableName", "Metrics");
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public MetricsRow NewMetricsRow() {
                return ((MetricsRow)(this.NewRow()));
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder) {
                return new MetricsRow(builder);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override System.Type GetRowType() {
                return typeof(MetricsRow);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanged(System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.MetricsRowChanged != null)) {
                    this.MetricsRowChanged(this, new MetricsRowChangeEvent(((MetricsRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanging(System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.MetricsRowChanging != null)) {
                    this.MetricsRowChanging(this, new MetricsRowChangeEvent(((MetricsRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleted(System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.MetricsRowDeleted != null)) {
                    this.MetricsRowDeleted(this, new MetricsRowChangeEvent(((MetricsRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleting(System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.MetricsRowDeleting != null)) {
                    this.MetricsRowDeleting(this, new MetricsRowChangeEvent(((MetricsRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void RemoveMetricsRow(MetricsRow row) {
                this.Rows.Remove(row);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public static System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(System.Xml.Schema.XmlSchemaSet xs) {
                System.Xml.Schema.XmlSchemaComplexType type = new System.Xml.Schema.XmlSchemaComplexType();
                System.Xml.Schema.XmlSchemaSequence sequence = new System.Xml.Schema.XmlSchemaSequence();
                Metrics ds = new Metrics();
                xs.Add(ds.GetSchemaSerializable());
                System.Xml.Schema.XmlSchemaAny any1 = new System.Xml.Schema.XmlSchemaAny();
                any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                any1.MinOccurs = new decimal(0);
                any1.MaxOccurs = decimal.MaxValue;
                any1.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any1);
                System.Xml.Schema.XmlSchemaAny any2 = new System.Xml.Schema.XmlSchemaAny();
                any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                any2.MinOccurs = new decimal(1);
                any2.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any2);
                System.Xml.Schema.XmlSchemaAttribute attribute1 = new System.Xml.Schema.XmlSchemaAttribute();
                attribute1.Name = "namespace";
                attribute1.FixedValue = ds.Namespace;
                type.Attributes.Add(attribute1);
                System.Xml.Schema.XmlSchemaAttribute attribute2 = new System.Xml.Schema.XmlSchemaAttribute();
                attribute2.Name = "tableTypeName";
                attribute2.FixedValue = "MetricsDataTable";
                type.Attributes.Add(attribute2);
                type.Particle = sequence;
                return type;
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public partial class MetricsRow : System.Data.DataRow {
            
            private MetricsDataTable tableMetrics;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal MetricsRow(System.Data.DataRowBuilder rb) : 
                    base(rb) {
                this.tableMetrics = ((MetricsDataTable)(this.Table));
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public int MetricKey {
                get {
                    return ((int)(this[this.tableMetrics.MetricKeyColumn]));
                }
                set {
                    this[this.tableMetrics.MetricKeyColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.DateTime MetricDate {
                get {
                    try {
                        return ((System.DateTime)(this[this.tableMetrics.MetricDateColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'MetricDate\' in table \'Metrics\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableMetrics.MetricDateColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string ApplicationName {
                get {
                    try {
                        return ((string)(this[this.tableMetrics.ApplicationNameColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'ApplicationName\' in table \'Metrics\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableMetrics.ApplicationNameColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string HostName {
                get {
                    try {
                        return ((string)(this[this.tableMetrics.HostNameColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'HostName\' in table \'Metrics\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableMetrics.HostNameColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string WorkStationID {
                get {
                    try {
                        return ((string)(this[this.tableMetrics.WorkStationIDColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'WorkStationID\' in table \'Metrics\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableMetrics.WorkStationIDColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string WindowsLogon {
                get {
                    try {
                        return ((string)(this[this.tableMetrics.WindowsLogonColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'WindowsLogon\' in table \'Metrics\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableMetrics.WindowsLogonColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string FormName {
                get {
                    try {
                        return ((string)(this[this.tableMetrics.FormNameColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'FormName\' in table \'Metrics\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableMetrics.FormNameColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string MetricFrom {
                get {
                    try {
                        return ((string)(this[this.tableMetrics.MetricFromColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'MetricFrom\' in table \'Metrics\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableMetrics.MetricFromColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string MetricTo {
                get {
                    try {
                        return ((string)(this[this.tableMetrics.MetricToColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'MetricTo\' in table \'Metrics\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableMetrics.MetricToColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string MetricAction {
                get {
                    try {
                        return ((string)(this[this.tableMetrics.MetricActionColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'MetricAction\' in table \'Metrics\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableMetrics.MetricActionColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string MetricInformation {
                get {
                    try {
                        return ((string)(this[this.tableMetrics.MetricInformationColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'MetricInformation\' in table \'Metrics\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableMetrics.MetricInformationColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsMetricDateNull() {
                return this.IsNull(this.tableMetrics.MetricDateColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetMetricDateNull() {
                this[this.tableMetrics.MetricDateColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsApplicationNameNull() {
                return this.IsNull(this.tableMetrics.ApplicationNameColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetApplicationNameNull() {
                this[this.tableMetrics.ApplicationNameColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsHostNameNull() {
                return this.IsNull(this.tableMetrics.HostNameColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetHostNameNull() {
                this[this.tableMetrics.HostNameColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsWorkStationIDNull() {
                return this.IsNull(this.tableMetrics.WorkStationIDColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetWorkStationIDNull() {
                this[this.tableMetrics.WorkStationIDColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsWindowsLogonNull() {
                return this.IsNull(this.tableMetrics.WindowsLogonColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetWindowsLogonNull() {
                this[this.tableMetrics.WindowsLogonColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsFormNameNull() {
                return this.IsNull(this.tableMetrics.FormNameColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetFormNameNull() {
                this[this.tableMetrics.FormNameColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsMetricFromNull() {
                return this.IsNull(this.tableMetrics.MetricFromColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetMetricFromNull() {
                this[this.tableMetrics.MetricFromColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsMetricToNull() {
                return this.IsNull(this.tableMetrics.MetricToColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetMetricToNull() {
                this[this.tableMetrics.MetricToColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsMetricActionNull() {
                return this.IsNull(this.tableMetrics.MetricActionColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetMetricActionNull() {
                this[this.tableMetrics.MetricActionColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsMetricInformationNull() {
                return this.IsNull(this.tableMetrics.MetricInformationColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetMetricInformationNull() {
                this[this.tableMetrics.MetricInformationColumn] = System.Convert.DBNull;
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public class MetricsRowChangeEvent : System.EventArgs {
            
            private MetricsRow eventRow;
            
            private System.Data.DataRowAction eventAction;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public MetricsRowChangeEvent(MetricsRow row, System.Data.DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public MetricsRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}

#pragma warning restore 1591