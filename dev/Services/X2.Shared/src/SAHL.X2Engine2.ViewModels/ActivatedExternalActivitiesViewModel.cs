namespace SAHL.X2Engine2.ViewModels
{
    public class ActivatedExternalActivitiesViewModel
    {
        public long? InstanceId { get; set; }

        public long? ParentInstanceId { get; set; }

        public string ActivityName { get; set; }

        public int ExternalActivityTarget { get; set; }

        public ActivatedExternalActivitiesViewModel(long? instanceId, long? parentInstanceId, string activityName, int externalActivityTarget)
        {
            this.InstanceId = instanceId;
            this.ParentInstanceId = parentInstanceId;
            this.ActivityName = activityName;
            this.ExternalActivityTarget = externalActivityTarget;
        }
    }
}