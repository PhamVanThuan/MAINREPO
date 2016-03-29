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
    /// CapType_DAO is used to hold the values of the different CAP types which can be offered to a client. The client
    /// can either be offered 1%,2% or 3% above their current rate.
    /// </summary>
    public partial class CapType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CapType_DAO>, ICapType
    {
        public CapType(SAHL.Common.BusinessModel.DAO.CapType_DAO CapType)
            : base(CapType)
        {
            this._DAO = CapType;
        }

        /// <summary>
        /// The Description of the CAP Type. e.g. 2% Above Current Rate
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// The percentage value above the clients rate which applies to the CAP Type. The example above would have a value of 0.02
        /// </summary>
        public Double Value
        {
            get { return _DAO.Value; }
            set { _DAO.Value = value; }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// A CAP Type can have many configuration details set up against it in the CapTypeConfigurationDetail table.
        /// </summary>
        private DAOEventList<CapTypeConfigurationDetail_DAO, ICapTypeConfigurationDetail, CapTypeConfigurationDetail> _CapTypeConfigurationDetails;

        /// <summary>
        /// A CAP Type can have many configuration details set up against it in the CapTypeConfigurationDetail table.
        /// </summary>
        public IEventList<ICapTypeConfigurationDetail> CapTypeConfigurationDetails
        {
            get
            {
                if (null == _CapTypeConfigurationDetails)
                {
                    if (null == _DAO.CapTypeConfigurationDetails)
                        _DAO.CapTypeConfigurationDetails = new List<CapTypeConfigurationDetail_DAO>();
                    _CapTypeConfigurationDetails = new DAOEventList<CapTypeConfigurationDetail_DAO, ICapTypeConfigurationDetail, CapTypeConfigurationDetail>(_DAO.CapTypeConfigurationDetails);
                    _CapTypeConfigurationDetails.BeforeAdd += new EventListHandler(OnCapTypeConfigurationDetails_BeforeAdd);
                    _CapTypeConfigurationDetails.BeforeRemove += new EventListHandler(OnCapTypeConfigurationDetails_BeforeRemove);
                    _CapTypeConfigurationDetails.AfterAdd += new EventListHandler(OnCapTypeConfigurationDetails_AfterAdd);
                    _CapTypeConfigurationDetails.AfterRemove += new EventListHandler(OnCapTypeConfigurationDetails_AfterRemove);
                }
                return _CapTypeConfigurationDetails;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _CapTypeConfigurationDetails = null;
        }
    }
}