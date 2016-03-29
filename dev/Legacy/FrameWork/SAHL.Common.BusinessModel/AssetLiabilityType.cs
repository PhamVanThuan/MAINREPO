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
    /// SAHL.Common.BusinessModel.DAO.AssetLiabilityType_DAO
    /// </summary>
    public partial class AssetLiabilityType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AssetLiabilityType_DAO>, IAssetLiabilityType
    {
        public AssetLiabilityType(SAHL.Common.BusinessModel.DAO.AssetLiabilityType_DAO AssetLiabilityType)
            : base(AssetLiabilityType)
        {
            this._DAO = AssetLiabilityType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityType_DAO.AssetLiabilitySubTypes
        /// </summary>
        private DAOEventList<AssetLiabilitySubType_DAO, IAssetLiabilitySubType, AssetLiabilitySubType> _AssetLiabilitySubTypes;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityType_DAO.AssetLiabilitySubTypes
        /// </summary>
        public IEventList<IAssetLiabilitySubType> AssetLiabilitySubTypes
        {
            get
            {
                if (null == _AssetLiabilitySubTypes)
                {
                    if (null == _DAO.AssetLiabilitySubTypes)
                        _DAO.AssetLiabilitySubTypes = new List<AssetLiabilitySubType_DAO>();
                    _AssetLiabilitySubTypes = new DAOEventList<AssetLiabilitySubType_DAO, IAssetLiabilitySubType, AssetLiabilitySubType>(_DAO.AssetLiabilitySubTypes);
                    _AssetLiabilitySubTypes.BeforeAdd += new EventListHandler(OnAssetLiabilitySubTypes_BeforeAdd);
                    _AssetLiabilitySubTypes.BeforeRemove += new EventListHandler(OnAssetLiabilitySubTypes_BeforeRemove);
                    _AssetLiabilitySubTypes.AfterAdd += new EventListHandler(OnAssetLiabilitySubTypes_AfterAdd);
                    _AssetLiabilitySubTypes.AfterRemove += new EventListHandler(OnAssetLiabilitySubTypes_AfterRemove);
                }
                return _AssetLiabilitySubTypes;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _AssetLiabilitySubTypes = null;
        }
    }
}