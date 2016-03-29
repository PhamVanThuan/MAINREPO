using SAHL.Core.Caching;

namespace SAHL.Core.Data
{
    public class CachedUIStatementProvider : IUIStatementProvider
    {
        private ICache cache;

        public CachedUIStatementProvider(ICache cache, IUIStatementProvider uiStatementProviderToCache)
        {
            this.UIStatementProviderToCache = uiStatementProviderToCache;
            this.cache = cache;
        }

        public IUIStatementProvider UIStatementProviderToCache { get; protected set; }

        public string Get(string statementContext, string uiStatementName)
        {
            string uniqueStatementName = string.Format("{0}_{1}", statementContext, uiStatementName);
            if (this.cache.Contains(uniqueStatementName))
            {
                return this.cache.GetItem<string>(uniqueStatementName);
            }
            else
            {
                string uiStatement = this.UIStatementProviderToCache.Get(statementContext, uiStatementName);
                if (!string.IsNullOrEmpty(uiStatement))
                {
                    this.cache.AddItem(uniqueStatementName, uiStatement);
                }
                return uiStatement;
            }
        }

        public void Add(IUIStatementsProvider uiStatementsProvider)
        {
            throw new System.NotImplementedException();
        }
    }
}