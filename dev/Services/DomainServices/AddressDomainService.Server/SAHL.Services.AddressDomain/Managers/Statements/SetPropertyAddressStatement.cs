using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.AddressDomain.Managers.Statements
{
    public class SetPropertyAddressStatement : ISqlStatement<PropertyDataModel>
    {
        public int PropertyKey { get; protected set; }

        public int AddressKey { get; protected set; }

        public SetPropertyAddressStatement(int propertyKey, int addressKey)
        {
            this.PropertyKey = propertyKey;
            this.AddressKey = addressKey;
        }

        public string GetStatement()
        {
            return "UPDATE [2AM].[dbo].[Property] SET [AddressKey] = @AddressKey WHERE [PropertyKey] = @PropertyKey";
        }
    }
}