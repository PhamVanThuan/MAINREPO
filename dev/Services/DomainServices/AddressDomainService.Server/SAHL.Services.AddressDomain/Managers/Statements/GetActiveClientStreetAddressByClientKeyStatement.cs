using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.AddressDomain.Managers.Statements
{
    public class GetActiveClientStreetAddressByClientKeyStatement : ISqlStatement<AddressDataModel>
    {
        public int ClientKey { get; protected set; }

        public int AddressFormatKey { get; protected set; }

        public int GeneralStatusKey { get; private set; }

        public GetActiveClientStreetAddressByClientKeyStatement(int ClientKey)
        {
            this.ClientKey = ClientKey;
            this.AddressFormatKey = (int)AddressFormat.Street;
            this.GeneralStatusKey = (int)GeneralStatus.Active;
        }

        public string GetStatement()
        {
            string query = @"SELECT 
                                A.* 
                            FROM 
                                [2AM].dbo.LegalEntityAddress LEA
                            JOIN
                                [2AM].dbo.Address A ON LEA.AddressKey = A.AddressKey
                            WHERE 
                                LEA.LegalEntityKey = @ClientKey 
                                AND A.AddressFormatKey = @AddressFormatKey 
                                AND LEA.AddressTypeKey in (1, 2) 
                                AND LEA.GeneralStatusKey = @GeneralStatusKey
                            ORDER BY 
                                LEA.AddressTypeKey, 
                                A.ChangeDate DESC
                            ";
            return query;
        }
    }
}