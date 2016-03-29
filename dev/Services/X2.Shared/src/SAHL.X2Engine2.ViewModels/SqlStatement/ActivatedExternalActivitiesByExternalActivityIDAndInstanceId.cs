using SAHL.Core.Data;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class ActivatedExternalActivitiesByExternalActivityIDAndInstanceId : ISqlStatement<ActivatedExternalActivitiesViewModel>
    {
        public int ActivatedByExternalActivityID { get; set; }

        public long? InstanceId { get; set; }

        public ActivatedExternalActivitiesByExternalActivityIDAndInstanceId(int activatedByExternalActivityId, long? instanceId)
        {
            this.ActivatedByExternalActivityID = activatedByExternalActivityId;
            this.InstanceId = instanceId;
        }

        public string GetStatement()
        {
            string sql = @"select I.ID as InstanceId, I.ParentInstanceID as ParentInstanceId, a.Name as ActivityName, a.ExternalActivityTarget
                            from x2.X2.Activity A (nolock)
                            inner join x2.X2.Instance I (rowlock)
                            on A.StateID = I.StateID
                            where
	                            A.ActivatedByExternalActivity = @ActivatedByExternalActivityID
                            and i.id=@InstanceId
                            and A.ExternalActivityTarget=1
                            union
                            -- Parent Instance
                            select I.ID as InstanceId, I.ParentInstanceID as ParentInstanceId, a.Name as ActivityName, a.ExternalActivityTarget
                            from x2.X2.Activity A (nolock)
                            inner join x2.X2.Instance I (rowlock)
                            on A.StateID = I.StateID
                            where
	                            A.ActivatedByExternalActivity = @ActivatedByExternalActivityID
                            and i.ParentInstanceID = @InstanceID
                            and A.ExternalActivityTarget=2
                            union
                            -- Any
                            select I.ID as InstanceId, I.ParentInstanceID as ParentInstanceId, a.Name as ActivityName, a.ExternalActivityTarget
                            from x2.X2.Activity A (nolock)
                            inner join x2.X2.Instance I (rowlock)
                            on A.StateID = I.StateID
                            where
	                            A.ActivatedByExternalActivity = @ActivatedByExternalActivityID
                            and A.ExternalActivityTarget=3
                            union
                            -- check for create activities (Any Creates)
                            select NULL as InstanceId, NULL as ParentInstanceId, a.Name as ActivityName, a.ExternalActivityTarget
                            from x2.X2.Activity A (nolock)
	                            where A.ActivatedByExternalActivity = @ActivatedByExternalActivityID
	                            and A.StateID is null

                            union
                            -- Child

                            select I.ID as InstanceId, I.ParentInstanceID as ParentInstanceId, a.Name as ActivityName, a.ExternalActivityTarget
                            from x2.X2.Activity A (nolock)
                            inner join x2.X2.Instance I (rowlock)
                            on A.StateID = I.StateID
                            inner join x2.X2.Instance I1 (Rowlock)
                            on i.id=i1.parentinstanceid and i1.id=@InstanceId
                            where
	                            A.ActivatedByExternalActivity = @ActivatedByExternalActivityID
                            and A.ExternalActivityTarget=4";
            return sql;
        }
    }
}