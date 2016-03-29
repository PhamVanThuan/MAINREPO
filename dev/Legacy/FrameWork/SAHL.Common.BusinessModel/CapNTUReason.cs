using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CapNTUReason_DAO
    /// </summary>
    public partial class CapNTUReason : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CapNTUReason_DAO>, ICapNTUReason
    {
        public CapNTUReason(SAHL.Common.BusinessModel.DAO.CapNTUReason_DAO CapNTUReason)
            : base(CapNTUReason)
        {
            this._DAO = CapNTUReason;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapNTUReason_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapNTUReason_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapNTUReason_DAO.CapApplicationDetails
        /// </summary>
        private DAOEventList<CapApplicationDetail_DAO, ICapApplicationDetail, CapApplicationDetail> _CapApplicationDetails;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapNTUReason_DAO.CapApplicationDetails
        /// </summary>
        public IEventList<ICapApplicationDetail> CapApplicationDetails
        {
            get
            {
                if (null == _CapApplicationDetails)
                {
                    if (null == _DAO.CapApplicationDetails)
                        _DAO.CapApplicationDetails = new List<CapApplicationDetail_DAO>();
                    _CapApplicationDetails = new DAOEventList<CapApplicationDetail_DAO, ICapApplicationDetail, CapApplicationDetail>(_DAO.CapApplicationDetails);
                    _CapApplicationDetails.BeforeAdd += new EventListHandler(OnCapApplicationDetails_BeforeAdd);
                    _CapApplicationDetails.BeforeRemove += new EventListHandler(OnCapApplicationDetails_BeforeRemove);
                    _CapApplicationDetails.AfterAdd += new EventListHandler(OnCapApplicationDetails_AfterAdd);
                    _CapApplicationDetails.AfterRemove += new EventListHandler(OnCapApplicationDetails_AfterRemove);
                }
                return _CapApplicationDetails;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _CapApplicationDetails = null;
        }
    }
}