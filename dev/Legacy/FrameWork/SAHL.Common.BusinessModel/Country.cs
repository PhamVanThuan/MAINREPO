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
    /// SAHL.Common.BusinessModel.DAO.Country_DAO
    /// </summary>
    public partial class Country : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Country_DAO>, ICountry
    {
        public Country(SAHL.Common.BusinessModel.DAO.Country_DAO Country)
            : base(Country)
        {
            this._DAO = Country;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Country_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Country_DAO.AllowFreeTextFormat
        /// </summary>
        public Boolean AllowFreeTextFormat
        {
            get { return _DAO.AllowFreeTextFormat; }
            set { _DAO.AllowFreeTextFormat = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Country_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// A collection of provinces that apply to the country.
        /// </summary>
        private DAOEventList<Province_DAO, IProvince, Province> _Provinces;

        /// <summary>
        /// A collection of provinces that apply to the country.
        /// </summary>
        public IEventList<IProvince> Provinces
        {
            get
            {
                if (null == _Provinces)
                {
                    if (null == _DAO.Provinces)
                        _DAO.Provinces = new List<Province_DAO>();
                    _Provinces = new DAOEventList<Province_DAO, IProvince, Province>(_DAO.Provinces);
                    _Provinces.BeforeAdd += new EventListHandler(OnProvinces_BeforeAdd);
                    _Provinces.BeforeRemove += new EventListHandler(OnProvinces_BeforeRemove);
                    _Provinces.AfterAdd += new EventListHandler(OnProvinces_AfterAdd);
                    _Provinces.AfterRemove += new EventListHandler(OnProvinces_AfterRemove);
                }
                return _Provinces;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _Provinces = null;
        }
    }
}