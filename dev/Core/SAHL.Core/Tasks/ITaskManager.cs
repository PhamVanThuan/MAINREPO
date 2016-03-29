using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Core.Tasks
{
    public interface ITaskManager
    {
        Task StartTask(Action action);
    }
}