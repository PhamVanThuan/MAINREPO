using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Regent_DAO
    /// </summary>
    public partial interface IRegent : IEntityValidation, IBusinessModelObject, IAccount
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSalutation
        /// </summary>
        System.String RegentClientSalutation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSurname
        /// </summary>
        System.String RegentClientSurname
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientFirstNames
        /// </summary>
        System.String RegentClientFirstNames
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientIDNumber
        /// </summary>
        System.Decimal RegentClientIDNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientGender
        /// </summary>
        System.Int16 RegentClientGender
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientDateBirth
        /// </summary>
        DateTime? RegentClientDateBirth
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentStatus
        /// </summary>
        IRegentStatus RegentStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentApplicationDate
        /// </summary>
        DateTime? RegentApplicationDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentInceptionDate
        /// </summary>
        DateTime? RegentInceptionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentExpiryDate
        /// </summary>
        DateTime? RegentExpiryDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentLoanTerm
        /// </summary>
        System.Decimal RegentLoanTerm
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentSumInsured
        /// </summary>
        System.Double RegentSumInsured
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentPremium
        /// </summary>
        System.Double RegentPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecSalutation
        /// </summary>
        System.String RegentClientSecSalutation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecSurname
        /// </summary>
        System.String RegentClientSecSurname
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecFirstNames
        /// </summary>
        System.String RegentClientSecFirstNames
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecIDNumber
        /// </summary>
        System.Decimal RegentClientSecIDNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecGender
        /// </summary>
        System.Int16 RegentClientSecGender
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientSecDateBirth
        /// </summary>
        DateTime? RegentClientSecDateBirth
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentJointIndicator
        /// </summary>
        System.Int16 RegentJointIndicator
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentReinstateDate
        /// </summary>
        DateTime? RegentReinstateDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentLastUpdateDate
        /// </summary>
        DateTime? RegentLastUpdateDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentCommision
        /// </summary>
        System.Double RegentCommision
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentUnderwritingFirst
        /// </summary>
        System.Int32 RegentUnderwritingFirst
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentUnderwritingSecond
        /// </summary>
        System.Int32 RegentUnderwritingSecond
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.SAHLEmployeeNumber
        /// </summary>
        System.Decimal SAHLEmployeeNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentUpdatedStatus
        /// </summary>
        System.Int16 RegentUpdatedStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientOccupation
        /// </summary>
        System.Int16 RegentClientOccupation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentClientAge
        /// </summary>
        System.Int16 RegentClientAge
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.ReplacementPolicy
        /// </summary>
        System.String ReplacementPolicy
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.AdviceRequired
        /// </summary>
        System.String AdviceRequired
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.LifeAssuredName
        /// </summary>
        System.String LifeAssuredName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.OldInsurer
        /// </summary>
        System.String OldInsurer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.OldPolicyNo
        /// </summary>
        System.String OldPolicyNo
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.RegentNewBusinessDate
        /// </summary>
        DateTime? RegentNewBusinessDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.CapitalizedMonthlyBalance
        /// </summary>
        System.Double CapitalizedMonthlyBalance
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Regent_DAO.Key
        /// </summary>
        System.Decimal Key
        {
            get;
            set;
        }
    }
}