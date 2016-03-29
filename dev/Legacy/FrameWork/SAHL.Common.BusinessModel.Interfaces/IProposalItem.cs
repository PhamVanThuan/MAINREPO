using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO
    /// </summary>
    public partial interface IProposalItem : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.StartDate
        /// </summary>
        System.DateTime StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.EndDate
        /// </summary>
        System.DateTime EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.InterestRate
        /// </summary>
        System.Double InterestRate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.Amount
        /// </summary>
        System.Double Amount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.AdditionalAmount
        /// </summary>
        System.Double AdditionalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.CreateDate
        /// </summary>
        System.DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.MarketRate
        /// </summary>
        IMarketRate MarketRate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.Proposal
        /// </summary>
        IProposal Proposal
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.InstalmentPercent
        /// </summary>
        Double? InstalmentPercent
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.AnnualEscalation
        /// </summary>
        Double? AnnualEscalation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.StartPeriod
        /// </summary>
        System.Int16 StartPeriod
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.EndPeriod
        /// </summary>
        System.Int16 EndPeriod
        {
            get;
            set;
        }
    }
}