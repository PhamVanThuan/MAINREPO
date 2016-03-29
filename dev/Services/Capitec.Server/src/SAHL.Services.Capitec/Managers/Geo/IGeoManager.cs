using System;

namespace SAHL.Services.Capitec.Managers.Geo
{
    public interface IGeoManager
    {
        void AddProvince(string provinceName, int sAHLProvinceKey, Guid countryId);

        void ChangeProvinceDetails(Guid id, string provinceName, int sahlProvinceKey, Guid countryId);

        void AddCity(string cityName, int sahlCityKey, Guid provinceId);

        void ChangeCityDetails(Guid id, string cityName, int sahlCityKey, Guid provinceId);

        void AddSuburb(string suburbName, int sahlSuburbKey, string postalCode, Guid cityId);

        void ChangeSuburbDetails(Guid id, string suburbName, int sahlSuburbKey, string postalCode, Guid cityId);
    }
}