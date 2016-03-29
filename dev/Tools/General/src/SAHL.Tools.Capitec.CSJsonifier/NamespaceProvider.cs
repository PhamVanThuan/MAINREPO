using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public class NamespaceProvider : INamespaceProvider
    {
        public string Namespace
        {
            get;
            protected set;
        }

        public string Prefix
        {
            get;
            protected set;
        }

        public NamespaceProvider(string nameSpace,string prefix)
        {
            this.Namespace = nameSpace;
            this.Prefix = prefix;
        }
    }
}
