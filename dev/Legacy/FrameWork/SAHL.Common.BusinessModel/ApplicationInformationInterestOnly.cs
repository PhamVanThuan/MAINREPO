using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// ApplicationInformationInterestOnly_DAO is instantiated in order to retrieve those details specific to an Interest Only
    /// Application.
    /// </summary>
    public partial class ApplicationInformationInterestOnly : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationInterestOnly_DAO>, IApplicationInformationInterestOnly
    {
        public ApplicationInformationInterestOnly(SAHL.Common.BusinessModel.DAO.ApplicationInformationInterestOnly_DAO ApplicationInformationInterestOnly)
            : base(ApplicationInformationInterestOnly)
        {
            this._DAO = ApplicationInformationInterestOnly;
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
        /// The Interest Only Instalment which does not take into account the client repaying any capital as part of their
        /// monthly instalment.
        /// </summary>
        public Double? Installment
        {
            get { return _DAO.Installment; }
            set { _DAO.Installment = value; }
        }

        /// <summary>
        /// The Interest Only Maturity Date is the date at which the client is required to repay any outstanding capital.
        /// </summary>
        public DateTime? MaturityDate
        {
            get { return _DAO.MaturityDate; }
            set { _DAO.MaturityDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationInterestOnly_DAO.ApplicationInformation
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