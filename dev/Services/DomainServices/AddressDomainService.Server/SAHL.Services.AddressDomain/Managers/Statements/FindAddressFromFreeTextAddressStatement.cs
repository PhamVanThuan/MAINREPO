using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Services.AddressDomain.Managers.Statements
{
    public class FindAddressFromFreeTextAddressStatement : ISqlStatement<AddressDataModel>
    {

        public FreeTextAddressModel FreeTextAddressModel { get; private set; }

        public int AddressFormatKey { get { return (int)FreeTextAddressModel.AddressFormat; } }

        public string FreeText1 { get { return FreeTextAddressModel.FreeText1; } }

        public string FreeText2 { get { return FreeTextAddressModel.FreeText2; } }

        public string FreeText3 { get { return FreeTextAddressModel.FreeText3; } }

        public string FreeText4 { get { return FreeTextAddressModel.FreeText4; } }

        public string FreeText5 { get { return FreeTextAddressModel.FreeText5; } }

        public string Country { get { return FreeTextAddressModel.Country; } }

        public FindAddressFromFreeTextAddressStatement(FreeTextAddressModel freeTextAddressModel)
        {
            this.FreeTextAddressModel = freeTextAddressModel;
        }

        public string GetStatement()
        {
            return @"SELECT * FROM [2AM].[dbo].[Address]
                        WHERE [AddressFormatKey] = @AddressFormatKey
                        AND ISNULL([FreeText1], '''') =  @FreeText1
                        AND (ISNULL([FreeText2], '''') = @FreeText2 or @FreeText2 is null)
                        AND (ISNULL([FreeText3], '''') = @FreeText3 or @FreeText3 is null)
                        AND (ISNULL([FreeText4], '''') = @FreeText4 or @FreeText4 is null)
                        AND (ISNULL([FreeText5], '''') = @FreeText5 or @FreeText5 is null)";
        }
    }
}