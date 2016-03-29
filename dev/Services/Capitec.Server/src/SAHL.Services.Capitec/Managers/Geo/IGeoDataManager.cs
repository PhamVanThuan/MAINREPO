using System;

namespace SAHL.Services.Capitec.Managers.Geo
{
    public interface IGeoDataManager
    {
        #region country methods
        bool DoesCountryIdExist(Guid id);
        #endregion

        #region province methods

        bool DoesProvinceNameExist(string provinceName);

        bool DoesSAHLProvinceKeyExist(int sahlProvinceKey);

        bool HasProvinceNameChanged(Guid id, string provinceName);

        bool DoesProvinceIdExist(Guid id);

        bool HasSAHLProvinceKeyChanged(Guid id, int sahlProvinceKey);

        void AddProvince(string provinceName, int sahlProvinceKey, Guid countryId);

        void ChangeProvinceDetails(Guid id, string provinceName, int sahlProvinceKey, Guid countryId);

        #endregion province methods

        #region city methods

        bool DoesCityIdExist(Guid id);

        bool HasSAHLCityKeyChanged(Guid id, int sahlCityKey);

        bool DoesSAHLCityKeyExist(int sahlCityKey);

        void AddCity(string cityName, int sahlCityKey, Guid provinceId);

        void ChangeCityDetails(Guid id, string cityName, int sahlCityKey, Guid provinceId);

        #endregion city methods

        #region suburb methods

        bool DoesSAHLSuburbKeyExist(int sahlSuburbKey);

        bool HasSAHLSuburbKeyChanged(Guid id, int sahlSuburbKey);

        bool DoesSuburbIdExist(Guid id);

        void AddSuburb(string suburbName, int sahlSuburbKey, string postalCode, Guid cityId);

        void ChangeSuburbsDetails(Guid id, string suburbName, int sahlSuburbKey, string postalCode, Guid cityId);

        #endregion city methods
    }
}