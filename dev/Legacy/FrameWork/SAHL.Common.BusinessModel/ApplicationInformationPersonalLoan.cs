using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO
    /// </summary>
    public partial class ApplicationInformationPersonalLoan : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO>, IApplicationInformationPersonalLoan
    {
        public ApplicationInformationPersonalLoan(SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO ApplicationInformationPersonalLoan)
            : base(ApplicationInformationPersonalLoan)
        {
            this._DAO = ApplicationInformationPersonalLoan;
        }

        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.LoanAmount
        /// </summary>
        public Double LoanAmount
        {
            get { return _DAO.LoanAmount; }
            set { _DAO.LoanAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.Term
        /// </summary>
        public Int32 Term
        {
            get { return _DAO.Term; }
            set { _DAO.Term = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.MonthlyInstalment
        /// </summary>
        public Double MonthlyInstalment
        {
            get { return _DAO.MonthlyInstalment; }
            set { _DAO.MonthlyInstalment = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.LifePremium
        /// </summary>
        public Double LifePremium
        {
            get { return _DAO.LifePremium; }
            set { _DAO.LifePremium = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.FeesTotal
        /// </summary>
        public Double FeesTotal
        {
            get { return _DAO.FeesTotal; }
            set { _DAO.FeesTotal = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.CreditCriteriaUnsecuredLending
        /// </summary>
        public ICreditCriteriaUnsecuredLending CreditCriteriaUnsecuredLending
        {
            get
            {
                if (null == _DAO.CreditCriteriaUnsecuredLending) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICreditCriteriaUnsecuredLending, CreditCriteriaUnsecuredLending_DAO>(_DAO.CreditCriteriaUnsecuredLending);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CreditCriteriaUnsecuredLending = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CreditCriteriaUnsecuredLending = (CreditCriteriaUnsecuredLending_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.Margin
        /// </summary>
        public IMargin Margin
        {
            get
            {
                if (null == _DAO.Margin) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IMargin, Margin_DAO>(_DAO.Margin);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Margin = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Margin = (Margin_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.MarketRate
        /// </summary>
        public IMarketRate MarketRate
        {
            get
            {
                if (null == _DAO.MarketRate) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IMarketRate, MarketRate_DAO>(_DAO.MarketRate);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.MarketRate = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.MarketRate = (MarketRate_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.ApplicationInformation
        /// </summary>
        public IApplicationInformation ApplicationInformation
        {
            get
            {
                if (null == _DAO.ApplicationInformation) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformation, ApplicationInformation_DAO>(_DAO.ApplicationInformation);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationInformation = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationInformation = (ApplicationInformation_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}