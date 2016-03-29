using System;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial class AssetLiabilityFixedProperty : AssetLiability, IAssetLiabilityFixedProperty
    {
        protected new SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedProperty_DAO _DAO;

        public AssetLiabilityFixedProperty(SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedProperty_DAO AssetLiabilityFixedProperty)
            : base(AssetLiabilityFixedProperty)
        {
            this._DAO = AssetLiabilityFixedProperty;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedProperty_DAO.DateAcquired
        /// </summary>
        public DateTime? DateAcquired
        {
            get { return _DAO.DateAcquired; }
            set { _DAO.DateAcquired = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedProperty_DAO.Address
        /// </summary>
        public IAddress Address
        {
            get
            {
                if (null == _DAO.Address) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAddress, Address_DAO>(_DAO.Address);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Address = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Address = (Address_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedProperty_DAO.AssetValue
        /// </summary>
        public Double AssetValue
        {
            get { return _DAO.AssetValue; }
            set { _DAO.AssetValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedProperty_DAO.LiabilityValue
        /// </summary>
        public Double LiabilityValue
        {
            get { return _DAO.LiabilityValue; }
            set { _DAO.LiabilityValue = value; }
        }
    }
}