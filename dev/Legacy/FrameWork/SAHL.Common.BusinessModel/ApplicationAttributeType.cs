using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO
    /// </summary>
    public partial class ApplicationAttributeType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO>, IApplicationAttributeType
    {
        public ApplicationAttributeType(SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO ApplicationAttributeType)
            : base(ApplicationAttributeType)
        {
            this._DAO = ApplicationAttributeType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.IsGeneric
        /// </summary>
        public Boolean IsGeneric
        {
            get { return _DAO.IsGeneric; }
            set { _DAO.IsGeneric = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.UserEditable
        /// </summary>
        public Boolean UserEditable
        {
            get { return _DAO.UserEditable; }
            set { _DAO.UserEditable = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.OriginationSourceProducts
        /// </summary>
        private DAOEventList<OriginationSourceProduct_DAO, IOriginationSourceProduct, OriginationSourceProduct> _OriginationSourceProducts;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.OriginationSourceProducts
        /// </summary>
        public IEventList<IOriginationSourceProduct> OriginationSourceProducts
        {
            get
            {
                if (null == _OriginationSourceProducts)
                {
                    if (null == _DAO.OriginationSourceProducts)
                        _DAO.OriginationSourceProducts = new List<OriginationSourceProduct_DAO>();
                    _OriginationSourceProducts = new DAOEventList<OriginationSourceProduct_DAO, IOriginationSourceProduct, OriginationSourceProduct>(_DAO.OriginationSourceProducts);
                    _OriginationSourceProducts.BeforeAdd += new EventListHandler(OnOriginationSourceProducts_BeforeAdd);
                    _OriginationSourceProducts.BeforeRemove += new EventListHandler(OnOriginationSourceProducts_BeforeRemove);
                    _OriginationSourceProducts.AfterAdd += new EventListHandler(OnOriginationSourceProducts_AfterAdd);
                    _OriginationSourceProducts.AfterRemove += new EventListHandler(OnOriginationSourceProducts_AfterRemove);
                }
                return _OriginationSourceProducts;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.ApplicationAttributeTypeGroup
        /// </summary>
        public IApplicationAttributeTypeGroup ApplicationAttributeTypeGroup
        {
            get
            {
                if (null == _DAO.ApplicationAttributeTypeGroup) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationAttributeTypeGroup, ApplicationAttributeTypeGroup_DAO>(_DAO.ApplicationAttributeTypeGroup);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationAttributeTypeGroup = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationAttributeTypeGroup = (ApplicationAttributeTypeGroup_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _OriginationSourceProducts = null;
        }
    }
}