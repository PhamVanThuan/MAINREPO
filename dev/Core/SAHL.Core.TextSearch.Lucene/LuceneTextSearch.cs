using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SAHL.Core.Data;
using SAHL.Core.TextSearch.Lucene.Configuration;
using SAHL.Core.TextSearch.Lucene.Models;
using SAHL.Core.TextSearch.Lucene.Queries;
using LuceneDotNet = Lucene.Net;

namespace SAHL.Core.TextSearch.Lucene
{
    public class LuceneTextSearch : ITextSearchProvider
    {
        private static readonly Func<string, string> wildcardMatchSearchTermTransformFunction = a => "*" + a.Replace("*", "").Replace("\'", "").Replace("\"", "").Replace(",", "") + "*";
        private static readonly Func<string, string> exactMatchSearchTermTransformFunction = a => a.Replace("\'", "").Replace("\"", "").Replace(",", "");
        private static FSDirectory indexDirectory = null;

        private readonly string capitecFailoverWebServer;
        private readonly IDbFactory dbFactory;
        private readonly int defaultPageSize;
        private readonly string luceneDirectory;

        public LuceneTextSearch(ILuceneTextSearchConfigurationProvider luceneIndexConfigurationProvider, IDbFactory dbFactory)
        {
            this.luceneDirectory = luceneIndexConfigurationProvider.IndexDirectory;
            this.defaultPageSize = luceneIndexConfigurationProvider.DefaultNumberOfResultsPerPage;
            this.capitecFailoverWebServer = luceneIndexConfigurationProvider.CapitecFailoverWebServer;
            this.dbFactory = dbFactory;
        }

        private FSDirectory IndexDirectory
        {
            get
            {
                if (indexDirectory != null)
                {
                    return indexDirectory;
                }
                indexDirectory = FSDirectory.Open(new DirectoryInfo(luceneDirectory));
                if (IndexWriter.IsLocked(indexDirectory))
                {
                    IndexWriter.Unlock(indexDirectory);
                }

                string lockFilePath = Path.Combine(luceneDirectory, "write.lock");
                if (File.Exists(lockFilePath))
                {
                    File.Delete(lockFilePath);
                }
                return indexDirectory;
            }
        }

        private bool IndexExists
        {
            get { return IndexReader.IndexExists(IndexDirectory); }
        }

        /// <summary>
        ///     Gets auto complete terms for given field
        /// </summary>
        /// <param name="field"></param>
        /// <param name="partialSearchTerm"></param>
        /// <returns></returns>
        public string[] GetAutoCompleteSuggestions(string field, string partialSearchTerm)
        {
            using (var searcher = new IndexSearcher(IndexDirectory, true))
            {
                var termEnum = searcher.IndexReader.Terms(new Term(field, partialSearchTerm));
                Dictionary<string, double> keywords = new Dictionary<string, double>();
                do
                {
                    if (termEnum.Term != null)
                    {
                        keywords.Add(termEnum.Term.Text, termEnum.DocFreq());
                    }
                } while (termEnum.Next() && termEnum.Term.Text.StartsWith(partialSearchTerm) && termEnum.Term.Text.Length > 1);

                return keywords.OrderByDescending(w => w.Value).Select(w => w.Key).Take(10).ToArray();
            }
        }

        /// <summary>
        ///     Gets autocomplete terms for given field where other filled in fields also match.
        ///     Indexing must add ShinglesAnalyzer to allow phrase auto suggest and only then will
        ///     filtering by already selected terms be added.
        /// </summary>
        /// <param name="propertyRequestingSuggestions"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public string[] GetMultiFieldAwareAutoCompleteSuggestions<T>(string propertyRequestingSuggestions, T queryModel)
            where T : class, new()
        {
            Dictionary<string, string> multiFieldSearchTerms = GetFieldValuePairsFromSearchModel(queryModel);
            var partialSearchTerm = multiFieldSearchTerms[propertyRequestingSuggestions];

            return GetAutoCompleteSuggestions(propertyRequestingSuggestions, partialSearchTerm);
        }

        /// <summary>
        ///     Performs search matching all supplied field terms doing wildcard within each field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public QueryResult<U> MultiFieldSearchAndAcrossWildcardWithin<T, U>(IQueryModel<T> model)
            where T : class, new()
            where U : class, IQueryResultModel, new()
        {
            return MultiFieldSearchAndAcrossWithin<T, U>(model, wildcardMatchSearchTermTransformFunction);
        }

        /// <summary>
        ///     Performs search matching all supplied field terms requiring exact matches within each field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public QueryResult<U> MultiFieldSearchAndAcrossExactMatchWithin<T, U>(IQueryModel<T> model)
            where T : class, new()
            where U : class, IQueryResultModel, new()
        {
            return MultiFieldSearchAndAcrossWithin<T, U>(model, exactMatchSearchTermTransformFunction);
        }

        /// <summary>
        ///     Performs search across all fields using freetext input
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="searchTerm"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public QueryResult<U> FreeTextSearch<T, U>(string searchTerm, int currentPage, int pageSize)
            where T : class, new()
            where U : class, IQueryResultModel, new()
        {
            var nonNullSearchTerm = searchTerm ?? string.Empty;
            var queryResult = new QueryResult<U>();

            var criterion = nonNullSearchTerm.Trim().Replace("-", " ").Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            var criteria = string.Join(" ", criterion);
            var page = currentPage + 1;
            int totalMatches;
            queryResult.CurrentPageResults = IndexPagedSearch<T, U>(criteria, currentPage, pageSize, out totalMatches);
            queryResult.Paging.TotalItems = totalMatches;
            queryResult.Paging.CurrentPage = page;
            queryResult.Paging.ItemsPerPage = pageSize > 0 ? pageSize : defaultPageSize;
            return queryResult;
        }

        public void RefreshIndex()
        {
            ApplicationLuceneDocumentQuery applicationStatusQuery = new ApplicationLuceneDocumentQuery();
            IEnumerable<ApplicationLuceneDocumentModel> models = GetLuceneDocumentModels(applicationStatusQuery);
            if (!models.Any())
            {
                return;
            }
            UpdateLuceneIndexModels(models);
            OptimizeLuceneIndex();
        }

        public void RefreshIndexOnCapitecFailoverWebServer()
        {
            if (!string.IsNullOrEmpty(capitecFailoverWebServer))
            {
                const string command = @"{""_name"":""SAHL.Services.Interfaces.CapitecSearch.Commands.RefreshFailoverLuceneIndexCommand,SAHL.Services.Interfaces.CapitecSearch""}";
                using (WebClient webClient = new WebClient())
                {
                    webClient.UploadString("http://" + capitecFailoverWebServer + "/CapitecSearchService/api/CommandHttpHandler/PerformHttpCommand", "POST", command);
                }
            }
        }

        public void RefreshIndexAndClearStagingData()
        {
            ApplicationLuceneDocumentQuery applicationStatusQuery = new ApplicationLuceneDocumentQuery();
            IEnumerable<ApplicationLuceneDocumentModel> models = GetLuceneDocumentModels(applicationStatusQuery);

            if (!models.Any())
            {
                return;
            }
            UpdateLuceneIndexModels(models);

            RemoveAllApplicationIndexQuery removeAllApplicationIndexQuery = new RemoveAllApplicationIndexQuery();
            DeleteIndexModel(removeAllApplicationIndexQuery);

            OptimizeLuceneIndex();
        }

        public QueryResult<U> MultiFieldSearchAndAcrossWithin<T, U>(IQueryModel<T> model, Func<string, string> searchTermTransformFunction)
            where T : class, new()
            where U : class, IQueryResultModel, new()
        {
            Dictionary<string, string> multiFieldSearchTerms = GetFieldValuePairsFromSearchModel(model.QueryModel);

            var queryResult = new QueryResult<U>();
            if (multiFieldSearchTerms == null || multiFieldSearchTerms.Count == 0)
            {
                queryResult.Paging.TotalItems = 0;
                queryResult.CurrentPageResults = new List<U>();
            }
            else
            {
                var innerQuery = new BooleanQuery();
                foreach (var key in multiFieldSearchTerms.Keys)
                {
                    if (key.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    string value = "*";
                    if (!string.IsNullOrEmpty(multiFieldSearchTerms[key]))
                    {
                        value = searchTermTransformFunction(multiFieldSearchTerms[key]);
                    }

                    string[] words = value.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        innerQuery.Add(new WildcardQuery(new Term(key, word)), Occur.MUST);
                    }
                }
                int totalResultSet, pageSize = model.PageSize > 0 ? model.PageSize : defaultPageSize;

                queryResult.CurrentPageResults = IndexPagedSearch<T, U>(innerQuery, model.CurrentPage, pageSize, out totalResultSet);
                queryResult.Paging.TotalItems = totalResultSet;
                queryResult.Paging.CurrentPage = model.CurrentPage;
                queryResult.Paging.ItemsPerPage = model.PageSize;
            }
            return queryResult;
        }

        private IEnumerable<T> GetLuceneDocumentModels<T>(ISqlStatement<T> query)
        {
            IEnumerable<T> results;
            using (Data.Context.IReadOnlyDbContext db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                results = db.Select(query).ToList();
            }
            return results;
        }

        private void UpdateLuceneIndexModels<T>(IEnumerable<T> indexModels)
        {
            foreach (T indexModel in indexModels)
            {
                Document document = new Document();
                string uniqueIdPropertyName = string.Empty;
                string uniqueIdValue = string.Empty;

                foreach (PropertyInfo propertyInfo in indexModel.GetType().GetProperties())
                {
                    bool analyse = false;
                    bool uniqueId = false;
                    foreach (CustomAttributeData customAttributeData in propertyInfo.CustomAttributes)
                    {
                        if (customAttributeData.AttributeType.FullName == "SAHL.Core.Services.Attributes.LuceneFieldUniqueIdAttribute")
                        {
                            uniqueId = true;
                        }

                        if (customAttributeData.AttributeType.FullName == "SAHL.Core.Services.Attributes.LuceneFieldAnalyseAttribute")
                        {
                            analyse = true;
                        }
                    }

                    object value;
                    if (analyse)
                    {
                        value = indexModel.GetType().GetProperty(propertyInfo.Name).GetValue(indexModel);
                        document.Add(new Field(propertyInfo.Name, value == null ? "" : value.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    }
                    else
                    {
                        value = indexModel.GetType().GetProperty(propertyInfo.Name).GetValue(indexModel);
                        document.Add(new Field(propertyInfo.Name, value == null ? "" : value.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    }

                    if (uniqueId)
                    {
                        uniqueIdPropertyName = propertyInfo.Name;
                        uniqueIdValue = value == null ? "" : value.ToString();
                    }
                }

                if (string.IsNullOrEmpty(uniqueIdPropertyName) || string.IsNullOrEmpty(uniqueIdValue) || document.fields_ForNUnit.Count <= 0)
                {
                    continue;
                }
                using (IndexWriter writer = new IndexWriter(IndexDirectory, new StandardAnalyzer(LuceneDotNet.Util.Version.LUCENE_30), !IndexExists, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.UpdateDocument(new Term(uniqueIdPropertyName, uniqueIdValue), document);
                }
            }
        }

        private void DeleteIndexModel<T>(ISqlStatement<T> query)
        {
            using (Data.Context.IDbContext db = this.dbFactory.NewDb().InAppContext())
            {
                db.Delete<T>(query);
                db.Complete();
            }
        }

        /// <summary>
        ///     Requests an "optimize" operation on an index, priming the index for the fastest available search.
        ///     See Lucene.Net.Index.Optimize for more details
        /// </summary>
        private void OptimizeLuceneIndex()
        {
            using (IndexWriter writer = new IndexWriter(IndexDirectory, new StandardAnalyzer(LuceneDotNet.Util.Version.LUCENE_30), false, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                writer.Optimize();
            }
        }

        private Dictionary<string, string> GetFieldValuePairsFromSearchModel<T>(T model)
        {
            Dictionary<string, string> multiFieldSearchTerms = new Dictionary<string, string>();
            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType)
                         ?? propertyInfo.PropertyType;

                var value = propertyInfo.GetValue(model, null);
                object safeValue = (value == null) ? null
                    : Convert.ChangeType(value, t);

                if (safeValue == null || safeValue.Equals(GetDefaultValue(t)))
                {
                    continue;
                }

                multiFieldSearchTerms.Add(propertyInfo.Name, value.ToString().ToLowerInvariant());
            }
            return multiFieldSearchTerms;
        }

        private object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private Query ParseQuery(string input, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Query(input.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(input.Trim()));
            }
            return query;
        }

        private string[] GetPropertyNamesFromModel<T>() where T : class, new()
        {
            T model = new T();
            return model.GetType().GetProperties().Select(x => x.Name).ToArray();
        }

        private IEnumerable<U> IndexPagedSearch<T, U>(string freeTextInput, int currentPage, int pageSize, out int totalResultSet)
            where T : class, new()
            where U : class, IQueryResultModel, new()
        {
            if (string.IsNullOrEmpty(freeTextInput.Replace("*", "").Replace("?", "")))
            {
                totalResultSet = 0;
                return new List<U>();
            }

            using (var searcher = new IndexSearcher(IndexDirectory, true))
            {
                searcher.SetDefaultFieldSortScoring(true, true);
                const int hitsLimit = 100;
                var analyzer = new StandardAnalyzer(LuceneDotNet.Util.Version.LUCENE_30);
                var parser = new MultiFieldQueryParser(LuceneDotNet.Util.Version.LUCENE_30, GetPropertyNamesFromModel<T>(), analyzer);
                var query = ParseQuery(freeTextInput, parser);
                var topDocs = searcher.Search(query, null, hitsLimit);

                var results = MapPagedResultsToModel<U>(topDocs, searcher, currentPage, pageSize);
                analyzer.Close();
                searcher.Dispose();
                totalResultSet = topDocs.TotalHits;
                return results.OrderByDescending(x => x.Score);
            }
        }

        private IEnumerable<U> IndexPagedSearch<T, U>(BooleanQuery mainQuery, int currentPage, int pageSize, out int totalResultSet)
            where T : class, new()
            where U : class, IQueryResultModel, new()
        {
            if (!mainQuery.Any())
            {
                totalResultSet = 0;
                return new List<U>();
            }

            var analyzer = new StandardAnalyzer(LuceneDotNet.Util.Version.LUCENE_30);
            using (var searcher = new IndexSearcher(IndexDirectory, true))
            {
                searcher.SetDefaultFieldSortScoring(true, true);
                // Lucene's costly part is reading the results from the index and not the search itself
                var hitsLimit = (currentPage) * pageSize;

                var topDocs = searcher.Search(mainQuery, null, hitsLimit);
                var results = MapPagedResultsToModel<U>(topDocs, searcher, currentPage, pageSize);
                analyzer.Close();
                searcher.Dispose();
                totalResultSet = topDocs.TotalHits;
                return results.OrderByDescending(x => x.Score);
            }
        }

        private IEnumerable<T> MapPagedResultsToModel<T>(TopDocs topDocs, IndexSearcher searcher, int currentPage, int pageSize)
            where T : class, IQueryResultModel, new()
        {
            int startDocIndex = (currentPage - 1) * pageSize;
            int endDocIndex = currentPage * pageSize;
            List<T> mappedModels = new List<T>();
            for (int i = startDocIndex; i < endDocIndex && i < topDocs.ScoreDocs.Length; i++)
            {
                var doc = searcher.IndexReader.Document(topDocs.ScoreDocs[i].Doc);
                var model = MapScoreDocToModel<T>(doc, topDocs.ScoreDocs[i].Score);
                mappedModels.Add(model);
            }

            return mappedModels;
        }

        private T MapScoreDocToModel<T>(Document document, float score)
            where T : class, IQueryResultModel, new()
        {
            T model = new T();
            model.Score = score;

            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                var targetType = propertyInfo.PropertyType;
                Object val = null;
                string docVal = document.Get(propertyInfo.Name);

                if (!string.IsNullOrEmpty(docVal))
                {
                    val = docVal;
                }

                var nullableType = Nullable.GetUnderlyingType(targetType);

                if (val == null)
                {
                    continue;
                }
                val = Convert.ChangeType(val, nullableType ?? targetType);
                propertyInfo.SetValue(model, val, null);
            }
            return model;
        }
    }
}