using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.Service.Base
{
    internal abstract class SameDomainLoader : MarshalByRefObject // static class (rulesystem would use this) - Framework
    {
        protected static object syncObj = new object();
        protected bool LoadPlugins()
        {
            if (!CreateRemoteLoader())
                return false;
            return true;
        }

        protected abstract bool CreateRemoteLoader();
    }


}
