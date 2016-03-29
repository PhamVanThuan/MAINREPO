using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Bond_DAO
    /// </summary>
    public partial class Bond : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Bond_DAO>, IBond
    {
        public Bond(SAHL.Common.BusinessModel.DAO.Bond_DAO Bond)
            : base(Bond)
        {
            this._DAO = Bond;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.BondRegistrationNumber
        /// </summary>
        public String BondRegistrationNumber
        {
            get { return _DAO.BondRegistrationNumber; }
            set { _DAO.BondRegistrationNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.BondRegistrationAmount
        /// </summary>
        public Double BondRegistrationAmount
        {
            get { return _DAO.BondRegistrationAmount; }
            set { _DAO.BondRegistrationAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.BondLoanAgreementAmount
        /// </summary>
        public Double BondLoanAgreementAmount
        {
            get { return _DAO.BondLoanAgreementAmount; }
            set { _DAO.BondLoanAgreementAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.UserID
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.ChangeDate
        /// </summary>
        public DateTime ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.LoanAgreements
        /// </summary>
        private DAOEventList<LoanAgreement_DAO, ILoanAgreement, LoanAgreement> _LoanAgreements;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.LoanAgreements
        /// </summary>
        public IEventList<ILoanAgreement> LoanAgreements
        {
            get
            {
                if (null == _LoanAgreements)
                {
                    if (null == _DAO.LoanAgreements)
                        _DAO.LoanAgreements = new List<LoanAgreement_DAO>();
                    _LoanAgreements = new DAOEventList<LoanAgreement_DAO, ILoanAgreement, LoanAgreement>(_DAO.LoanAgreements);
                    _LoanAgreements.BeforeAdd += new EventListHandler(OnLoanAgreements_BeforeAdd);
                    _LoanAgreements.BeforeRemove += new EventListHandler(OnLoanAgreements_BeforeRemove);
                    _LoanAgreements.AfterAdd += new EventListHandler(OnLoanAgreements_AfterAdd);
                    _LoanAgreements.AfterRemove += new EventListHandler(OnLoanAgreements_AfterRemove);
                }
                return _LoanAgreements;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.Attorney
        /// </summary>
        public IAttorney Attorney
        {
            get
            {
                if (null == _DAO.Attorney) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAttorney, Attorney_DAO>(_DAO.Attorney);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Attorney = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Attorney = (Attorney_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.DeedsOffice
        /// </summary>
        public IDeedsOffice DeedsOffice
        {
            get
            {
                if (null == _DAO.DeedsOffice) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDeedsOffice, DeedsOffice_DAO>(_DAO.DeedsOffice);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.DeedsOffice = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.DeedsOffice = (DeedsOffice_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.Application
        /// </summary>
        public IApplication Application
        {
            get
            {
                if (null == _DAO.Application) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Application);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Application = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Application = (Application_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.MortgageLoans
        /// </summary>
        private DAOEventList<MortgageLoan_DAO, IMortgageLoan, MortgageLoan> _MortgageLoans;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.MortgageLoans
        /// </summary>
        public IEventList<IMortgageLoan> MortgageLoans
        {
            get
            {
                if (null == _MortgageLoans)
                {
                    if (null == _DAO.MortgageLoans)
                        _DAO.MortgageLoans = new List<MortgageLoan_DAO>();
                    _MortgageLoans = new DAOEventList<MortgageLoan_DAO, IMortgageLoan, MortgageLoan>(_DAO.MortgageLoans);
                    _MortgageLoans.BeforeAdd += new EventListHandler(OnMortgageLoans_BeforeAdd);
                    _MortgageLoans.BeforeRemove += new EventListHandler(OnMortgageLoans_BeforeRemove);
                    _MortgageLoans.AfterAdd += new EventListHandler(OnMortgageLoans_AfterAdd);
                    _MortgageLoans.AfterRemove += new EventListHandler(OnMortgageLoans_AfterRemove);
                }
                return _MortgageLoans;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _LoanAgreements = null;
            _MortgageLoans = null;
        }
    }
}