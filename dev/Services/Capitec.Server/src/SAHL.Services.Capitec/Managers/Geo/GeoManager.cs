using System;

namespace SAHL.Services.Capitec.Managers.Geo
{
    public class GeoManager : IGeoManager
    {
        private IGeoDataManager geoDataServices;

        public GeoManager(IGeoDataManager geoDataServices)
        {
            this.geoDataServices = geoDataServices;
        }

        #region province methods

        public void AddProvince(string provinceName, int sahlProvinceKey, Guid countryId)
        {
            if (string.IsNullOrEmpty(provinceName))
                throw new ArgumentException("Province name cannot be empty");

            if (geoDataServices.DoesProvinceNameExist(provinceName))
                throw new Exception("Province name already exists");

            if (geoDataServices.DoesSAHLProvinceKeyExist(sahlProvinceKey))
                throw new Exception("External Province Key already in use.");

            if (!geoDataServices.DoesCountryIdExist(countryId))
                throw new Exception("Invalid Country Id");

            geoDataServices.AddProvince(provinceName, sahlProvinceKey, countryId);
        }

        public void ChangeProvinceDetails(Guid id, string provinceName, int sahlProvinceKey, Guid countryId)
        {
            if (string.IsNullOrEmpty(provinceName))
            {
                throw new ArgumentException("Province name cannot be empty");
            }

            if (!geoDataServices.DoesProvinceIdExist(id))
            {
                throw new Exception("Invalid Province Id");
            }

            if (!geoDataServices.DoesCountryIdExist(countryId))
                throw new Exception("Invalid Country Id");

            if (geoDataServices.HasProvinceNameChanged(id, provinceName))
            {
                if (geoDataServices.DoesProvinceNameExist(provinceName))
                    throw new Exception("Province name already exists");
            }

            if (geoDataServices.HasSAHLProvinceKeyChanged(id, sahlProvinceKey))
                if (geoDataServices.DoesSAHLProvinceKeyExist(sahlProvinceKey))
                    throw new Exception("External Province Key already in use.");

            geoDataServices.ChangeProvinceDetails(id, provinceName, sahlProvinceKey, countryId);
        }

        #endregion province methods

        #region city methods

        public void AddCity(string cityName, int sahlCityKey, Guid provinceId)
        {
            if (string.IsNullOrEmpty(cityName))
                throw new ArgumentException("City name cannot be empty");

            if (geoDataServices.DoesSAHLCityKeyExist(sahlCityKey))
                throw new Exception("External City Key already in use");

            if (!geoDataServices.DoesProvinceIdExist(provinceId))
                throw new Exception("Province Id invalid");

            geoDataServices.AddCity(cityName, sahlCityKey, provinceId);
        }

        public void ChangeCityDetails(Guid id, string cityName, int sahlCityKey, Guid provinceId)
        {
            if (string.IsNullOrEmpty(cityName))
                throw new ArgumentException("City name cannot be empty");

            if (!geoDataServices.DoesCityIdExist(id))
                throw new Exception("Invalid City Id");

            if (!geoDataServices.DoesProvinceIdExist(provinceId))
                throw new Exception("Province Id invalid");

            if (geoDataServices.HasSAHLCityKeyChanged(id, sahlCityKey))
            {
                if (geoDataServices.DoesSAHLCityKeyExist(sahlCityKey))
                    throw new Exception("External City Key already in use.");
            }

            geoDataServices.ChangeCityDetails(id, cityName, sahlCityKey, provinceId);
        }

        #endregion city methods

        #region suburb methods

        public void AddSuburb(string suburbName, int sahlSuburbKey, string postalCode, Guid cityId)
        {
            if (string.IsNullOrEmpty(suburbName))
                throw new ArgumentException("Suburb name cannot be empty");

            if (geoDataServices.DoesSAHLSuburbKeyExist(sahlSuburbKey))
                throw new Exception("External Suburb Key already in use");

            if (!geoDataServices.DoesCityIdExist(cityId))
                throw new Exception("City Id invalid");

            geoDataServices.AddSuburb(suburbName, sahlSuburbKey, postalCode, cityId);
        }

        public void ChangeSuburbDetails(Guid id, string suburbName, int sahlSuburbKey, string postalCode, Guid cityId)
        {
            if (string.IsNullOrEmpty(suburbName))
                throw new ArgumentException("Suburb name cannot be empty");

            if (!geoDataServices.DoesSuburbIdExist(id))
                throw new Exception("Invalid Suburb Id");

            if (!geoDataServices.DoesCityIdExist(cityId))
                throw new Exception("City Id invalid");

            if (geoDataServices.HasSAHLSuburbKeyChanged(id, sahlSuburbKey))
            {
                if (geoDataServices.DoesSAHLSuburbKeyExist(sahlSuburbKey))
                    throw new Exception("External Suburb Key already in use.");
            }

            geoDataServices.ChangeSuburbsDetails(id, suburbName, sahlSuburbKey, postalCode, cityId);
        }

        #endregion suburb methods
    }
}