using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// This class is used to represent the composite primary key in the LifeInsurableInterest_DAO class.
    /// </summary>
    public partial interface ILifeInsurableInterest : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeInsurableInterest_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeInsurableInterest_DAO.LifeInsurableInterestType
        /// </summary>
        ILifeInsurableInterestType LifeInsurableInterestType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeInsurableInterest_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeInsurableInterest_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }
    }
}