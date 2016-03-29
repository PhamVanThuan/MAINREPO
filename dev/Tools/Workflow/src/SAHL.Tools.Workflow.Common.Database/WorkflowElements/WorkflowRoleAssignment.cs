namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class WorkflowRoleAssignment
    {
        public WorkflowRoleAssignment() { }

        public virtual int Id { get; set; }

        public virtual long InstanceID { get; set; }

        public virtual int WorkflowRoleTypeOrganisationStructureMappingKey { get; set; }

        public virtual int Aduserkey { get; set; }

        public virtual int GeneralStatusKey { get; set; }

        public virtual System.Nullable<System.DateTime> InsertDate { get; set; }

        public virtual string Message { get; set; }
    }
}