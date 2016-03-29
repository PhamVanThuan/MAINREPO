using System;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Box format.
    /// </summary>
    public partial class AddressBox : Address, IAddressBox
    {
        protected new SAHL.Common.BusinessModel.DAO.AddressBox_DAO _DAO;

        public AddressBox(SAHL.Common.BusinessModel.DAO.AddressBox_DAO AddressBox)
            : base(AddressBox)
        {
            this._DAO = AddressBox;
        }

        /// <summary>
        /// The Post Office Box Number of the Address
        /// </summary>
        public String BoxNumber
        {
            get { return _DAO.BoxNumber; }
            set { _DAO.BoxNumber = value; }
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