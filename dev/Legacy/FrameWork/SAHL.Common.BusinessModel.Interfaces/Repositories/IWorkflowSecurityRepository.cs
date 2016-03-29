using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.Globals;
using offerRole = SAHL.Common.BusinessModel.Interfaces.Repositories.OfferRole;
using workflowRole = SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IWorkflowSecurityRepository
    {
        DataTable GetLatestWorkflowAssignmentForADUserKeyAndInstance(long instanceID, int adUserKey);

        DataTable GetWorkflowHistoryForActivityByInstance(long instanceID, string activityName);

        DataTable GetWorkflowInstanceInPreviousState(Int64 sourceInstanceID, Int64 instanceID, string previousStateName);

        void ActivateLatestOfferRoleForEachOfferType(int offerKey, int offerRoleTypeKey);

        void ActivateLatestWorkflowRoleForEachWorklflowRoleType(int genericKey, int workflowRoleTypeKey);

        void Assign2AMOfferRole(int offerKey, int offerRoleTypeKey, int legalEntityKey);

        void AssignWorkflowRole(long instanceID, int adUserKey, int blaKey, string stateName);

        void AssignWorkflowRoleAssignmentForADUser(long instanceID, string adUserName, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, string state);

        void AssignWorkflowRoleForADUser(long instanceID, string adUserName, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey);

        void Create2AMWorkflowRole(int genericKey, int workflowRoleTypeKey, int legalEntityKey);

        void CreateWorkflowRole(SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, int legalEntityKey);

        void CreateWorkflowRoleAssignment(long instanceID, int workflowRoleTypeOrganisationStructureMapping, int adUserKey, string message);

        void DeactivateAllWorkflowRoleAssigmentsForInstance(long instanceID);

        void DeactivateWorkflowRole(long instanceID);

        void DeactivateWorkflowRoleForDynamicRole(int workflowRoleTypeKey, int genericKey);

        workflowRole.WorkflowAssignment.ADUserRow GetADUser(int adUserKey);

        workflowRole.WorkflowAssignment.ADUserRow GetADUser(string adUserName);

        offerRole.WorkflowAssignment.ADUserRow GetADUserRowByName(string adUserName);

        int GetLastAssignedUserForGroupByRole(long instanceID, int debtCounsellingKey, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, out System.Data.DataRow workflowRoleAssignmentRow);

        offerRole.WorkflowAssignment GetOfferRoleOrganisationStructure();

        offerRole.WorkflowAssignment.OfferRoleTypeRow GetOfferRoleRow(string dynamicRole);

        string GetProcessName(SAHL.Common.Globals.Process process);

        List<long> GetRelatedInstances(int debtCounsellingKey);

        int GetStateIDByName(string workflow, string process, string state);

        string GetUIStatement(string applicationName, string statementName);

        offerRole.WorkflowAssignment GetWFAssignment(int genericKey, string dynamicRole, long instanceID);

        int GetWorkFlowIDByName(string workflowName, out int processID);

        string GetWorkflowName(SAHL.Common.Globals.Workflow workflow);

        workflowRole.WorkflowAssignment GetWorkflowRoleAssignment(SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, long instanceID);

        workflowRole.WorkflowAssignment GetWorkflowRoleAssignment(List<WorkflowRoleTypes> workflowRoleTypes, long instanceID);

        DataRow GetWorkflowRoleAssignment(System.Collections.Generic.List<long> instanceIDs, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType);

        workflowRole.WorkflowAssignment GetWorkflowRoleOrganisationStructure();

        int GetWorkflowRoleTypeOrgStructMapKey(string adUserName, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType);

        workflowRole.WorkflowAssignment.WorkflowRoleTypeRow GetWorkflowRoleTypeRow(SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType);

        bool IfOfferRoleExistsReactivate(int offerKey, int offerRoleTypeKey, int LEKey);

        bool IsUserActive(SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int adUserKey);

        bool IsUserStillInSameOrgStructureForCaseReassign(int adUserKey, SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, long instanceID);

        string LoadBalanceAssign(SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, System.Collections.Generic.List<string> statesToDetermineLoad, SAHL.Common.Globals.Process process, SAHL.Common.Globals.Workflow workflow, bool includeStates, bool checkRoundRobinStatus);

        bool ReactivateIfWorkflowRoleExists(int genericKey, int workflowRoleTypeKey, int legalEntityKey);

        offerRole.WorkflowAssignment GetWFAssignmentAcrossInstances(string dynamicRole, long instanceID);

        DataTable GetADUsersForRoundRobinByRoundRobinPointerDescription(int roundRobinPointerKey);

        DataTable RemoveDuplicateRows(DataTable dTable, string colName);

        int TrueRoundRobin(DataTable fullTable, int roundRobinPointerKey);

        string AssignToWorkflowAssignmentAndOfferRoleTables(int adUserKey, int blaKey, string dynamicRole, int genericKey, long instanceID, string state, Process process);

        DataTable GetUsersForOrgStructureKeyAndDynamicRole(long instanceID, int organisationStructureKey, string dynamicRole);

        long GetSourceInstanceFromInstanceID(long instanceID);

        offerRole.WorkflowAssignment.WFAssignmentDataTable GetLastSecurityRecordForApplicationKeyOrgStuctAndOfferRoleType(int ApplicationKey, int OrgStuctKey, int ORTKey, string Map);

        int TrueRoundRobinForWorkflowRoleAssignment(DataTable userTable, int roundRobinPointerKey);

        bool IsUserStillInSameOrgStructureForCaseReassign(int adUserKey, GenericKeyTypes userOrganisationStructureGenericType, WorkflowRoleTypes workflowRoleType);

        DataTable GetADUsersForRoundRobinByRoundRobinPointerKey(int roundRobinPointerKey);

        string RoundRobinAssignForPointer(long instanceID, RoundRobinPointers roundRobinPointer, int genericKey, WorkflowRoleTypes workflowRoleType);
    }

    namespace OfferRole
    {
        /// <summary>
        ///Represents a strongly typed in-memory cache of data.
        ///</summary>
        [global::System.Serializable()]
        [global::System.ComponentModel.DesignerCategoryAttribute("code")]
        [global::System.ComponentModel.ToolboxItem(true)]
        [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedDataSetSchema")]
        [global::System.Xml.Serialization.XmlRootAttribute("WorkflowAssignment")]
        [global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.DataSet")]
        public partial class WorkflowAssignment : global::System.Data.DataSet
        {
            private WorkflowAssignmentDataTable tableWorkflowAssignment;

            private ADUserDataTable tableADUser;

            private OfferRoleTypeDataTable tableOfferRoleType;

            private OfferRoleTypeOrganisationStructureMappingDataTable tableOfferRoleTypeOrganisationStructureMapping;

            private OrganisationStructureDataTable tableOrganisationStructure;

            private UserOrganisationStructureDataTable tableUserOrganisationStructure;

            private WFAssignmentDataTable tableWFAssignment;

            private global::System.Data.DataRelation relationFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure;

            private global::System.Data.DataRelation relationFK_UserOrganisationStructure_OrganisationStructure;

            private global::System.Data.DataRelation relationFK_UserOrganisationStructure_ADUser;

            private global::System.Data.DataRelation relationFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType;

            private global::System.Data.SchemaSerializationMode _schemaSerializationMode = global::System.Data.SchemaSerializationMode.IncludeSchema;

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public WorkflowAssignment()
            {
                this.BeginInit();
                this.InitClass();
                global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                base.Tables.CollectionChanged += schemaChangedHandler;
                base.Relations.CollectionChanged += schemaChangedHandler;
                this.EndInit();
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected WorkflowAssignment(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                base(info, context, false)
            {
                if ((this.IsBinarySerialized(info, context) == true))
                {
                    this.InitVars(false);
                    global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler1 = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                    this.Tables.CollectionChanged += schemaChangedHandler1;
                    this.Relations.CollectionChanged += schemaChangedHandler1;
                    return;
                }
                string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
                if ((this.DetermineSchemaSerializationMode(info, context) == global::System.Data.SchemaSerializationMode.IncludeSchema))
                {
                    global::System.Data.DataSet ds = new global::System.Data.DataSet();
                    ds.ReadXmlSchema(new global::System.Xml.XmlTextReader(new global::System.IO.StringReader(strSchema)));
                    if ((ds.Tables["WorkflowAssignment"] != null))
                    {
                        base.Tables.Add(new WorkflowAssignmentDataTable(ds.Tables["WorkflowAssignment"]));
                    }
                    if ((ds.Tables["ADUser"] != null))
                    {
                        base.Tables.Add(new ADUserDataTable(ds.Tables["ADUser"]));
                    }
                    if ((ds.Tables["OfferRoleType"] != null))
                    {
                        base.Tables.Add(new OfferRoleTypeDataTable(ds.Tables["OfferRoleType"]));
                    }
                    if ((ds.Tables["OfferRoleTypeOrganisationStructureMapping"] != null))
                    {
                        base.Tables.Add(new OfferRoleTypeOrganisationStructureMappingDataTable(ds.Tables["OfferRoleTypeOrganisationStructureMapping"]));
                    }
                    if ((ds.Tables["OrganisationStructure"] != null))
                    {
                        base.Tables.Add(new OrganisationStructureDataTable(ds.Tables["OrganisationStructure"]));
                    }
                    if ((ds.Tables["UserOrganisationStructure"] != null))
                    {
                        base.Tables.Add(new UserOrganisationStructureDataTable(ds.Tables["UserOrganisationStructure"]));
                    }
                    if ((ds.Tables["WFAssignment"] != null))
                    {
                        base.Tables.Add(new WFAssignmentDataTable(ds.Tables["WFAssignment"]));
                    }
                    this.DataSetName = ds.DataSetName;
                    this.Prefix = ds.Prefix;
                    this.Namespace = ds.Namespace;
                    this.Locale = ds.Locale;
                    this.CaseSensitive = ds.CaseSensitive;
                    this.EnforceConstraints = ds.EnforceConstraints;
                    this.Merge(ds, false, global::System.Data.MissingSchemaAction.Add);
                    this.InitVars();
                }
                else
                {
                    this.ReadXmlSchema(new global::System.Xml.XmlTextReader(new global::System.IO.StringReader(strSchema)));
                }
                this.GetSerializationData(info, context);
                global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                base.Tables.CollectionChanged += schemaChangedHandler;
                this.Relations.CollectionChanged += schemaChangedHandler;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public WorkflowAssignmentDataTable _WorkflowAssignment
            {
                get
                {
                    return this.tableWorkflowAssignment;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public ADUserDataTable ADUser
            {
                get
                {
                    return this.tableADUser;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public OfferRoleTypeDataTable OfferRoleType
            {
                get
                {
                    return this.tableOfferRoleType;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public OfferRoleTypeOrganisationStructureMappingDataTable OfferRoleTypeOrganisationStructureMapping
            {
                get
                {
                    return this.tableOfferRoleTypeOrganisationStructureMapping;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public OrganisationStructureDataTable OrganisationStructure
            {
                get
                {
                    return this.tableOrganisationStructure;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public UserOrganisationStructureDataTable UserOrganisationStructure
            {
                get
                {
                    return this.tableUserOrganisationStructure;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public WFAssignmentDataTable WFAssignment
            {
                get
                {
                    return this.tableWFAssignment;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.BrowsableAttribute(true)]
            [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Visible)]
            public override global::System.Data.SchemaSerializationMode SchemaSerializationMode
            {
                get
                {
                    return this._schemaSerializationMode;
                }
                set
                {
                    this._schemaSerializationMode = value;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Hidden)]
            public new global::System.Data.DataTableCollection Tables
            {
                get
                {
                    return base.Tables;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Hidden)]
            public new global::System.Data.DataRelationCollection Relations
            {
                get
                {
                    return base.Relations;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void InitializeDerivedDataSet()
            {
                this.BeginInit();
                this.InitClass();
                this.EndInit();
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public override global::System.Data.DataSet Clone()
            {
                WorkflowAssignment cln = ((WorkflowAssignment)(base.Clone()));
                cln.InitVars();
                cln.SchemaSerializationMode = this.SchemaSerializationMode;
                return cln;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override bool ShouldSerializeTables()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override bool ShouldSerializeRelations()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void ReadXmlSerializable(global::System.Xml.XmlReader reader)
            {
                if ((this.DetermineSchemaSerializationMode(reader) == global::System.Data.SchemaSerializationMode.IncludeSchema))
                {
                    this.Reset();
                    global::System.Data.DataSet ds = new global::System.Data.DataSet();
                    ds.ReadXml(reader);
                    if ((ds.Tables["WorkflowAssignment"] != null))
                    {
                        base.Tables.Add(new WorkflowAssignmentDataTable(ds.Tables["WorkflowAssignment"]));
                    }
                    if ((ds.Tables["ADUser"] != null))
                    {
                        base.Tables.Add(new ADUserDataTable(ds.Tables["ADUser"]));
                    }
                    if ((ds.Tables["OfferRoleType"] != null))
                    {
                        base.Tables.Add(new OfferRoleTypeDataTable(ds.Tables["OfferRoleType"]));
                    }
                    if ((ds.Tables["OfferRoleTypeOrganisationStructureMapping"] != null))
                    {
                        base.Tables.Add(new OfferRoleTypeOrganisationStructureMappingDataTable(ds.Tables["OfferRoleTypeOrganisationStructureMapping"]));
                    }
                    if ((ds.Tables["OrganisationStructure"] != null))
                    {
                        base.Tables.Add(new OrganisationStructureDataTable(ds.Tables["OrganisationStructure"]));
                    }
                    if ((ds.Tables["UserOrganisationStructure"] != null))
                    {
                        base.Tables.Add(new UserOrganisationStructureDataTable(ds.Tables["UserOrganisationStructure"]));
                    }
                    if ((ds.Tables["WFAssignment"] != null))
                    {
                        base.Tables.Add(new WFAssignmentDataTable(ds.Tables["WFAssignment"]));
                    }
                    this.DataSetName = ds.DataSetName;
                    this.Prefix = ds.Prefix;
                    this.Namespace = ds.Namespace;
                    this.Locale = ds.Locale;
                    this.CaseSensitive = ds.CaseSensitive;
                    this.EnforceConstraints = ds.EnforceConstraints;
                    this.Merge(ds, false, global::System.Data.MissingSchemaAction.Add);
                    this.InitVars();
                }
                else
                {
                    this.ReadXml(reader);
                    this.InitVars();
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override global::System.Xml.Schema.XmlSchema GetSchemaSerializable()
            {
                global::System.IO.MemoryStream stream = new global::System.IO.MemoryStream();
                this.WriteXmlSchema(new global::System.Xml.XmlTextWriter(stream, null));
                stream.Position = 0;
                return global::System.Xml.Schema.XmlSchema.Read(new global::System.Xml.XmlTextReader(stream), null);
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal void InitVars()
            {
                this.InitVars(true);
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal void InitVars(bool initTable)
            {
                this.tableWorkflowAssignment = ((WorkflowAssignmentDataTable)(base.Tables["WorkflowAssignment"]));
                if ((initTable == true))
                {
                    if ((this.tableWorkflowAssignment != null))
                    {
                        this.tableWorkflowAssignment.InitVars();
                    }
                }
                this.tableADUser = ((ADUserDataTable)(base.Tables["ADUser"]));
                if ((initTable == true))
                {
                    if ((this.tableADUser != null))
                    {
                        this.tableADUser.InitVars();
                    }
                }
                this.tableOfferRoleType = ((OfferRoleTypeDataTable)(base.Tables["OfferRoleType"]));
                if ((initTable == true))
                {
                    if ((this.tableOfferRoleType != null))
                    {
                        this.tableOfferRoleType.InitVars();
                    }
                }
                this.tableOfferRoleTypeOrganisationStructureMapping = ((OfferRoleTypeOrganisationStructureMappingDataTable)(base.Tables["OfferRoleTypeOrganisationStructureMapping"]));
                if ((initTable == true))
                {
                    if ((this.tableOfferRoleTypeOrganisationStructureMapping != null))
                    {
                        this.tableOfferRoleTypeOrganisationStructureMapping.InitVars();
                    }
                }
                this.tableOrganisationStructure = ((OrganisationStructureDataTable)(base.Tables["OrganisationStructure"]));
                if ((initTable == true))
                {
                    if ((this.tableOrganisationStructure != null))
                    {
                        this.tableOrganisationStructure.InitVars();
                    }
                }
                this.tableUserOrganisationStructure = ((UserOrganisationStructureDataTable)(base.Tables["UserOrganisationStructure"]));
                if ((initTable == true))
                {
                    if ((this.tableUserOrganisationStructure != null))
                    {
                        this.tableUserOrganisationStructure.InitVars();
                    }
                }
                this.tableWFAssignment = ((WFAssignmentDataTable)(base.Tables["WFAssignment"]));
                if ((initTable == true))
                {
                    if ((this.tableWFAssignment != null))
                    {
                        this.tableWFAssignment.InitVars();
                    }
                }
                this.relationFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure = this.Relations["FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure"];
                this.relationFK_UserOrganisationStructure_OrganisationStructure = this.Relations["FK_UserOrganisationStructure_OrganisationStructure"];
                this.relationFK_UserOrganisationStructure_ADUser = this.Relations["FK_UserOrganisationStructure_ADUser"];
                this.relationFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType = this.Relations["FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType"];
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private void InitClass()
            {
                this.DataSetName = "WorkflowAssignment";
                this.Prefix = "";
                this.Namespace = "http://tempuri.org/WorkflowAssignment.xsd";
                this.EnforceConstraints = true;
                this.SchemaSerializationMode = global::System.Data.SchemaSerializationMode.IncludeSchema;
                this.tableWorkflowAssignment = new WorkflowAssignmentDataTable();
                base.Tables.Add(this.tableWorkflowAssignment);
                this.tableADUser = new ADUserDataTable();
                base.Tables.Add(this.tableADUser);
                this.tableOfferRoleType = new OfferRoleTypeDataTable();
                base.Tables.Add(this.tableOfferRoleType);
                this.tableOfferRoleTypeOrganisationStructureMapping = new OfferRoleTypeOrganisationStructureMappingDataTable();
                base.Tables.Add(this.tableOfferRoleTypeOrganisationStructureMapping);
                this.tableOrganisationStructure = new OrganisationStructureDataTable();
                base.Tables.Add(this.tableOrganisationStructure);
                this.tableUserOrganisationStructure = new UserOrganisationStructureDataTable();
                base.Tables.Add(this.tableUserOrganisationStructure);
                this.tableWFAssignment = new WFAssignmentDataTable();
                base.Tables.Add(this.tableWFAssignment);
                this.relationFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure = new global::System.Data.DataRelation("FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure", new global::System.Data.DataColumn[] {
                        this.tableOrganisationStructure.OrganisationStructureKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableOfferRoleTypeOrganisationStructureMapping.OrganisationStructureKeyColumn}, false);
                this.Relations.Add(this.relationFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure);
                this.relationFK_UserOrganisationStructure_OrganisationStructure = new global::System.Data.DataRelation("FK_UserOrganisationStructure_OrganisationStructure", new global::System.Data.DataColumn[] {
                        this.tableOrganisationStructure.OrganisationStructureKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableUserOrganisationStructure.OrganisationStructureKeyColumn}, false);
                this.Relations.Add(this.relationFK_UserOrganisationStructure_OrganisationStructure);
                this.relationFK_UserOrganisationStructure_ADUser = new global::System.Data.DataRelation("FK_UserOrganisationStructure_ADUser", new global::System.Data.DataColumn[] {
                        this.tableADUser.ADUserKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableUserOrganisationStructure.ADUserKeyColumn}, false);
                this.Relations.Add(this.relationFK_UserOrganisationStructure_ADUser);
                this.relationFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType = new global::System.Data.DataRelation("FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType", new global::System.Data.DataColumn[] {
                        this.tableOfferRoleType.OfferRoleTypeKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableOfferRoleTypeOrganisationStructureMapping.OfferRoleTypeKeyColumn}, false);
                this.Relations.Add(this.relationFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType);
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerialize_WorkflowAssignment()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeADUser()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeOfferRoleType()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeOfferRoleTypeOrganisationStructureMapping()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeOrganisationStructure()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeUserOrganisationStructure()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeWFAssignment()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private void SchemaChanged(object sender, global::System.ComponentModel.CollectionChangeEventArgs e)
            {
                if ((e.Action == global::System.ComponentModel.CollectionChangeAction.Remove))
                {
                    this.InitVars();
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedDataSetSchema(global::System.Xml.Schema.XmlSchemaSet xs)
            {
                WorkflowAssignment ds = new WorkflowAssignment();
                global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                global::System.Xml.Schema.XmlSchemaAny any = new global::System.Xml.Schema.XmlSchemaAny();
                any.Namespace = ds.Namespace;
                sequence.Items.Add(any);
                type.Particle = sequence;
                global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                if (xs.Contains(dsSchema.TargetNamespace))
                {
                    global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                    global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                    try
                    {
                        global::System.Xml.Schema.XmlSchema schema = null;
                        dsSchema.Write(s1);
                        for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                        {
                            schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                            s2.SetLength(0);
                            schema.Write(s2);
                            if ((s1.Length == s2.Length))
                            {
                                s1.Position = 0;
                                s2.Position = 0;
                                for (; ((s1.Position != s1.Length)
                                            && (s1.ReadByte() == s2.ReadByte())); )
                                {
                                    ;
                                }
                                if ((s1.Position == s1.Length))
                                {
                                    return type;
                                }
                            }
                        }
                    }
                    finally
                    {
                        if ((s1 != null))
                        {
                            s1.Close();
                        }
                        if ((s2 != null))
                        {
                            s2.Close();
                        }
                    }
                }
                xs.Add(dsSchema);
                return type;
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void WorkflowAssignmentRowChangeEventHandler(object sender, WorkflowAssignmentRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void ADUserRowChangeEventHandler(object sender, ADUserRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void OfferRoleTypeRowChangeEventHandler(object sender, OfferRoleTypeRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void OfferRoleTypeOrganisationStructureMappingRowChangeEventHandler(object sender, OfferRoleTypeOrganisationStructureMappingRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void OrganisationStructureRowChangeEventHandler(object sender, OrganisationStructureRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void UserOrganisationStructureRowChangeEventHandler(object sender, UserOrganisationStructureRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void WFAssignmentRowChangeEventHandler(object sender, WFAssignmentRowChangeEvent e);

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class WorkflowAssignmentDataTable : global::System.Data.DataTable, global::System.Collections.IEnumerable
            {
                private global::System.Data.DataColumn columnID;

                private global::System.Data.DataColumn columnInstanceID;

                private global::System.Data.DataColumn columnOfferRoleTypeOrganisationStructureMappingKey;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                private global::System.Data.DataColumn columnADUserKey;

                private global::System.Data.DataColumn columnState;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentDataTable()
                {
                    this.TableName = "WorkflowAssignment";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowAssignmentDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected WorkflowAssignmentDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn IDColumn
                {
                    get
                    {
                        return this.columnID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn InstanceIDColumn
                {
                    get
                    {
                        return this.columnInstanceID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeOrganisationStructureMappingKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeOrganisationStructureMappingKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserKeyColumn
                {
                    get
                    {
                        return this.columnADUserKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn StateColumn
                {
                    get
                    {
                        return this.columnState;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRow this[int index]
                {
                    get
                    {
                        return ((WorkflowAssignmentRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowAssignmentRowChangeEventHandler WorkflowAssignmentRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowAssignmentRowChangeEventHandler WorkflowAssignmentRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowAssignmentRowChangeEventHandler WorkflowAssignmentRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowAssignmentRowChangeEventHandler WorkflowAssignmentRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddWorkflowAssignmentRow(WorkflowAssignmentRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRow AddWorkflowAssignmentRow(string ID, long InstanceID, int OfferRoleTypeOrganisationStructureMappingKey, int GeneralStatusKey, int ADUserKey, string State)
                {
                    WorkflowAssignmentRow rowWorkflowAssignmentRow = ((WorkflowAssignmentRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        ID,
                        InstanceID,
                        OfferRoleTypeOrganisationStructureMappingKey,
                        GeneralStatusKey,
                        ADUserKey,
                        State};
                    rowWorkflowAssignmentRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowWorkflowAssignmentRow);
                    return rowWorkflowAssignmentRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRow FindByID(string ID)
                {
                    return ((WorkflowAssignmentRow)(this.Rows.Find(new object[] {
                            ID})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public virtual global::System.Collections.IEnumerator GetEnumerator()
                {
                    return this.Rows.GetEnumerator();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    WorkflowAssignmentDataTable cln = ((WorkflowAssignmentDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new WorkflowAssignmentDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnID = base.Columns["ID"];
                    this.columnInstanceID = base.Columns["InstanceID"];
                    this.columnOfferRoleTypeOrganisationStructureMappingKey = base.Columns["OfferRoleTypeOrganisationStructureMappingKey"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                    this.columnADUserKey = base.Columns["ADUserKey"];
                    this.columnState = base.Columns["State"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnID = new global::System.Data.DataColumn("ID", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnID);
                    this.columnInstanceID = new global::System.Data.DataColumn("InstanceID", typeof(long), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnInstanceID);
                    this.columnOfferRoleTypeOrganisationStructureMappingKey = new global::System.Data.DataColumn("OfferRoleTypeOrganisationStructureMappingKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeOrganisationStructureMappingKey);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.columnADUserKey = new global::System.Data.DataColumn("ADUserKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserKey);
                    this.columnState = new global::System.Data.DataColumn("State", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnState);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnID}, true));
                    this.columnID.AllowDBNull = false;
                    this.columnID.Unique = true;
                    this.columnID.Caption = "Column1";
                    this.columnInstanceID.Caption = "Column1";
                    this.columnOfferRoleTypeOrganisationStructureMappingKey.Caption = "Column1";
                    this.columnGeneralStatusKey.Caption = "Column1";
                    this.ExtendedProperties.Add("Generator_TablePropName", "_WorkflowAssignment");
                    this.ExtendedProperties.Add("Generator_UserTableName", "WorkflowAssignment");
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRow NewWorkflowAssignmentRow()
                {
                    return ((WorkflowAssignmentRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new WorkflowAssignmentRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(WorkflowAssignmentRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.WorkflowAssignmentRowChanged != null))
                    {
                        this.WorkflowAssignmentRowChanged(this, new WorkflowAssignmentRowChangeEvent(((WorkflowAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.WorkflowAssignmentRowChanging != null))
                    {
                        this.WorkflowAssignmentRowChanging(this, new WorkflowAssignmentRowChangeEvent(((WorkflowAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.WorkflowAssignmentRowDeleted != null))
                    {
                        this.WorkflowAssignmentRowDeleted(this, new WorkflowAssignmentRowChangeEvent(((WorkflowAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.WorkflowAssignmentRowDeleting != null))
                    {
                        this.WorkflowAssignmentRowDeleting(this, new WorkflowAssignmentRowChangeEvent(((WorkflowAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveWorkflowAssignmentRow(WorkflowAssignmentRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "WorkflowAssignmentDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class ADUserDataTable : global::System.Data.DataTable, global::System.Collections.IEnumerable
            {
                private global::System.Data.DataColumn columnADUserKey;

                private global::System.Data.DataColumn columnADUserName;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                private global::System.Data.DataColumn columnPassword;

                private global::System.Data.DataColumn columnPasswordQuestion;

                private global::System.Data.DataColumn columnPasswordAnswer;

                private global::System.Data.DataColumn columnLegalEntityKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserDataTable()
                {
                    this.TableName = "ADUser";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal ADUserDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected ADUserDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserKeyColumn
                {
                    get
                    {
                        return this.columnADUserKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserNameColumn
                {
                    get
                    {
                        return this.columnADUserName;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn PasswordColumn
                {
                    get
                    {
                        return this.columnPassword;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn PasswordQuestionColumn
                {
                    get
                    {
                        return this.columnPasswordQuestion;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn PasswordAnswerColumn
                {
                    get
                    {
                        return this.columnPasswordAnswer;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn LegalEntityKeyColumn
                {
                    get
                    {
                        return this.columnLegalEntityKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow this[int index]
                {
                    get
                    {
                        return ((ADUserRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event ADUserRowChangeEventHandler ADUserRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event ADUserRowChangeEventHandler ADUserRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event ADUserRowChangeEventHandler ADUserRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event ADUserRowChangeEventHandler ADUserRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddADUserRow(ADUserRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow AddADUserRow(string ADUserName, int GeneralStatusKey, string Password, string PasswordQuestion, string PasswordAnswer, int LegalEntityKey)
                {
                    ADUserRow rowADUserRow = ((ADUserRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        ADUserName,
                        GeneralStatusKey,
                        Password,
                        PasswordQuestion,
                        PasswordAnswer,
                        LegalEntityKey};
                    rowADUserRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowADUserRow);
                    return rowADUserRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow FindByADUserKey(int ADUserKey)
                {
                    return ((ADUserRow)(this.Rows.Find(new object[] {
                            ADUserKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public virtual global::System.Collections.IEnumerator GetEnumerator()
                {
                    return this.Rows.GetEnumerator();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    ADUserDataTable cln = ((ADUserDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new ADUserDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnADUserKey = base.Columns["ADUserKey"];
                    this.columnADUserName = base.Columns["ADUserName"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                    this.columnPassword = base.Columns["Password"];
                    this.columnPasswordQuestion = base.Columns["PasswordQuestion"];
                    this.columnPasswordAnswer = base.Columns["PasswordAnswer"];
                    this.columnLegalEntityKey = base.Columns["LegalEntityKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnADUserKey = new global::System.Data.DataColumn("ADUserKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserKey);
                    this.columnADUserName = new global::System.Data.DataColumn("ADUserName", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserName);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.columnPassword = new global::System.Data.DataColumn("Password", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnPassword);
                    this.columnPasswordQuestion = new global::System.Data.DataColumn("PasswordQuestion", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnPasswordQuestion);
                    this.columnPasswordAnswer = new global::System.Data.DataColumn("PasswordAnswer", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnPasswordAnswer);
                    this.columnLegalEntityKey = new global::System.Data.DataColumn("LegalEntityKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnLegalEntityKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnADUserKey}, true));
                    this.columnADUserKey.AutoIncrement = true;
                    this.columnADUserKey.AllowDBNull = false;
                    this.columnADUserKey.ReadOnly = true;
                    this.columnADUserKey.Unique = true;
                    this.columnADUserName.AllowDBNull = false;
                    this.columnADUserName.MaxLength = 100;
                    this.columnGeneralStatusKey.AllowDBNull = false;
                    this.columnPassword.MaxLength = 50;
                    this.columnPasswordQuestion.MaxLength = 100;
                    this.columnPasswordAnswer.MaxLength = 100;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow NewADUserRow()
                {
                    return ((ADUserRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new ADUserRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(ADUserRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.ADUserRowChanged != null))
                    {
                        this.ADUserRowChanged(this, new ADUserRowChangeEvent(((ADUserRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.ADUserRowChanging != null))
                    {
                        this.ADUserRowChanging(this, new ADUserRowChangeEvent(((ADUserRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.ADUserRowDeleted != null))
                    {
                        this.ADUserRowDeleted(this, new ADUserRowChangeEvent(((ADUserRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.ADUserRowDeleting != null))
                    {
                        this.ADUserRowDeleting(this, new ADUserRowChangeEvent(((ADUserRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveADUserRow(ADUserRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "ADUserDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class OfferRoleTypeDataTable : global::System.Data.DataTable, global::System.Collections.IEnumerable
            {
                private global::System.Data.DataColumn columnOfferRoleTypeKey;

                private global::System.Data.DataColumn columnDescription;

                private global::System.Data.DataColumn columnOfferRoleTypeGroupKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeDataTable()
                {
                    this.TableName = "OfferRoleType";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OfferRoleTypeDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected OfferRoleTypeDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn DescriptionColumn
                {
                    get
                    {
                        return this.columnDescription;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeGroupKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeGroupKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow this[int index]
                {
                    get
                    {
                        return ((OfferRoleTypeRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeRowChangeEventHandler OfferRoleTypeRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeRowChangeEventHandler OfferRoleTypeRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeRowChangeEventHandler OfferRoleTypeRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeRowChangeEventHandler OfferRoleTypeRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddOfferRoleTypeRow(OfferRoleTypeRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow AddOfferRoleTypeRow(string Description, int OfferRoleTypeGroupKey)
                {
                    OfferRoleTypeRow rowOfferRoleTypeRow = ((OfferRoleTypeRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        Description,
                        OfferRoleTypeGroupKey};
                    rowOfferRoleTypeRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowOfferRoleTypeRow);
                    return rowOfferRoleTypeRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow FindByOfferRoleTypeKey(int OfferRoleTypeKey)
                {
                    return ((OfferRoleTypeRow)(this.Rows.Find(new object[] {
                            OfferRoleTypeKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public virtual global::System.Collections.IEnumerator GetEnumerator()
                {
                    return this.Rows.GetEnumerator();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    OfferRoleTypeDataTable cln = ((OfferRoleTypeDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new OfferRoleTypeDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnOfferRoleTypeKey = base.Columns["OfferRoleTypeKey"];
                    this.columnDescription = base.Columns["Description"];
                    this.columnOfferRoleTypeGroupKey = base.Columns["OfferRoleTypeGroupKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnOfferRoleTypeKey = new global::System.Data.DataColumn("OfferRoleTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeKey);
                    this.columnDescription = new global::System.Data.DataColumn("Description", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnDescription);
                    this.columnOfferRoleTypeGroupKey = new global::System.Data.DataColumn("OfferRoleTypeGroupKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeGroupKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnOfferRoleTypeKey}, true));
                    this.columnOfferRoleTypeKey.AutoIncrement = true;
                    this.columnOfferRoleTypeKey.AllowDBNull = false;
                    this.columnOfferRoleTypeKey.ReadOnly = true;
                    this.columnOfferRoleTypeKey.Unique = true;
                    this.columnDescription.AllowDBNull = false;
                    this.columnDescription.MaxLength = 50;
                    this.columnOfferRoleTypeGroupKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow NewOfferRoleTypeRow()
                {
                    return ((OfferRoleTypeRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new OfferRoleTypeRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(OfferRoleTypeRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.OfferRoleTypeRowChanged != null))
                    {
                        this.OfferRoleTypeRowChanged(this, new OfferRoleTypeRowChangeEvent(((OfferRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.OfferRoleTypeRowChanging != null))
                    {
                        this.OfferRoleTypeRowChanging(this, new OfferRoleTypeRowChangeEvent(((OfferRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.OfferRoleTypeRowDeleted != null))
                    {
                        this.OfferRoleTypeRowDeleted(this, new OfferRoleTypeRowChangeEvent(((OfferRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.OfferRoleTypeRowDeleting != null))
                    {
                        this.OfferRoleTypeRowDeleting(this, new OfferRoleTypeRowChangeEvent(((OfferRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveOfferRoleTypeRow(OfferRoleTypeRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "OfferRoleTypeDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class OfferRoleTypeOrganisationStructureMappingDataTable : global::System.Data.DataTable, global::System.Collections.IEnumerable
            {
                private global::System.Data.DataColumn columnOfferRoleTypeOrganisationStructureMappingKey;

                private global::System.Data.DataColumn columnOfferRoleTypeKey;

                private global::System.Data.DataColumn columnOrganisationStructureKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingDataTable()
                {
                    this.TableName = "OfferRoleTypeOrganisationStructureMapping";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OfferRoleTypeOrganisationStructureMappingDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected OfferRoleTypeOrganisationStructureMappingDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeOrganisationStructureMappingKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeOrganisationStructureMappingKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow this[int index]
                {
                    get
                    {
                        return ((OfferRoleTypeOrganisationStructureMappingRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeOrganisationStructureMappingRowChangeEventHandler OfferRoleTypeOrganisationStructureMappingRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeOrganisationStructureMappingRowChangeEventHandler OfferRoleTypeOrganisationStructureMappingRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeOrganisationStructureMappingRowChangeEventHandler OfferRoleTypeOrganisationStructureMappingRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeOrganisationStructureMappingRowChangeEventHandler OfferRoleTypeOrganisationStructureMappingRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddOfferRoleTypeOrganisationStructureMappingRow(OfferRoleTypeOrganisationStructureMappingRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow AddOfferRoleTypeOrganisationStructureMappingRow(OfferRoleTypeRow parentOfferRoleTypeRowByFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType, OrganisationStructureRow parentOrganisationStructureRowByFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure)
                {
                    OfferRoleTypeOrganisationStructureMappingRow rowOfferRoleTypeOrganisationStructureMappingRow = ((OfferRoleTypeOrganisationStructureMappingRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        null,
                        null};
                    if ((parentOfferRoleTypeRowByFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType != null))
                    {
                        columnValuesArray[1] = parentOfferRoleTypeRowByFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType[0];
                    }
                    if ((parentOrganisationStructureRowByFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure != null))
                    {
                        columnValuesArray[2] = parentOrganisationStructureRowByFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure[0];
                    }
                    rowOfferRoleTypeOrganisationStructureMappingRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowOfferRoleTypeOrganisationStructureMappingRow);
                    return rowOfferRoleTypeOrganisationStructureMappingRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow FindByOfferRoleTypeOrganisationStructureMappingKey(int OfferRoleTypeOrganisationStructureMappingKey)
                {
                    return ((OfferRoleTypeOrganisationStructureMappingRow)(this.Rows.Find(new object[] {
                            OfferRoleTypeOrganisationStructureMappingKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public virtual global::System.Collections.IEnumerator GetEnumerator()
                {
                    return this.Rows.GetEnumerator();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    OfferRoleTypeOrganisationStructureMappingDataTable cln = ((OfferRoleTypeOrganisationStructureMappingDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new OfferRoleTypeOrganisationStructureMappingDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnOfferRoleTypeOrganisationStructureMappingKey = base.Columns["OfferRoleTypeOrganisationStructureMappingKey"];
                    this.columnOfferRoleTypeKey = base.Columns["OfferRoleTypeKey"];
                    this.columnOrganisationStructureKey = base.Columns["OrganisationStructureKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnOfferRoleTypeOrganisationStructureMappingKey = new global::System.Data.DataColumn("OfferRoleTypeOrganisationStructureMappingKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeOrganisationStructureMappingKey);
                    this.columnOfferRoleTypeKey = new global::System.Data.DataColumn("OfferRoleTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeKey);
                    this.columnOrganisationStructureKey = new global::System.Data.DataColumn("OrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationStructureKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnOfferRoleTypeOrganisationStructureMappingKey}, true));
                    this.columnOfferRoleTypeOrganisationStructureMappingKey.AutoIncrement = true;
                    this.columnOfferRoleTypeOrganisationStructureMappingKey.AllowDBNull = false;
                    this.columnOfferRoleTypeOrganisationStructureMappingKey.ReadOnly = true;
                    this.columnOfferRoleTypeOrganisationStructureMappingKey.Unique = true;
                    this.columnOfferRoleTypeKey.AllowDBNull = false;
                    this.columnOrganisationStructureKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow NewOfferRoleTypeOrganisationStructureMappingRow()
                {
                    return ((OfferRoleTypeOrganisationStructureMappingRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new OfferRoleTypeOrganisationStructureMappingRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(OfferRoleTypeOrganisationStructureMappingRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.OfferRoleTypeOrganisationStructureMappingRowChanged != null))
                    {
                        this.OfferRoleTypeOrganisationStructureMappingRowChanged(this, new OfferRoleTypeOrganisationStructureMappingRowChangeEvent(((OfferRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.OfferRoleTypeOrganisationStructureMappingRowChanging != null))
                    {
                        this.OfferRoleTypeOrganisationStructureMappingRowChanging(this, new OfferRoleTypeOrganisationStructureMappingRowChangeEvent(((OfferRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.OfferRoleTypeOrganisationStructureMappingRowDeleted != null))
                    {
                        this.OfferRoleTypeOrganisationStructureMappingRowDeleted(this, new OfferRoleTypeOrganisationStructureMappingRowChangeEvent(((OfferRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.OfferRoleTypeOrganisationStructureMappingRowDeleting != null))
                    {
                        this.OfferRoleTypeOrganisationStructureMappingRowDeleting(this, new OfferRoleTypeOrganisationStructureMappingRowChangeEvent(((OfferRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveOfferRoleTypeOrganisationStructureMappingRow(OfferRoleTypeOrganisationStructureMappingRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "OfferRoleTypeOrganisationStructureMappingDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class OrganisationStructureDataTable : global::System.Data.DataTable, global::System.Collections.IEnumerable
            {
                private global::System.Data.DataColumn columnOrganisationStructureKey;

                private global::System.Data.DataColumn columnParentKey;

                private global::System.Data.DataColumn columnDescription;

                private global::System.Data.DataColumn columnOrganisationTypeKey;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureDataTable()
                {
                    this.TableName = "OrganisationStructure";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OrganisationStructureDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected OrganisationStructureDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ParentKeyColumn
                {
                    get
                    {
                        return this.columnParentKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn DescriptionColumn
                {
                    get
                    {
                        return this.columnDescription;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationTypeKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow this[int index]
                {
                    get
                    {
                        return ((OrganisationStructureRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OrganisationStructureRowChangeEventHandler OrganisationStructureRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OrganisationStructureRowChangeEventHandler OrganisationStructureRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OrganisationStructureRowChangeEventHandler OrganisationStructureRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OrganisationStructureRowChangeEventHandler OrganisationStructureRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddOrganisationStructureRow(OrganisationStructureRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow AddOrganisationStructureRow(int ParentKey, string Description, int OrganisationTypeKey, int GeneralStatusKey)
                {
                    OrganisationStructureRow rowOrganisationStructureRow = ((OrganisationStructureRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        ParentKey,
                        Description,
                        OrganisationTypeKey,
                        GeneralStatusKey};
                    rowOrganisationStructureRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowOrganisationStructureRow);
                    return rowOrganisationStructureRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow FindByOrganisationStructureKey(int OrganisationStructureKey)
                {
                    return ((OrganisationStructureRow)(this.Rows.Find(new object[] {
                            OrganisationStructureKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public virtual global::System.Collections.IEnumerator GetEnumerator()
                {
                    return this.Rows.GetEnumerator();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    OrganisationStructureDataTable cln = ((OrganisationStructureDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new OrganisationStructureDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnOrganisationStructureKey = base.Columns["OrganisationStructureKey"];
                    this.columnParentKey = base.Columns["ParentKey"];
                    this.columnDescription = base.Columns["Description"];
                    this.columnOrganisationTypeKey = base.Columns["OrganisationTypeKey"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnOrganisationStructureKey = new global::System.Data.DataColumn("OrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationStructureKey);
                    this.columnParentKey = new global::System.Data.DataColumn("ParentKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnParentKey);
                    this.columnDescription = new global::System.Data.DataColumn("Description", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnDescription);
                    this.columnOrganisationTypeKey = new global::System.Data.DataColumn("OrganisationTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationTypeKey);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnOrganisationStructureKey}, true));
                    this.columnOrganisationStructureKey.AutoIncrement = true;
                    this.columnOrganisationStructureKey.AllowDBNull = false;
                    this.columnOrganisationStructureKey.ReadOnly = true;
                    this.columnOrganisationStructureKey.Unique = true;
                    this.columnDescription.AllowDBNull = false;
                    this.columnDescription.MaxLength = 255;
                    this.columnGeneralStatusKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow NewOrganisationStructureRow()
                {
                    return ((OrganisationStructureRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new OrganisationStructureRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(OrganisationStructureRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.OrganisationStructureRowChanged != null))
                    {
                        this.OrganisationStructureRowChanged(this, new OrganisationStructureRowChangeEvent(((OrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.OrganisationStructureRowChanging != null))
                    {
                        this.OrganisationStructureRowChanging(this, new OrganisationStructureRowChangeEvent(((OrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.OrganisationStructureRowDeleted != null))
                    {
                        this.OrganisationStructureRowDeleted(this, new OrganisationStructureRowChangeEvent(((OrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.OrganisationStructureRowDeleting != null))
                    {
                        this.OrganisationStructureRowDeleting(this, new OrganisationStructureRowChangeEvent(((OrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveOrganisationStructureRow(OrganisationStructureRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "OrganisationStructureDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class UserOrganisationStructureDataTable : global::System.Data.DataTable, global::System.Collections.IEnumerable
            {
                private global::System.Data.DataColumn columnUserOrganisationStructureKey;

                private global::System.Data.DataColumn columnADUserKey;

                private global::System.Data.DataColumn columnOrganisationStructureKey;

                private global::System.Data.DataColumn columnGenericKey;

                private global::System.Data.DataColumn columnGenericKeyTypeKey;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureDataTable()
                {
                    this.TableName = "UserOrganisationStructure";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal UserOrganisationStructureDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected UserOrganisationStructureDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn UserOrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnUserOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserKeyColumn
                {
                    get
                    {
                        return this.columnADUserKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GenericKeyColumn
                {
                    get
                    {
                        return this.columnGenericKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GenericKeyTypeKeyColumn
                {
                    get
                    {
                        return this.columnGenericKeyTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow this[int index]
                {
                    get
                    {
                        return ((UserOrganisationStructureRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRowChangeEventHandler UserOrganisationStructureRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRowChangeEventHandler UserOrganisationStructureRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRowChangeEventHandler UserOrganisationStructureRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRowChangeEventHandler UserOrganisationStructureRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddUserOrganisationStructureRow(UserOrganisationStructureRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow AddUserOrganisationStructureRow(ADUserRow parentADUserRowByFK_UserOrganisationStructure_ADUser, OrganisationStructureRow parentOrganisationStructureRowByFK_UserOrganisationStructure_OrganisationStructure, int GenericKey, int GenericKeyTypeKey, int GeneralStatusKey)
                {
                    UserOrganisationStructureRow rowUserOrganisationStructureRow = ((UserOrganisationStructureRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        null,
                        null,
                        GenericKey,
                        GenericKeyTypeKey,
                        GeneralStatusKey};
                    if ((parentADUserRowByFK_UserOrganisationStructure_ADUser != null))
                    {
                        columnValuesArray[1] = parentADUserRowByFK_UserOrganisationStructure_ADUser[0];
                    }
                    if ((parentOrganisationStructureRowByFK_UserOrganisationStructure_OrganisationStructure != null))
                    {
                        columnValuesArray[2] = parentOrganisationStructureRowByFK_UserOrganisationStructure_OrganisationStructure[0];
                    }
                    rowUserOrganisationStructureRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowUserOrganisationStructureRow);
                    return rowUserOrganisationStructureRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow FindByUserOrganisationStructureKey(int UserOrganisationStructureKey)
                {
                    return ((UserOrganisationStructureRow)(this.Rows.Find(new object[] {
                            UserOrganisationStructureKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public virtual global::System.Collections.IEnumerator GetEnumerator()
                {
                    return this.Rows.GetEnumerator();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    UserOrganisationStructureDataTable cln = ((UserOrganisationStructureDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new UserOrganisationStructureDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnUserOrganisationStructureKey = base.Columns["UserOrganisationStructureKey"];
                    this.columnADUserKey = base.Columns["ADUserKey"];
                    this.columnOrganisationStructureKey = base.Columns["OrganisationStructureKey"];
                    this.columnGenericKey = base.Columns["GenericKey"];
                    this.columnGenericKeyTypeKey = base.Columns["GenericKeyTypeKey"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnUserOrganisationStructureKey = new global::System.Data.DataColumn("UserOrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnUserOrganisationStructureKey);
                    this.columnADUserKey = new global::System.Data.DataColumn("ADUserKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserKey);
                    this.columnOrganisationStructureKey = new global::System.Data.DataColumn("OrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationStructureKey);
                    this.columnGenericKey = new global::System.Data.DataColumn("GenericKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGenericKey);
                    this.columnGenericKeyTypeKey = new global::System.Data.DataColumn("GenericKeyTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGenericKeyTypeKey);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnUserOrganisationStructureKey}, true));
                    this.columnUserOrganisationStructureKey.AutoIncrement = true;
                    this.columnUserOrganisationStructureKey.AllowDBNull = false;
                    this.columnUserOrganisationStructureKey.ReadOnly = true;
                    this.columnUserOrganisationStructureKey.Unique = true;
                    this.columnADUserKey.AllowDBNull = false;
                    this.columnOrganisationStructureKey.AllowDBNull = false;
                    this.columnGeneralStatusKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow NewUserOrganisationStructureRow()
                {
                    return ((UserOrganisationStructureRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new UserOrganisationStructureRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(UserOrganisationStructureRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.UserOrganisationStructureRowChanged != null))
                    {
                        this.UserOrganisationStructureRowChanged(this, new UserOrganisationStructureRowChangeEvent(((UserOrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.UserOrganisationStructureRowChanging != null))
                    {
                        this.UserOrganisationStructureRowChanging(this, new UserOrganisationStructureRowChangeEvent(((UserOrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.UserOrganisationStructureRowDeleted != null))
                    {
                        this.UserOrganisationStructureRowDeleted(this, new UserOrganisationStructureRowChangeEvent(((UserOrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.UserOrganisationStructureRowDeleting != null))
                    {
                        this.UserOrganisationStructureRowDeleting(this, new UserOrganisationStructureRowChangeEvent(((UserOrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveUserOrganisationStructureRow(UserOrganisationStructureRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "UserOrganisationStructureDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class WFAssignmentDataTable : global::System.Data.DataTable, global::System.Collections.IEnumerable
            {
                private global::System.Data.DataColumn columnID;

                private global::System.Data.DataColumn columnIID;

                private global::System.Data.DataColumn columnADUserKey;

                private global::System.Data.DataColumn columnBlaKey;

                private global::System.Data.DataColumn columnGSKey;

                private global::System.Data.DataColumn columnADUserName;

                private global::System.Data.DataColumn columnORT;

                private global::System.Data.DataColumn columnOfferRoleTypeKey;

                private global::System.Data.DataColumn columnDescription;

                private global::System.Data.DataColumn columnOrganisationStructureKey;

                private global::System.Data.DataColumn columnParentKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentDataTable()
                {
                    this.TableName = "WFAssignment";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WFAssignmentDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected WFAssignmentDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn IDColumn
                {
                    get
                    {
                        return this.columnID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn IIDColumn
                {
                    get
                    {
                        return this.columnIID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserKeyColumn
                {
                    get
                    {
                        return this.columnADUserKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn BlaKeyColumn
                {
                    get
                    {
                        return this.columnBlaKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GSKeyColumn
                {
                    get
                    {
                        return this.columnGSKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserNameColumn
                {
                    get
                    {
                        return this.columnADUserName;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ORTColumn
                {
                    get
                    {
                        return this.columnORT;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn DescriptionColumn
                {
                    get
                    {
                        return this.columnDescription;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ParentKeyColumn
                {
                    get
                    {
                        return this.columnParentKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentRow this[int index]
                {
                    get
                    {
                        return ((WFAssignmentRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFAssignmentRowChangeEventHandler WFAssignmentRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFAssignmentRowChangeEventHandler WFAssignmentRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFAssignmentRowChangeEventHandler WFAssignmentRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFAssignmentRowChangeEventHandler WFAssignmentRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddWFAssignmentRow(WFAssignmentRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentRow AddWFAssignmentRow(int ID, int IID, int ADUserKey, int BlaKey, int GSKey, string ADUserName, string ORT, int OfferRoleTypeKey, string Description, int OrganisationStructureKey, int ParentKey)
                {
                    WFAssignmentRow rowWFAssignmentRow = ((WFAssignmentRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        ID,
                        IID,
                        ADUserKey,
                        BlaKey,
                        GSKey,
                        ADUserName,
                        ORT,
                        OfferRoleTypeKey,
                        Description,
                        OrganisationStructureKey,
                        ParentKey};
                    rowWFAssignmentRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowWFAssignmentRow);
                    return rowWFAssignmentRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public virtual global::System.Collections.IEnumerator GetEnumerator()
                {
                    return this.Rows.GetEnumerator();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    WFAssignmentDataTable cln = ((WFAssignmentDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new WFAssignmentDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnID = base.Columns["ID"];
                    this.columnIID = base.Columns["IID"];
                    this.columnADUserKey = base.Columns["ADUserKey"];
                    this.columnBlaKey = base.Columns["BlaKey"];
                    this.columnGSKey = base.Columns["GSKey"];
                    this.columnADUserName = base.Columns["ADUserName"];
                    this.columnORT = base.Columns["ORT"];
                    this.columnOfferRoleTypeKey = base.Columns["OfferRoleTypeKey"];
                    this.columnDescription = base.Columns["Description"];
                    this.columnOrganisationStructureKey = base.Columns["OrganisationStructureKey"];
                    this.columnParentKey = base.Columns["ParentKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnID = new global::System.Data.DataColumn("ID", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnID);
                    this.columnIID = new global::System.Data.DataColumn("IID", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnIID);
                    this.columnADUserKey = new global::System.Data.DataColumn("ADUserKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserKey);
                    this.columnBlaKey = new global::System.Data.DataColumn("BlaKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnBlaKey);
                    this.columnGSKey = new global::System.Data.DataColumn("GSKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGSKey);
                    this.columnADUserName = new global::System.Data.DataColumn("ADUserName", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserName);
                    this.columnORT = new global::System.Data.DataColumn("ORT", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnORT);
                    this.columnOfferRoleTypeKey = new global::System.Data.DataColumn("OfferRoleTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeKey);
                    this.columnDescription = new global::System.Data.DataColumn("Description", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnDescription);
                    this.columnOrganisationStructureKey = new global::System.Data.DataColumn("OrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationStructureKey);
                    this.columnParentKey = new global::System.Data.DataColumn("ParentKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnParentKey);
                    this.columnIID.Caption = "ID";
                    this.columnADUserKey.Caption = "ID";
                    this.columnBlaKey.Caption = "ID";
                    this.columnGSKey.Caption = "ID";
                    this.columnADUserName.Caption = "ID";
                    this.columnORT.Caption = "ID";
                    this.columnOfferRoleTypeKey.Caption = "ID";
                    this.columnDescription.Caption = "ID";
                    this.columnOrganisationStructureKey.Caption = "ID";
                    this.columnParentKey.DefaultValue = ((int)(-1));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentRow NewWFAssignmentRow()
                {
                    return ((WFAssignmentRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new WFAssignmentRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(WFAssignmentRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.WFAssignmentRowChanged != null))
                    {
                        this.WFAssignmentRowChanged(this, new WFAssignmentRowChangeEvent(((WFAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.WFAssignmentRowChanging != null))
                    {
                        this.WFAssignmentRowChanging(this, new WFAssignmentRowChangeEvent(((WFAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.WFAssignmentRowDeleted != null))
                    {
                        this.WFAssignmentRowDeleted(this, new WFAssignmentRowChangeEvent(((WFAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.WFAssignmentRowDeleting != null))
                    {
                        this.WFAssignmentRowDeleting(this, new WFAssignmentRowChangeEvent(((WFAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveWFAssignmentRow(WFAssignmentRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "WFAssignmentDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class WorkflowAssignmentRow : global::System.Data.DataRow
            {
                private WorkflowAssignmentDataTable tableWorkflowAssignment;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowAssignmentRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableWorkflowAssignment = ((WorkflowAssignmentDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string ID
                {
                    get
                    {
                        return ((string)(this[this.tableWorkflowAssignment.IDColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.IDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public long InstanceID
                {
                    get
                    {
                        try
                        {
                            return ((long)(this[this.tableWorkflowAssignment.InstanceIDColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'InstanceID\' in table \'WorkflowAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.InstanceIDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeOrganisationStructureMappingKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWorkflowAssignment.OfferRoleTypeOrganisationStructureMappingKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'OfferRoleTypeOrganisationStructureMappingKey\' in table \'Wor" +
                                    "kflowAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.OfferRoleTypeOrganisationStructureMappingKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWorkflowAssignment.GeneralStatusKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'GeneralStatusKey\' in table \'WorkflowAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.GeneralStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ADUserKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWorkflowAssignment.ADUserKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ADUserKey\' in table \'WorkflowAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.ADUserKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string State
                {
                    get
                    {
                        if (this.IsStateNull())
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return ((string)(this[this.tableWorkflowAssignment.StateColumn]));
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.StateColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsInstanceIDNull()
                {
                    return this.IsNull(this.tableWorkflowAssignment.InstanceIDColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetInstanceIDNull()
                {
                    this[this.tableWorkflowAssignment.InstanceIDColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsOfferRoleTypeOrganisationStructureMappingKeyNull()
                {
                    return this.IsNull(this.tableWorkflowAssignment.OfferRoleTypeOrganisationStructureMappingKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetOfferRoleTypeOrganisationStructureMappingKeyNull()
                {
                    this[this.tableWorkflowAssignment.OfferRoleTypeOrganisationStructureMappingKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsGeneralStatusKeyNull()
                {
                    return this.IsNull(this.tableWorkflowAssignment.GeneralStatusKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetGeneralStatusKeyNull()
                {
                    this[this.tableWorkflowAssignment.GeneralStatusKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsADUserKeyNull()
                {
                    return this.IsNull(this.tableWorkflowAssignment.ADUserKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetADUserKeyNull()
                {
                    this[this.tableWorkflowAssignment.ADUserKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsStateNull()
                {
                    return this.IsNull(this.tableWorkflowAssignment.StateColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetStateNull()
                {
                    this[this.tableWorkflowAssignment.StateColumn] = global::System.Convert.DBNull;
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class ADUserRow : global::System.Data.DataRow
            {
                private ADUserDataTable tableADUser;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal ADUserRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableADUser = ((ADUserDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ADUserKey
                {
                    get
                    {
                        return ((int)(this[this.tableADUser.ADUserKeyColumn]));
                    }
                    set
                    {
                        this[this.tableADUser.ADUserKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string ADUserName
                {
                    get
                    {
                        return ((string)(this[this.tableADUser.ADUserNameColumn]));
                    }
                    set
                    {
                        this[this.tableADUser.ADUserNameColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        return ((int)(this[this.tableADUser.GeneralStatusKeyColumn]));
                    }
                    set
                    {
                        this[this.tableADUser.GeneralStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Password
                {
                    get
                    {
                        if (this.IsPasswordNull())
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return ((string)(this[this.tableADUser.PasswordColumn]));
                        }
                    }
                    set
                    {
                        this[this.tableADUser.PasswordColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string PasswordQuestion
                {
                    get
                    {
                        if (this.IsPasswordQuestionNull())
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return ((string)(this[this.tableADUser.PasswordQuestionColumn]));
                        }
                    }
                    set
                    {
                        this[this.tableADUser.PasswordQuestionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string PasswordAnswer
                {
                    get
                    {
                        if (this.IsPasswordAnswerNull())
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return ((string)(this[this.tableADUser.PasswordAnswerColumn]));
                        }
                    }
                    set
                    {
                        this[this.tableADUser.PasswordAnswerColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int LegalEntityKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableADUser.LegalEntityKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'LegalEntityKey\' in table \'ADUser\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableADUser.LegalEntityKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsPasswordNull()
                {
                    return this.IsNull(this.tableADUser.PasswordColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetPasswordNull()
                {
                    this[this.tableADUser.PasswordColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsPasswordQuestionNull()
                {
                    return this.IsNull(this.tableADUser.PasswordQuestionColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetPasswordQuestionNull()
                {
                    this[this.tableADUser.PasswordQuestionColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsPasswordAnswerNull()
                {
                    return this.IsNull(this.tableADUser.PasswordAnswerColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetPasswordAnswerNull()
                {
                    this[this.tableADUser.PasswordAnswerColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsLegalEntityKeyNull()
                {
                    return this.IsNull(this.tableADUser.LegalEntityKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetLegalEntityKeyNull()
                {
                    this[this.tableADUser.LegalEntityKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow[] GetUserOrganisationStructureRows()
                {
                    if ((this.Table.ChildRelations["FK_UserOrganisationStructure_ADUser"] == null))
                    {
                        return new UserOrganisationStructureRow[0];
                    }
                    else
                    {
                        return ((UserOrganisationStructureRow[])(base.GetChildRows(this.Table.ChildRelations["FK_UserOrganisationStructure_ADUser"])));
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class OfferRoleTypeRow : global::System.Data.DataRow
            {
                private OfferRoleTypeDataTable tableOfferRoleType;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OfferRoleTypeRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableOfferRoleType = ((OfferRoleTypeDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeKey
                {
                    get
                    {
                        return ((int)(this[this.tableOfferRoleType.OfferRoleTypeKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleType.OfferRoleTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Description
                {
                    get
                    {
                        return ((string)(this[this.tableOfferRoleType.DescriptionColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleType.DescriptionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeGroupKey
                {
                    get
                    {
                        return ((int)(this[this.tableOfferRoleType.OfferRoleTypeGroupKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleType.OfferRoleTypeGroupKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow[] GetOfferRoleTypeOrganisationStructureMappingRows()
                {
                    if ((this.Table.ChildRelations["FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType"] == null))
                    {
                        return new OfferRoleTypeOrganisationStructureMappingRow[0];
                    }
                    else
                    {
                        return ((OfferRoleTypeOrganisationStructureMappingRow[])(base.GetChildRows(this.Table.ChildRelations["FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType"])));
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class OfferRoleTypeOrganisationStructureMappingRow : global::System.Data.DataRow
            {
                private OfferRoleTypeOrganisationStructureMappingDataTable tableOfferRoleTypeOrganisationStructureMapping;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OfferRoleTypeOrganisationStructureMappingRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableOfferRoleTypeOrganisationStructureMapping = ((OfferRoleTypeOrganisationStructureMappingDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeOrganisationStructureMappingKey
                {
                    get
                    {
                        return ((int)(this[this.tableOfferRoleTypeOrganisationStructureMapping.OfferRoleTypeOrganisationStructureMappingKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleTypeOrganisationStructureMapping.OfferRoleTypeOrganisationStructureMappingKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeKey
                {
                    get
                    {
                        return ((int)(this[this.tableOfferRoleTypeOrganisationStructureMapping.OfferRoleTypeKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleTypeOrganisationStructureMapping.OfferRoleTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationStructureKey
                {
                    get
                    {
                        return ((int)(this[this.tableOfferRoleTypeOrganisationStructureMapping.OrganisationStructureKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleTypeOrganisationStructureMapping.OrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow OrganisationStructureRow
                {
                    get
                    {
                        return ((OrganisationStructureRow)(this.GetParentRow(this.Table.ParentRelations["FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure"]);
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow OfferRoleTypeRow
                {
                    get
                    {
                        return ((OfferRoleTypeRow)(this.GetParentRow(this.Table.ParentRelations["FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType"]);
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class OrganisationStructureRow : global::System.Data.DataRow
            {
                private OrganisationStructureDataTable tableOrganisationStructure;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OrganisationStructureRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableOrganisationStructure = ((OrganisationStructureDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationStructureKey
                {
                    get
                    {
                        return ((int)(this[this.tableOrganisationStructure.OrganisationStructureKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOrganisationStructure.OrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ParentKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableOrganisationStructure.ParentKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ParentKey\' in table \'OrganisationStructure\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableOrganisationStructure.ParentKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Description
                {
                    get
                    {
                        return ((string)(this[this.tableOrganisationStructure.DescriptionColumn]));
                    }
                    set
                    {
                        this[this.tableOrganisationStructure.DescriptionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationTypeKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableOrganisationStructure.OrganisationTypeKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'OrganisationTypeKey\' in table \'OrganisationStructure\' is DB" +
                                    "Null.", e);
                        }
                    }
                    set
                    {
                        this[this.tableOrganisationStructure.OrganisationTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        return ((int)(this[this.tableOrganisationStructure.GeneralStatusKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOrganisationStructure.GeneralStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsParentKeyNull()
                {
                    return this.IsNull(this.tableOrganisationStructure.ParentKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetParentKeyNull()
                {
                    this[this.tableOrganisationStructure.ParentKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsOrganisationTypeKeyNull()
                {
                    return this.IsNull(this.tableOrganisationStructure.OrganisationTypeKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetOrganisationTypeKeyNull()
                {
                    this[this.tableOrganisationStructure.OrganisationTypeKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow[] GetOfferRoleTypeOrganisationStructureMappingRows()
                {
                    if ((this.Table.ChildRelations["FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure"] == null))
                    {
                        return new OfferRoleTypeOrganisationStructureMappingRow[0];
                    }
                    else
                    {
                        return ((OfferRoleTypeOrganisationStructureMappingRow[])(base.GetChildRows(this.Table.ChildRelations["FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure"])));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow[] GetUserOrganisationStructureRows()
                {
                    if ((this.Table.ChildRelations["FK_UserOrganisationStructure_OrganisationStructure"] == null))
                    {
                        return new UserOrganisationStructureRow[0];
                    }
                    else
                    {
                        return ((UserOrganisationStructureRow[])(base.GetChildRows(this.Table.ChildRelations["FK_UserOrganisationStructure_OrganisationStructure"])));
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class UserOrganisationStructureRow : global::System.Data.DataRow
            {
                private UserOrganisationStructureDataTable tableUserOrganisationStructure;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal UserOrganisationStructureRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableUserOrganisationStructure = ((UserOrganisationStructureDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int UserOrganisationStructureKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructure.UserOrganisationStructureKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.UserOrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ADUserKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructure.ADUserKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.ADUserKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationStructureKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructure.OrganisationStructureKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.OrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GenericKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableUserOrganisationStructure.GenericKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'GenericKey\' in table \'UserOrganisationStructure\' is DBNull." +
                                    "", e);
                        }
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.GenericKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GenericKeyTypeKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableUserOrganisationStructure.GenericKeyTypeKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'GenericKeyTypeKey\' in table \'UserOrganisationStructure\' is " +
                                    "DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.GenericKeyTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructure.GeneralStatusKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.GeneralStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow OrganisationStructureRow
                {
                    get
                    {
                        return ((OrganisationStructureRow)(this.GetParentRow(this.Table.ParentRelations["FK_UserOrganisationStructure_OrganisationStructure"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["FK_UserOrganisationStructure_OrganisationStructure"]);
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow ADUserRow
                {
                    get
                    {
                        return ((ADUserRow)(this.GetParentRow(this.Table.ParentRelations["FK_UserOrganisationStructure_ADUser"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["FK_UserOrganisationStructure_ADUser"]);
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsGenericKeyNull()
                {
                    return this.IsNull(this.tableUserOrganisationStructure.GenericKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetGenericKeyNull()
                {
                    this[this.tableUserOrganisationStructure.GenericKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsGenericKeyTypeKeyNull()
                {
                    return this.IsNull(this.tableUserOrganisationStructure.GenericKeyTypeKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetGenericKeyTypeKeyNull()
                {
                    this[this.tableUserOrganisationStructure.GenericKeyTypeKeyColumn] = global::System.Convert.DBNull;
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class WFAssignmentRow : global::System.Data.DataRow
            {
                private WFAssignmentDataTable tableWFAssignment;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WFAssignmentRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableWFAssignment = ((WFAssignmentDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ID
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.IDColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ID\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.IDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int IID
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.IIDColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'IID\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.IIDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ADUserKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.ADUserKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ADUserKey\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.ADUserKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int BlaKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.BlaKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'BlaKey\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.BlaKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GSKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.GSKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'GSKey\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.GSKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string ADUserName
                {
                    get
                    {
                        try
                        {
                            return ((string)(this[this.tableWFAssignment.ADUserNameColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ADUserName\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.ADUserNameColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string ORT
                {
                    get
                    {
                        try
                        {
                            return ((string)(this[this.tableWFAssignment.ORTColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ORT\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.ORTColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.OfferRoleTypeKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'OfferRoleTypeKey\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.OfferRoleTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Description
                {
                    get
                    {
                        try
                        {
                            return ((string)(this[this.tableWFAssignment.DescriptionColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'Description\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.DescriptionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationStructureKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.OrganisationStructureKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'OrganisationStructureKey\' in table \'WFAssignment\' is DBNull" +
                                    ".", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.OrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ParentKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.ParentKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ParentKey\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.ParentKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsIDNull()
                {
                    return this.IsNull(this.tableWFAssignment.IDColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetIDNull()
                {
                    this[this.tableWFAssignment.IDColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsIIDNull()
                {
                    return this.IsNull(this.tableWFAssignment.IIDColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetIIDNull()
                {
                    this[this.tableWFAssignment.IIDColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsADUserKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.ADUserKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetADUserKeyNull()
                {
                    this[this.tableWFAssignment.ADUserKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsBlaKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.BlaKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetBlaKeyNull()
                {
                    this[this.tableWFAssignment.BlaKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsGSKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.GSKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetGSKeyNull()
                {
                    this[this.tableWFAssignment.GSKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsADUserNameNull()
                {
                    return this.IsNull(this.tableWFAssignment.ADUserNameColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetADUserNameNull()
                {
                    this[this.tableWFAssignment.ADUserNameColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsORTNull()
                {
                    return this.IsNull(this.tableWFAssignment.ORTColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetORTNull()
                {
                    this[this.tableWFAssignment.ORTColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsOfferRoleTypeKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.OfferRoleTypeKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetOfferRoleTypeKeyNull()
                {
                    this[this.tableWFAssignment.OfferRoleTypeKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsDescriptionNull()
                {
                    return this.IsNull(this.tableWFAssignment.DescriptionColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetDescriptionNull()
                {
                    this[this.tableWFAssignment.DescriptionColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsOrganisationStructureKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.OrganisationStructureKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetOrganisationStructureKeyNull()
                {
                    this[this.tableWFAssignment.OrganisationStructureKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsParentKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.ParentKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetParentKeyNull()
                {
                    this[this.tableWFAssignment.ParentKeyColumn] = global::System.Convert.DBNull;
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class WorkflowAssignmentRowChangeEvent : global::System.EventArgs
            {
                private WorkflowAssignmentRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRowChangeEvent(WorkflowAssignmentRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class ADUserRowChangeEvent : global::System.EventArgs
            {
                private ADUserRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRowChangeEvent(ADUserRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class OfferRoleTypeRowChangeEvent : global::System.EventArgs
            {
                private OfferRoleTypeRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRowChangeEvent(OfferRoleTypeRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class OfferRoleTypeOrganisationStructureMappingRowChangeEvent : global::System.EventArgs
            {
                private OfferRoleTypeOrganisationStructureMappingRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRowChangeEvent(OfferRoleTypeOrganisationStructureMappingRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class OrganisationStructureRowChangeEvent : global::System.EventArgs
            {
                private OrganisationStructureRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRowChangeEvent(OrganisationStructureRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class UserOrganisationStructureRowChangeEvent : global::System.EventArgs
            {
                private UserOrganisationStructureRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRowChangeEvent(UserOrganisationStructureRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class WFAssignmentRowChangeEvent : global::System.EventArgs
            {
                private WFAssignmentRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentRowChangeEvent(WFAssignmentRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }
        }
    }

    namespace WorkflowRole
    {
        /// <summary>
        ///Represents a strongly typed in-memory cache of data.
        ///</summary>
        [global::System.Serializable()]
        [global::System.ComponentModel.DesignerCategoryAttribute("code")]
        [global::System.ComponentModel.ToolboxItem(true)]
        [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedDataSetSchema")]
        [global::System.Xml.Serialization.XmlRootAttribute("WorkflowAssignment")]
        [global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.DataSet")]
        public partial class WorkflowAssignment : global::System.Data.DataSet
        {
            private WorkflowAssignmentDataTable tableWorkflowAssignment;

            private ADUserDataTable tableADUser;

            private OfferRoleTypeDataTable tableOfferRoleType;

            private OfferRoleTypeOrganisationStructureMappingDataTable tableOfferRoleTypeOrganisationStructureMapping;

            private OrganisationStructureDataTable tableOrganisationStructure;

            private UserOrganisationStructureDataTable tableUserOrganisationStructure;

            private WFAssignmentDataTable tableWFAssignment;

            //private RoundRobinPointerDataTable tableRoundRobinPointer;

            private RoundRobinPointerDefinitionDataTable tableRoundRobinPointerDefinition;

            private UserOrganisationStructureRoundRobinStatusDataTable tableUserOrganisationStructureRoundRobinStatus;

            private WorkflowRoleAssignmentDataTable tableWorkflowRoleAssignment;

            private WorkflowRoleTypeGroupDataTable tableWorkflowRoleTypeGroup;

            private WorkflowRoleTypeOrganisationStructureMappingDataTable tableWorkflowRoleTypeOrganisationStructureMapping;

            private WorkflowRoleTypeDataTable tableWorkflowRoleType;

            private WFRAssignmentDataTable tableWFRAssignment;

            private global::System.Data.DataRelation relationFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure;

            private global::System.Data.DataRelation relationFK_UserOrganisationStructure_OrganisationStructure;

            private global::System.Data.DataRelation relationFK_UserOrganisationStructure_ADUser;

            private global::System.Data.DataRelation relationFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType;

            private global::System.Data.DataRelation relationFK_RoundRobinPointerDefinition_RoundRobinPointer;

            private global::System.Data.DataRelation relationUserOrganisationStructure_UserOrganisationStructureRoundRobinStatus;

            private global::System.Data.DataRelation relationFK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType;

            private global::System.Data.DataRelation relationFK_WorkflowRoleType_WorkflowRoleTypeGroup;

            private global::System.Data.DataRelation relationOrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping;

            private global::System.Data.SchemaSerializationMode _schemaSerializationMode = global::System.Data.SchemaSerializationMode.IncludeSchema;

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public WorkflowAssignment()
            {
                this.BeginInit();
                this.InitClass();
                global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                base.Tables.CollectionChanged += schemaChangedHandler;
                base.Relations.CollectionChanged += schemaChangedHandler;
                this.EndInit();
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected WorkflowAssignment(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                base(info, context, false)
            {
                if ((this.IsBinarySerialized(info, context) == true))
                {
                    this.InitVars(false);
                    global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler1 = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                    this.Tables.CollectionChanged += schemaChangedHandler1;
                    this.Relations.CollectionChanged += schemaChangedHandler1;
                    return;
                }
                string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
                if ((this.DetermineSchemaSerializationMode(info, context) == global::System.Data.SchemaSerializationMode.IncludeSchema))
                {
                    global::System.Data.DataSet ds = new global::System.Data.DataSet();
                    ds.ReadXmlSchema(new global::System.Xml.XmlTextReader(new global::System.IO.StringReader(strSchema)));
                    if ((ds.Tables["WorkflowAssignment"] != null))
                    {
                        base.Tables.Add(new WorkflowAssignmentDataTable(ds.Tables["WorkflowAssignment"]));
                    }
                    if ((ds.Tables["ADUser"] != null))
                    {
                        base.Tables.Add(new ADUserDataTable(ds.Tables["ADUser"]));
                    }
                    if ((ds.Tables["OfferRoleType"] != null))
                    {
                        base.Tables.Add(new OfferRoleTypeDataTable(ds.Tables["OfferRoleType"]));
                    }
                    if ((ds.Tables["OfferRoleTypeOrganisationStructureMapping"] != null))
                    {
                        base.Tables.Add(new OfferRoleTypeOrganisationStructureMappingDataTable(ds.Tables["OfferRoleTypeOrganisationStructureMapping"]));
                    }
                    if ((ds.Tables["OrganisationStructure"] != null))
                    {
                        base.Tables.Add(new OrganisationStructureDataTable(ds.Tables["OrganisationStructure"]));
                    }
                    if ((ds.Tables["UserOrganisationStructure"] != null))
                    {
                        base.Tables.Add(new UserOrganisationStructureDataTable(ds.Tables["UserOrganisationStructure"]));
                    }
                    if ((ds.Tables["WFAssignment"] != null))
                    {
                        base.Tables.Add(new WFAssignmentDataTable(ds.Tables["WFAssignment"]));
                    }
                    //if ((ds.Tables["RoundRobinPointer"] != null))
                    //{
                    //    base.Tables.Add(new RoundRobinPointerDataTable(ds.Tables["RoundRobinPointer"]));
                    //}
                    if ((ds.Tables["RoundRobinPointerDefinition"] != null))
                    {
                        base.Tables.Add(new RoundRobinPointerDefinitionDataTable(ds.Tables["RoundRobinPointerDefinition"]));
                    }
                    if ((ds.Tables["UserOrganisationStructureRoundRobinStatus"] != null))
                    {
                        base.Tables.Add(new UserOrganisationStructureRoundRobinStatusDataTable(ds.Tables["UserOrganisationStructureRoundRobinStatus"]));
                    }
                    if ((ds.Tables["WorkflowRoleAssignment"] != null))
                    {
                        base.Tables.Add(new WorkflowRoleAssignmentDataTable(ds.Tables["WorkflowRoleAssignment"]));
                    }
                    if ((ds.Tables["WorkflowRoleTypeGroup"] != null))
                    {
                        base.Tables.Add(new WorkflowRoleTypeGroupDataTable(ds.Tables["WorkflowRoleTypeGroup"]));
                    }
                    if ((ds.Tables["WorkflowRoleTypeOrganisationStructureMapping"] != null))
                    {
                        base.Tables.Add(new WorkflowRoleTypeOrganisationStructureMappingDataTable(ds.Tables["WorkflowRoleTypeOrganisationStructureMapping"]));
                    }
                    if ((ds.Tables["WorkflowRoleType"] != null))
                    {
                        base.Tables.Add(new WorkflowRoleTypeDataTable(ds.Tables["WorkflowRoleType"]));
                    }
                    if ((ds.Tables["WFRAssignment"] != null))
                    {
                        base.Tables.Add(new WFRAssignmentDataTable(ds.Tables["WFRAssignment"]));
                    }
                    this.DataSetName = ds.DataSetName;
                    this.Prefix = ds.Prefix;
                    this.Namespace = ds.Namespace;
                    this.Locale = ds.Locale;
                    this.CaseSensitive = ds.CaseSensitive;
                    this.EnforceConstraints = ds.EnforceConstraints;
                    this.Merge(ds, false, global::System.Data.MissingSchemaAction.Add);
                    this.InitVars();
                }
                else
                {
                    this.ReadXmlSchema(new global::System.Xml.XmlTextReader(new global::System.IO.StringReader(strSchema)));
                }
                this.GetSerializationData(info, context);
                global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                base.Tables.CollectionChanged += schemaChangedHandler;
                this.Relations.CollectionChanged += schemaChangedHandler;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public WorkflowAssignmentDataTable _WorkflowAssignment
            {
                get
                {
                    return this.tableWorkflowAssignment;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public ADUserDataTable ADUser
            {
                get
                {
                    return this.tableADUser;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public OfferRoleTypeDataTable OfferRoleType
            {
                get
                {
                    return this.tableOfferRoleType;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public OfferRoleTypeOrganisationStructureMappingDataTable OfferRoleTypeOrganisationStructureMapping
            {
                get
                {
                    return this.tableOfferRoleTypeOrganisationStructureMapping;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public OrganisationStructureDataTable OrganisationStructure
            {
                get
                {
                    return this.tableOrganisationStructure;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public UserOrganisationStructureDataTable UserOrganisationStructure
            {
                get
                {
                    return this.tableUserOrganisationStructure;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public WFAssignmentDataTable WFAssignment
            {
                get
                {
                    return this.tableWFAssignment;
                }
            }

            //[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //[global::System.ComponentModel.Browsable(false)]
            //[global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            //public RoundRobinPointerDataTable RoundRobinPointer
            //{
            //    get
            //    {
            //        return this.tableRoundRobinPointer;
            //    }
            //}

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public RoundRobinPointerDefinitionDataTable RoundRobinPointerDefinition
            {
                get
                {
                    return this.tableRoundRobinPointerDefinition;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public UserOrganisationStructureRoundRobinStatusDataTable UserOrganisationStructureRoundRobinStatus
            {
                get
                {
                    return this.tableUserOrganisationStructureRoundRobinStatus;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public WorkflowRoleAssignmentDataTable WorkflowRoleAssignment
            {
                get
                {
                    return this.tableWorkflowRoleAssignment;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public WorkflowRoleTypeGroupDataTable WorkflowRoleTypeGroup
            {
                get
                {
                    return this.tableWorkflowRoleTypeGroup;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public WorkflowRoleTypeOrganisationStructureMappingDataTable WorkflowRoleTypeOrganisationStructureMapping
            {
                get
                {
                    return this.tableWorkflowRoleTypeOrganisationStructureMapping;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public WorkflowRoleTypeDataTable WorkflowRoleType
            {
                get
                {
                    return this.tableWorkflowRoleType;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.Browsable(false)]
            [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
            public WFRAssignmentDataTable WFRAssignment
            {
                get
                {
                    return this.tableWFRAssignment;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.BrowsableAttribute(true)]
            [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Visible)]
            public override global::System.Data.SchemaSerializationMode SchemaSerializationMode
            {
                get
                {
                    return this._schemaSerializationMode;
                }
                set
                {
                    this._schemaSerializationMode = value;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Hidden)]
            public new global::System.Data.DataTableCollection Tables
            {
                get
                {
                    return base.Tables;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Hidden)]
            public new global::System.Data.DataRelationCollection Relations
            {
                get
                {
                    return base.Relations;
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void InitializeDerivedDataSet()
            {
                this.BeginInit();
                this.InitClass();
                this.EndInit();
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public override global::System.Data.DataSet Clone()
            {
                WorkflowAssignment cln = ((WorkflowAssignment)(base.Clone()));
                cln.InitVars();
                cln.SchemaSerializationMode = this.SchemaSerializationMode;
                return cln;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override bool ShouldSerializeTables()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override bool ShouldSerializeRelations()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void ReadXmlSerializable(global::System.Xml.XmlReader reader)
            {
                if ((this.DetermineSchemaSerializationMode(reader) == global::System.Data.SchemaSerializationMode.IncludeSchema))
                {
                    this.Reset();
                    global::System.Data.DataSet ds = new global::System.Data.DataSet();
                    ds.ReadXml(reader);
                    if ((ds.Tables["WorkflowAssignment"] != null))
                    {
                        base.Tables.Add(new WorkflowAssignmentDataTable(ds.Tables["WorkflowAssignment"]));
                    }
                    if ((ds.Tables["ADUser"] != null))
                    {
                        base.Tables.Add(new ADUserDataTable(ds.Tables["ADUser"]));
                    }
                    if ((ds.Tables["OfferRoleType"] != null))
                    {
                        base.Tables.Add(new OfferRoleTypeDataTable(ds.Tables["OfferRoleType"]));
                    }
                    if ((ds.Tables["OfferRoleTypeOrganisationStructureMapping"] != null))
                    {
                        base.Tables.Add(new OfferRoleTypeOrganisationStructureMappingDataTable(ds.Tables["OfferRoleTypeOrganisationStructureMapping"]));
                    }
                    if ((ds.Tables["OrganisationStructure"] != null))
                    {
                        base.Tables.Add(new OrganisationStructureDataTable(ds.Tables["OrganisationStructure"]));
                    }
                    if ((ds.Tables["UserOrganisationStructure"] != null))
                    {
                        base.Tables.Add(new UserOrganisationStructureDataTable(ds.Tables["UserOrganisationStructure"]));
                    }
                    if ((ds.Tables["WFAssignment"] != null))
                    {
                        base.Tables.Add(new WFAssignmentDataTable(ds.Tables["WFAssignment"]));
                    }
                    //if ((ds.Tables["RoundRobinPointer"] != null))
                    //{
                    //    base.Tables.Add(new RoundRobinPointerDataTable(ds.Tables["RoundRobinPointer"]));
                    //}
                    if ((ds.Tables["RoundRobinPointerDefinition"] != null))
                    {
                        base.Tables.Add(new RoundRobinPointerDefinitionDataTable(ds.Tables["RoundRobinPointerDefinition"]));
                    }
                    if ((ds.Tables["UserOrganisationStructureRoundRobinStatus"] != null))
                    {
                        base.Tables.Add(new UserOrganisationStructureRoundRobinStatusDataTable(ds.Tables["UserOrganisationStructureRoundRobinStatus"]));
                    }
                    if ((ds.Tables["WorkflowRoleAssignment"] != null))
                    {
                        base.Tables.Add(new WorkflowRoleAssignmentDataTable(ds.Tables["WorkflowRoleAssignment"]));
                    }
                    if ((ds.Tables["WorkflowRoleTypeGroup"] != null))
                    {
                        base.Tables.Add(new WorkflowRoleTypeGroupDataTable(ds.Tables["WorkflowRoleTypeGroup"]));
                    }
                    if ((ds.Tables["WorkflowRoleTypeOrganisationStructureMapping"] != null))
                    {
                        base.Tables.Add(new WorkflowRoleTypeOrganisationStructureMappingDataTable(ds.Tables["WorkflowRoleTypeOrganisationStructureMapping"]));
                    }
                    if ((ds.Tables["WorkflowRoleType"] != null))
                    {
                        base.Tables.Add(new WorkflowRoleTypeDataTable(ds.Tables["WorkflowRoleType"]));
                    }
                    if ((ds.Tables["WFRAssignment"] != null))
                    {
                        base.Tables.Add(new WFRAssignmentDataTable(ds.Tables["WFRAssignment"]));
                    }
                    this.DataSetName = ds.DataSetName;
                    this.Prefix = ds.Prefix;
                    this.Namespace = ds.Namespace;
                    this.Locale = ds.Locale;
                    this.CaseSensitive = ds.CaseSensitive;
                    this.EnforceConstraints = ds.EnforceConstraints;
                    this.Merge(ds, false, global::System.Data.MissingSchemaAction.Add);
                    this.InitVars();
                }
                else
                {
                    this.ReadXml(reader);
                    this.InitVars();
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override global::System.Xml.Schema.XmlSchema GetSchemaSerializable()
            {
                global::System.IO.MemoryStream stream = new global::System.IO.MemoryStream();
                this.WriteXmlSchema(new global::System.Xml.XmlTextWriter(stream, null));
                stream.Position = 0;
                return global::System.Xml.Schema.XmlSchema.Read(new global::System.Xml.XmlTextReader(stream), null);
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal void InitVars()
            {
                this.InitVars(true);
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal void InitVars(bool initTable)
            {
                this.tableWorkflowAssignment = ((WorkflowAssignmentDataTable)(base.Tables["WorkflowAssignment"]));
                if ((initTable == true))
                {
                    if ((this.tableWorkflowAssignment != null))
                    {
                        this.tableWorkflowAssignment.InitVars();
                    }
                }
                this.tableADUser = ((ADUserDataTable)(base.Tables["ADUser"]));
                if ((initTable == true))
                {
                    if ((this.tableADUser != null))
                    {
                        this.tableADUser.InitVars();
                    }
                }
                this.tableOfferRoleType = ((OfferRoleTypeDataTable)(base.Tables["OfferRoleType"]));
                if ((initTable == true))
                {
                    if ((this.tableOfferRoleType != null))
                    {
                        this.tableOfferRoleType.InitVars();
                    }
                }
                this.tableOfferRoleTypeOrganisationStructureMapping = ((OfferRoleTypeOrganisationStructureMappingDataTable)(base.Tables["OfferRoleTypeOrganisationStructureMapping"]));
                if ((initTable == true))
                {
                    if ((this.tableOfferRoleTypeOrganisationStructureMapping != null))
                    {
                        this.tableOfferRoleTypeOrganisationStructureMapping.InitVars();
                    }
                }
                this.tableOrganisationStructure = ((OrganisationStructureDataTable)(base.Tables["OrganisationStructure"]));
                if ((initTable == true))
                {
                    if ((this.tableOrganisationStructure != null))
                    {
                        this.tableOrganisationStructure.InitVars();
                    }
                }
                this.tableUserOrganisationStructure = ((UserOrganisationStructureDataTable)(base.Tables["UserOrganisationStructure"]));
                if ((initTable == true))
                {
                    if ((this.tableUserOrganisationStructure != null))
                    {
                        this.tableUserOrganisationStructure.InitVars();
                    }
                }
                this.tableWFAssignment = ((WFAssignmentDataTable)(base.Tables["WFAssignment"]));
                if ((initTable == true))
                {
                    if ((this.tableWFAssignment != null))
                    {
                        this.tableWFAssignment.InitVars();
                    }
                }
                //this.tableRoundRobinPointer = ((RoundRobinPointerDataTable)(base.Tables["RoundRobinPointer"]));
                //if ((initTable == true))
                //{
                //    if ((this.tableRoundRobinPointer != null))
                //    {
                //        this.tableRoundRobinPointer.InitVars();
                //    }
                //}
                this.tableRoundRobinPointerDefinition = ((RoundRobinPointerDefinitionDataTable)(base.Tables["RoundRobinPointerDefinition"]));
                if ((initTable == true))
                {
                    if ((this.tableRoundRobinPointerDefinition != null))
                    {
                        this.tableRoundRobinPointerDefinition.InitVars();
                    }
                }
                this.tableUserOrganisationStructureRoundRobinStatus = ((UserOrganisationStructureRoundRobinStatusDataTable)(base.Tables["UserOrganisationStructureRoundRobinStatus"]));
                if ((initTable == true))
                {
                    if ((this.tableUserOrganisationStructureRoundRobinStatus != null))
                    {
                        this.tableUserOrganisationStructureRoundRobinStatus.InitVars();
                    }
                }
                this.tableWorkflowRoleAssignment = ((WorkflowRoleAssignmentDataTable)(base.Tables["WorkflowRoleAssignment"]));
                if ((initTable == true))
                {
                    if ((this.tableWorkflowRoleAssignment != null))
                    {
                        this.tableWorkflowRoleAssignment.InitVars();
                    }
                }
                this.tableWorkflowRoleTypeGroup = ((WorkflowRoleTypeGroupDataTable)(base.Tables["WorkflowRoleTypeGroup"]));
                if ((initTable == true))
                {
                    if ((this.tableWorkflowRoleTypeGroup != null))
                    {
                        this.tableWorkflowRoleTypeGroup.InitVars();
                    }
                }
                this.tableWorkflowRoleTypeOrganisationStructureMapping = ((WorkflowRoleTypeOrganisationStructureMappingDataTable)(base.Tables["WorkflowRoleTypeOrganisationStructureMapping"]));
                if ((initTable == true))
                {
                    if ((this.tableWorkflowRoleTypeOrganisationStructureMapping != null))
                    {
                        this.tableWorkflowRoleTypeOrganisationStructureMapping.InitVars();
                    }
                }
                this.tableWorkflowRoleType = ((WorkflowRoleTypeDataTable)(base.Tables["WorkflowRoleType"]));
                if ((initTable == true))
                {
                    if ((this.tableWorkflowRoleType != null))
                    {
                        this.tableWorkflowRoleType.InitVars();
                    }
                }
                this.tableWFRAssignment = ((WFRAssignmentDataTable)(base.Tables["WFRAssignment"]));
                if ((initTable == true))
                {
                    if ((this.tableWFRAssignment != null))
                    {
                        this.tableWFRAssignment.InitVars();
                    }
                }
                this.relationFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure = this.Relations["FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure"];
                this.relationFK_UserOrganisationStructure_OrganisationStructure = this.Relations["FK_UserOrganisationStructure_OrganisationStructure"];
                this.relationFK_UserOrganisationStructure_ADUser = this.Relations["FK_UserOrganisationStructure_ADUser"];
                this.relationFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType = this.Relations["FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType"];
                this.relationFK_RoundRobinPointerDefinition_RoundRobinPointer = this.Relations["FK_RoundRobinPointerDefinition_RoundRobinPointer"];
                this.relationUserOrganisationStructure_UserOrganisationStructureRoundRobinStatus = this.Relations["UserOrganisationStructure_UserOrganisationStructureRoundRobinStatus"];
                this.relationFK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType = this.Relations["FK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType"];
                this.relationFK_WorkflowRoleType_WorkflowRoleTypeGroup = this.Relations["FK_WorkflowRoleType_WorkflowRoleTypeGroup"];
                this.relationOrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping = this.Relations["OrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping"];
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private void InitClass()
            {
                this.DataSetName = "WorkflowAssignment";
                this.Prefix = "";
                this.Namespace = "http://tempuri.org/WorkflowAssignment.xsd";
                this.EnforceConstraints = true;
                this.SchemaSerializationMode = global::System.Data.SchemaSerializationMode.IncludeSchema;
                this.tableWorkflowAssignment = new WorkflowAssignmentDataTable();
                base.Tables.Add(this.tableWorkflowAssignment);
                this.tableADUser = new ADUserDataTable();
                base.Tables.Add(this.tableADUser);
                this.tableOfferRoleType = new OfferRoleTypeDataTable();
                base.Tables.Add(this.tableOfferRoleType);
                this.tableOfferRoleTypeOrganisationStructureMapping = new OfferRoleTypeOrganisationStructureMappingDataTable();
                base.Tables.Add(this.tableOfferRoleTypeOrganisationStructureMapping);
                this.tableOrganisationStructure = new OrganisationStructureDataTable();
                base.Tables.Add(this.tableOrganisationStructure);
                this.tableUserOrganisationStructure = new UserOrganisationStructureDataTable();
                base.Tables.Add(this.tableUserOrganisationStructure);
                this.tableWFAssignment = new WFAssignmentDataTable();
                base.Tables.Add(this.tableWFAssignment);
                //this.tableRoundRobinPointer = new RoundRobinPointerDataTable();
                //base.Tables.Add(this.tableRoundRobinPointer);
                this.tableRoundRobinPointerDefinition = new RoundRobinPointerDefinitionDataTable();
                base.Tables.Add(this.tableRoundRobinPointerDefinition);
                this.tableUserOrganisationStructureRoundRobinStatus = new UserOrganisationStructureRoundRobinStatusDataTable();
                base.Tables.Add(this.tableUserOrganisationStructureRoundRobinStatus);
                this.tableWorkflowRoleAssignment = new WorkflowRoleAssignmentDataTable();
                base.Tables.Add(this.tableWorkflowRoleAssignment);
                this.tableWorkflowRoleTypeGroup = new WorkflowRoleTypeGroupDataTable();
                base.Tables.Add(this.tableWorkflowRoleTypeGroup);
                this.tableWorkflowRoleTypeOrganisationStructureMapping = new WorkflowRoleTypeOrganisationStructureMappingDataTable();
                base.Tables.Add(this.tableWorkflowRoleTypeOrganisationStructureMapping);
                this.tableWorkflowRoleType = new WorkflowRoleTypeDataTable();
                base.Tables.Add(this.tableWorkflowRoleType);
                this.tableWFRAssignment = new WFRAssignmentDataTable();
                base.Tables.Add(this.tableWFRAssignment);
                this.relationFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure = new global::System.Data.DataRelation("FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure", new global::System.Data.DataColumn[] {
                        this.tableOrganisationStructure.OrganisationStructureKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableOfferRoleTypeOrganisationStructureMapping.OrganisationStructureKeyColumn}, false);
                this.Relations.Add(this.relationFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure);
                this.relationFK_UserOrganisationStructure_OrganisationStructure = new global::System.Data.DataRelation("FK_UserOrganisationStructure_OrganisationStructure", new global::System.Data.DataColumn[] {
                        this.tableOrganisationStructure.OrganisationStructureKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableUserOrganisationStructure.OrganisationStructureKeyColumn}, false);
                this.Relations.Add(this.relationFK_UserOrganisationStructure_OrganisationStructure);
                this.relationFK_UserOrganisationStructure_ADUser = new global::System.Data.DataRelation("FK_UserOrganisationStructure_ADUser", new global::System.Data.DataColumn[] {
                        this.tableADUser.ADUserKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableUserOrganisationStructure.ADUserKeyColumn}, false);
                this.Relations.Add(this.relationFK_UserOrganisationStructure_ADUser);
                this.relationFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType = new global::System.Data.DataRelation("FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType", new global::System.Data.DataColumn[] {
                        this.tableOfferRoleType.OfferRoleTypeKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableOfferRoleTypeOrganisationStructureMapping.OfferRoleTypeKeyColumn}, false);
                this.Relations.Add(this.relationFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType);
                //this.relationFK_RoundRobinPointerDefinition_RoundRobinPointer = new global::System.Data.DataRelation("FK_RoundRobinPointerDefinition_RoundRobinPointer", new global::System.Data.DataColumn[] {
                //        this.tableRoundRobinPointer.RoundRobinPointerKeyColumn}, new global::System.Data.DataColumn[] {
                //        this.tableRoundRobinPointerDefinition.RoundRobinPointerKeyColumn}, false);
                //this.Relations.Add(this.relationFK_RoundRobinPointerDefinition_RoundRobinPointer);
                this.relationUserOrganisationStructure_UserOrganisationStructureRoundRobinStatus = new global::System.Data.DataRelation("UserOrganisationStructure_UserOrganisationStructureRoundRobinStatus", new global::System.Data.DataColumn[] {
                        this.tableUserOrganisationStructure.UserOrganisationStructureKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableUserOrganisationStructureRoundRobinStatus.UserOrganisationStructureKeyColumn}, false);
                this.Relations.Add(this.relationUserOrganisationStructure_UserOrganisationStructureRoundRobinStatus);
                this.relationFK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType = new global::System.Data.DataRelation("FK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType", new global::System.Data.DataColumn[] {
                        this.tableWorkflowRoleType.WorkflowRoleTypeKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableWorkflowRoleTypeOrganisationStructureMapping.WorkflowRoleTypeKeyColumn}, false);
                this.Relations.Add(this.relationFK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType);
                this.relationFK_WorkflowRoleType_WorkflowRoleTypeGroup = new global::System.Data.DataRelation("FK_WorkflowRoleType_WorkflowRoleTypeGroup", new global::System.Data.DataColumn[] {
                        this.tableWorkflowRoleTypeGroup.WorkflowRoleTypeGroupKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableWorkflowRoleType.WorkflowRoleTypeGroupKeyColumn}, false);
                this.Relations.Add(this.relationFK_WorkflowRoleType_WorkflowRoleTypeGroup);
                this.relationOrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping = new global::System.Data.DataRelation("OrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping", new global::System.Data.DataColumn[] {
                        this.tableOrganisationStructure.OrganisationStructureKeyColumn}, new global::System.Data.DataColumn[] {
                        this.tableWorkflowRoleTypeOrganisationStructureMapping.OrganisationStructureKeyColumn}, false);
                this.Relations.Add(this.relationOrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping);
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerialize_WorkflowAssignment()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeADUser()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeOfferRoleType()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeOfferRoleTypeOrganisationStructureMapping()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeOrganisationStructure()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeUserOrganisationStructure()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeWFAssignment()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeRoundRobinPointer()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeRoundRobinPointerDefinition()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeUserOrganisationStructureRoundRobinStatus()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeWorkflowRoleAssignment()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeWorkflowRoleTypeGroup()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeWorkflowRoleTypeOrganisationStructureMapping()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeWorkflowRoleType()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private bool ShouldSerializeWFRAssignment()
            {
                return false;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private void SchemaChanged(object sender, global::System.ComponentModel.CollectionChangeEventArgs e)
            {
                if ((e.Action == global::System.ComponentModel.CollectionChangeAction.Remove))
                {
                    this.InitVars();
                }
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedDataSetSchema(global::System.Xml.Schema.XmlSchemaSet xs)
            {
                WorkflowAssignment ds = new WorkflowAssignment();
                global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                global::System.Xml.Schema.XmlSchemaAny any = new global::System.Xml.Schema.XmlSchemaAny();
                any.Namespace = ds.Namespace;
                sequence.Items.Add(any);
                type.Particle = sequence;
                global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                if (xs.Contains(dsSchema.TargetNamespace))
                {
                    global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                    global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                    try
                    {
                        global::System.Xml.Schema.XmlSchema schema = null;
                        dsSchema.Write(s1);
                        for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                        {
                            schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                            s2.SetLength(0);
                            schema.Write(s2);
                            if ((s1.Length == s2.Length))
                            {
                                s1.Position = 0;
                                s2.Position = 0;
                                for (; ((s1.Position != s1.Length)
                                            && (s1.ReadByte() == s2.ReadByte())); )
                                {
                                    ;
                                }
                                if ((s1.Position == s1.Length))
                                {
                                    return type;
                                }
                            }
                        }
                    }
                    finally
                    {
                        if ((s1 != null))
                        {
                            s1.Close();
                        }
                        if ((s2 != null))
                        {
                            s2.Close();
                        }
                    }
                }
                xs.Add(dsSchema);
                return type;
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void WorkflowAssignmentRowChangeEventHandler(object sender, WorkflowAssignmentRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void ADUserRowChangeEventHandler(object sender, ADUserRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void OfferRoleTypeRowChangeEventHandler(object sender, OfferRoleTypeRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void OfferRoleTypeOrganisationStructureMappingRowChangeEventHandler(object sender, OfferRoleTypeOrganisationStructureMappingRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void OrganisationStructureRowChangeEventHandler(object sender, OrganisationStructureRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void UserOrganisationStructureRowChangeEventHandler(object sender, UserOrganisationStructureRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void WFAssignmentRowChangeEventHandler(object sender, WFAssignmentRowChangeEvent e);

            //[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //public delegate void RoundRobinPointerRowChangeEventHandler(object sender, RoundRobinPointerRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void RoundRobinPointerDefinitionRowChangeEventHandler(object sender, RoundRobinPointerDefinitionRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void UserOrganisationStructureRoundRobinStatusRowChangeEventHandler(object sender, UserOrganisationStructureRoundRobinStatusRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void WorkflowRoleAssignmentRowChangeEventHandler(object sender, WorkflowRoleAssignmentRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void WorkflowRoleTypeGroupRowChangeEventHandler(object sender, WorkflowRoleTypeGroupRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void WorkflowRoleTypeOrganisationStructureMappingRowChangeEventHandler(object sender, WorkflowRoleTypeOrganisationStructureMappingRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void WorkflowRoleTypeRowChangeEventHandler(object sender, WorkflowRoleTypeRowChangeEvent e);

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public delegate void WFRAssignmentRowChangeEventHandler(object sender, WFRAssignmentRowChangeEvent e);

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class WorkflowAssignmentDataTable : global::System.Data.TypedTableBase<WorkflowAssignmentRow>
            {
                private global::System.Data.DataColumn columnID;

                private global::System.Data.DataColumn columnInstanceID;

                private global::System.Data.DataColumn columnOfferRoleTypeOrganisationStructureMappingKey;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                private global::System.Data.DataColumn columnADUserKey;

                private global::System.Data.DataColumn columnState;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentDataTable()
                {
                    this.TableName = "WorkflowAssignment";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowAssignmentDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected WorkflowAssignmentDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn IDColumn
                {
                    get
                    {
                        return this.columnID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn InstanceIDColumn
                {
                    get
                    {
                        return this.columnInstanceID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeOrganisationStructureMappingKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeOrganisationStructureMappingKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserKeyColumn
                {
                    get
                    {
                        return this.columnADUserKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn StateColumn
                {
                    get
                    {
                        return this.columnState;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRow this[int index]
                {
                    get
                    {
                        return ((WorkflowAssignmentRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowAssignmentRowChangeEventHandler WorkflowAssignmentRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowAssignmentRowChangeEventHandler WorkflowAssignmentRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowAssignmentRowChangeEventHandler WorkflowAssignmentRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowAssignmentRowChangeEventHandler WorkflowAssignmentRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddWorkflowAssignmentRow(WorkflowAssignmentRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRow AddWorkflowAssignmentRow(string ID, long InstanceID, int OfferRoleTypeOrganisationStructureMappingKey, int GeneralStatusKey, int ADUserKey, string State)
                {
                    WorkflowAssignmentRow rowWorkflowAssignmentRow = ((WorkflowAssignmentRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        ID,
                        InstanceID,
                        OfferRoleTypeOrganisationStructureMappingKey,
                        GeneralStatusKey,
                        ADUserKey,
                        State};
                    rowWorkflowAssignmentRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowWorkflowAssignmentRow);
                    return rowWorkflowAssignmentRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRow FindByID(string ID)
                {
                    return ((WorkflowAssignmentRow)(this.Rows.Find(new object[] {
                            ID})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    WorkflowAssignmentDataTable cln = ((WorkflowAssignmentDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new WorkflowAssignmentDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnID = base.Columns["ID"];
                    this.columnInstanceID = base.Columns["InstanceID"];
                    this.columnOfferRoleTypeOrganisationStructureMappingKey = base.Columns["OfferRoleTypeOrganisationStructureMappingKey"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                    this.columnADUserKey = base.Columns["ADUserKey"];
                    this.columnState = base.Columns["State"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnID = new global::System.Data.DataColumn("ID", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnID);
                    this.columnInstanceID = new global::System.Data.DataColumn("InstanceID", typeof(long), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnInstanceID);
                    this.columnOfferRoleTypeOrganisationStructureMappingKey = new global::System.Data.DataColumn("OfferRoleTypeOrganisationStructureMappingKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeOrganisationStructureMappingKey);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.columnADUserKey = new global::System.Data.DataColumn("ADUserKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserKey);
                    this.columnState = new global::System.Data.DataColumn("State", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnState);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnID}, true));
                    this.columnID.AllowDBNull = false;
                    this.columnID.Unique = true;
                    this.columnID.Caption = "Column1";
                    this.columnInstanceID.Caption = "Column1";
                    this.columnOfferRoleTypeOrganisationStructureMappingKey.Caption = "Column1";
                    this.columnGeneralStatusKey.Caption = "Column1";
                    this.ExtendedProperties.Add("Generator_TablePropName", "_WorkflowAssignment");
                    this.ExtendedProperties.Add("Generator_UserTableName", "WorkflowAssignment");
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRow NewWorkflowAssignmentRow()
                {
                    return ((WorkflowAssignmentRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new WorkflowAssignmentRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(WorkflowAssignmentRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.WorkflowAssignmentRowChanged != null))
                    {
                        this.WorkflowAssignmentRowChanged(this, new WorkflowAssignmentRowChangeEvent(((WorkflowAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.WorkflowAssignmentRowChanging != null))
                    {
                        this.WorkflowAssignmentRowChanging(this, new WorkflowAssignmentRowChangeEvent(((WorkflowAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.WorkflowAssignmentRowDeleted != null))
                    {
                        this.WorkflowAssignmentRowDeleted(this, new WorkflowAssignmentRowChangeEvent(((WorkflowAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.WorkflowAssignmentRowDeleting != null))
                    {
                        this.WorkflowAssignmentRowDeleting(this, new WorkflowAssignmentRowChangeEvent(((WorkflowAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveWorkflowAssignmentRow(WorkflowAssignmentRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "WorkflowAssignmentDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class ADUserDataTable : global::System.Data.TypedTableBase<ADUserRow>
            {
                private global::System.Data.DataColumn columnADUserKey;

                private global::System.Data.DataColumn columnADUserName;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                private global::System.Data.DataColumn columnPassword;

                private global::System.Data.DataColumn columnPasswordQuestion;

                private global::System.Data.DataColumn columnPasswordAnswer;

                private global::System.Data.DataColumn columnLegalEntityKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserDataTable()
                {
                    this.TableName = "ADUser";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal ADUserDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected ADUserDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserKeyColumn
                {
                    get
                    {
                        return this.columnADUserKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserNameColumn
                {
                    get
                    {
                        return this.columnADUserName;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn PasswordColumn
                {
                    get
                    {
                        return this.columnPassword;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn PasswordQuestionColumn
                {
                    get
                    {
                        return this.columnPasswordQuestion;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn PasswordAnswerColumn
                {
                    get
                    {
                        return this.columnPasswordAnswer;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn LegalEntityKeyColumn
                {
                    get
                    {
                        return this.columnLegalEntityKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow this[int index]
                {
                    get
                    {
                        return ((ADUserRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event ADUserRowChangeEventHandler ADUserRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event ADUserRowChangeEventHandler ADUserRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event ADUserRowChangeEventHandler ADUserRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event ADUserRowChangeEventHandler ADUserRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddADUserRow(ADUserRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow AddADUserRow(string ADUserName, int GeneralStatusKey, string Password, string PasswordQuestion, string PasswordAnswer, int LegalEntityKey)
                {
                    ADUserRow rowADUserRow = ((ADUserRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        ADUserName,
                        GeneralStatusKey,
                        Password,
                        PasswordQuestion,
                        PasswordAnswer,
                        LegalEntityKey};
                    rowADUserRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowADUserRow);
                    return rowADUserRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow FindByADUserKey(int ADUserKey)
                {
                    return ((ADUserRow)(this.Rows.Find(new object[] {
                            ADUserKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    ADUserDataTable cln = ((ADUserDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new ADUserDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnADUserKey = base.Columns["ADUserKey"];
                    this.columnADUserName = base.Columns["ADUserName"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                    this.columnPassword = base.Columns["Password"];
                    this.columnPasswordQuestion = base.Columns["PasswordQuestion"];
                    this.columnPasswordAnswer = base.Columns["PasswordAnswer"];
                    this.columnLegalEntityKey = base.Columns["LegalEntityKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnADUserKey = new global::System.Data.DataColumn("ADUserKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserKey);
                    this.columnADUserName = new global::System.Data.DataColumn("ADUserName", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserName);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.columnPassword = new global::System.Data.DataColumn("Password", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnPassword);
                    this.columnPasswordQuestion = new global::System.Data.DataColumn("PasswordQuestion", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnPasswordQuestion);
                    this.columnPasswordAnswer = new global::System.Data.DataColumn("PasswordAnswer", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnPasswordAnswer);
                    this.columnLegalEntityKey = new global::System.Data.DataColumn("LegalEntityKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnLegalEntityKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnADUserKey}, true));
                    this.columnADUserKey.AutoIncrement = true;
                    this.columnADUserKey.AllowDBNull = false;
                    this.columnADUserKey.ReadOnly = true;
                    this.columnADUserKey.Unique = true;
                    this.columnADUserName.AllowDBNull = false;
                    this.columnADUserName.MaxLength = 100;
                    this.columnGeneralStatusKey.AllowDBNull = false;
                    this.columnPassword.MaxLength = 50;
                    this.columnPasswordQuestion.MaxLength = 100;
                    this.columnPasswordAnswer.MaxLength = 100;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow NewADUserRow()
                {
                    return ((ADUserRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new ADUserRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(ADUserRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.ADUserRowChanged != null))
                    {
                        this.ADUserRowChanged(this, new ADUserRowChangeEvent(((ADUserRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.ADUserRowChanging != null))
                    {
                        this.ADUserRowChanging(this, new ADUserRowChangeEvent(((ADUserRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.ADUserRowDeleted != null))
                    {
                        this.ADUserRowDeleted(this, new ADUserRowChangeEvent(((ADUserRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.ADUserRowDeleting != null))
                    {
                        this.ADUserRowDeleting(this, new ADUserRowChangeEvent(((ADUserRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveADUserRow(ADUserRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "ADUserDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class OfferRoleTypeDataTable : global::System.Data.TypedTableBase<OfferRoleTypeRow>
            {
                private global::System.Data.DataColumn columnOfferRoleTypeKey;

                private global::System.Data.DataColumn columnDescription;

                private global::System.Data.DataColumn columnOfferRoleTypeGroupKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeDataTable()
                {
                    this.TableName = "OfferRoleType";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OfferRoleTypeDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected OfferRoleTypeDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn DescriptionColumn
                {
                    get
                    {
                        return this.columnDescription;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeGroupKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeGroupKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow this[int index]
                {
                    get
                    {
                        return ((OfferRoleTypeRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeRowChangeEventHandler OfferRoleTypeRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeRowChangeEventHandler OfferRoleTypeRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeRowChangeEventHandler OfferRoleTypeRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeRowChangeEventHandler OfferRoleTypeRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddOfferRoleTypeRow(OfferRoleTypeRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow AddOfferRoleTypeRow(string Description, int OfferRoleTypeGroupKey)
                {
                    OfferRoleTypeRow rowOfferRoleTypeRow = ((OfferRoleTypeRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        Description,
                        OfferRoleTypeGroupKey};
                    rowOfferRoleTypeRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowOfferRoleTypeRow);
                    return rowOfferRoleTypeRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow FindByOfferRoleTypeKey(int OfferRoleTypeKey)
                {
                    return ((OfferRoleTypeRow)(this.Rows.Find(new object[] {
                            OfferRoleTypeKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    OfferRoleTypeDataTable cln = ((OfferRoleTypeDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new OfferRoleTypeDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnOfferRoleTypeKey = base.Columns["OfferRoleTypeKey"];
                    this.columnDescription = base.Columns["Description"];
                    this.columnOfferRoleTypeGroupKey = base.Columns["OfferRoleTypeGroupKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnOfferRoleTypeKey = new global::System.Data.DataColumn("OfferRoleTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeKey);
                    this.columnDescription = new global::System.Data.DataColumn("Description", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnDescription);
                    this.columnOfferRoleTypeGroupKey = new global::System.Data.DataColumn("OfferRoleTypeGroupKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeGroupKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnOfferRoleTypeKey}, true));
                    this.columnOfferRoleTypeKey.AutoIncrement = true;
                    this.columnOfferRoleTypeKey.AllowDBNull = false;
                    this.columnOfferRoleTypeKey.ReadOnly = true;
                    this.columnOfferRoleTypeKey.Unique = true;
                    this.columnDescription.AllowDBNull = false;
                    this.columnDescription.MaxLength = 50;
                    this.columnOfferRoleTypeGroupKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow NewOfferRoleTypeRow()
                {
                    return ((OfferRoleTypeRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new OfferRoleTypeRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(OfferRoleTypeRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.OfferRoleTypeRowChanged != null))
                    {
                        this.OfferRoleTypeRowChanged(this, new OfferRoleTypeRowChangeEvent(((OfferRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.OfferRoleTypeRowChanging != null))
                    {
                        this.OfferRoleTypeRowChanging(this, new OfferRoleTypeRowChangeEvent(((OfferRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.OfferRoleTypeRowDeleted != null))
                    {
                        this.OfferRoleTypeRowDeleted(this, new OfferRoleTypeRowChangeEvent(((OfferRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.OfferRoleTypeRowDeleting != null))
                    {
                        this.OfferRoleTypeRowDeleting(this, new OfferRoleTypeRowChangeEvent(((OfferRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveOfferRoleTypeRow(OfferRoleTypeRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "OfferRoleTypeDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class OfferRoleTypeOrganisationStructureMappingDataTable : global::System.Data.TypedTableBase<OfferRoleTypeOrganisationStructureMappingRow>
            {
                private global::System.Data.DataColumn columnOfferRoleTypeOrganisationStructureMappingKey;

                private global::System.Data.DataColumn columnOfferRoleTypeKey;

                private global::System.Data.DataColumn columnOrganisationStructureKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingDataTable()
                {
                    this.TableName = "OfferRoleTypeOrganisationStructureMapping";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OfferRoleTypeOrganisationStructureMappingDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected OfferRoleTypeOrganisationStructureMappingDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeOrganisationStructureMappingKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeOrganisationStructureMappingKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow this[int index]
                {
                    get
                    {
                        return ((OfferRoleTypeOrganisationStructureMappingRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeOrganisationStructureMappingRowChangeEventHandler OfferRoleTypeOrganisationStructureMappingRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeOrganisationStructureMappingRowChangeEventHandler OfferRoleTypeOrganisationStructureMappingRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeOrganisationStructureMappingRowChangeEventHandler OfferRoleTypeOrganisationStructureMappingRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OfferRoleTypeOrganisationStructureMappingRowChangeEventHandler OfferRoleTypeOrganisationStructureMappingRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddOfferRoleTypeOrganisationStructureMappingRow(OfferRoleTypeOrganisationStructureMappingRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow AddOfferRoleTypeOrganisationStructureMappingRow(OfferRoleTypeRow parentOfferRoleTypeRowByFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType, OrganisationStructureRow parentOrganisationStructureRowByFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure)
                {
                    OfferRoleTypeOrganisationStructureMappingRow rowOfferRoleTypeOrganisationStructureMappingRow = ((OfferRoleTypeOrganisationStructureMappingRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        null,
                        null};
                    if ((parentOfferRoleTypeRowByFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType != null))
                    {
                        columnValuesArray[1] = parentOfferRoleTypeRowByFK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType[0];
                    }
                    if ((parentOrganisationStructureRowByFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure != null))
                    {
                        columnValuesArray[2] = parentOrganisationStructureRowByFK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure[0];
                    }
                    rowOfferRoleTypeOrganisationStructureMappingRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowOfferRoleTypeOrganisationStructureMappingRow);
                    return rowOfferRoleTypeOrganisationStructureMappingRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow FindByOfferRoleTypeOrganisationStructureMappingKey(int OfferRoleTypeOrganisationStructureMappingKey)
                {
                    return ((OfferRoleTypeOrganisationStructureMappingRow)(this.Rows.Find(new object[] {
                            OfferRoleTypeOrganisationStructureMappingKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    OfferRoleTypeOrganisationStructureMappingDataTable cln = ((OfferRoleTypeOrganisationStructureMappingDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new OfferRoleTypeOrganisationStructureMappingDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnOfferRoleTypeOrganisationStructureMappingKey = base.Columns["OfferRoleTypeOrganisationStructureMappingKey"];
                    this.columnOfferRoleTypeKey = base.Columns["OfferRoleTypeKey"];
                    this.columnOrganisationStructureKey = base.Columns["OrganisationStructureKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnOfferRoleTypeOrganisationStructureMappingKey = new global::System.Data.DataColumn("OfferRoleTypeOrganisationStructureMappingKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeOrganisationStructureMappingKey);
                    this.columnOfferRoleTypeKey = new global::System.Data.DataColumn("OfferRoleTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeKey);
                    this.columnOrganisationStructureKey = new global::System.Data.DataColumn("OrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationStructureKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnOfferRoleTypeOrganisationStructureMappingKey}, true));
                    this.columnOfferRoleTypeOrganisationStructureMappingKey.AutoIncrement = true;
                    this.columnOfferRoleTypeOrganisationStructureMappingKey.AllowDBNull = false;
                    this.columnOfferRoleTypeOrganisationStructureMappingKey.ReadOnly = true;
                    this.columnOfferRoleTypeOrganisationStructureMappingKey.Unique = true;
                    this.columnOfferRoleTypeKey.AllowDBNull = false;
                    this.columnOrganisationStructureKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow NewOfferRoleTypeOrganisationStructureMappingRow()
                {
                    return ((OfferRoleTypeOrganisationStructureMappingRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new OfferRoleTypeOrganisationStructureMappingRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(OfferRoleTypeOrganisationStructureMappingRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.OfferRoleTypeOrganisationStructureMappingRowChanged != null))
                    {
                        this.OfferRoleTypeOrganisationStructureMappingRowChanged(this, new OfferRoleTypeOrganisationStructureMappingRowChangeEvent(((OfferRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.OfferRoleTypeOrganisationStructureMappingRowChanging != null))
                    {
                        this.OfferRoleTypeOrganisationStructureMappingRowChanging(this, new OfferRoleTypeOrganisationStructureMappingRowChangeEvent(((OfferRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.OfferRoleTypeOrganisationStructureMappingRowDeleted != null))
                    {
                        this.OfferRoleTypeOrganisationStructureMappingRowDeleted(this, new OfferRoleTypeOrganisationStructureMappingRowChangeEvent(((OfferRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.OfferRoleTypeOrganisationStructureMappingRowDeleting != null))
                    {
                        this.OfferRoleTypeOrganisationStructureMappingRowDeleting(this, new OfferRoleTypeOrganisationStructureMappingRowChangeEvent(((OfferRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveOfferRoleTypeOrganisationStructureMappingRow(OfferRoleTypeOrganisationStructureMappingRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "OfferRoleTypeOrganisationStructureMappingDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class OrganisationStructureDataTable : global::System.Data.TypedTableBase<OrganisationStructureRow>
            {
                private global::System.Data.DataColumn columnOrganisationStructureKey;

                private global::System.Data.DataColumn columnParentKey;

                private global::System.Data.DataColumn columnDescription;

                private global::System.Data.DataColumn columnOrganisationTypeKey;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureDataTable()
                {
                    this.TableName = "OrganisationStructure";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OrganisationStructureDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected OrganisationStructureDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ParentKeyColumn
                {
                    get
                    {
                        return this.columnParentKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn DescriptionColumn
                {
                    get
                    {
                        return this.columnDescription;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationTypeKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow this[int index]
                {
                    get
                    {
                        return ((OrganisationStructureRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OrganisationStructureRowChangeEventHandler OrganisationStructureRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OrganisationStructureRowChangeEventHandler OrganisationStructureRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OrganisationStructureRowChangeEventHandler OrganisationStructureRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event OrganisationStructureRowChangeEventHandler OrganisationStructureRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddOrganisationStructureRow(OrganisationStructureRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow AddOrganisationStructureRow(int ParentKey, string Description, int OrganisationTypeKey, int GeneralStatusKey)
                {
                    OrganisationStructureRow rowOrganisationStructureRow = ((OrganisationStructureRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        ParentKey,
                        Description,
                        OrganisationTypeKey,
                        GeneralStatusKey};
                    rowOrganisationStructureRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowOrganisationStructureRow);
                    return rowOrganisationStructureRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow FindByOrganisationStructureKey(int OrganisationStructureKey)
                {
                    return ((OrganisationStructureRow)(this.Rows.Find(new object[] {
                            OrganisationStructureKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    OrganisationStructureDataTable cln = ((OrganisationStructureDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new OrganisationStructureDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnOrganisationStructureKey = base.Columns["OrganisationStructureKey"];
                    this.columnParentKey = base.Columns["ParentKey"];
                    this.columnDescription = base.Columns["Description"];
                    this.columnOrganisationTypeKey = base.Columns["OrganisationTypeKey"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnOrganisationStructureKey = new global::System.Data.DataColumn("OrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationStructureKey);
                    this.columnParentKey = new global::System.Data.DataColumn("ParentKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnParentKey);
                    this.columnDescription = new global::System.Data.DataColumn("Description", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnDescription);
                    this.columnOrganisationTypeKey = new global::System.Data.DataColumn("OrganisationTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationTypeKey);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnOrganisationStructureKey}, true));
                    this.columnOrganisationStructureKey.AutoIncrement = true;
                    this.columnOrganisationStructureKey.AllowDBNull = false;
                    this.columnOrganisationStructureKey.ReadOnly = true;
                    this.columnOrganisationStructureKey.Unique = true;
                    this.columnDescription.AllowDBNull = false;
                    this.columnDescription.MaxLength = 255;
                    this.columnGeneralStatusKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow NewOrganisationStructureRow()
                {
                    return ((OrganisationStructureRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new OrganisationStructureRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(OrganisationStructureRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.OrganisationStructureRowChanged != null))
                    {
                        this.OrganisationStructureRowChanged(this, new OrganisationStructureRowChangeEvent(((OrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.OrganisationStructureRowChanging != null))
                    {
                        this.OrganisationStructureRowChanging(this, new OrganisationStructureRowChangeEvent(((OrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.OrganisationStructureRowDeleted != null))
                    {
                        this.OrganisationStructureRowDeleted(this, new OrganisationStructureRowChangeEvent(((OrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.OrganisationStructureRowDeleting != null))
                    {
                        this.OrganisationStructureRowDeleting(this, new OrganisationStructureRowChangeEvent(((OrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveOrganisationStructureRow(OrganisationStructureRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "OrganisationStructureDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class UserOrganisationStructureDataTable : global::System.Data.TypedTableBase<UserOrganisationStructureRow>
            {
                private global::System.Data.DataColumn columnUserOrganisationStructureKey;

                private global::System.Data.DataColumn columnADUserKey;

                private global::System.Data.DataColumn columnOrganisationStructureKey;

                private global::System.Data.DataColumn columnGenericKey;

                private global::System.Data.DataColumn columnGenericKeyTypeKey;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureDataTable()
                {
                    this.TableName = "UserOrganisationStructure";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal UserOrganisationStructureDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected UserOrganisationStructureDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn UserOrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnUserOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserKeyColumn
                {
                    get
                    {
                        return this.columnADUserKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GenericKeyColumn
                {
                    get
                    {
                        return this.columnGenericKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GenericKeyTypeKeyColumn
                {
                    get
                    {
                        return this.columnGenericKeyTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow this[int index]
                {
                    get
                    {
                        return ((UserOrganisationStructureRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRowChangeEventHandler UserOrganisationStructureRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRowChangeEventHandler UserOrganisationStructureRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRowChangeEventHandler UserOrganisationStructureRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRowChangeEventHandler UserOrganisationStructureRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddUserOrganisationStructureRow(UserOrganisationStructureRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow AddUserOrganisationStructureRow(ADUserRow parentADUserRowByFK_UserOrganisationStructure_ADUser, OrganisationStructureRow parentOrganisationStructureRowByFK_UserOrganisationStructure_OrganisationStructure, int GenericKey, int GenericKeyTypeKey, int GeneralStatusKey)
                {
                    UserOrganisationStructureRow rowUserOrganisationStructureRow = ((UserOrganisationStructureRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        null,
                        null,
                        GenericKey,
                        GenericKeyTypeKey,
                        GeneralStatusKey};
                    if ((parentADUserRowByFK_UserOrganisationStructure_ADUser != null))
                    {
                        columnValuesArray[1] = parentADUserRowByFK_UserOrganisationStructure_ADUser[0];
                    }
                    if ((parentOrganisationStructureRowByFK_UserOrganisationStructure_OrganisationStructure != null))
                    {
                        columnValuesArray[2] = parentOrganisationStructureRowByFK_UserOrganisationStructure_OrganisationStructure[0];
                    }
                    rowUserOrganisationStructureRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowUserOrganisationStructureRow);
                    return rowUserOrganisationStructureRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow FindByUserOrganisationStructureKey(int UserOrganisationStructureKey)
                {
                    return ((UserOrganisationStructureRow)(this.Rows.Find(new object[] {
                            UserOrganisationStructureKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    UserOrganisationStructureDataTable cln = ((UserOrganisationStructureDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new UserOrganisationStructureDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnUserOrganisationStructureKey = base.Columns["UserOrganisationStructureKey"];
                    this.columnADUserKey = base.Columns["ADUserKey"];
                    this.columnOrganisationStructureKey = base.Columns["OrganisationStructureKey"];
                    this.columnGenericKey = base.Columns["GenericKey"];
                    this.columnGenericKeyTypeKey = base.Columns["GenericKeyTypeKey"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnUserOrganisationStructureKey = new global::System.Data.DataColumn("UserOrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnUserOrganisationStructureKey);
                    this.columnADUserKey = new global::System.Data.DataColumn("ADUserKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserKey);
                    this.columnOrganisationStructureKey = new global::System.Data.DataColumn("OrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationStructureKey);
                    this.columnGenericKey = new global::System.Data.DataColumn("GenericKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGenericKey);
                    this.columnGenericKeyTypeKey = new global::System.Data.DataColumn("GenericKeyTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGenericKeyTypeKey);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnUserOrganisationStructureKey}, true));
                    this.columnUserOrganisationStructureKey.AutoIncrement = true;
                    this.columnUserOrganisationStructureKey.AllowDBNull = false;
                    this.columnUserOrganisationStructureKey.ReadOnly = true;
                    this.columnUserOrganisationStructureKey.Unique = true;
                    this.columnADUserKey.AllowDBNull = false;
                    this.columnOrganisationStructureKey.AllowDBNull = false;
                    this.columnGeneralStatusKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow NewUserOrganisationStructureRow()
                {
                    return ((UserOrganisationStructureRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new UserOrganisationStructureRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(UserOrganisationStructureRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.UserOrganisationStructureRowChanged != null))
                    {
                        this.UserOrganisationStructureRowChanged(this, new UserOrganisationStructureRowChangeEvent(((UserOrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.UserOrganisationStructureRowChanging != null))
                    {
                        this.UserOrganisationStructureRowChanging(this, new UserOrganisationStructureRowChangeEvent(((UserOrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.UserOrganisationStructureRowDeleted != null))
                    {
                        this.UserOrganisationStructureRowDeleted(this, new UserOrganisationStructureRowChangeEvent(((UserOrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.UserOrganisationStructureRowDeleting != null))
                    {
                        this.UserOrganisationStructureRowDeleting(this, new UserOrganisationStructureRowChangeEvent(((UserOrganisationStructureRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveUserOrganisationStructureRow(UserOrganisationStructureRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "UserOrganisationStructureDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class WFAssignmentDataTable : global::System.Data.TypedTableBase<WFAssignmentRow>
            {
                private global::System.Data.DataColumn columnID;

                private global::System.Data.DataColumn columnIID;

                private global::System.Data.DataColumn columnADUserKey;

                private global::System.Data.DataColumn columnBlaKey;

                private global::System.Data.DataColumn columnGSKey;

                private global::System.Data.DataColumn columnADUserName;

                private global::System.Data.DataColumn columnORT;

                private global::System.Data.DataColumn columnOfferRoleTypeKey;

                private global::System.Data.DataColumn columnDescription;

                private global::System.Data.DataColumn columnOrganisationStructureKey;

                private global::System.Data.DataColumn columnParentKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentDataTable()
                {
                    this.TableName = "WFAssignment";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WFAssignmentDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected WFAssignmentDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn IDColumn
                {
                    get
                    {
                        return this.columnID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn IIDColumn
                {
                    get
                    {
                        return this.columnIID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserKeyColumn
                {
                    get
                    {
                        return this.columnADUserKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn BlaKeyColumn
                {
                    get
                    {
                        return this.columnBlaKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GSKeyColumn
                {
                    get
                    {
                        return this.columnGSKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserNameColumn
                {
                    get
                    {
                        return this.columnADUserName;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ORTColumn
                {
                    get
                    {
                        return this.columnORT;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OfferRoleTypeKeyColumn
                {
                    get
                    {
                        return this.columnOfferRoleTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn DescriptionColumn
                {
                    get
                    {
                        return this.columnDescription;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ParentKeyColumn
                {
                    get
                    {
                        return this.columnParentKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentRow this[int index]
                {
                    get
                    {
                        return ((WFAssignmentRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFAssignmentRowChangeEventHandler WFAssignmentRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFAssignmentRowChangeEventHandler WFAssignmentRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFAssignmentRowChangeEventHandler WFAssignmentRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFAssignmentRowChangeEventHandler WFAssignmentRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddWFAssignmentRow(WFAssignmentRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentRow AddWFAssignmentRow(int ID, int IID, int ADUserKey, int BlaKey, int GSKey, string ADUserName, string ORT, int OfferRoleTypeKey, string Description, int OrganisationStructureKey, int ParentKey)
                {
                    WFAssignmentRow rowWFAssignmentRow = ((WFAssignmentRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        ID,
                        IID,
                        ADUserKey,
                        BlaKey,
                        GSKey,
                        ADUserName,
                        ORT,
                        OfferRoleTypeKey,
                        Description,
                        OrganisationStructureKey,
                        ParentKey};
                    rowWFAssignmentRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowWFAssignmentRow);
                    return rowWFAssignmentRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    WFAssignmentDataTable cln = ((WFAssignmentDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new WFAssignmentDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnID = base.Columns["ID"];
                    this.columnIID = base.Columns["IID"];
                    this.columnADUserKey = base.Columns["ADUserKey"];
                    this.columnBlaKey = base.Columns["BlaKey"];
                    this.columnGSKey = base.Columns["GSKey"];
                    this.columnADUserName = base.Columns["ADUserName"];
                    this.columnORT = base.Columns["ORT"];
                    this.columnOfferRoleTypeKey = base.Columns["OfferRoleTypeKey"];
                    this.columnDescription = base.Columns["Description"];
                    this.columnOrganisationStructureKey = base.Columns["OrganisationStructureKey"];
                    this.columnParentKey = base.Columns["ParentKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnID = new global::System.Data.DataColumn("ID", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnID);
                    this.columnIID = new global::System.Data.DataColumn("IID", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnIID);
                    this.columnADUserKey = new global::System.Data.DataColumn("ADUserKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserKey);
                    this.columnBlaKey = new global::System.Data.DataColumn("BlaKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnBlaKey);
                    this.columnGSKey = new global::System.Data.DataColumn("GSKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGSKey);
                    this.columnADUserName = new global::System.Data.DataColumn("ADUserName", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserName);
                    this.columnORT = new global::System.Data.DataColumn("ORT", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnORT);
                    this.columnOfferRoleTypeKey = new global::System.Data.DataColumn("OfferRoleTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOfferRoleTypeKey);
                    this.columnDescription = new global::System.Data.DataColumn("Description", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnDescription);
                    this.columnOrganisationStructureKey = new global::System.Data.DataColumn("OrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationStructureKey);
                    this.columnParentKey = new global::System.Data.DataColumn("ParentKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnParentKey);
                    this.columnIID.Caption = "ID";
                    this.columnADUserKey.Caption = "ID";
                    this.columnBlaKey.Caption = "ID";
                    this.columnGSKey.Caption = "ID";
                    this.columnADUserName.Caption = "ID";
                    this.columnORT.Caption = "ID";
                    this.columnOfferRoleTypeKey.Caption = "ID";
                    this.columnDescription.Caption = "ID";
                    this.columnOrganisationStructureKey.Caption = "ID";
                    this.columnParentKey.DefaultValue = ((int)(-1));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentRow NewWFAssignmentRow()
                {
                    return ((WFAssignmentRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new WFAssignmentRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(WFAssignmentRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.WFAssignmentRowChanged != null))
                    {
                        this.WFAssignmentRowChanged(this, new WFAssignmentRowChangeEvent(((WFAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.WFAssignmentRowChanging != null))
                    {
                        this.WFAssignmentRowChanging(this, new WFAssignmentRowChangeEvent(((WFAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.WFAssignmentRowDeleted != null))
                    {
                        this.WFAssignmentRowDeleted(this, new WFAssignmentRowChangeEvent(((WFAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.WFAssignmentRowDeleting != null))
                    {
                        this.WFAssignmentRowDeleting(this, new WFAssignmentRowChangeEvent(((WFAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveWFAssignmentRow(WFAssignmentRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "WFAssignmentDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            //[global::System.Serializable()]
            //[global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            //public partial class RoundRobinPointerDataTable : global::System.Data.TypedTableBase<RoundRobinPointerRow>
            //{
            //    private global::System.Data.DataColumn columnRoundRobinPointerKey;

            //    private global::System.Data.DataColumn columnRoundRobinPointerIndexID;

            //    private global::System.Data.DataColumn columnDescription;

            //    private global::System.Data.DataColumn columnGeneralStatusKey;

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public RoundRobinPointerDataTable()
            //    {
            //        this.TableName = "RoundRobinPointer";
            //        this.BeginInit();
            //        this.InitClass();
            //        this.EndInit();
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    internal RoundRobinPointerDataTable(global::System.Data.DataTable table)
            //    {
            //        this.TableName = table.TableName;
            //        if ((table.CaseSensitive != table.DataSet.CaseSensitive))
            //        {
            //            this.CaseSensitive = table.CaseSensitive;
            //        }
            //        if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
            //        {
            //            this.Locale = table.Locale;
            //        }
            //        if ((table.Namespace != table.DataSet.Namespace))
            //        {
            //            this.Namespace = table.Namespace;
            //        }
            //        this.Prefix = table.Prefix;
            //        this.MinimumCapacity = table.MinimumCapacity;
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    protected RoundRobinPointerDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
            //        base(info, context)
            //    {
            //        this.InitVars();
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public global::System.Data.DataColumn RoundRobinPointerKeyColumn
            //    {
            //        get
            //        {
            //            return this.columnRoundRobinPointerKey;
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public global::System.Data.DataColumn RoundRobinPointerIndexIDColumn
            //    {
            //        get
            //        {
            //            return this.columnRoundRobinPointerIndexID;
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public global::System.Data.DataColumn DescriptionColumn
            //    {
            //        get
            //        {
            //            return this.columnDescription;
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public global::System.Data.DataColumn GeneralStatusKeyColumn
            //    {
            //        get
            //        {
            //            return this.columnGeneralStatusKey;
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    [global::System.ComponentModel.Browsable(false)]
            //    public int Count
            //    {
            //        get
            //        {
            //            return this.Rows.Count;
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public RoundRobinPointerRow this[int index]
            //    {
            //        get
            //        {
            //            return ((RoundRobinPointerRow)(this.Rows[index]));
            //        }
            //    }

            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public event RoundRobinPointerRowChangeEventHandler RoundRobinPointerRowChanging;

            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public event RoundRobinPointerRowChangeEventHandler RoundRobinPointerRowChanged;

            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public event RoundRobinPointerRowChangeEventHandler RoundRobinPointerRowDeleting;

            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public event RoundRobinPointerRowChangeEventHandler RoundRobinPointerRowDeleted;

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public void AddRoundRobinPointerRow(RoundRobinPointerRow row)
            //    {
            //        this.Rows.Add(row);
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public RoundRobinPointerRow AddRoundRobinPointerRow(int RoundRobinPointerIndexID, string Description, int GeneralStatusKey)
            //    {
            //        RoundRobinPointerRow rowRoundRobinPointerRow = ((RoundRobinPointerRow)(this.NewRow()));
            //        object[] columnValuesArray = new object[] {
            //            null,
            //            RoundRobinPointerIndexID,
            //            Description,
            //            GeneralStatusKey};
            //        rowRoundRobinPointerRow.ItemArray = columnValuesArray;
            //        this.Rows.Add(rowRoundRobinPointerRow);
            //        return rowRoundRobinPointerRow;
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public RoundRobinPointerRow FindByRoundRobinPointerKey(int RoundRobinPointerKey)
            //    {
            //        return ((RoundRobinPointerRow)(this.Rows.Find(new object[] {
            //                RoundRobinPointerKey})));
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public override global::System.Data.DataTable Clone()
            //    {
            //        RoundRobinPointerDataTable cln = ((RoundRobinPointerDataTable)(base.Clone()));
            //        cln.InitVars();
            //        return cln;
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    protected override global::System.Data.DataTable CreateInstance()
            //    {
            //        return new RoundRobinPointerDataTable();
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    internal void InitVars()
            //    {
            //        this.columnRoundRobinPointerKey = base.Columns["RoundRobinPointerKey"];
            //        this.columnRoundRobinPointerIndexID = base.Columns["RoundRobinPointerIndexID"];
            //        this.columnDescription = base.Columns["Description"];
            //        this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    private void InitClass()
            //    {
            //        this.columnRoundRobinPointerKey = new global::System.Data.DataColumn("RoundRobinPointerKey", typeof(int), null, global::System.Data.MappingType.Element);
            //        base.Columns.Add(this.columnRoundRobinPointerKey);
            //        this.columnRoundRobinPointerIndexID = new global::System.Data.DataColumn("RoundRobinPointerIndexID", typeof(int), null, global::System.Data.MappingType.Element);
            //        base.Columns.Add(this.columnRoundRobinPointerIndexID);
            //        this.columnDescription = new global::System.Data.DataColumn("Description", typeof(string), null, global::System.Data.MappingType.Element);
            //        base.Columns.Add(this.columnDescription);
            //        this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
            //        base.Columns.Add(this.columnGeneralStatusKey);
            //        this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
            //                    this.columnRoundRobinPointerKey}, true));
            //        this.columnRoundRobinPointerKey.AutoIncrement = true;
            //        this.columnRoundRobinPointerKey.AllowDBNull = false;
            //        this.columnRoundRobinPointerKey.ReadOnly = true;
            //        this.columnRoundRobinPointerKey.Unique = true;
            //        this.columnDescription.AllowDBNull = false;
            //        this.columnDescription.MaxLength = 50;
            //        this.columnGeneralStatusKey.AllowDBNull = false;
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public RoundRobinPointerRow NewRoundRobinPointerRow()
            //    {
            //        return ((RoundRobinPointerRow)(this.NewRow()));
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
            //    {
            //        return new RoundRobinPointerRow(builder);
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    protected override global::System.Type GetRowType()
            //    {
            //        return typeof(RoundRobinPointerRow);
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
            //    {
            //        base.OnRowChanged(e);
            //        if ((this.RoundRobinPointerRowChanged != null))
            //        {
            //            this.RoundRobinPointerRowChanged(this, new RoundRobinPointerRowChangeEvent(((RoundRobinPointerRow)(e.Row)), e.Action));
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
            //    {
            //        base.OnRowChanging(e);
            //        if ((this.RoundRobinPointerRowChanging != null))
            //        {
            //            this.RoundRobinPointerRowChanging(this, new RoundRobinPointerRowChangeEvent(((RoundRobinPointerRow)(e.Row)), e.Action));
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
            //    {
            //        base.OnRowDeleted(e);
            //        if ((this.RoundRobinPointerRowDeleted != null))
            //        {
            //            this.RoundRobinPointerRowDeleted(this, new RoundRobinPointerRowChangeEvent(((RoundRobinPointerRow)(e.Row)), e.Action));
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
            //    {
            //        base.OnRowDeleting(e);
            //        if ((this.RoundRobinPointerRowDeleting != null))
            //        {
            //            this.RoundRobinPointerRowDeleting(this, new RoundRobinPointerRowChangeEvent(((RoundRobinPointerRow)(e.Row)), e.Action));
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public void RemoveRoundRobinPointerRow(RoundRobinPointerRow row)
            //    {
            //        this.Rows.Remove(row);
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
            //    {
            //        global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
            //        global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
            //        WorkflowAssignment ds = new WorkflowAssignment();
            //        global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
            //        any1.Namespace = "http://www.w3.org/2001/XMLSchema";
            //        any1.MinOccurs = new decimal(0);
            //        any1.MaxOccurs = decimal.MaxValue;
            //        any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
            //        sequence.Items.Add(any1);
            //        global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
            //        any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
            //        any2.MinOccurs = new decimal(1);
            //        any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
            //        sequence.Items.Add(any2);
            //        global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
            //        attribute1.Name = "namespace";
            //        attribute1.FixedValue = ds.Namespace;
            //        type.Attributes.Add(attribute1);
            //        global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
            //        attribute2.Name = "tableTypeName";
            //        attribute2.FixedValue = "RoundRobinPointerDataTable";
            //        type.Attributes.Add(attribute2);
            //        type.Particle = sequence;
            //        global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
            //        if (xs.Contains(dsSchema.TargetNamespace))
            //        {
            //            global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
            //            global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
            //            try
            //            {
            //                global::System.Xml.Schema.XmlSchema schema = null;
            //                dsSchema.Write(s1);
            //                for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
            //                {
            //                    schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
            //                    s2.SetLength(0);
            //                    schema.Write(s2);
            //                    if ((s1.Length == s2.Length))
            //                    {
            //                        s1.Position = 0;
            //                        s2.Position = 0;
            //                        for (; ((s1.Position != s1.Length)
            //                                    && (s1.ReadByte() == s2.ReadByte())); )
            //                        {
            //                            ;
            //                        }
            //                        if ((s1.Position == s1.Length))
            //                        {
            //                            return type;
            //                        }
            //                    }
            //                }
            //            }
            //            finally
            //            {
            //                if ((s1 != null))
            //                {
            //                    s1.Close();
            //                }
            //                if ((s2 != null))
            //                {
            //                    s2.Close();
            //                }
            //            }
            //        }
            //        xs.Add(dsSchema);
            //        return type;
            //    }
            //}

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class RoundRobinPointerDefinitionDataTable : global::System.Data.TypedTableBase<RoundRobinPointerDefinitionRow>
            {
                private global::System.Data.DataColumn columnRoundRobinPointerDefinitionKey;

                private global::System.Data.DataColumn columnRoundRobinPointerKey;

                private global::System.Data.DataColumn columnGenericKeyTypeKey;

                private global::System.Data.DataColumn columnGenericKey;

                private global::System.Data.DataColumn columnApplicationName;

                private global::System.Data.DataColumn columnStatementName;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public RoundRobinPointerDefinitionDataTable()
                {
                    this.TableName = "RoundRobinPointerDefinition";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal RoundRobinPointerDefinitionDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected RoundRobinPointerDefinitionDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn RoundRobinPointerDefinitionKeyColumn
                {
                    get
                    {
                        return this.columnRoundRobinPointerDefinitionKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn RoundRobinPointerKeyColumn
                {
                    get
                    {
                        return this.columnRoundRobinPointerKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GenericKeyTypeKeyColumn
                {
                    get
                    {
                        return this.columnGenericKeyTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GenericKeyColumn
                {
                    get
                    {
                        return this.columnGenericKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ApplicationNameColumn
                {
                    get
                    {
                        return this.columnApplicationName;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn StatementNameColumn
                {
                    get
                    {
                        return this.columnStatementName;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public RoundRobinPointerDefinitionRow this[int index]
                {
                    get
                    {
                        return ((RoundRobinPointerDefinitionRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event RoundRobinPointerDefinitionRowChangeEventHandler RoundRobinPointerDefinitionRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event RoundRobinPointerDefinitionRowChangeEventHandler RoundRobinPointerDefinitionRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event RoundRobinPointerDefinitionRowChangeEventHandler RoundRobinPointerDefinitionRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event RoundRobinPointerDefinitionRowChangeEventHandler RoundRobinPointerDefinitionRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddRoundRobinPointerDefinitionRow(RoundRobinPointerDefinitionRow row)
                {
                    this.Rows.Add(row);
                }

                //[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                //[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                //public RoundRobinPointerDefinitionRow AddRoundRobinPointerDefinitionRow(RoundRobinPointerRow parentRoundRobinPointerRowByFK_RoundRobinPointerDefinition_RoundRobinPointer, int GenericKeyTypeKey, int GenericKey, string ApplicationName, string StatementName, int GeneralStatusKey)
                //{
                //    RoundRobinPointerDefinitionRow rowRoundRobinPointerDefinitionRow = ((RoundRobinPointerDefinitionRow)(this.NewRow()));
                //    object[] columnValuesArray = new object[] {
                //        null,
                //        null,
                //        GenericKeyTypeKey,
                //        GenericKey,
                //        ApplicationName,
                //        StatementName,
                //        GeneralStatusKey};
                //    if ((parentRoundRobinPointerRowByFK_RoundRobinPointerDefinition_RoundRobinPointer != null))
                //    {
                //        columnValuesArray[1] = parentRoundRobinPointerRowByFK_RoundRobinPointerDefinition_RoundRobinPointer[0];
                //    }
                //    rowRoundRobinPointerDefinitionRow.ItemArray = columnValuesArray;
                //    this.Rows.Add(rowRoundRobinPointerDefinitionRow);
                //    return rowRoundRobinPointerDefinitionRow;
                //}

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public RoundRobinPointerDefinitionRow FindByRoundRobinPointerDefinitionKey(int RoundRobinPointerDefinitionKey)
                {
                    return ((RoundRobinPointerDefinitionRow)(this.Rows.Find(new object[] {
                            RoundRobinPointerDefinitionKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    RoundRobinPointerDefinitionDataTable cln = ((RoundRobinPointerDefinitionDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new RoundRobinPointerDefinitionDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnRoundRobinPointerDefinitionKey = base.Columns["RoundRobinPointerDefinitionKey"];
                    this.columnRoundRobinPointerKey = base.Columns["RoundRobinPointerKey"];
                    this.columnGenericKeyTypeKey = base.Columns["GenericKeyTypeKey"];
                    this.columnGenericKey = base.Columns["GenericKey"];
                    this.columnApplicationName = base.Columns["ApplicationName"];
                    this.columnStatementName = base.Columns["StatementName"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnRoundRobinPointerDefinitionKey = new global::System.Data.DataColumn("RoundRobinPointerDefinitionKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnRoundRobinPointerDefinitionKey);
                    this.columnRoundRobinPointerKey = new global::System.Data.DataColumn("RoundRobinPointerKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnRoundRobinPointerKey);
                    this.columnGenericKeyTypeKey = new global::System.Data.DataColumn("GenericKeyTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGenericKeyTypeKey);
                    this.columnGenericKey = new global::System.Data.DataColumn("GenericKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGenericKey);
                    this.columnApplicationName = new global::System.Data.DataColumn("ApplicationName", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnApplicationName);
                    this.columnStatementName = new global::System.Data.DataColumn("StatementName", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnStatementName);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnRoundRobinPointerDefinitionKey}, true));
                    this.columnRoundRobinPointerDefinitionKey.AutoIncrement = true;
                    this.columnRoundRobinPointerDefinitionKey.AllowDBNull = false;
                    this.columnRoundRobinPointerDefinitionKey.ReadOnly = true;
                    this.columnRoundRobinPointerDefinitionKey.Unique = true;
                    this.columnRoundRobinPointerKey.AllowDBNull = false;
                    this.columnGenericKeyTypeKey.AllowDBNull = false;
                    this.columnGenericKey.AllowDBNull = false;
                    this.columnApplicationName.AllowDBNull = false;
                    this.columnApplicationName.MaxLength = 100;
                    this.columnStatementName.AllowDBNull = false;
                    this.columnStatementName.MaxLength = 50;
                    this.columnGeneralStatusKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public RoundRobinPointerDefinitionRow NewRoundRobinPointerDefinitionRow()
                {
                    return ((RoundRobinPointerDefinitionRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new RoundRobinPointerDefinitionRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(RoundRobinPointerDefinitionRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.RoundRobinPointerDefinitionRowChanged != null))
                    {
                        this.RoundRobinPointerDefinitionRowChanged(this, new RoundRobinPointerDefinitionRowChangeEvent(((RoundRobinPointerDefinitionRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.RoundRobinPointerDefinitionRowChanging != null))
                    {
                        this.RoundRobinPointerDefinitionRowChanging(this, new RoundRobinPointerDefinitionRowChangeEvent(((RoundRobinPointerDefinitionRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.RoundRobinPointerDefinitionRowDeleted != null))
                    {
                        this.RoundRobinPointerDefinitionRowDeleted(this, new RoundRobinPointerDefinitionRowChangeEvent(((RoundRobinPointerDefinitionRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.RoundRobinPointerDefinitionRowDeleting != null))
                    {
                        this.RoundRobinPointerDefinitionRowDeleting(this, new RoundRobinPointerDefinitionRowChangeEvent(((RoundRobinPointerDefinitionRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveRoundRobinPointerDefinitionRow(RoundRobinPointerDefinitionRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "RoundRobinPointerDefinitionDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class UserOrganisationStructureRoundRobinStatusDataTable : global::System.Data.TypedTableBase<UserOrganisationStructureRoundRobinStatusRow>
            {
                private global::System.Data.DataColumn columnUserOrganisationStructureRoundRobinStatusKey;

                private global::System.Data.DataColumn columnUserOrganisationStructureKey;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                private global::System.Data.DataColumn columnCapitecGeneralStatusKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRoundRobinStatusDataTable()
                {
                    this.TableName = "UserOrganisationStructureRoundRobinStatus";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal UserOrganisationStructureRoundRobinStatusDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected UserOrganisationStructureRoundRobinStatusDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn UserOrganisationStructureRoundRobinStatusKeyColumn
                {
                    get
                    {
                        return this.columnUserOrganisationStructureRoundRobinStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn UserOrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnUserOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn CapitecGeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnCapitecGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRoundRobinStatusRow this[int index]
                {
                    get
                    {
                        return ((UserOrganisationStructureRoundRobinStatusRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRoundRobinStatusRowChangeEventHandler UserOrganisationStructureRoundRobinStatusRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRoundRobinStatusRowChangeEventHandler UserOrganisationStructureRoundRobinStatusRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRoundRobinStatusRowChangeEventHandler UserOrganisationStructureRoundRobinStatusRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event UserOrganisationStructureRoundRobinStatusRowChangeEventHandler UserOrganisationStructureRoundRobinStatusRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddUserOrganisationStructureRoundRobinStatusRow(UserOrganisationStructureRoundRobinStatusRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRoundRobinStatusRow AddUserOrganisationStructureRoundRobinStatusRow(UserOrganisationStructureRow parentUserOrganisationStructureRowByUserOrganisationStructure_UserOrganisationStructureRoundRobinStatus, int GeneralStatusKey)
                {
                    UserOrganisationStructureRoundRobinStatusRow rowUserOrganisationStructureRoundRobinStatusRow = ((UserOrganisationStructureRoundRobinStatusRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        null,
                        GeneralStatusKey};
                    if ((parentUserOrganisationStructureRowByUserOrganisationStructure_UserOrganisationStructureRoundRobinStatus != null))
                    {
                        columnValuesArray[1] = parentUserOrganisationStructureRowByUserOrganisationStructure_UserOrganisationStructureRoundRobinStatus[0];
                    }
                    rowUserOrganisationStructureRoundRobinStatusRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowUserOrganisationStructureRoundRobinStatusRow);
                    return rowUserOrganisationStructureRoundRobinStatusRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRoundRobinStatusRow FindByUserOrganisationStructureRoundRobinStatusKey(int UserOrganisationStructureRoundRobinStatusKey)
                {
                    return ((UserOrganisationStructureRoundRobinStatusRow)(this.Rows.Find(new object[] {
                            UserOrganisationStructureRoundRobinStatusKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    UserOrganisationStructureRoundRobinStatusDataTable cln = ((UserOrganisationStructureRoundRobinStatusDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new UserOrganisationStructureRoundRobinStatusDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnUserOrganisationStructureRoundRobinStatusKey = base.Columns["UserOrganisationStructureRoundRobinStatusKey"];
                    this.columnUserOrganisationStructureKey = base.Columns["UserOrganisationStructureKey"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                    this.columnCapitecGeneralStatusKey = base.Columns["CapitecGeneralStatusKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnUserOrganisationStructureRoundRobinStatusKey = new global::System.Data.DataColumn("UserOrganisationStructureRoundRobinStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnUserOrganisationStructureRoundRobinStatusKey);
                    this.columnUserOrganisationStructureKey = new global::System.Data.DataColumn("UserOrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnUserOrganisationStructureKey);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.columnCapitecGeneralStatusKey = new global::System.Data.DataColumn("CapitecGeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnCapitecGeneralStatusKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnUserOrganisationStructureRoundRobinStatusKey}, true));
                    this.columnUserOrganisationStructureRoundRobinStatusKey.AutoIncrement = true;
                    this.columnUserOrganisationStructureRoundRobinStatusKey.AllowDBNull = false;
                    this.columnUserOrganisationStructureRoundRobinStatusKey.ReadOnly = true;
                    this.columnUserOrganisationStructureRoundRobinStatusKey.Unique = true;
                    this.columnUserOrganisationStructureKey.AllowDBNull = false;
                    this.columnGeneralStatusKey.AllowDBNull = false;
                    this.columnCapitecGeneralStatusKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRoundRobinStatusRow NewUserOrganisationStructureRoundRobinStatusRow()
                {
                    return ((UserOrganisationStructureRoundRobinStatusRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new UserOrganisationStructureRoundRobinStatusRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(UserOrganisationStructureRoundRobinStatusRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.UserOrganisationStructureRoundRobinStatusRowChanged != null))
                    {
                        this.UserOrganisationStructureRoundRobinStatusRowChanged(this, new UserOrganisationStructureRoundRobinStatusRowChangeEvent(((UserOrganisationStructureRoundRobinStatusRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.UserOrganisationStructureRoundRobinStatusRowChanging != null))
                    {
                        this.UserOrganisationStructureRoundRobinStatusRowChanging(this, new UserOrganisationStructureRoundRobinStatusRowChangeEvent(((UserOrganisationStructureRoundRobinStatusRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.UserOrganisationStructureRoundRobinStatusRowDeleted != null))
                    {
                        this.UserOrganisationStructureRoundRobinStatusRowDeleted(this, new UserOrganisationStructureRoundRobinStatusRowChangeEvent(((UserOrganisationStructureRoundRobinStatusRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.UserOrganisationStructureRoundRobinStatusRowDeleting != null))
                    {
                        this.UserOrganisationStructureRoundRobinStatusRowDeleting(this, new UserOrganisationStructureRoundRobinStatusRowChangeEvent(((UserOrganisationStructureRoundRobinStatusRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveUserOrganisationStructureRoundRobinStatusRow(UserOrganisationStructureRoundRobinStatusRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "UserOrganisationStructureRoundRobinStatusDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class WorkflowRoleAssignmentDataTable : global::System.Data.TypedTableBase<WorkflowRoleAssignmentRow>
            {
                private global::System.Data.DataColumn columnID;

                private global::System.Data.DataColumn columnInstanceID;

                private global::System.Data.DataColumn columnWorkflowRoleTypeOrganisationStructureMappingKey;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                private global::System.Data.DataColumn columnADUserKey;

                private global::System.Data.DataColumn columnState;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleAssignmentDataTable()
                {
                    this.TableName = "WorkflowRoleAssignment";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowRoleAssignmentDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected WorkflowRoleAssignmentDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn IDColumn
                {
                    get
                    {
                        return this.columnID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn InstanceIDColumn
                {
                    get
                    {
                        return this.columnInstanceID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn WorkflowRoleTypeOrganisationStructureMappingKeyColumn
                {
                    get
                    {
                        return this.columnWorkflowRoleTypeOrganisationStructureMappingKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserKeyColumn
                {
                    get
                    {
                        return this.columnADUserKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn StateColumn
                {
                    get
                    {
                        return this.columnState;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleAssignmentRow this[int index]
                {
                    get
                    {
                        return ((WorkflowRoleAssignmentRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleAssignmentRowChangeEventHandler WorkflowRoleAssignmentRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleAssignmentRowChangeEventHandler WorkflowRoleAssignmentRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleAssignmentRowChangeEventHandler WorkflowRoleAssignmentRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleAssignmentRowChangeEventHandler WorkflowRoleAssignmentRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddWorkflowRoleAssignmentRow(WorkflowRoleAssignmentRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleAssignmentRow AddWorkflowRoleAssignmentRow(string ID, long InstanceID, int WorkflowRoleTypeOrganisationStructureMappingKey, int GeneralStatusKey, int ADUserKey, string State)
                {
                    WorkflowRoleAssignmentRow rowWorkflowRoleAssignmentRow = ((WorkflowRoleAssignmentRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        ID,
                        InstanceID,
                        WorkflowRoleTypeOrganisationStructureMappingKey,
                        GeneralStatusKey,
                        ADUserKey,
                        State};
                    rowWorkflowRoleAssignmentRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowWorkflowRoleAssignmentRow);
                    return rowWorkflowRoleAssignmentRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleAssignmentRow FindByID(string ID)
                {
                    return ((WorkflowRoleAssignmentRow)(this.Rows.Find(new object[] {
                            ID})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    WorkflowRoleAssignmentDataTable cln = ((WorkflowRoleAssignmentDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new WorkflowRoleAssignmentDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnID = base.Columns["ID"];
                    this.columnInstanceID = base.Columns["InstanceID"];
                    this.columnWorkflowRoleTypeOrganisationStructureMappingKey = base.Columns["WorkflowRoleTypeOrganisationStructureMappingKey"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                    this.columnADUserKey = base.Columns["ADUserKey"];
                    this.columnState = base.Columns["State"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnID = new global::System.Data.DataColumn("ID", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnID);
                    this.columnInstanceID = new global::System.Data.DataColumn("InstanceID", typeof(long), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnInstanceID);
                    this.columnWorkflowRoleTypeOrganisationStructureMappingKey = new global::System.Data.DataColumn("WorkflowRoleTypeOrganisationStructureMappingKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnWorkflowRoleTypeOrganisationStructureMappingKey);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.columnADUserKey = new global::System.Data.DataColumn("ADUserKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserKey);
                    this.columnState = new global::System.Data.DataColumn("State", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnState);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnID}, true));
                    this.columnID.AllowDBNull = false;
                    this.columnID.Unique = true;
                    this.columnID.Caption = "Column1";
                    this.columnInstanceID.Caption = "Column1";
                    this.columnWorkflowRoleTypeOrganisationStructureMappingKey.Caption = "Column1";
                    this.columnGeneralStatusKey.Caption = "Column1";
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleAssignmentRow NewWorkflowRoleAssignmentRow()
                {
                    return ((WorkflowRoleAssignmentRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new WorkflowRoleAssignmentRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(WorkflowRoleAssignmentRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.WorkflowRoleAssignmentRowChanged != null))
                    {
                        this.WorkflowRoleAssignmentRowChanged(this, new WorkflowRoleAssignmentRowChangeEvent(((WorkflowRoleAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.WorkflowRoleAssignmentRowChanging != null))
                    {
                        this.WorkflowRoleAssignmentRowChanging(this, new WorkflowRoleAssignmentRowChangeEvent(((WorkflowRoleAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.WorkflowRoleAssignmentRowDeleted != null))
                    {
                        this.WorkflowRoleAssignmentRowDeleted(this, new WorkflowRoleAssignmentRowChangeEvent(((WorkflowRoleAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.WorkflowRoleAssignmentRowDeleting != null))
                    {
                        this.WorkflowRoleAssignmentRowDeleting(this, new WorkflowRoleAssignmentRowChangeEvent(((WorkflowRoleAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveWorkflowRoleAssignmentRow(WorkflowRoleAssignmentRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "WorkflowRoleAssignmentDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class WorkflowRoleTypeGroupDataTable : global::System.Data.TypedTableBase<WorkflowRoleTypeGroupRow>
            {
                private global::System.Data.DataColumn columnWorkflowRoleTypeGroupKey;

                private global::System.Data.DataColumn columnDescription;

                private global::System.Data.DataColumn columnGenericKeyTypeKey;

                private global::System.Data.DataColumn columnWorkflowOrganisationStructureKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeGroupDataTable()
                {
                    this.TableName = "WorkflowRoleTypeGroup";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowRoleTypeGroupDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected WorkflowRoleTypeGroupDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn WorkflowRoleTypeGroupKeyColumn
                {
                    get
                    {
                        return this.columnWorkflowRoleTypeGroupKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn DescriptionColumn
                {
                    get
                    {
                        return this.columnDescription;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GenericKeyTypeKeyColumn
                {
                    get
                    {
                        return this.columnGenericKeyTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn WorkflowOrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnWorkflowOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeGroupRow this[int index]
                {
                    get
                    {
                        return ((WorkflowRoleTypeGroupRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeGroupRowChangeEventHandler WorkflowRoleTypeGroupRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeGroupRowChangeEventHandler WorkflowRoleTypeGroupRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeGroupRowChangeEventHandler WorkflowRoleTypeGroupRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeGroupRowChangeEventHandler WorkflowRoleTypeGroupRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddWorkflowRoleTypeGroupRow(WorkflowRoleTypeGroupRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeGroupRow AddWorkflowRoleTypeGroupRow(int WorkflowRoleTypeGroupKey, string Description, int GenericKeyTypeKey, int WorkflowOrganisationStructureKey)
                {
                    WorkflowRoleTypeGroupRow rowWorkflowRoleTypeGroupRow = ((WorkflowRoleTypeGroupRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        WorkflowRoleTypeGroupKey,
                        Description,
                        GenericKeyTypeKey,
                        WorkflowOrganisationStructureKey};
                    rowWorkflowRoleTypeGroupRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowWorkflowRoleTypeGroupRow);
                    return rowWorkflowRoleTypeGroupRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeGroupRow FindByWorkflowRoleTypeGroupKey(int WorkflowRoleTypeGroupKey)
                {
                    return ((WorkflowRoleTypeGroupRow)(this.Rows.Find(new object[] {
                            WorkflowRoleTypeGroupKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    WorkflowRoleTypeGroupDataTable cln = ((WorkflowRoleTypeGroupDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new WorkflowRoleTypeGroupDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnWorkflowRoleTypeGroupKey = base.Columns["WorkflowRoleTypeGroupKey"];
                    this.columnDescription = base.Columns["Description"];
                    this.columnGenericKeyTypeKey = base.Columns["GenericKeyTypeKey"];
                    this.columnWorkflowOrganisationStructureKey = base.Columns["WorkflowOrganisationStructureKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnWorkflowRoleTypeGroupKey = new global::System.Data.DataColumn("WorkflowRoleTypeGroupKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnWorkflowRoleTypeGroupKey);
                    this.columnDescription = new global::System.Data.DataColumn("Description", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnDescription);
                    this.columnGenericKeyTypeKey = new global::System.Data.DataColumn("GenericKeyTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGenericKeyTypeKey);
                    this.columnWorkflowOrganisationStructureKey = new global::System.Data.DataColumn("WorkflowOrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnWorkflowOrganisationStructureKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnWorkflowRoleTypeGroupKey}, true));
                    this.columnWorkflowRoleTypeGroupKey.AllowDBNull = false;
                    this.columnWorkflowRoleTypeGroupKey.Unique = true;
                    this.columnDescription.AllowDBNull = false;
                    this.columnDescription.MaxLength = 50;
                    this.columnGenericKeyTypeKey.AllowDBNull = false;
                    this.columnWorkflowOrganisationStructureKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeGroupRow NewWorkflowRoleTypeGroupRow()
                {
                    return ((WorkflowRoleTypeGroupRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new WorkflowRoleTypeGroupRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(WorkflowRoleTypeGroupRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.WorkflowRoleTypeGroupRowChanged != null))
                    {
                        this.WorkflowRoleTypeGroupRowChanged(this, new WorkflowRoleTypeGroupRowChangeEvent(((WorkflowRoleTypeGroupRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.WorkflowRoleTypeGroupRowChanging != null))
                    {
                        this.WorkflowRoleTypeGroupRowChanging(this, new WorkflowRoleTypeGroupRowChangeEvent(((WorkflowRoleTypeGroupRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.WorkflowRoleTypeGroupRowDeleted != null))
                    {
                        this.WorkflowRoleTypeGroupRowDeleted(this, new WorkflowRoleTypeGroupRowChangeEvent(((WorkflowRoleTypeGroupRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.WorkflowRoleTypeGroupRowDeleting != null))
                    {
                        this.WorkflowRoleTypeGroupRowDeleting(this, new WorkflowRoleTypeGroupRowChangeEvent(((WorkflowRoleTypeGroupRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveWorkflowRoleTypeGroupRow(WorkflowRoleTypeGroupRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "WorkflowRoleTypeGroupDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class WorkflowRoleTypeOrganisationStructureMappingDataTable : global::System.Data.TypedTableBase<WorkflowRoleTypeOrganisationStructureMappingRow>
            {
                private global::System.Data.DataColumn columnWorkflowRoleTypeOrganisationStructureMappingKey;

                private global::System.Data.DataColumn columnWorkflowRoleTypeKey;

                private global::System.Data.DataColumn columnOrganisationStructureKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeOrganisationStructureMappingDataTable()
                {
                    this.TableName = "WorkflowRoleTypeOrganisationStructureMapping";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowRoleTypeOrganisationStructureMappingDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected WorkflowRoleTypeOrganisationStructureMappingDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn WorkflowRoleTypeOrganisationStructureMappingKeyColumn
                {
                    get
                    {
                        return this.columnWorkflowRoleTypeOrganisationStructureMappingKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn WorkflowRoleTypeKeyColumn
                {
                    get
                    {
                        return this.columnWorkflowRoleTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeOrganisationStructureMappingRow this[int index]
                {
                    get
                    {
                        return ((WorkflowRoleTypeOrganisationStructureMappingRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeOrganisationStructureMappingRowChangeEventHandler WorkflowRoleTypeOrganisationStructureMappingRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeOrganisationStructureMappingRowChangeEventHandler WorkflowRoleTypeOrganisationStructureMappingRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeOrganisationStructureMappingRowChangeEventHandler WorkflowRoleTypeOrganisationStructureMappingRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeOrganisationStructureMappingRowChangeEventHandler WorkflowRoleTypeOrganisationStructureMappingRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddWorkflowRoleTypeOrganisationStructureMappingRow(WorkflowRoleTypeOrganisationStructureMappingRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeOrganisationStructureMappingRow AddWorkflowRoleTypeOrganisationStructureMappingRow(WorkflowRoleTypeRow parentWorkflowRoleTypeRowByFK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType, OrganisationStructureRow parentOrganisationStructureRowByOrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping)
                {
                    WorkflowRoleTypeOrganisationStructureMappingRow rowWorkflowRoleTypeOrganisationStructureMappingRow = ((WorkflowRoleTypeOrganisationStructureMappingRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        null,
                        null,
                        null};
                    if ((parentWorkflowRoleTypeRowByFK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType != null))
                    {
                        columnValuesArray[1] = parentWorkflowRoleTypeRowByFK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType[0];
                    }
                    if ((parentOrganisationStructureRowByOrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping != null))
                    {
                        columnValuesArray[2] = parentOrganisationStructureRowByOrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping[0];
                    }
                    rowWorkflowRoleTypeOrganisationStructureMappingRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowWorkflowRoleTypeOrganisationStructureMappingRow);
                    return rowWorkflowRoleTypeOrganisationStructureMappingRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeOrganisationStructureMappingRow FindByWorkflowRoleTypeOrganisationStructureMappingKey(int WorkflowRoleTypeOrganisationStructureMappingKey)
                {
                    return ((WorkflowRoleTypeOrganisationStructureMappingRow)(this.Rows.Find(new object[] {
                            WorkflowRoleTypeOrganisationStructureMappingKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    WorkflowRoleTypeOrganisationStructureMappingDataTable cln = ((WorkflowRoleTypeOrganisationStructureMappingDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new WorkflowRoleTypeOrganisationStructureMappingDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnWorkflowRoleTypeOrganisationStructureMappingKey = base.Columns["WorkflowRoleTypeOrganisationStructureMappingKey"];
                    this.columnWorkflowRoleTypeKey = base.Columns["WorkflowRoleTypeKey"];
                    this.columnOrganisationStructureKey = base.Columns["OrganisationStructureKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnWorkflowRoleTypeOrganisationStructureMappingKey = new global::System.Data.DataColumn("WorkflowRoleTypeOrganisationStructureMappingKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnWorkflowRoleTypeOrganisationStructureMappingKey);
                    this.columnWorkflowRoleTypeKey = new global::System.Data.DataColumn("WorkflowRoleTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnWorkflowRoleTypeKey);
                    this.columnOrganisationStructureKey = new global::System.Data.DataColumn("OrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationStructureKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnWorkflowRoleTypeOrganisationStructureMappingKey}, true));
                    this.columnWorkflowRoleTypeOrganisationStructureMappingKey.AutoIncrement = true;
                    this.columnWorkflowRoleTypeOrganisationStructureMappingKey.AutoIncrementSeed = -1;
                    this.columnWorkflowRoleTypeOrganisationStructureMappingKey.AutoIncrementStep = -1;
                    this.columnWorkflowRoleTypeOrganisationStructureMappingKey.AllowDBNull = false;
                    this.columnWorkflowRoleTypeOrganisationStructureMappingKey.ReadOnly = true;
                    this.columnWorkflowRoleTypeOrganisationStructureMappingKey.Unique = true;
                    this.columnWorkflowRoleTypeKey.AllowDBNull = false;
                    this.columnOrganisationStructureKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeOrganisationStructureMappingRow NewWorkflowRoleTypeOrganisationStructureMappingRow()
                {
                    return ((WorkflowRoleTypeOrganisationStructureMappingRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new WorkflowRoleTypeOrganisationStructureMappingRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(WorkflowRoleTypeOrganisationStructureMappingRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.WorkflowRoleTypeOrganisationStructureMappingRowChanged != null))
                    {
                        this.WorkflowRoleTypeOrganisationStructureMappingRowChanged(this, new WorkflowRoleTypeOrganisationStructureMappingRowChangeEvent(((WorkflowRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.WorkflowRoleTypeOrganisationStructureMappingRowChanging != null))
                    {
                        this.WorkflowRoleTypeOrganisationStructureMappingRowChanging(this, new WorkflowRoleTypeOrganisationStructureMappingRowChangeEvent(((WorkflowRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.WorkflowRoleTypeOrganisationStructureMappingRowDeleted != null))
                    {
                        this.WorkflowRoleTypeOrganisationStructureMappingRowDeleted(this, new WorkflowRoleTypeOrganisationStructureMappingRowChangeEvent(((WorkflowRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.WorkflowRoleTypeOrganisationStructureMappingRowDeleting != null))
                    {
                        this.WorkflowRoleTypeOrganisationStructureMappingRowDeleting(this, new WorkflowRoleTypeOrganisationStructureMappingRowChangeEvent(((WorkflowRoleTypeOrganisationStructureMappingRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveWorkflowRoleTypeOrganisationStructureMappingRow(WorkflowRoleTypeOrganisationStructureMappingRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "WorkflowRoleTypeOrganisationStructureMappingDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class WorkflowRoleTypeDataTable : global::System.Data.TypedTableBase<WorkflowRoleTypeRow>
            {
                private global::System.Data.DataColumn columnWorkflowRoleTypeKey;

                private global::System.Data.DataColumn columnDescription;

                private global::System.Data.DataColumn columnWorkflowRoleTypeGroupKey;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeDataTable()
                {
                    this.TableName = "WorkflowRoleType";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowRoleTypeDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected WorkflowRoleTypeDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn WorkflowRoleTypeKeyColumn
                {
                    get
                    {
                        return this.columnWorkflowRoleTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn DescriptionColumn
                {
                    get
                    {
                        return this.columnDescription;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn WorkflowRoleTypeGroupKeyColumn
                {
                    get
                    {
                        return this.columnWorkflowRoleTypeGroupKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeRow this[int index]
                {
                    get
                    {
                        return ((WorkflowRoleTypeRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeRowChangeEventHandler WorkflowRoleTypeRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeRowChangeEventHandler WorkflowRoleTypeRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeRowChangeEventHandler WorkflowRoleTypeRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WorkflowRoleTypeRowChangeEventHandler WorkflowRoleTypeRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddWorkflowRoleTypeRow(WorkflowRoleTypeRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeRow AddWorkflowRoleTypeRow(int WorkflowRoleTypeKey, string Description, WorkflowRoleTypeGroupRow parentWorkflowRoleTypeGroupRowByFK_WorkflowRoleType_WorkflowRoleTypeGroup)
                {
                    WorkflowRoleTypeRow rowWorkflowRoleTypeRow = ((WorkflowRoleTypeRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        WorkflowRoleTypeKey,
                        Description,
                        null};
                    if ((parentWorkflowRoleTypeGroupRowByFK_WorkflowRoleType_WorkflowRoleTypeGroup != null))
                    {
                        columnValuesArray[2] = parentWorkflowRoleTypeGroupRowByFK_WorkflowRoleType_WorkflowRoleTypeGroup[0];
                    }
                    rowWorkflowRoleTypeRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowWorkflowRoleTypeRow);
                    return rowWorkflowRoleTypeRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeRow FindByWorkflowRoleTypeKey(int WorkflowRoleTypeKey)
                {
                    return ((WorkflowRoleTypeRow)(this.Rows.Find(new object[] {
                            WorkflowRoleTypeKey})));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    WorkflowRoleTypeDataTable cln = ((WorkflowRoleTypeDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new WorkflowRoleTypeDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnWorkflowRoleTypeKey = base.Columns["WorkflowRoleTypeKey"];
                    this.columnDescription = base.Columns["Description"];
                    this.columnWorkflowRoleTypeGroupKey = base.Columns["WorkflowRoleTypeGroupKey"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnWorkflowRoleTypeKey = new global::System.Data.DataColumn("WorkflowRoleTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnWorkflowRoleTypeKey);
                    this.columnDescription = new global::System.Data.DataColumn("Description", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnDescription);
                    this.columnWorkflowRoleTypeGroupKey = new global::System.Data.DataColumn("WorkflowRoleTypeGroupKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnWorkflowRoleTypeGroupKey);
                    this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnWorkflowRoleTypeKey}, true));
                    this.columnWorkflowRoleTypeKey.AllowDBNull = false;
                    this.columnWorkflowRoleTypeKey.Unique = true;
                    this.columnDescription.AllowDBNull = false;
                    this.columnDescription.MaxLength = 50;
                    this.columnWorkflowRoleTypeGroupKey.AllowDBNull = false;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeRow NewWorkflowRoleTypeRow()
                {
                    return ((WorkflowRoleTypeRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new WorkflowRoleTypeRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(WorkflowRoleTypeRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.WorkflowRoleTypeRowChanged != null))
                    {
                        this.WorkflowRoleTypeRowChanged(this, new WorkflowRoleTypeRowChangeEvent(((WorkflowRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.WorkflowRoleTypeRowChanging != null))
                    {
                        this.WorkflowRoleTypeRowChanging(this, new WorkflowRoleTypeRowChangeEvent(((WorkflowRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.WorkflowRoleTypeRowDeleted != null))
                    {
                        this.WorkflowRoleTypeRowDeleted(this, new WorkflowRoleTypeRowChangeEvent(((WorkflowRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.WorkflowRoleTypeRowDeleting != null))
                    {
                        this.WorkflowRoleTypeRowDeleting(this, new WorkflowRoleTypeRowChangeEvent(((WorkflowRoleTypeRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveWorkflowRoleTypeRow(WorkflowRoleTypeRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "WorkflowRoleTypeDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents the strongly named DataTable class.
            ///</summary>
            [global::System.Serializable()]
            [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
            public partial class WFRAssignmentDataTable : global::System.Data.TypedTableBase<WFRAssignmentRow>
            {
                private global::System.Data.DataColumn columnID;

                private global::System.Data.DataColumn columnInstanceID;

                private global::System.Data.DataColumn columnADUserKey;

                private global::System.Data.DataColumn columnBlaKey;

                private global::System.Data.DataColumn columnGeneralStatusKey;

                private global::System.Data.DataColumn columnADUserName;

                private global::System.Data.DataColumn columnWorkflowRoleTypeKey;

                private global::System.Data.DataColumn columnWorkflowRoleTypeDescription;

                private global::System.Data.DataColumn columnWorkflowRoleTypeGroupKey;

                private global::System.Data.DataColumn columnWorkflowRoleTypeGroupDescription;

                private global::System.Data.DataColumn columnDescription;

                private global::System.Data.DataColumn columnOrganisationStructureKey;

                private global::System.Data.DataColumn columnParentKey;

                private global::System.Data.DataColumn columnMessage;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFRAssignmentDataTable()
                {
                    this.TableName = "WFRAssignment";
                    this.BeginInit();
                    this.InitClass();
                    this.EndInit();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WFRAssignmentDataTable(global::System.Data.DataTable table)
                {
                    this.TableName = table.TableName;
                    if ((table.CaseSensitive != table.DataSet.CaseSensitive))
                    {
                        this.CaseSensitive = table.CaseSensitive;
                    }
                    if ((table.Locale.ToString() != table.DataSet.Locale.ToString()))
                    {
                        this.Locale = table.Locale;
                    }
                    if ((table.Namespace != table.DataSet.Namespace))
                    {
                        this.Namespace = table.Namespace;
                    }
                    this.Prefix = table.Prefix;
                    this.MinimumCapacity = table.MinimumCapacity;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected WFRAssignmentDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) :
                    base(info, context)
                {
                    this.InitVars();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn IDColumn
                {
                    get
                    {
                        return this.columnID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn InstanceIDColumn
                {
                    get
                    {
                        return this.columnInstanceID;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserKeyColumn
                {
                    get
                    {
                        return this.columnADUserKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn BlaKeyColumn
                {
                    get
                    {
                        return this.columnBlaKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn GeneralStatusKeyColumn
                {
                    get
                    {
                        return this.columnGeneralStatusKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ADUserNameColumn
                {
                    get
                    {
                        return this.columnADUserName;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn WorkflowRoleTypeKeyColumn
                {
                    get
                    {
                        return this.columnWorkflowRoleTypeKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn WorkflowRoleTypeDescriptionColumn
                {
                    get
                    {
                        return this.columnWorkflowRoleTypeDescription;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn WorkflowRoleTypeGroupKeyColumn
                {
                    get
                    {
                        return this.columnWorkflowRoleTypeGroupKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn WorkflowRoleTypeGroupDescriptionColumn
                {
                    get
                    {
                        return this.columnWorkflowRoleTypeGroupDescription;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn DescriptionColumn
                {
                    get
                    {
                        return this.columnDescription;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn OrganisationStructureKeyColumn
                {
                    get
                    {
                        return this.columnOrganisationStructureKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn ParentKeyColumn
                {
                    get
                    {
                        return this.columnParentKey;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataColumn MessageColumn
                {
                    get
                    {
                        return this.columnMessage;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                [global::System.ComponentModel.Browsable(false)]
                public int Count
                {
                    get
                    {
                        return this.Rows.Count;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFRAssignmentRow this[int index]
                {
                    get
                    {
                        return ((WFRAssignmentRow)(this.Rows[index]));
                    }
                }

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFRAssignmentRowChangeEventHandler WFRAssignmentRowChanging;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFRAssignmentRowChangeEventHandler WFRAssignmentRowChanged;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFRAssignmentRowChangeEventHandler WFRAssignmentRowDeleting;

                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public event WFRAssignmentRowChangeEventHandler WFRAssignmentRowDeleted;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void AddWFRAssignmentRow(WFRAssignmentRow row)
                {
                    this.Rows.Add(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFRAssignmentRow AddWFRAssignmentRow(int ID, long InstanceID, int ADUserKey, int BlaKey, int GeneralStatusKey, string ADUserName, int WorkflowRoleTypeKey, string WorkflowRoleTypeDescription, int WorkflowRoleTypeGroupKey, string WorkflowRoleTypeGroupDescription, string Description, int OrganisationStructureKey, int ParentKey, string Message)
                {
                    WFRAssignmentRow rowWFRAssignmentRow = ((WFRAssignmentRow)(this.NewRow()));
                    object[] columnValuesArray = new object[] {
                        ID,
                        InstanceID,
                        ADUserKey,
                        BlaKey,
                        GeneralStatusKey,
                        ADUserName,
                        WorkflowRoleTypeKey,
                        WorkflowRoleTypeDescription,
                        WorkflowRoleTypeGroupKey,
                        WorkflowRoleTypeGroupDescription,
                        Description,
                        OrganisationStructureKey,
                        ParentKey,
                        Message};
                    rowWFRAssignmentRow.ItemArray = columnValuesArray;
                    this.Rows.Add(rowWFRAssignmentRow);
                    return rowWFRAssignmentRow;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public override global::System.Data.DataTable Clone()
                {
                    WFRAssignmentDataTable cln = ((WFRAssignmentDataTable)(base.Clone()));
                    cln.InitVars();
                    return cln;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataTable CreateInstance()
                {
                    return new WFRAssignmentDataTable();
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal void InitVars()
                {
                    this.columnID = base.Columns["ID"];
                    this.columnInstanceID = base.Columns["InstanceID"];
                    this.columnADUserKey = base.Columns["ADUserKey"];
                    this.columnBlaKey = base.Columns["BlaKey"];
                    this.columnGeneralStatusKey = base.Columns["GeneralStatusKey"];
                    this.columnADUserName = base.Columns["ADUserName"];
                    this.columnWorkflowRoleTypeKey = base.Columns["WorkflowRoleTypeKey"];
                    this.columnWorkflowRoleTypeDescription = base.Columns["WorkflowRoleTypeDescription"];
                    this.columnWorkflowRoleTypeGroupKey = base.Columns["WorkflowRoleTypeGroupKey"];
                    this.columnWorkflowRoleTypeGroupDescription = base.Columns["WorkflowRoleTypeGroupDescription"];
                    this.columnDescription = base.Columns["Description"];
                    this.columnOrganisationStructureKey = base.Columns["OrganisationStructureKey"];
                    this.columnParentKey = base.Columns["ParentKey"];
                    this.columnMessage = base.Columns["Message"];
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                private void InitClass()
                {
                    this.columnID = new global::System.Data.DataColumn("ID", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnID);
                    this.columnInstanceID = new global::System.Data.DataColumn("InstanceID", typeof(long), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnInstanceID);
                    this.columnADUserKey = new global::System.Data.DataColumn("ADUserKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserKey);
                    this.columnBlaKey = new global::System.Data.DataColumn("BlaKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnBlaKey);
                    this.columnGeneralStatusKey = new global::System.Data.DataColumn("GeneralStatusKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnGeneralStatusKey);
                    this.columnADUserName = new global::System.Data.DataColumn("ADUserName", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnADUserName);
                    this.columnWorkflowRoleTypeKey = new global::System.Data.DataColumn("WorkflowRoleTypeKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnWorkflowRoleTypeKey);
                    this.columnWorkflowRoleTypeDescription = new global::System.Data.DataColumn("WorkflowRoleTypeDescription", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnWorkflowRoleTypeDescription);
                    this.columnWorkflowRoleTypeGroupKey = new global::System.Data.DataColumn("WorkflowRoleTypeGroupKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnWorkflowRoleTypeGroupKey);
                    this.columnWorkflowRoleTypeGroupDescription = new global::System.Data.DataColumn("WorkflowRoleTypeGroupDescription", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnWorkflowRoleTypeGroupDescription);
                    this.columnDescription = new global::System.Data.DataColumn("Description", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnDescription);
                    this.columnOrganisationStructureKey = new global::System.Data.DataColumn("OrganisationStructureKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnOrganisationStructureKey);
                    this.columnParentKey = new global::System.Data.DataColumn("ParentKey", typeof(int), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnParentKey);
                    this.columnMessage = new global::System.Data.DataColumn("Message", typeof(string), null, global::System.Data.MappingType.Element);
                    base.Columns.Add(this.columnMessage);
                    this.columnID.AllowDBNull = false;
                    this.columnInstanceID.AllowDBNull = false;
                    this.columnADUserKey.AllowDBNull = false;
                    this.columnBlaKey.AllowDBNull = false;
                    this.columnGeneralStatusKey.AllowDBNull = false;
                    this.columnADUserName.AllowDBNull = false;
                    this.columnADUserName.MaxLength = 100;
                    this.columnWorkflowRoleTypeDescription.MaxLength = 50;
                    this.columnWorkflowRoleTypeGroupDescription.MaxLength = 50;
                    this.columnDescription.MaxLength = 255;
                    this.columnMessage.MaxLength = 50;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFRAssignmentRow NewWFRAssignmentRow()
                {
                    return ((WFRAssignmentRow)(this.NewRow()));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder)
                {
                    return new WFRAssignmentRow(builder);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override global::System.Type GetRowType()
                {
                    return typeof(WFRAssignmentRow);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanged(e);
                    if ((this.WFRAssignmentRowChanged != null))
                    {
                        this.WFRAssignmentRowChanged(this, new WFRAssignmentRowChangeEvent(((WFRAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowChanging(e);
                    if ((this.WFRAssignmentRowChanging != null))
                    {
                        this.WFRAssignmentRowChanging(this, new WFRAssignmentRowChangeEvent(((WFRAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleted(e);
                    if ((this.WFRAssignmentRowDeleted != null))
                    {
                        this.WFRAssignmentRowDeleted(this, new WFRAssignmentRowChangeEvent(((WFRAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e)
                {
                    base.OnRowDeleting(e);
                    if ((this.WFRAssignmentRowDeleting != null))
                    {
                        this.WFRAssignmentRowDeleting(this, new WFRAssignmentRowChangeEvent(((WFRAssignmentRow)(e.Row)), e.Action));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void RemoveWFRAssignmentRow(WFRAssignmentRow row)
                {
                    this.Rows.Remove(row);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs)
                {
                    global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                    global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                    WorkflowAssignment ds = new WorkflowAssignment();
                    global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                    any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                    any1.MinOccurs = new decimal(0);
                    any1.MaxOccurs = decimal.MaxValue;
                    any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any1);
                    global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                    any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                    any2.MinOccurs = new decimal(1);
                    any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                    sequence.Items.Add(any2);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute1.Name = "namespace";
                    attribute1.FixedValue = ds.Namespace;
                    type.Attributes.Add(attribute1);
                    global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                    attribute2.Name = "tableTypeName";
                    attribute2.FixedValue = "WFRAssignmentDataTable";
                    type.Attributes.Add(attribute2);
                    type.Particle = sequence;
                    global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                    if (xs.Contains(dsSchema.TargetNamespace))
                    {
                        global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                        global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                        try
                        {
                            global::System.Xml.Schema.XmlSchema schema = null;
                            dsSchema.Write(s1);
                            for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); )
                            {
                                schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                                s2.SetLength(0);
                                schema.Write(s2);
                                if ((s1.Length == s2.Length))
                                {
                                    s1.Position = 0;
                                    s2.Position = 0;
                                    for (; ((s1.Position != s1.Length)
                                                && (s1.ReadByte() == s2.ReadByte())); )
                                    {
                                        ;
                                    }
                                    if ((s1.Position == s1.Length))
                                    {
                                        return type;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if ((s1 != null))
                            {
                                s1.Close();
                            }
                            if ((s2 != null))
                            {
                                s2.Close();
                            }
                        }
                    }
                    xs.Add(dsSchema);
                    return type;
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class WorkflowAssignmentRow : global::System.Data.DataRow
            {
                private WorkflowAssignmentDataTable tableWorkflowAssignment;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowAssignmentRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableWorkflowAssignment = ((WorkflowAssignmentDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string ID
                {
                    get
                    {
                        return ((string)(this[this.tableWorkflowAssignment.IDColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.IDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public long InstanceID
                {
                    get
                    {
                        try
                        {
                            return ((long)(this[this.tableWorkflowAssignment.InstanceIDColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'InstanceID\' in table \'WorkflowAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.InstanceIDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeOrganisationStructureMappingKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWorkflowAssignment.OfferRoleTypeOrganisationStructureMappingKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'OfferRoleTypeOrganisationStructureMappingKey\' in table \'Wor" +
                                    "kflowAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.OfferRoleTypeOrganisationStructureMappingKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWorkflowAssignment.GeneralStatusKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'GeneralStatusKey\' in table \'WorkflowAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.GeneralStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ADUserKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWorkflowAssignment.ADUserKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ADUserKey\' in table \'WorkflowAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.ADUserKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string State
                {
                    get
                    {
                        if (this.IsStateNull())
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return ((string)(this[this.tableWorkflowAssignment.StateColumn]));
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowAssignment.StateColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsInstanceIDNull()
                {
                    return this.IsNull(this.tableWorkflowAssignment.InstanceIDColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetInstanceIDNull()
                {
                    this[this.tableWorkflowAssignment.InstanceIDColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsOfferRoleTypeOrganisationStructureMappingKeyNull()
                {
                    return this.IsNull(this.tableWorkflowAssignment.OfferRoleTypeOrganisationStructureMappingKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetOfferRoleTypeOrganisationStructureMappingKeyNull()
                {
                    this[this.tableWorkflowAssignment.OfferRoleTypeOrganisationStructureMappingKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsGeneralStatusKeyNull()
                {
                    return this.IsNull(this.tableWorkflowAssignment.GeneralStatusKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetGeneralStatusKeyNull()
                {
                    this[this.tableWorkflowAssignment.GeneralStatusKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsADUserKeyNull()
                {
                    return this.IsNull(this.tableWorkflowAssignment.ADUserKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetADUserKeyNull()
                {
                    this[this.tableWorkflowAssignment.ADUserKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsStateNull()
                {
                    return this.IsNull(this.tableWorkflowAssignment.StateColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetStateNull()
                {
                    this[this.tableWorkflowAssignment.StateColumn] = global::System.Convert.DBNull;
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class ADUserRow : global::System.Data.DataRow
            {
                private ADUserDataTable tableADUser;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal ADUserRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableADUser = ((ADUserDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ADUserKey
                {
                    get
                    {
                        return ((int)(this[this.tableADUser.ADUserKeyColumn]));
                    }
                    set
                    {
                        this[this.tableADUser.ADUserKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string ADUserName
                {
                    get
                    {
                        return ((string)(this[this.tableADUser.ADUserNameColumn]));
                    }
                    set
                    {
                        this[this.tableADUser.ADUserNameColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        return ((int)(this[this.tableADUser.GeneralStatusKeyColumn]));
                    }
                    set
                    {
                        this[this.tableADUser.GeneralStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Password
                {
                    get
                    {
                        if (this.IsPasswordNull())
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return ((string)(this[this.tableADUser.PasswordColumn]));
                        }
                    }
                    set
                    {
                        this[this.tableADUser.PasswordColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string PasswordQuestion
                {
                    get
                    {
                        if (this.IsPasswordQuestionNull())
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return ((string)(this[this.tableADUser.PasswordQuestionColumn]));
                        }
                    }
                    set
                    {
                        this[this.tableADUser.PasswordQuestionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string PasswordAnswer
                {
                    get
                    {
                        if (this.IsPasswordAnswerNull())
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return ((string)(this[this.tableADUser.PasswordAnswerColumn]));
                        }
                    }
                    set
                    {
                        this[this.tableADUser.PasswordAnswerColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int LegalEntityKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableADUser.LegalEntityKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'LegalEntityKey\' in table \'ADUser\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableADUser.LegalEntityKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsPasswordNull()
                {
                    return this.IsNull(this.tableADUser.PasswordColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetPasswordNull()
                {
                    this[this.tableADUser.PasswordColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsPasswordQuestionNull()
                {
                    return this.IsNull(this.tableADUser.PasswordQuestionColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetPasswordQuestionNull()
                {
                    this[this.tableADUser.PasswordQuestionColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsPasswordAnswerNull()
                {
                    return this.IsNull(this.tableADUser.PasswordAnswerColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetPasswordAnswerNull()
                {
                    this[this.tableADUser.PasswordAnswerColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsLegalEntityKeyNull()
                {
                    return this.IsNull(this.tableADUser.LegalEntityKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetLegalEntityKeyNull()
                {
                    this[this.tableADUser.LegalEntityKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow[] GetUserOrganisationStructureRows()
                {
                    if ((this.Table.ChildRelations["FK_UserOrganisationStructure_ADUser"] == null))
                    {
                        return new UserOrganisationStructureRow[0];
                    }
                    else
                    {
                        return ((UserOrganisationStructureRow[])(base.GetChildRows(this.Table.ChildRelations["FK_UserOrganisationStructure_ADUser"])));
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class OfferRoleTypeRow : global::System.Data.DataRow
            {
                private OfferRoleTypeDataTable tableOfferRoleType;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OfferRoleTypeRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableOfferRoleType = ((OfferRoleTypeDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeKey
                {
                    get
                    {
                        return ((int)(this[this.tableOfferRoleType.OfferRoleTypeKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleType.OfferRoleTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Description
                {
                    get
                    {
                        return ((string)(this[this.tableOfferRoleType.DescriptionColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleType.DescriptionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeGroupKey
                {
                    get
                    {
                        return ((int)(this[this.tableOfferRoleType.OfferRoleTypeGroupKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleType.OfferRoleTypeGroupKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow[] GetOfferRoleTypeOrganisationStructureMappingRows()
                {
                    if ((this.Table.ChildRelations["FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType"] == null))
                    {
                        return new OfferRoleTypeOrganisationStructureMappingRow[0];
                    }
                    else
                    {
                        return ((OfferRoleTypeOrganisationStructureMappingRow[])(base.GetChildRows(this.Table.ChildRelations["FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType"])));
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class OfferRoleTypeOrganisationStructureMappingRow : global::System.Data.DataRow
            {
                private OfferRoleTypeOrganisationStructureMappingDataTable tableOfferRoleTypeOrganisationStructureMapping;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OfferRoleTypeOrganisationStructureMappingRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableOfferRoleTypeOrganisationStructureMapping = ((OfferRoleTypeOrganisationStructureMappingDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeOrganisationStructureMappingKey
                {
                    get
                    {
                        return ((int)(this[this.tableOfferRoleTypeOrganisationStructureMapping.OfferRoleTypeOrganisationStructureMappingKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleTypeOrganisationStructureMapping.OfferRoleTypeOrganisationStructureMappingKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeKey
                {
                    get
                    {
                        return ((int)(this[this.tableOfferRoleTypeOrganisationStructureMapping.OfferRoleTypeKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleTypeOrganisationStructureMapping.OfferRoleTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationStructureKey
                {
                    get
                    {
                        return ((int)(this[this.tableOfferRoleTypeOrganisationStructureMapping.OrganisationStructureKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOfferRoleTypeOrganisationStructureMapping.OrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow OrganisationStructureRow
                {
                    get
                    {
                        return ((OrganisationStructureRow)(this.GetParentRow(this.Table.ParentRelations["FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure"]);
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow OfferRoleTypeRow
                {
                    get
                    {
                        return ((OfferRoleTypeRow)(this.GetParentRow(this.Table.ParentRelations["FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["FK_OfferRoleTypeOrganisationStructureMapping_OfferRoleType"]);
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class OrganisationStructureRow : global::System.Data.DataRow
            {
                private OrganisationStructureDataTable tableOrganisationStructure;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal OrganisationStructureRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableOrganisationStructure = ((OrganisationStructureDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationStructureKey
                {
                    get
                    {
                        return ((int)(this[this.tableOrganisationStructure.OrganisationStructureKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOrganisationStructure.OrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ParentKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableOrganisationStructure.ParentKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ParentKey\' in table \'OrganisationStructure\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableOrganisationStructure.ParentKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Description
                {
                    get
                    {
                        return ((string)(this[this.tableOrganisationStructure.DescriptionColumn]));
                    }
                    set
                    {
                        this[this.tableOrganisationStructure.DescriptionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationTypeKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableOrganisationStructure.OrganisationTypeKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'OrganisationTypeKey\' in table \'OrganisationStructure\' is DB" +
                                    "Null.", e);
                        }
                    }
                    set
                    {
                        this[this.tableOrganisationStructure.OrganisationTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        return ((int)(this[this.tableOrganisationStructure.GeneralStatusKeyColumn]));
                    }
                    set
                    {
                        this[this.tableOrganisationStructure.GeneralStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsParentKeyNull()
                {
                    return this.IsNull(this.tableOrganisationStructure.ParentKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetParentKeyNull()
                {
                    this[this.tableOrganisationStructure.ParentKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsOrganisationTypeKeyNull()
                {
                    return this.IsNull(this.tableOrganisationStructure.OrganisationTypeKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetOrganisationTypeKeyNull()
                {
                    this[this.tableOrganisationStructure.OrganisationTypeKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow[] GetOfferRoleTypeOrganisationStructureMappingRows()
                {
                    if ((this.Table.ChildRelations["FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure"] == null))
                    {
                        return new OfferRoleTypeOrganisationStructureMappingRow[0];
                    }
                    else
                    {
                        return ((OfferRoleTypeOrganisationStructureMappingRow[])(base.GetChildRows(this.Table.ChildRelations["FK_OfferRoleTypeOrganisationStructureMapping_OrganisationStructure"])));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow[] GetUserOrganisationStructureRows()
                {
                    if ((this.Table.ChildRelations["FK_UserOrganisationStructure_OrganisationStructure"] == null))
                    {
                        return new UserOrganisationStructureRow[0];
                    }
                    else
                    {
                        return ((UserOrganisationStructureRow[])(base.GetChildRows(this.Table.ChildRelations["FK_UserOrganisationStructure_OrganisationStructure"])));
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeOrganisationStructureMappingRow[] GetWorkflowRoleTypeOrganisationStructureMappingRows()
                {
                    if ((this.Table.ChildRelations["OrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping"] == null))
                    {
                        return new WorkflowRoleTypeOrganisationStructureMappingRow[0];
                    }
                    else
                    {
                        return ((WorkflowRoleTypeOrganisationStructureMappingRow[])(base.GetChildRows(this.Table.ChildRelations["OrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping"])));
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class UserOrganisationStructureRow : global::System.Data.DataRow
            {
                private UserOrganisationStructureDataTable tableUserOrganisationStructure;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal UserOrganisationStructureRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableUserOrganisationStructure = ((UserOrganisationStructureDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int UserOrganisationStructureKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructure.UserOrganisationStructureKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.UserOrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ADUserKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructure.ADUserKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.ADUserKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationStructureKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructure.OrganisationStructureKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.OrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GenericKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableUserOrganisationStructure.GenericKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'GenericKey\' in table \'UserOrganisationStructure\' is DBNull." +
                                    "", e);
                        }
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.GenericKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GenericKeyTypeKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableUserOrganisationStructure.GenericKeyTypeKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'GenericKeyTypeKey\' in table \'UserOrganisationStructure\' is " +
                                    "DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.GenericKeyTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructure.GeneralStatusKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructure.GeneralStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow OrganisationStructureRow
                {
                    get
                    {
                        return ((OrganisationStructureRow)(this.GetParentRow(this.Table.ParentRelations["FK_UserOrganisationStructure_OrganisationStructure"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["FK_UserOrganisationStructure_OrganisationStructure"]);
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow ADUserRow
                {
                    get
                    {
                        return ((ADUserRow)(this.GetParentRow(this.Table.ParentRelations["FK_UserOrganisationStructure_ADUser"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["FK_UserOrganisationStructure_ADUser"]);
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsGenericKeyNull()
                {
                    return this.IsNull(this.tableUserOrganisationStructure.GenericKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetGenericKeyNull()
                {
                    this[this.tableUserOrganisationStructure.GenericKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsGenericKeyTypeKeyNull()
                {
                    return this.IsNull(this.tableUserOrganisationStructure.GenericKeyTypeKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetGenericKeyTypeKeyNull()
                {
                    this[this.tableUserOrganisationStructure.GenericKeyTypeKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRoundRobinStatusRow[] GetUserOrganisationStructureRoundRobinStatusRows()
                {
                    if ((this.Table.ChildRelations["UserOrganisationStructure_UserOrganisationStructureRoundRobinStatus"] == null))
                    {
                        return new UserOrganisationStructureRoundRobinStatusRow[0];
                    }
                    else
                    {
                        return ((UserOrganisationStructureRoundRobinStatusRow[])(base.GetChildRows(this.Table.ChildRelations["UserOrganisationStructure_UserOrganisationStructureRoundRobinStatus"])));
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class WFAssignmentRow : global::System.Data.DataRow
            {
                private WFAssignmentDataTable tableWFAssignment;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WFAssignmentRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableWFAssignment = ((WFAssignmentDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ID
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.IDColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ID\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.IDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int IID
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.IIDColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'IID\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.IIDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ADUserKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.ADUserKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ADUserKey\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.ADUserKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int BlaKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.BlaKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'BlaKey\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.BlaKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GSKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.GSKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'GSKey\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.GSKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string ADUserName
                {
                    get
                    {
                        try
                        {
                            return ((string)(this[this.tableWFAssignment.ADUserNameColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ADUserName\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.ADUserNameColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string ORT
                {
                    get
                    {
                        try
                        {
                            return ((string)(this[this.tableWFAssignment.ORTColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ORT\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.ORTColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OfferRoleTypeKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.OfferRoleTypeKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'OfferRoleTypeKey\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.OfferRoleTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Description
                {
                    get
                    {
                        try
                        {
                            return ((string)(this[this.tableWFAssignment.DescriptionColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'Description\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.DescriptionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationStructureKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.OrganisationStructureKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'OrganisationStructureKey\' in table \'WFAssignment\' is DBNull" +
                                    ".", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.OrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ParentKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFAssignment.ParentKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ParentKey\' in table \'WFAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFAssignment.ParentKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsIDNull()
                {
                    return this.IsNull(this.tableWFAssignment.IDColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetIDNull()
                {
                    this[this.tableWFAssignment.IDColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsIIDNull()
                {
                    return this.IsNull(this.tableWFAssignment.IIDColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetIIDNull()
                {
                    this[this.tableWFAssignment.IIDColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsADUserKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.ADUserKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetADUserKeyNull()
                {
                    this[this.tableWFAssignment.ADUserKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsBlaKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.BlaKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetBlaKeyNull()
                {
                    this[this.tableWFAssignment.BlaKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsGSKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.GSKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetGSKeyNull()
                {
                    this[this.tableWFAssignment.GSKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsADUserNameNull()
                {
                    return this.IsNull(this.tableWFAssignment.ADUserNameColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetADUserNameNull()
                {
                    this[this.tableWFAssignment.ADUserNameColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsORTNull()
                {
                    return this.IsNull(this.tableWFAssignment.ORTColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetORTNull()
                {
                    this[this.tableWFAssignment.ORTColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsOfferRoleTypeKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.OfferRoleTypeKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetOfferRoleTypeKeyNull()
                {
                    this[this.tableWFAssignment.OfferRoleTypeKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsDescriptionNull()
                {
                    return this.IsNull(this.tableWFAssignment.DescriptionColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetDescriptionNull()
                {
                    this[this.tableWFAssignment.DescriptionColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsOrganisationStructureKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.OrganisationStructureKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetOrganisationStructureKeyNull()
                {
                    this[this.tableWFAssignment.OrganisationStructureKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsParentKeyNull()
                {
                    return this.IsNull(this.tableWFAssignment.ParentKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetParentKeyNull()
                {
                    this[this.tableWFAssignment.ParentKeyColumn] = global::System.Convert.DBNull;
                }
            }

            ///// <summary>
            /////Represents strongly named DataRow class.
            /////</summary>
            //public partial class RoundRobinPointerRow : global::System.Data.DataRow
            //{
            //    private RoundRobinPointerDataTable tableRoundRobinPointer;

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    internal RoundRobinPointerRow(global::System.Data.DataRowBuilder rb) :
            //        base(rb)
            //    {
            //        this.tableRoundRobinPointer = ((RoundRobinPointerDataTable)(this.Table));
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public int RoundRobinPointerKey
            //    {
            //        get
            //        {
            //            return ((int)(this[this.tableRoundRobinPointer.RoundRobinPointerKeyColumn]));
            //        }
            //        set
            //        {
            //            this[this.tableRoundRobinPointer.RoundRobinPointerKeyColumn] = value;
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public int RoundRobinPointerIndexID
            //    {
            //        get
            //        {
            //            try
            //            {
            //                return ((int)(this[this.tableRoundRobinPointer.RoundRobinPointerIndexIDColumn]));
            //            }
            //            catch (global::System.InvalidCastException e)
            //            {
            //                throw new global::System.Data.StrongTypingException("The value for column \'RoundRobinPointerIndexID\' in table \'RoundRobinPointer\' is D" +
            //                        "BNull.", e);
            //            }
            //        }
            //        set
            //        {
            //            this[this.tableRoundRobinPointer.RoundRobinPointerIndexIDColumn] = value;
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public string Description
            //    {
            //        get
            //        {
            //            return ((string)(this[this.tableRoundRobinPointer.DescriptionColumn]));
            //        }
            //        set
            //        {
            //            this[this.tableRoundRobinPointer.DescriptionColumn] = value;
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public int GeneralStatusKey
            //    {
            //        get
            //        {
            //            return ((int)(this[this.tableRoundRobinPointer.GeneralStatusKeyColumn]));
            //        }
            //        set
            //        {
            //            this[this.tableRoundRobinPointer.GeneralStatusKeyColumn] = value;
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public bool IsRoundRobinPointerIndexIDNull()
            //    {
            //        return this.IsNull(this.tableRoundRobinPointer.RoundRobinPointerIndexIDColumn);
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public void SetRoundRobinPointerIndexIDNull()
            //    {
            //        this[this.tableRoundRobinPointer.RoundRobinPointerIndexIDColumn] = global::System.Convert.DBNull;
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public RoundRobinPointerDefinitionRow[] GetRoundRobinPointerDefinitionRows()
            //    {
            //        if ((this.Table.ChildRelations["FK_RoundRobinPointerDefinition_RoundRobinPointer"] == null))
            //        {
            //            return new RoundRobinPointerDefinitionRow[0];
            //        }
            //        else
            //        {
            //            return ((RoundRobinPointerDefinitionRow[])(base.GetChildRows(this.Table.ChildRelations["FK_RoundRobinPointerDefinition_RoundRobinPointer"])));
            //        }
            //    }
            //}

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class RoundRobinPointerDefinitionRow : global::System.Data.DataRow
            {
                private RoundRobinPointerDefinitionDataTable tableRoundRobinPointerDefinition;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal RoundRobinPointerDefinitionRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableRoundRobinPointerDefinition = ((RoundRobinPointerDefinitionDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int RoundRobinPointerDefinitionKey
                {
                    get
                    {
                        return ((int)(this[this.tableRoundRobinPointerDefinition.RoundRobinPointerDefinitionKeyColumn]));
                    }
                    set
                    {
                        this[this.tableRoundRobinPointerDefinition.RoundRobinPointerDefinitionKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int RoundRobinPointerKey
                {
                    get
                    {
                        return ((int)(this[this.tableRoundRobinPointerDefinition.RoundRobinPointerKeyColumn]));
                    }
                    set
                    {
                        this[this.tableRoundRobinPointerDefinition.RoundRobinPointerKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GenericKeyTypeKey
                {
                    get
                    {
                        return ((int)(this[this.tableRoundRobinPointerDefinition.GenericKeyTypeKeyColumn]));
                    }
                    set
                    {
                        this[this.tableRoundRobinPointerDefinition.GenericKeyTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GenericKey
                {
                    get
                    {
                        return ((int)(this[this.tableRoundRobinPointerDefinition.GenericKeyColumn]));
                    }
                    set
                    {
                        this[this.tableRoundRobinPointerDefinition.GenericKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string ApplicationName
                {
                    get
                    {
                        return ((string)(this[this.tableRoundRobinPointerDefinition.ApplicationNameColumn]));
                    }
                    set
                    {
                        this[this.tableRoundRobinPointerDefinition.ApplicationNameColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string StatementName
                {
                    get
                    {
                        return ((string)(this[this.tableRoundRobinPointerDefinition.StatementNameColumn]));
                    }
                    set
                    {
                        this[this.tableRoundRobinPointerDefinition.StatementNameColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        return ((int)(this[this.tableRoundRobinPointerDefinition.GeneralStatusKeyColumn]));
                    }
                    set
                    {
                        this[this.tableRoundRobinPointerDefinition.GeneralStatusKeyColumn] = value;
                    }
                }

                //[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                //[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                //public RoundRobinPointerRow RoundRobinPointerRow
                //{
                //    get
                //    {
                //        return ((RoundRobinPointerRow)(this.GetParentRow(this.Table.ParentRelations["FK_RoundRobinPointerDefinition_RoundRobinPointer"])));
                //    }
                //    set
                //    {
                //        this.SetParentRow(value, this.Table.ParentRelations["FK_RoundRobinPointerDefinition_RoundRobinPointer"]);
                //    }
                //}
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class UserOrganisationStructureRoundRobinStatusRow : global::System.Data.DataRow
            {
                private UserOrganisationStructureRoundRobinStatusDataTable tableUserOrganisationStructureRoundRobinStatus;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal UserOrganisationStructureRoundRobinStatusRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableUserOrganisationStructureRoundRobinStatus = ((UserOrganisationStructureRoundRobinStatusDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int UserOrganisationStructureRoundRobinStatusKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructureRoundRobinStatus.UserOrganisationStructureRoundRobinStatusKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructureRoundRobinStatus.UserOrganisationStructureRoundRobinStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int UserOrganisationStructureKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructureRoundRobinStatus.UserOrganisationStructureKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructureRoundRobinStatus.UserOrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructureRoundRobinStatus.GeneralStatusKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructureRoundRobinStatus.GeneralStatusKeyColumn] = value;
                    }
                }


                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int CapitecGeneralStatusKey
                {
                    get
                    {
                        return ((int)(this[this.tableUserOrganisationStructureRoundRobinStatus.CapitecGeneralStatusKeyColumn]));
                    }
                    set
                    {
                        this[this.tableUserOrganisationStructureRoundRobinStatus.CapitecGeneralStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow UserOrganisationStructureRow
                {
                    get
                    {
                        return ((UserOrganisationStructureRow)(this.GetParentRow(this.Table.ParentRelations["UserOrganisationStructure_UserOrganisationStructureRoundRobinStatus"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["UserOrganisationStructure_UserOrganisationStructureRoundRobinStatus"]);
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class WorkflowRoleAssignmentRow : global::System.Data.DataRow
            {
                private WorkflowRoleAssignmentDataTable tableWorkflowRoleAssignment;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowRoleAssignmentRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableWorkflowRoleAssignment = ((WorkflowRoleAssignmentDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string ID
                {
                    get
                    {
                        return ((string)(this[this.tableWorkflowRoleAssignment.IDColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowRoleAssignment.IDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public long InstanceID
                {
                    get
                    {
                        try
                        {
                            return ((long)(this[this.tableWorkflowRoleAssignment.InstanceIDColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'InstanceID\' in table \'WorkflowRoleAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowRoleAssignment.InstanceIDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int WorkflowRoleTypeOrganisationStructureMappingKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWorkflowRoleAssignment.WorkflowRoleTypeOrganisationStructureMappingKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'WorkflowRoleTypeOrganisationStructureMappingKey\' in table \'" +
                                    "WorkflowRoleAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowRoleAssignment.WorkflowRoleTypeOrganisationStructureMappingKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWorkflowRoleAssignment.GeneralStatusKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'GeneralStatusKey\' in table \'WorkflowRoleAssignment\' is DBNu" +
                                    "ll.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowRoleAssignment.GeneralStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ADUserKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWorkflowRoleAssignment.ADUserKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ADUserKey\' in table \'WorkflowRoleAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowRoleAssignment.ADUserKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string State
                {
                    get
                    {
                        if (this.IsStateNull())
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return ((string)(this[this.tableWorkflowRoleAssignment.StateColumn]));
                        }
                    }
                    set
                    {
                        this[this.tableWorkflowRoleAssignment.StateColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsInstanceIDNull()
                {
                    return this.IsNull(this.tableWorkflowRoleAssignment.InstanceIDColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetInstanceIDNull()
                {
                    this[this.tableWorkflowRoleAssignment.InstanceIDColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsWorkflowRoleTypeOrganisationStructureMappingKeyNull()
                {
                    return this.IsNull(this.tableWorkflowRoleAssignment.WorkflowRoleTypeOrganisationStructureMappingKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetWorkflowRoleTypeOrganisationStructureMappingKeyNull()
                {
                    this[this.tableWorkflowRoleAssignment.WorkflowRoleTypeOrganisationStructureMappingKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsGeneralStatusKeyNull()
                {
                    return this.IsNull(this.tableWorkflowRoleAssignment.GeneralStatusKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetGeneralStatusKeyNull()
                {
                    this[this.tableWorkflowRoleAssignment.GeneralStatusKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsADUserKeyNull()
                {
                    return this.IsNull(this.tableWorkflowRoleAssignment.ADUserKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetADUserKeyNull()
                {
                    this[this.tableWorkflowRoleAssignment.ADUserKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsStateNull()
                {
                    return this.IsNull(this.tableWorkflowRoleAssignment.StateColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetStateNull()
                {
                    this[this.tableWorkflowRoleAssignment.StateColumn] = global::System.Convert.DBNull;
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class WorkflowRoleTypeGroupRow : global::System.Data.DataRow
            {
                private WorkflowRoleTypeGroupDataTable tableWorkflowRoleTypeGroup;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowRoleTypeGroupRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableWorkflowRoleTypeGroup = ((WorkflowRoleTypeGroupDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int WorkflowRoleTypeGroupKey
                {
                    get
                    {
                        return ((int)(this[this.tableWorkflowRoleTypeGroup.WorkflowRoleTypeGroupKeyColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowRoleTypeGroup.WorkflowRoleTypeGroupKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Description
                {
                    get
                    {
                        return ((string)(this[this.tableWorkflowRoleTypeGroup.DescriptionColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowRoleTypeGroup.DescriptionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GenericKeyTypeKey
                {
                    get
                    {
                        return ((int)(this[this.tableWorkflowRoleTypeGroup.GenericKeyTypeKeyColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowRoleTypeGroup.GenericKeyTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int WorkflowOrganisationStructureKey
                {
                    get
                    {
                        return ((int)(this[this.tableWorkflowRoleTypeGroup.WorkflowOrganisationStructureKeyColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowRoleTypeGroup.WorkflowOrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeRow[] GetWorkflowRoleTypeRows()
                {
                    if ((this.Table.ChildRelations["FK_WorkflowRoleType_WorkflowRoleTypeGroup"] == null))
                    {
                        return new WorkflowRoleTypeRow[0];
                    }
                    else
                    {
                        return ((WorkflowRoleTypeRow[])(base.GetChildRows(this.Table.ChildRelations["FK_WorkflowRoleType_WorkflowRoleTypeGroup"])));
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class WorkflowRoleTypeOrganisationStructureMappingRow : global::System.Data.DataRow
            {
                private WorkflowRoleTypeOrganisationStructureMappingDataTable tableWorkflowRoleTypeOrganisationStructureMapping;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowRoleTypeOrganisationStructureMappingRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableWorkflowRoleTypeOrganisationStructureMapping = ((WorkflowRoleTypeOrganisationStructureMappingDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int WorkflowRoleTypeOrganisationStructureMappingKey
                {
                    get
                    {
                        return ((int)(this[this.tableWorkflowRoleTypeOrganisationStructureMapping.WorkflowRoleTypeOrganisationStructureMappingKeyColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowRoleTypeOrganisationStructureMapping.WorkflowRoleTypeOrganisationStructureMappingKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int WorkflowRoleTypeKey
                {
                    get
                    {
                        return ((int)(this[this.tableWorkflowRoleTypeOrganisationStructureMapping.WorkflowRoleTypeKeyColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowRoleTypeOrganisationStructureMapping.WorkflowRoleTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationStructureKey
                {
                    get
                    {
                        return ((int)(this[this.tableWorkflowRoleTypeOrganisationStructureMapping.OrganisationStructureKeyColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowRoleTypeOrganisationStructureMapping.OrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeRow WorkflowRoleTypeRow
                {
                    get
                    {
                        return ((WorkflowRoleTypeRow)(this.GetParentRow(this.Table.ParentRelations["FK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["FK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType"]);
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow OrganisationStructureRow
                {
                    get
                    {
                        return ((OrganisationStructureRow)(this.GetParentRow(this.Table.ParentRelations["OrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["OrganisationStructure_WorkflowRoleTypeOrganisationStructureMapping"]);
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class WorkflowRoleTypeRow : global::System.Data.DataRow
            {
                private WorkflowRoleTypeDataTable tableWorkflowRoleType;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WorkflowRoleTypeRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableWorkflowRoleType = ((WorkflowRoleTypeDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int WorkflowRoleTypeKey
                {
                    get
                    {
                        return ((int)(this[this.tableWorkflowRoleType.WorkflowRoleTypeKeyColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowRoleType.WorkflowRoleTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Description
                {
                    get
                    {
                        return ((string)(this[this.tableWorkflowRoleType.DescriptionColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowRoleType.DescriptionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int WorkflowRoleTypeGroupKey
                {
                    get
                    {
                        return ((int)(this[this.tableWorkflowRoleType.WorkflowRoleTypeGroupKeyColumn]));
                    }
                    set
                    {
                        this[this.tableWorkflowRoleType.WorkflowRoleTypeGroupKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeGroupRow WorkflowRoleTypeGroupRow
                {
                    get
                    {
                        return ((WorkflowRoleTypeGroupRow)(this.GetParentRow(this.Table.ParentRelations["FK_WorkflowRoleType_WorkflowRoleTypeGroup"])));
                    }
                    set
                    {
                        this.SetParentRow(value, this.Table.ParentRelations["FK_WorkflowRoleType_WorkflowRoleTypeGroup"]);
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeOrganisationStructureMappingRow[] GetWorkflowRoleTypeOrganisationStructureMappingRows()
                {
                    if ((this.Table.ChildRelations["FK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType"] == null))
                    {
                        return new WorkflowRoleTypeOrganisationStructureMappingRow[0];
                    }
                    else
                    {
                        return ((WorkflowRoleTypeOrganisationStructureMappingRow[])(base.GetChildRows(this.Table.ChildRelations["FK_WorkflowRoleTypeOrganisationStructureMapping_WorkflowRoleType"])));
                    }
                }
            }

            /// <summary>
            ///Represents strongly named DataRow class.
            ///</summary>
            public partial class WFRAssignmentRow : global::System.Data.DataRow
            {
                private WFRAssignmentDataTable tableWFRAssignment;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                internal WFRAssignmentRow(global::System.Data.DataRowBuilder rb) :
                    base(rb)
                {
                    this.tableWFRAssignment = ((WFRAssignmentDataTable)(this.Table));
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ID
                {
                    get
                    {
                        return ((int)(this[this.tableWFRAssignment.IDColumn]));
                    }
                    set
                    {
                        this[this.tableWFRAssignment.IDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public long InstanceID
                {
                    get
                    {
                        return ((long)(this[this.tableWFRAssignment.InstanceIDColumn]));
                    }
                    set
                    {
                        this[this.tableWFRAssignment.InstanceIDColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ADUserKey
                {
                    get
                    {
                        return ((int)(this[this.tableWFRAssignment.ADUserKeyColumn]));
                    }
                    set
                    {
                        this[this.tableWFRAssignment.ADUserKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int BlaKey
                {
                    get
                    {
                        return ((int)(this[this.tableWFRAssignment.BlaKeyColumn]));
                    }
                    set
                    {
                        this[this.tableWFRAssignment.BlaKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int GeneralStatusKey
                {
                    get
                    {
                        return ((int)(this[this.tableWFRAssignment.GeneralStatusKeyColumn]));
                    }
                    set
                    {
                        this[this.tableWFRAssignment.GeneralStatusKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string ADUserName
                {
                    get
                    {
                        return ((string)(this[this.tableWFRAssignment.ADUserNameColumn]));
                    }
                    set
                    {
                        this[this.tableWFRAssignment.ADUserNameColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int WorkflowRoleTypeKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFRAssignment.WorkflowRoleTypeKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'WorkflowRoleTypeKey\' in table \'WFRAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFRAssignment.WorkflowRoleTypeKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string WorkflowRoleTypeDescription
                {
                    get
                    {
                        try
                        {
                            return ((string)(this[this.tableWFRAssignment.WorkflowRoleTypeDescriptionColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'WorkflowRoleTypeDescription\' in table \'WFRAssignment\' is DB" +
                                    "Null.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFRAssignment.WorkflowRoleTypeDescriptionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int WorkflowRoleTypeGroupKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFRAssignment.WorkflowRoleTypeGroupKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'WorkflowRoleTypeGroupKey\' in table \'WFRAssignment\' is DBNul" +
                                    "l.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFRAssignment.WorkflowRoleTypeGroupKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string WorkflowRoleTypeGroupDescription
                {
                    get
                    {
                        try
                        {
                            return ((string)(this[this.tableWFRAssignment.WorkflowRoleTypeGroupDescriptionColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'WorkflowRoleTypeGroupDescription\' in table \'WFRAssignment\' " +
                                    "is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFRAssignment.WorkflowRoleTypeGroupDescriptionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Description
                {
                    get
                    {
                        try
                        {
                            return ((string)(this[this.tableWFRAssignment.DescriptionColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'Description\' in table \'WFRAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFRAssignment.DescriptionColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int OrganisationStructureKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFRAssignment.OrganisationStructureKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'OrganisationStructureKey\' in table \'WFRAssignment\' is DBNul" +
                                    "l.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFRAssignment.OrganisationStructureKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public int ParentKey
                {
                    get
                    {
                        try
                        {
                            return ((int)(this[this.tableWFRAssignment.ParentKeyColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'ParentKey\' in table \'WFRAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFRAssignment.ParentKeyColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public string Message
                {
                    get
                    {
                        try
                        {
                            return ((string)(this[this.tableWFRAssignment.MessageColumn]));
                        }
                        catch (global::System.InvalidCastException e)
                        {
                            throw new global::System.Data.StrongTypingException("The value for column \'Message\' in table \'WFRAssignment\' is DBNull.", e);
                        }
                    }
                    set
                    {
                        this[this.tableWFRAssignment.MessageColumn] = value;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsWorkflowRoleTypeKeyNull()
                {
                    return this.IsNull(this.tableWFRAssignment.WorkflowRoleTypeKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetWorkflowRoleTypeKeyNull()
                {
                    this[this.tableWFRAssignment.WorkflowRoleTypeKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsWorkflowRoleTypeDescriptionNull()
                {
                    return this.IsNull(this.tableWFRAssignment.WorkflowRoleTypeDescriptionColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetWorkflowRoleTypeDescriptionNull()
                {
                    this[this.tableWFRAssignment.WorkflowRoleTypeDescriptionColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsWorkflowRoleTypeGroupKeyNull()
                {
                    return this.IsNull(this.tableWFRAssignment.WorkflowRoleTypeGroupKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetWorkflowRoleTypeGroupKeyNull()
                {
                    this[this.tableWFRAssignment.WorkflowRoleTypeGroupKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsWorkflowRoleTypeGroupDescriptionNull()
                {
                    return this.IsNull(this.tableWFRAssignment.WorkflowRoleTypeGroupDescriptionColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetWorkflowRoleTypeGroupDescriptionNull()
                {
                    this[this.tableWFRAssignment.WorkflowRoleTypeGroupDescriptionColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsDescriptionNull()
                {
                    return this.IsNull(this.tableWFRAssignment.DescriptionColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetDescriptionNull()
                {
                    this[this.tableWFRAssignment.DescriptionColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsOrganisationStructureKeyNull()
                {
                    return this.IsNull(this.tableWFRAssignment.OrganisationStructureKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetOrganisationStructureKeyNull()
                {
                    this[this.tableWFRAssignment.OrganisationStructureKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsParentKeyNull()
                {
                    return this.IsNull(this.tableWFRAssignment.ParentKeyColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetParentKeyNull()
                {
                    this[this.tableWFRAssignment.ParentKeyColumn] = global::System.Convert.DBNull;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public bool IsMessageNull()
                {
                    return this.IsNull(this.tableWFRAssignment.MessageColumn);
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public void SetMessageNull()
                {
                    this[this.tableWFRAssignment.MessageColumn] = global::System.Convert.DBNull;
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class WorkflowAssignmentRowChangeEvent : global::System.EventArgs
            {
                private WorkflowAssignmentRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRowChangeEvent(WorkflowAssignmentRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowAssignmentRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class ADUserRowChangeEvent : global::System.EventArgs
            {
                private ADUserRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRowChangeEvent(ADUserRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public ADUserRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class OfferRoleTypeRowChangeEvent : global::System.EventArgs
            {
                private OfferRoleTypeRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRowChangeEvent(OfferRoleTypeRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class OfferRoleTypeOrganisationStructureMappingRowChangeEvent : global::System.EventArgs
            {
                private OfferRoleTypeOrganisationStructureMappingRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRowChangeEvent(OfferRoleTypeOrganisationStructureMappingRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OfferRoleTypeOrganisationStructureMappingRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class OrganisationStructureRowChangeEvent : global::System.EventArgs
            {
                private OrganisationStructureRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRowChangeEvent(OrganisationStructureRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public OrganisationStructureRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class UserOrganisationStructureRowChangeEvent : global::System.EventArgs
            {
                private UserOrganisationStructureRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRowChangeEvent(UserOrganisationStructureRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class WFAssignmentRowChangeEvent : global::System.EventArgs
            {
                private WFAssignmentRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentRowChangeEvent(WFAssignmentRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFAssignmentRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            //[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //public class RoundRobinPointerRowChangeEvent : global::System.EventArgs
            //{
            //    private RoundRobinPointerRow eventRow;

            //    private global::System.Data.DataRowAction eventAction;

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public RoundRobinPointerRowChangeEvent(RoundRobinPointerRow row, global::System.Data.DataRowAction action)
            //    {
            //        this.eventRow = row;
            //        this.eventAction = action;
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public RoundRobinPointerRow Row
            //    {
            //        get
            //        {
            //            return this.eventRow;
            //        }
            //    }

            //    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            //    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            //    public global::System.Data.DataRowAction Action
            //    {
            //        get
            //        {
            //            return this.eventAction;
            //        }
            //    }
            //}

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class RoundRobinPointerDefinitionRowChangeEvent : global::System.EventArgs
            {
                private RoundRobinPointerDefinitionRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public RoundRobinPointerDefinitionRowChangeEvent(RoundRobinPointerDefinitionRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public RoundRobinPointerDefinitionRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class UserOrganisationStructureRoundRobinStatusRowChangeEvent : global::System.EventArgs
            {
                private UserOrganisationStructureRoundRobinStatusRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRoundRobinStatusRowChangeEvent(UserOrganisationStructureRoundRobinStatusRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public UserOrganisationStructureRoundRobinStatusRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class WorkflowRoleAssignmentRowChangeEvent : global::System.EventArgs
            {
                private WorkflowRoleAssignmentRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleAssignmentRowChangeEvent(WorkflowRoleAssignmentRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleAssignmentRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class WorkflowRoleTypeGroupRowChangeEvent : global::System.EventArgs
            {
                private WorkflowRoleTypeGroupRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeGroupRowChangeEvent(WorkflowRoleTypeGroupRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeGroupRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class WorkflowRoleTypeOrganisationStructureMappingRowChangeEvent : global::System.EventArgs
            {
                private WorkflowRoleTypeOrganisationStructureMappingRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeOrganisationStructureMappingRowChangeEvent(WorkflowRoleTypeOrganisationStructureMappingRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeOrganisationStructureMappingRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class WorkflowRoleTypeRowChangeEvent : global::System.EventArgs
            {
                private WorkflowRoleTypeRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeRowChangeEvent(WorkflowRoleTypeRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WorkflowRoleTypeRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }

            /// <summary>
            ///Row event argument class
            ///</summary>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public class WFRAssignmentRowChangeEvent : global::System.EventArgs
            {
                private WFRAssignmentRow eventRow;

                private global::System.Data.DataRowAction eventAction;

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFRAssignmentRowChangeEvent(WFRAssignmentRow row, global::System.Data.DataRowAction action)
                {
                    this.eventRow = row;
                    this.eventAction = action;
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public WFRAssignmentRow Row
                {
                    get
                    {
                        return this.eventRow;
                    }
                }

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
                public global::System.Data.DataRowAction Action
                {
                    get
                    {
                        return this.eventAction;
                    }
                }
            }
        }
    }
}