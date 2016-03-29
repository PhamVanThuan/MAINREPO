using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Managers.Statements
{
    public class GetAddressFromStreetAddressStatement : ISqlStatement<AddressDataModel>
    {
        private const int addressFormatKey = (int)AddressFormat.Street;

        public int AddressFormatKey { get { return addressFormatKey; } }

        public StreetAddressModel StreetAddressModel { get; protected set; }

        public string UnitNumber { get { return StreetAddressModel.UnitNumber; } }

        public string BuildingNumber { get { return StreetAddressModel.BuildingNumber; } }

        public string BuildingName { get { return StreetAddressModel.BuildingName; } }

        public string StreetNumber { get { return StreetAddressModel.StreetNumber; } }

        public string StreetName { get { return StreetAddressModel.StreetName; } }

        public string Suburb { get { return StreetAddressModel.Suburb; } }

        public string City { get { return StreetAddressModel.City; } }

        public string PostalCode { get { return StreetAddressModel.PostalCode; } }

        public string Province { get { return StreetAddressModel.Province; } }

        public GetAddressFromStreetAddressStatement(StreetAddressModel streetAddress)
        {
            this.StreetAddressModel = streetAddress;
        }

        public string GetStatement()
        {
            var query = @"SELECT
                               [AddressKey]
                              ,[AddressFormatKey]
                              ,[BoxNumber]
                              ,[UnitNumber]
                              ,[BuildingNumber]
                              ,[BuildingName]
                              ,[StreetNumber]
                              ,[StreetName]
                              ,[SuburbKey]
                              ,[PostOfficeKey]
                              ,[RRR_CountryDescription]
                              ,[RRR_ProvinceDescription]
                              ,[RRR_CityDescription]
                              ,[RRR_SuburbDescription]
                              ,[RRR_PostalCode]
                              ,[UserID]
                              ,[ChangeDate]
                              ,[SuiteNumber]
                              ,[FreeText1]
                              ,[FreeText2]
                              ,[FreeText3]
                              ,[FreeText4]
                              ,[FreeText5]
                        FROM
                              [2AM].[dbo].[Address]
                         WHERE
                              [AddressFormatKey] = @AddressFormatKey
                        AND
                              (ISNULL([UnitNumber], '') = @UnitNumber or @UnitNumber is Null)
                        AND
                              (ISNULL([BuildingNumber], '') = @BuildingNumber or @BuildingNumber is Null)
                        AND
                              (ISNULL([BuildingName], '') = @BuildingName or @BuildingName is Null)
                        AND
                              [StreetNumber] = @StreetNumber
                        AND
                              [StreetName] = @StreetName
                        AND
                              [RRR_SuburbDescription] = @Suburb
                        AND
                              [RRR_CityDescription] = @City
                        AND
                              [RRR_PostalCode] = @PostalCode
                        AND
                              [RRR_ProvinceDescription] = @Province";

            return query;
        }
    }
}