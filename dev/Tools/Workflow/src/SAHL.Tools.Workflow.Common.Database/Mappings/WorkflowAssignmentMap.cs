using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class WorkflowAssignmentMap : ClassMap<WorkflowAssignment>
    {
        public WorkflowAssignmentMap()
        {
            Table("WorkflowAssignment");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            Map(x => x.InstanceID).Column("InstanceID").Not.Nullable();
            Map(x => x.OfferRoleTypeOrganisationStructureMappingKey).Column("OfferRoleTypeOrganisationStructureMappingKey").Not.Nullable();
            Map(x => x.Aduserkey).Column("ADUserKey").Not.Nullable();
            Map(x => x.GeneralStatusKey).Column("GeneralStatusKey").Not.Nullable();
            Map(x => x.InsertDate).Column("InsertDate");
            Map(x => x.StateName).Column("StateName").Length(50);
        }
    }
}