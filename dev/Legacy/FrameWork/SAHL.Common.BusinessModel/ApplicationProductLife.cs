using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    public class ApplicationProductLife : ApplicationProduct
    {
        public ApplicationProductLife(IApplication Application, bool CreateNew)
            : base(Application, CreateNew)
        {
        }
    }
}