using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.AddressDomain.Managers.Statements
{
    public class GetPostOfficeStatement : ISqlStatement<PostOfficeDataModel>
    {
        public string CityName { get; protected set; }

        public string ProvinceName { get; protected set; }

        public string PostalCode { get; protected set; }

        public GetPostOfficeStatement(string city, string postalCode, string province)
        {
            this.CityName = city;
            this.PostalCode = postalCode;
            this.ProvinceName = province;
        }

        public string GetStatement()
        {
            var query = @"select p.* from [2AM].[dbo].PostOffice p
                            join [2AM].[dbo].city c on p.CityKey = c.CityKey
                            join [2AM].[dbo].Province pv on c.ProvinceKey = pv.ProvinceKey
                                where pv.Description = @ProvinceName
                                    and p.PostalCode = @PostalCode
                                    and c.Description = @CityName";
            return query;
        }
    }
}