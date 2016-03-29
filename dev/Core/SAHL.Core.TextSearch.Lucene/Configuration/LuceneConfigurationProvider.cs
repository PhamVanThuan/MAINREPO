using SAHL.Core.Configuration;

namespace SAHL.Core.TextSearch.Lucene.Configuration
{
    public class LuceneConfigurationProvider : ConfigurationProvider, ILuceneTextSearchConfigurationProvider
    {
        public string IndexDirectory
        {
            get { return this.Config.AppSettings.Settings["lucene_index"].Value; }
        }

        public string CapitecFailoverWebServer
        {
            get { return this.Config.AppSettings.Settings["CapitecFailoverWebServer"].Value; } // server name and port i.e. uat1-cw02:8080
        }

        public int DefaultNumberOfResultsPerPage
        {
            get
            {
                int pageSize;
                if (!int.TryParse(this.Config.AppSettings.Settings["default_page_size"].Value, out pageSize))
                {
                    pageSize = 100;
                }
                return pageSize;
            }
        }
    }
}