using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.TextSearch.Solr.Models
{
    public class SolrResponse<T>
    {
        public ResponseHeader responseHeader { get; set; }
        public Response<T> response { get; set; }
    }

    public class Params
    {
        public string q { get; set; }
        public string defType { get; set; }
        public string qf { get; set; }
        public string start { get; set; }
        public string rows { get; set; }
        public string wt { get; set; }
    }

    public class ResponseHeader
    {
        public int status { get; set; }
        public int QTime { get; set; }
        public Params @params { get; set; }
    }

    public class Response<T>
    {
        public int numFound { get; set; }
        public int start { get; set; }
        public List<T> docs { get; set; }
    }


}
