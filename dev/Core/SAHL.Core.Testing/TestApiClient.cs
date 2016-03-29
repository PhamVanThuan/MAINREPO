using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SAHL.Core.Testing
{
    public static class TestApiClient
    {
        private static readonly string baseUrl = ConfigurationManager.AppSettings["TestAPIClient"];

        public static T GetByKey<T>(int keyValue) where T : class, IDataModel
        {
            var model = typeof(T).Name.Replace("DataModel", "");
            UriBuilder builder = new UriBuilder(baseUrl);
            builder.Path = string.Format("/api/{0}/{1}", model.ToLower(), keyValue);
            var stream = QueryAPI(builder.Uri);
            using (var reader = new StreamReader(stream))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
        }

        public static IEnumerable<T1> Get<T1>(object where, int limit = 0)
        {
            ValidateWhereFilter(where, typeof(T1));
            var model = typeof(T1).Name.Replace("DataModel", "");
            UriBuilder builder = new UriBuilder(baseUrl);
            builder.Path = string.Format("/api/{0}", model.ToLower());
            var urlQueryString = BuildFilteredQueryString(where);
            urlQueryString = ApplyLimit(urlQueryString, limit);
            builder.Query = urlQueryString;
            var stream = QueryAPI(builder.Uri);
            using (var reader = new StreamReader(stream))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<T1>>(reader.ReadToEnd());
            }
        }

        private static void ValidateWhereFilter(object where, Type type)
        {
            foreach (var prop in where.GetType().GetProperties())
            {
                var properties = type.GetProperties();
                var exists = properties.Any(x => x.Name.ToLower().Replace("rrr_", string.Empty) == prop.Name.ToLower());
                if (!exists)
                {
                    var message = string.Format("Filter object contained parameter [{0}] that does not exist on the {1}."
                        , prop.Name
                        , type.Name
                        );
                    throw new Exception(message);
                }
            }
        }

        private static string ApplyLimit(string urlQueryString, int limit)
        {
            return limit == 0
                ? urlQueryString
                : string.Format("{0}{1}", urlQueryString, string.Format("&filter[limit]={0}", limit));
        }

        private static string BuildFilteredQueryString(object where)
        {
            const string whereFilter = "filter[where]";
            int count = 0;
            var urlQueryString = string.Empty;
            foreach (var prop in where.GetType().GetProperties())
            {
                var propValue = prop.GetValue(where);
                var propName = prop.Name;
                string filter = string.Empty;
                filter = string.Format("{0}[{1}]={2}", whereFilter, propName, propValue);
                filter = count == 0 ? filter : filter.Insert(0, "&");
                urlQueryString += filter.ToLower();
                count++;
            }
            return urlQueryString;
        }

        private static string BuildExcludeQueryString(object neq)
        {
            int count = 0;
            var urlQueryString = string.Empty;
            foreach (var prop in neq.GetType().GetProperties())
            {
                var propValue = prop.GetValue(neq);
                var propName = prop.Name;
                string filter = string.Empty;
                filter = string.Format("filter[where][{0}][neq]={1}", propName, propValue);
                filter = count == 0 ? filter : filter.Insert(0, "&");
                urlQueryString += filter.ToLower();
                count++;
            }
            return urlQueryString;
        }

        private static Stream QueryAPI(Uri uri)
        {
            var client = new WebClient();
            return client.OpenRead(uri);
        }

        public static T2 GetAny<T2>() where T2 : class, IDataModel
        {
            var model = typeof(T2).Name.Replace("DataModel", "");
            UriBuilder builder = new UriBuilder(baseUrl);
            builder.Path = string.Format("/api/{0}", model.ToLower());
            var stream = QueryAPI(builder.Uri);
            using (var reader = new StreamReader(stream))
            {
                var allInstances = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<T2>>(reader.ReadToEnd());
                return allInstances.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            }
        }
        public static T2 GetAny<T2>(object where, int limit = 0) where T2 : class, IDataModel
        {
            var model = typeof(T2).Name.Replace("DataModel", "");
            UriBuilder builder = new UriBuilder(baseUrl);
            builder.Path = string.Format("/api/{0}", model.ToLower());
            var urlQueryString = BuildFilteredQueryString(where);
            urlQueryString = ApplyLimit(urlQueryString, limit);
            builder.Query = urlQueryString;
            var stream = QueryAPI(builder.Uri);
            using (var reader = new StreamReader(stream))
            {
                var allInstances = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<T2>>(reader.ReadToEnd());
                return allInstances.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            }
        }

        public static T3 Update<T3>(T3 dataModel, int key) where T3 : IDataModel, IDataModelWithIdentitySeed
        {
            var model = typeof(T3).Name.Replace("DataModel", "");
            UriBuilder builder = new UriBuilder(baseUrl);
            builder.Path = string.Format("/api/{0}/{1}", model.ToLower(), key);
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(dataModel).ToLower();
            var msg = Encoding.UTF8.GetBytes(data);
            using (var client = new System.Net.WebClient())
            {
                var response = client.UploadData(builder.Uri, "PUT", msg);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T3>(Encoding.UTF8.GetString(response));
            }
        }

        public static T4 GetAnyExcluding<T4>(object where, int limit = 0) where T4 : class, IDataModel
        {
            var model = typeof(T4).Name.Replace("DataModel", "");
            UriBuilder builder = new UriBuilder(baseUrl);
            builder.Path = string.Format("/api/{0}", model.ToLower());
            var urlQueryString = BuildExcludeQueryString(where);
            urlQueryString = ApplyLimit(urlQueryString, limit);
            builder.Query = urlQueryString;
            var stream = QueryAPI(builder.Uri);
            using (var reader = new StreamReader(stream))
            {
                var allInstances = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<T4>>(reader.ReadToEnd());
                return allInstances.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            }
        }
    }
}