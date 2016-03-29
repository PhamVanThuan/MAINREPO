using SAHL.Core.Configuration;

namespace SAHL.Core.TextSearch.Lucene.Configuration
{
    public interface ILuceneTextSearchConfigurationProvider : IConfigurationProvider
    {
        string IndexDirectory { get; }

        int DefaultNumberOfResultsPerPage { get; }

        string CapitecFailoverWebServer { get; }
    }
}