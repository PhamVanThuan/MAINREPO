using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetProvinceQueryResult
    {
        public Guid Id { get; set; }

        public int SAHLProvinceKey { get; set; }

        public string ProvinceName { get; set; }

        public Guid CountryId { get; set; }
    }
}