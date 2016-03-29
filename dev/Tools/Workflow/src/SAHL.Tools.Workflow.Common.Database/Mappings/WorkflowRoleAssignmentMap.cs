using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class WorkflowRoleAssignmentMap : ClassMap<WorkflowRoleAssignment>
    {
        public WorkflowRoleAssignmentMap()
        {
            Table("WorkflowRoleAssignment");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            Map(x => x.InstanceID).Column("InstanceID").Not.Nullable();
            Map(x => x.WorkflowRoleTypeOrganisationStructureMappingKey).Column("WorkflowRoleTypeOrganisationStructureMappingKey").Not.Nullable();
            Map(x => x.Aduserkey).Column("ADUserKey").Not.Nullable();
            Map(x => x.GeneralStatusKey).Column("GeneralStatusKey").Not.Nullable();
            Map(x => x.InsertDate).Column("InsertDate");
            Map(x => x.Message).Column("Message").Length(50);
        }
    }
}