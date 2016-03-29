using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetSuburbProvinceQueryResult
    {
        public Guid Id { get; set; }

        public int SAHLSuburbKey { get; set; }

        public string SuburbName { get; set; }

        public string PostalCode { get; set; }

        public Guid CityId { get; set; }

        public string CityName { get; set; }

        public string ProvinceName { get; set; }
    }
}