using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Identity;
using System;

namespace SAHL.Services.Capitec.Managers.Geo
{
    public class GeoDataManager : IGeoDataManager
    {
        private IDbFactory dbFactory;
        public GeoDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        #region country methods
        public bool DoesCountryIdExist(Guid id)
        {
            bool result = false;
            DoesCountryIdExistQuery query = new DoesCountryIdExistQuery(id);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var province = db.SelectOne<CountryDataModel>(query);
                if (province != null)
                {
                    result = true;
                }
            }
            return result;
        }
        #endregion

        #region province methods

        public void AddProvince(string provinceName, int sahlProvinceKey, Guid countryId)
        {
            ProvinceDataModel newProvince = new ProvinceDataModel(CombGuid.Instance.Generate(), sahlProvinceKey, provinceName, countryId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<ProvinceDataModel>(newProvince);
                db.Complete();
            }
        }

        public bool DoesSAHLProvinceKeyExist(int sahlProvinceKey)
        {
            bool result = false;
            DoesSAHLProvinceKeyExistQuery query = new DoesSAHLProvinceKeyExistQuery(sahlProvinceKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var province = db.SelectOne<ProvinceDataModel>(query);
                if (province != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool DoesProvinceNameExist(string provinceName)
        {
            bool result = false;
            DoesProvinceNameExistsQuery query = new DoesProvinceNameExistsQuery(provinceName);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var province = db.SelectOne<ProvinceDataModel>(query);
                if (province != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool DoesProvinceIdExist(Guid id)
        {
            bool result = false;
            DoesProvinceIdExistQuery query = new DoesProvinceIdExistQuery(id);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var province = db.SelectOne<ProvinceDataModel>(query);
                if (province != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool HasSAHLProvinceKeyChanged(Guid id, int sahlProvinceKey)
        {
            bool result = true;
            HasSAHLProvinceKeyChangedQuery query = new HasSAHLProvinceKeyChangedQuery(id, sahlProvinceKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var province = db.SelectOne<ProvinceDataModel>(query);
                if (province != null)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool HasProvinceNameChanged(Guid id, string provinceName)
        {
            bool result = true;
            HasProvinceNameChangedQuery query = new HasProvinceNameChangedQuery(id, provinceName);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var province = db.SelectOne<ProvinceDataModel>(query);
                if (province != null)
                {
                    result = false;
                }
            }
            return result;
        }

        public void ChangeProvinceDetails(Guid id, string provinceName, int sahlProvinceKey,Guid countryId)
        {
            ChangeProvinceDetailsQuery query = new ChangeProvinceDetailsQuery(id, provinceName, sahlProvinceKey, countryId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<ProvinceDataModel>(query);
                db.Complete();
            }
        }

        #endregion province methods

        #region city methods

        public bool DoesCityIdExist(Guid id)
        {
            bool result = false;
            DoesCityIdExistQuery query = new DoesCityIdExistQuery(id);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var city = db.SelectOne<CityDataModel>(query);
                if (city != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool HasSAHLCityKeyChanged(Guid id, int sahlCityKey)
        {
            bool result = true;
            HasSAHLCityKeyChangedQuery query = new HasSAHLCityKeyChangedQuery(id, sahlCityKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var city = db.SelectOne<CityDataModel>(query);
                if (city != null)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool DoesSAHLCityKeyExist(int sahlCityKey)
        {
            bool result = false;
            DoesSAHLCityKeyExistQuery query = new DoesSAHLCityKeyExistQuery(sahlCityKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var city = db.SelectOne<CityDataModel>(query);
                if (city != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public void AddCity(string cityName, int sahlCityKey, Guid provinceId)
        {
            CityDataModel query = new CityDataModel(CombGuid.Instance.Generate(), sahlCityKey, cityName, provinceId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<CityDataModel>(query);
                db.Complete();
            }
        }

        public void ChangeCityDetails(Guid id, string cityName, int sahlCityKey, Guid provinceId)
        {
            ChangeCityDetailsQuery query = new ChangeCityDetailsQuery(id, cityName, sahlCityKey, provinceId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<CityDataModel>(query);
                db.Complete();
            }
        }

        #endregion city methods

        #region suburb methods

        public bool DoesSuburbIdExist(Guid id)
        {
            bool result = false;
            DoesSuburbIdExistQuery query = new DoesSuburbIdExistQuery(id);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var suburb = db.SelectOne<SuburbDataModel>(query);
                if (suburb != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool HasSAHLSuburbKeyChanged(Guid id, int sahlSuburbKey)
        {
            bool result = true;
            HasSAHLSuburbKeyChangedQuery query = new HasSAHLSuburbKeyChangedQuery(id, sahlSuburbKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var suburb = db.SelectOne<SuburbDataModel>(query);
                if (suburb != null)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool DoesSAHLSuburbKeyExist(int sahlSuburbKey)
        {
            bool result = false;
            DoesSAHLSuburbKeyExistQuery query = new DoesSAHLSuburbKeyExistQuery(sahlSuburbKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var suburb = db.SelectOne<SuburbDataModel>(query);
                if (suburb != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public void AddSuburb(string suburbName, int sahlSuburbKey, string postalCode, Guid cityId)
        {
            SuburbDataModel query = new SuburbDataModel(CombGuid.Instance.Generate(), sahlSuburbKey, suburbName, postalCode, cityId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<SuburbDataModel>(query);
                db.Complete();
            }
        }

        public void ChangeSuburbsDetails(Guid id, string suburbName, int sahlSuburbKey, string postalCode, Guid cityId)
        {
            ChangeSuburbsDetailsQuery query = new ChangeSuburbsDetailsQuery(id, suburbName, sahlSuburbKey, postalCode, cityId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<SuburbDataModel>(query);
                db.Complete();
            }
        }

        #endregion suburb methods
    }
}