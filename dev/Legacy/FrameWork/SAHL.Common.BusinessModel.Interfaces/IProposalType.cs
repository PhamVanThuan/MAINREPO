using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ProposalType_DAO
    /// </summary>
    public partial interface IProposalType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

    }
}