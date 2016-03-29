using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IAffordabilityAssessment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.GenericKey
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.AffordabilityAssessmentStatus
        /// </summary>
        IAffordabilityAssessmentStatus AffordabilityAssessmentStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.AffordabilityAssessmentStressFactor
        /// </summary>
        IAffordabilityAssessmentStressFactor AffordabilityAssessmentStressFactor
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.ModifiedDate
        /// </summary>
        System.DateTime ModifiedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.ModifiedByUser
        /// </summary>
        IADUser ModifiedByUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.NumberOfContributingApplicants
        /// </summary>
        int NumberOfContributingApplicants
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.NumberOfHouseholdDependants
        /// </summary>
        System.Int32 NumberOfHouseholdDependants
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.MinimumMonthlyFixedExpenses
        /// </summary>
        System.Int32? MinimumMonthlyFixedExpenses
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.ConfirmedDate
        /// </summary>
        System.DateTime? ConfirmedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.Notes
        /// </summary>
        string Notes
        {
            get;
            set;
        }
    }
}
