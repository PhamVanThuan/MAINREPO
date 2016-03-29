using System;
using System.Collections.Generic;
using System.Dynamic;
using Westwind.Utilities.Dynamic;

namespace SAHL.Services.Query.Builders.Core
{
    public class Filter
    {
        public Filter()
        {
            Where = new Dictionary<string, Object>();
        }

        public IDictionary<string, Object> Where { get; set; }

    }
}