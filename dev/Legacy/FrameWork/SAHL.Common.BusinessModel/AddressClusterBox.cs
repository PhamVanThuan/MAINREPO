using System;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Cluster Box format.
    /// </summary>
    public partial class AddressClusterBox : Address, IAddressClusterBox
    {
        protected new SAHL.Common.BusinessModel.DAO.AddressClusterBox_DAO _DAO;

        public AddressClusterBox(SAHL.Common.BusinessModel.DAO.AddressClusterBox_DAO AddressClusterBox)
            : base(AddressClusterBox)
        {
            this._DAO = AddressClusterBox;
        }

        /// <summary>
        /// The Cluster Box number of the Address
        /// </summary>
        public String ClusterBoxNumber
        {
            get { return _DAO.ClusterBoxNumber; }
            set { _DAO.ClusterBoxNumber = value; }
        }

        /// <summary>
        /// The Post Office which the Address belongs to.
        /// </summary>
        public IPostOffice PostOffice
        {
            get
            {
                if (null == _DAO.PostOffice) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IPostOffice, PostOffice_DAO>(_DAO.PostOffice);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.PostOffice = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.PostOffice = (PostOffice_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}