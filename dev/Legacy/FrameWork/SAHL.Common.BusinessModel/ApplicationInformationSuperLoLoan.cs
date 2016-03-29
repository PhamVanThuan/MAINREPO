using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// ApplicationInformationSuperLoLoan_DAO is instantiated in order to retrieve those details specific to a Super Lo
    /// Application.
    /// </summary>
    public partial class ApplicationInformationSuperLoLoan : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationSuperLoLoan_DAO>, IApplicationInformationSuperLoLoan
    {
        public ApplicationInformationSuperLoLoan(SAHL.Common.BusinessModel.DAO.ApplicationInformationSuperLoLoan_DAO ApplicationInformationSuperLoLoan)
            : base(ApplicationInformationSuperLoLoan)
        {
            this._DAO = ApplicationInformationSuperLoLoan;
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
        /// The date on which the client elected to take the Super Lo Product option.
        /// </summary>
        public DateTime? ElectionDate
        {
            get { return _DAO.ElectionDate; }
            set { _DAO.ElectionDate = value; }
        }

        /// <summary>
        /// The Prepayment Threshold for the 1st year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        public Double PPThresholdYr1
        {
            get { return _DAO.PPThresholdYr1; }
            set { _DAO.PPThresholdYr1 = value; }
        }

        /// <summary>
        /// The Prepayment Threshold for the 2nd year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        public Double PPThresholdYr2
        {
            get { return _DAO.PPThresholdYr2; }
            set { _DAO.PPThresholdYr2 = value; }
        }

        /// <summary>
        /// The Prepayment Threshold for the 3rd year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        public Double PPThresholdYr3
        {
            get { return _DAO.PPThresholdYr3; }
            set { _DAO.PPThresholdYr3 = value; }
        }

        /// <summary>
        /// The Prepayment Threshold for the 4th year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        public Double PPThresholdYr4
        {
            get { return _DAO.PPThresholdYr4; }
            set { _DAO.PPThresholdYr4 = value; }
        }

        /// <summary>
        /// The Prepayment Threshold for the 5th year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        public Double PPThresholdYr5
        {
            get { return _DAO.PPThresholdYr5; }
            set { _DAO.PPThresholdYr5 = value; }
        }

        /// <summary>
        /// The Status.
        /// </summary>
        public Int32 Status
        {
            get { return _DAO.Status; }
            set { _DAO.Status = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationSuperLoLoan_DAO.ApplicationInformation
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