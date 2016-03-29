namespace SAHL.Services.Interfaces.Search.Models
{
    public class GetClientSearchDetailQueryResult
    {
        public GetClientSearchDetailQueryResult(string type, SAHL.Core.BusinessModel.Enums.Product product, int key, int parentKey, string role, int level, string address)
        {
            this.Type = type;
            this.Product = product;
            this.Key = key;
            this.ParentKey = parentKey;
            this.Role = role;
            this.Level = level;
            this.Address = address;
        }

        public string Type { get; protected set; }

        public SAHL.Core.BusinessModel.Enums.Product Product { get; protected set; }

        public int Key { get; protected set; }

        public int ParentKey { get; protected set; }

        public string Role { get; protected set; }

        public int Level { get; protected set; }

        public string Address { get; protected set; }
    }
}