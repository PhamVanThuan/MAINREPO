using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DisabilityClaimStatus_DAO
    /// </summary>
    public partial interface IDisabilityClaimStatus : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaimStatus_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaimStatus_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}
