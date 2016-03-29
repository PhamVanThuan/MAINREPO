using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration
{
    public class TaskHomeConfiguration : HaloModuleConfiguration,
                                            IHaloModuleApplicationConfiguration<HomeHaloApplicationConfiguration>
    {
        public TaskHomeConfiguration()
            : base("Task", 1)
        {
        }
    }
}