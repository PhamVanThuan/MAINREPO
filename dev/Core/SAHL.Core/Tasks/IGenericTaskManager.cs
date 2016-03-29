using System;
using System.Threading.Tasks;

namespace SAHL.Core.Tasks
{
    interface IGenericTaskManager<T>
    {
        Task<bool> QueueTask(Action<T> action, T obj);

        Task StartTask(Action<T> action, T obj);
    }
}