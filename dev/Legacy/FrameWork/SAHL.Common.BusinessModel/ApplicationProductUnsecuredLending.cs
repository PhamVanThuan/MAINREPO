using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.Common.BusinessModel
{
    public class ApplicationProductUnsecuredLending : ApplicationProduct, IApplicationProductUnsecuredLending
    {
        public ApplicationProductUnsecuredLending(IApplication Application, bool CreateNew)
            : base(Application, CreateNew)
        {
        }

		public ApplicationProductUnsecuredLending(IApplicationInformation applicationInformation) : base(applicationInformation)
		{
		}
    }
}