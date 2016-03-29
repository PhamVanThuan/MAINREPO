using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetRandomGeoLocationQueryStatement : IServiceQuerySqlStatement<GetRandomGeoLocationQuery, GetRandomGeoLocationQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 1 s.SuburbKey, s.Description as Suburb, c.CityKey, c.Description as City, p.ProvinceKey, p.Description as Province,
	                co.CountryKey, co.Description as Country, po.PostOfficeKey, po.PostalCode
                from Suburb s
	                join City c on s.CityKey = c.CityKey
	                join Province p on c.ProvinceKey = p.ProvinceKey
	                join Country co on p.CountryKey = co.CountryKey
	                join PostOffice po on c.CityKey = po.CityKey
                order by NEWID()";
        }
    }
}