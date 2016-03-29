using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AssetLiabilitySubType_DAO
    /// </summary>
    public partial class AssetLiabilitySubType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AssetLiabilitySubType_DAO>, IAssetLiabilitySubType
    {
        public AssetLiabilitySubType(SAHL.Common.BusinessModel.DAO.AssetLiabilitySubType_DAO AssetLiabilitySubType)
            : base(AssetLiabilitySubType)
        {
            this._DAO = AssetLiabilitySubType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilitySubType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilitySubType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilitySubType_DAO.AssetLiabilityType
        /// </summary>
        public IAssetLiabilityType AssetLiabilityType
        {
            get
            {
                if (null == _DAO.AssetLiabilityType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAssetLiabilityType, AssetLiabilityType_DAO>(_DAO.AssetLiabilityType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.AssetLiabilityType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.AssetLiabilityType = (AssetLiabilityType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}