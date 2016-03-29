using Castle.ActiveRecord.Framework.Config;

namespace SAHL.Common.Utils
{
    public class InPlaceConfigurationSourceWithLazyDefault : InPlaceConfigurationSource
    {
        public InPlaceConfigurationSourceWithLazyDefault()
        {
            this.SetIsLazyByDefault(true);
        }
    }
}