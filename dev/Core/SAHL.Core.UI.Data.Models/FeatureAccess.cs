namespace SAHL.Core.UI.Data.Models
{
    public class FeatureAccess
    {
        public FeatureAccess(string featureName)
        {
            this.FeatureName = featureName;
        }

        public string FeatureName { get; protected set; }
    }
}