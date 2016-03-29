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
    /// SAHL.Common.BusinessModel.DAO.City_DAO
    /// </summary>
    public partial class City : BusinessModelBase<SAHL.Common.BusinessModel.DAO.City_DAO>, ICity
    {
        public City(SAHL.Common.BusinessModel.DAO.City_DAO City)
            : base(City)
        {
            this._DAO = City;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.PostOffices
        /// </summary>
        private DAOEventList<PostOffice_DAO, IPostOffice, PostOffice> _PostOffices;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.PostOffices
        /// </summary>
        public IEventList<IPostOffice> PostOffices
        {
            get
            {
                if (null == _PostOffices)
                {
                    if (null == _DAO.PostOffices)
                        _DAO.PostOffices = new List<PostOffice_DAO>();
                    _PostOffices = new DAOEventList<PostOffice_DAO, IPostOffice, PostOffice>(_DAO.PostOffices);
                    _PostOffices.BeforeAdd += new EventListHandler(OnPostOffices_BeforeAdd);
                    _PostOffices.BeforeRemove += new EventListHandler(OnPostOffices_BeforeRemove);
                    _PostOffices.AfterAdd += new EventListHandler(OnPostOffices_AfterAdd);
                    _PostOffices.AfterRemove += new EventListHandler(OnPostOffices_AfterRemove);
                }
                return _PostOffices;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.Suburbs
        /// </summary>
        private DAOEventList<Suburb_DAO, ISuburb, Suburb> _Suburbs;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.Suburbs
        /// </summary>
        public IEventList<ISuburb> Suburbs
        {
            get
            {
                if (null == _Suburbs)
                {
                    if (null == _DAO.Suburbs)
                        _DAO.Suburbs = new List<Suburb_DAO>();
                    _Suburbs = new DAOEventList<Suburb_DAO, ISuburb, Suburb>(_DAO.Suburbs);
                    _Suburbs.BeforeAdd += new EventListHandler(OnSuburbs_BeforeAdd);
                    _Suburbs.BeforeRemove += new EventListHandler(OnSuburbs_BeforeRemove);
                    _Suburbs.AfterAdd += new EventListHandler(OnSuburbs_AfterAdd);
                    _Suburbs.AfterRemove += new EventListHandler(OnSuburbs_AfterRemove);
                }
                return _Suburbs;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.City_DAO.Province
        /// </summary>
        public IProvince Province
        {
            get
            {
                if (null == _DAO.Province) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IProvince, Province_DAO>(_DAO.Province);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Province = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Province = (Province_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _PostOffices = null;
            _Suburbs = null;
        }
    }
}