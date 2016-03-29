using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    public class ApplicationProductHOC : ApplicationProduct
    {
        public ApplicationProductHOC(IApplication Application, bool CreateNew)
            : base(Application, CreateNew)
        {
        }

        public ApplicationProductHOC(IApplicationInformation ApplicationInformation)
            : base(ApplicationInformation)
        {
        }
    }
}