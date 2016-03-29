using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO
    /// </summary>
    public partial interface ICreditMatrixUnsecuredLending : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO.NewBusinessIndicator
        /// </summary>
        System.Char NewBusinessIndicator
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO.ImplementationDate
        /// </summary>
        DateTime? ImplementationDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO.CreditCriteriaUnsecuredLendings
        /// </summary>
        IEventList<ICreditCriteriaUnsecuredLending> CreditCriteriaUnsecuredLendings
        {
            get;
        }
    }
}