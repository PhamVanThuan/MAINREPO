using System;
using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface ICapApplication : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        DateTime? CapitalisationDate
        {
            get;

            //This value is only set by the backend when the cap offer is capitalised
            //Done by the pBatchRaiseCapPremium stored procedure
            //set;
        }
    }
}