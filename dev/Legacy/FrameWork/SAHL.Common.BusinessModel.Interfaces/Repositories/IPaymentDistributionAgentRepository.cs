using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.Globals;
using System.Collections.ObjectModel;
using System.Data;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IPaymentDistributionAgentRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eaos"></param>
        void SavePaymentDistributionAgentOrganisationStructure(IPaymentDistributionAgentOrganisationNode eaos);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IPaymentDistributionAgentOrganisationNode GetPaymentDistributionAgentOrganisationNodeForKey(int Key);

        /// <summary>
        /// Get the IPaymentDistributionAgentOrganisationNode for the LegalEntityKey provided
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IPaymentDistributionAgentOrganisationNode GetPaymentDistributionAgentOrganisationNodeForLegalEntity(int key);
    }
}
