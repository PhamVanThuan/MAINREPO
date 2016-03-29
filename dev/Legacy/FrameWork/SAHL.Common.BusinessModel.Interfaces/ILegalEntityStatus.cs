using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// LegalEntityStatus_DAO is used to store the different statuses that a Legal Entity can be in.
    /// </summary>
    public partial interface ILegalEntityStatus : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The description of the Legal Entity Status. e.g. Alive/Deceased/Disabled
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}