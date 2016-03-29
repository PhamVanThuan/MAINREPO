using Mono.Cecil;
using System.IO;

namespace SAHL.Tools.DomainServiceDocumenter.Lib
{
    public class ScopedAssemblyResolver : BaseAssemblyResolver
    {
        private readonly IAssemblyResolver defaultResolver;

        public ScopedAssemblyResolver(string location)
        {
            if (!string.IsNullOrEmpty(location) && Directory.Exists(location))
                AddSearchDirectory(location);

            defaultResolver = new DefaultAssemblyResolver();
        }

        public override AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            try
            {
                return base.Resolve(name);
            }
            catch (AssemblyResolutionException)
            {
                return defaultResolver.Resolve(name);
            }
        }

        public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            if (parameters == null)
                parameters = new ReaderParameters();
            // Force the resolver.
            parameters.AssemblyResolver = this;
            try
            {
                return base.Resolve(name, parameters);
            }
            catch (AssemblyResolutionException)
            {
                return defaultResolver.Resolve(name, parameters);
            }
        }

        public override AssemblyDefinition Resolve(string fullName)
        {
            try
            {
                return base.Resolve(fullName);
            }
            catch (AssemblyResolutionException)
            {
                return defaultResolver.Resolve(fullName);
            }
        }

        public override AssemblyDefinition Resolve(string fullName, ReaderParameters parameters)
        {
            if (parameters == null)
                parameters = new ReaderParameters();
            // Force the resolver.
            parameters.AssemblyResolver = this;
            try
            {
                return base.Resolve(fullName, parameters);
            }
            catch (AssemblyResolutionException)
            {
                return defaultResolver.Resolve(fullName, parameters);
            }
        }
    }
}