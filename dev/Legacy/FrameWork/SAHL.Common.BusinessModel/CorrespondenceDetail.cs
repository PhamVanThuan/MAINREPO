using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CorrespondenceDetail_DAO
    /// </summary>
    public partial class CorrespondenceDetail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CorrespondenceDetail_DAO>, ICorrespondenceDetail
    {
        public CorrespondenceDetail(SAHL.Common.BusinessModel.DAO.CorrespondenceDetail_DAO CorrespondenceDetail)
            : base(CorrespondenceDetail)
        {
            this._DAO = CorrespondenceDetail;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceDetail_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceDetail_DAO.Correspondence
        /// </summary>
        public ICorrespondence Correspondence
        {
            get
            {
                if (null == _DAO.Correspondence) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICorrespondence, Correspondence_DAO>(_DAO.Correspondence);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Correspondence = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Correspondence = (Correspondence_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceDetail_DAO.CorrespondenceText
        /// </summary>
        public String CorrespondenceText
        {
            get { return _DAO.CorrespondenceText; }
            set { _DAO.CorrespondenceText = value; }
        }
    }
}