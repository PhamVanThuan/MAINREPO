using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.AddressDomain.Managers.Statements
{
    public class GetSuburbStatement : ISqlStatement<SuburbDataModel>
    {
        public string SuburbName { get; protected set; }

        public string CityName { get; protected set; }

        public string ProvinceName { get; protected set; }

        public string PostalCode { get; protected set; }

        public GetSuburbStatement(string suburb, string city, string postalCode, string province)
        {
            this.SuburbName = suburb;
            this.CityName = city;
            this.PostalCode = postalCode;
            this.ProvinceName = province;
        }

        public string GetStatement()
        {
            var query = @"SELECT
                               s.[SuburbKey]
                             , s.[Description]
                             , s.[CityKey]
                             , s.[PostalCode]
                        FROM
                            [2AM].[dbo].[Suburb] s
                        JOIN
                            [2AM].[dbo].[City] c
                          On
                            s.CityKey = c.CityKey
                        JOIN
                            [2AM].[dbo].[Province] p
                          On
                            c.ProvinceKey = p.ProvinceKey
                        WHERE
                            s.[Description] = @SuburbName
                        AND
                            c.[Description] = @CityName
                        AND
                            s.PostalCode = @PostalCode
                        AND
                            p.[Description] = @ProvinceName";

            return query;
        }
    }
}