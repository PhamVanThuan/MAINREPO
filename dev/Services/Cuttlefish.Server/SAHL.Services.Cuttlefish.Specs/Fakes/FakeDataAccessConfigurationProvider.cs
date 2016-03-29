using SAHL.Services.Cuttlefish.Services;

namespace SAHL.Services.Cuttlefish.Specs.Fakes
{
    public class FakeDataAccessConfigurationProvider : IDataAccessConfigurationProvider
    {
        private string connectionString;

        public FakeDataAccessConfigurationProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public string ConnectionString
        {
            get { return this.connectionString; }
        }
    }
}