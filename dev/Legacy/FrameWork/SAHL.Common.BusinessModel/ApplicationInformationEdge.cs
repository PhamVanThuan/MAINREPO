using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationInformationEdge_DAO
    /// </summary>
    public partial class ApplicationInformationEdge : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationEdge_DAO>, IApplicationInformationEdge
    {
        public ApplicationInformationEdge(SAHL.Common.BusinessModel.DAO.ApplicationInformationEdge_DAO ApplicationInformationEdge)
            : base(ApplicationInformationEdge)
        {
            this._DAO = ApplicationInformationEdge;
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationEdge_DAO.FullTermInstalment
        /// </summary>
        public Double FullTermInstalment
        {
            get { return _DAO.FullTermInstalment; }
            set { _DAO.FullTermInstalment = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationEdge_DAO.AmortisationTermInstalment
        /// </summary>
        public Double AmortisationTermInstalment
        {
            get { return _DAO.AmortisationTermInstalment; }
            set { _DAO.AmortisationTermInstalment = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationEdge_DAO.InterestOnlyInstalment
        /// </summary>
        public Double InterestOnlyInstalment
        {
            get { return _DAO.InterestOnlyInstalment; }
            set { _DAO.InterestOnlyInstalment = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationEdge_DAO.InterestOnlyTerm
        /// </summary>
        public Int32 InterestOnlyTerm
        {
            get { return _DAO.InterestOnlyTerm; }
            set { _DAO.InterestOnlyTerm = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationEdge_DAO.ApplicationInformation
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