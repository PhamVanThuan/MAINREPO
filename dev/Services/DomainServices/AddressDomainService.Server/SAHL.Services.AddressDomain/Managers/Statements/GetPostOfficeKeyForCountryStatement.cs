using SAHL.Core.Data;

namespace SAHL.Services.AddressDomain.Managers.Statements
{
    public class GetPostOfficeKeyForCountryStatement : ISqlStatement<int?>
    {
        public string Country { get; private set; }

        public GetPostOfficeKeyForCountryStatement(string country)
        {
            this.Country = country;
        }

        public string GetStatement()
        {
            return @"SELECT po.PostOfficeKey FROM
                    [2AM].dbo.Country c
                    JOIN[2AM].dbo.Province p ON c.CountryKey = p.CountryKey
                    JOIN[2AM].dbo.City ON p.ProvinceKey = city.ProvinceKey
                    JOIN[2AM].dbo.PostOffice po ON city.CityKey = po.CityKey
                    WHERE c.Description = @Country";
        }
    }
}