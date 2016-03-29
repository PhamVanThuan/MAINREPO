using System;

namespace SAHL.Core.Data.Dapper.Specs.DapperDdlRepository
{
    internal class TestDataModel : IDataModel
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}