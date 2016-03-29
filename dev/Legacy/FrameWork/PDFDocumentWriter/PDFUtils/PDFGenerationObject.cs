using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFUtils.PDFWriting
{
    public class PDFGenerationObject
    {
        int _ReportStatementKey;

        public int ReportStatementKey
        {
            get { return _ReportStatementKey; }
            set { _ReportStatementKey = value; }
        }

        Dictionary<string, object> _Params = new Dictionary<string, object>();

        public Dictionary<string, object> Params { get { return _Params; } }

        public PDFGenerationObject(Dictionary<string, object> Params, int ReportStatementKey)
        {
            _ReportStatementKey = ReportStatementKey;
            _Params = Params;
        }
    }
}
