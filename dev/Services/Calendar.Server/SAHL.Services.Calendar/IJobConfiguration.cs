using Quartz;
using SAHL.Core;

namespace SAHL.Services.Calendar
{
    public interface IJobConfiguration : IJob
    {
        ITrigger GetJobTrigger(IIocContainer iocContainer);
    }
}
