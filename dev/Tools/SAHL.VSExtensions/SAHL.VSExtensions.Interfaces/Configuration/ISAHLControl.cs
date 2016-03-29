using SAHL.VSExtensions.Interfaces.Reflection;
using SAHL.VSExtensions.Interfaces.Validation;

namespace SAHL.VSExtensions.Interfaces.Configuration
{
    public interface ISAHLControl
    {
        void PerformScan(IAssemblyFinder assemblyFinder, ITypeScanner typeScanner, ISAHLProjectItem projectItem);

        void Validate(IControlValidation controlValidation);

        void UpdateModel(dynamic model);
    }
}