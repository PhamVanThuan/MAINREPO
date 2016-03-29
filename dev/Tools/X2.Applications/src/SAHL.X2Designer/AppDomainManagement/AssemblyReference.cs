using System.Reflection;

namespace SAHL.X2Designer.AppDomainManagement
{
    public class AssemblyReference
    {
        public AssemblyReference(string fullPath, Assembly reflectionLoadedAssembly)
        {
            this.FullPath = fullPath;
            this.ReflectionLoadedAssembly = reflectionLoadedAssembly;
        }

        public string FullName
        {
            get
            {
                return this.ReflectionLoadedAssembly.FullName;
            }
        }

        public string FullPath { get; protected set; }

        public bool IsInternalBin
        {
            get
            {
                if (!string.IsNullOrEmpty(this.FullPath))
                {
                    return this.FullPath.Contains("Internal Binaries");
                }
                return false;
            }
        }

        public bool IsExternalBin
        {
            get
            {
                if (!string.IsNullOrEmpty(this.FullPath))
                {
                    return this.FullPath.Contains("External Binaries");
                }
                return false;
            }
        }

        public bool IsDomainServiceBin
        {
            get
            {
                if (!string.IsNullOrEmpty(this.FullPath))
                {
                    return this.FullPath.Contains("DomainService");
                }
                return false;
            }
        }

        public Assembly ReflectionLoadedAssembly { get; protected set; }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                AssemblyReference asmRef = obj as AssemblyReference;
                if (asmRef != null)
                {
                    if (asmRef.FullName == this.FullName)
                    {
                        return true;
                    }
                }
            }

            return base.Equals(obj);
        }
    }
}