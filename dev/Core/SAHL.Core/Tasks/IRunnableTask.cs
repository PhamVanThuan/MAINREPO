using System.Threading.Tasks;

namespace SAHL.Core.Tasks
{
    public interface IRunnableTask
    {
        Task Task { get; }

        void Start();
    }
}