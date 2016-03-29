using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO
    /// </summary>
    public partial interface ICreditMatrix : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.NewBusinessIndicator
        /// </summary>
        System.Char NewBusinessIndicator
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.ImplementationDate
        /// </summary>
        DateTime? ImplementationDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.CreditCriterias
        /// </summary>
        IEventList<ICreditCriteria> CreditCriterias
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.OriginationSourceProducts
        /// </summary>
        IEventList<IOriginationSourceProduct> OriginationSourceProducts
        {
            get;
        }
    }
}