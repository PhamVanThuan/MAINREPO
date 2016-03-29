using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// ApplicationInformationVariFixLoan_DAO is instantiated in order to retrieve those details specific to a VariFix Loan
    /// Application.
    /// </summary>
    public partial class ApplicationInformationVarifixLoan : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationVarifixLoan_DAO>, IApplicationInformationVarifixLoan
    {
        public ApplicationInformationVarifixLoan(SAHL.Common.BusinessModel.DAO.ApplicationInformationVarifixLoan_DAO ApplicationInformationVarifixLoan)
            : base(ApplicationInformationVarifixLoan)
        {
            this._DAO = ApplicationInformationVarifixLoan;
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
        /// The Percentage of the Loan that the client wishes to fix.
        /// </summary>
        public Double FixedPercent
        {
            get { return _DAO.FixedPercent; }
            set { _DAO.FixedPercent = value; }
        }

        /// <summary>
        /// The Instalment due on the Fixed Portion of the Loan.
        /// </summary>
        public Double FixedInstallment
        {
            get { return _DAO.FixedInstallment; }
            set { _DAO.FixedInstallment = value; }
        }

        /// <summary>
        /// The date which the client elected to take the VariFix product.
        /// </summary>
        public DateTime? ElectionDate
        {
            get { return _DAO.ElectionDate; }
            set { _DAO.ElectionDate = value; }
        }

        /// <summary>
        /// The market rate key.
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
        /// The Conversion Status.
        /// </summary>
        public Int32 ConversionStatus
        {
            get { return _DAO.ConversionStatus; }
            set { _DAO.ConversionStatus = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationVarifixLoan_DAO.ApplicationInformation
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