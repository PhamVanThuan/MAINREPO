using System;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Private Bag format.
    /// </summary>
    public partial class AddressPrivateBag : Address, IAddressPrivateBag
    {
        protected new SAHL.Common.BusinessModel.DAO.AddressPrivateBag_DAO _DAO;

        public AddressPrivateBag(SAHL.Common.BusinessModel.DAO.AddressPrivateBag_DAO AddressPrivateBag)
            : base(AddressPrivateBag)
        {
            this._DAO = AddressPrivateBag;
        }

        /// <summary>
        /// The Private Bag Number of the Address
        /// </summary>
        public String PrivateBagNumber
        {
            get { return _DAO.PrivateBagNumber; }
            set { _DAO.PrivateBagNumber = value; }
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