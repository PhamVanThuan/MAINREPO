using System.Diagnostics;
using System.Threading.Tasks;

namespace SAHL.Core
{
    public static class CoreGlobals
    {
        public const string DomainProcessIdName = "DomainProcessId";

        public readonly static ParallelOptions DefaultParallelOptions;

        static CoreGlobals()
        {
            DefaultParallelOptions = new ParallelOptions();

//if compiling in debug mode
#if DEBUG
            if (Debugger.IsAttached)
            {
                //if compiling in debug mode, and debugger is attached, then use only 1 thread when executing parallel queries
                DefaultParallelOptions.MaxDegreeOfParallelism = 1;
            }
#endif
        }
    }
}