using System.Reflection;
namespace SAHL.Core.Testing.FileConventions
{
    public interface IAssemblyConvention
    {
        bool Process(Assembly assembly);
    }
}
