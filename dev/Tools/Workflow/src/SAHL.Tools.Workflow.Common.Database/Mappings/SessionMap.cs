using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class SessionMap : ClassMap<Session>
    {
        public SessionMap()
        {
            Table("Session");
            Schema("X2");
            LazyLoad();
            Id(x => x.SessionID).GeneratedBy.Assigned().Column("SessionID");
            Map(x => x.Adusername).Column("ADUserName").Not.Nullable().Length(255);
            Map(x => x.SessionStartTime).Column("SessionStartTime").Not.Nullable();
            Map(x => x.LastActivityTime).Column("LastActivityTime").Not.Nullable();
        }
    }
}