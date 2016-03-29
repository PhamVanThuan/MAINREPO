using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO
    /// </summary>
    public partial interface ISubsidy : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.SalaryNumber
        /// </summary>
        System.String SalaryNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.Paypoint
        /// </summary>
        System.String Paypoint
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.Notch
        /// </summary>
        System.String Notch
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.Rank
        /// </summary>
        System.String Rank
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.StopOrderAmount
        /// </summary>
        System.Double StopOrderAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.Employment
        /// </summary>
        IEmploymentSubsidised Employment
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.SubsidyProvider
        /// </summary>
        ISubsidyProvider SubsidyProvider
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.GEPFMember
        /// </summary>
        bool GEPFMember
        {
            get;
            set;
        }
    }
}