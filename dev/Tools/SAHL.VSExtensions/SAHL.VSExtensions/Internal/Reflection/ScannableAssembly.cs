using SAHL.VSExtensions.Interfaces.Reflection;

namespace SAHomeloans.SAHL_VSExtensions.Internal.Reflection
{
    public class ScannableAssembly : IScannableAssembly
    {
        public string AssemblyPath
        {
            get;
            protected set;
        }

        public ScannableAssembly(string assemblyPath)
        {
            this.AssemblyPath = assemblyPath;
        }
    }
}