using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Configuration;
using System.Configuration;
using System.IO;
using SAHL.Common.Rules.Service;
using System.Web;

namespace SAHL.Common.Service.Base
{
    internal abstract class PluginManager : MarshalByRefObject
    {
        protected string _pluginFolder;
        protected string _assemblyName;
        protected string _fullPath;
        protected string _debugFullPath;
        protected static Object _lockObject = new Object();
        

        public override Object InitializeLifetimeService()
        {
            return null;
        }

        protected abstract bool LoadSection();

        protected bool Start()
        {
            if (LoadSection())
            {

                // if there is an HttpContext and the directory doesn't exist, then try and map the path
                if (!Directory.Exists(_pluginFolder) && HttpContext.Current != null)
                {
                    //System.Web.HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath)	"C:\\Development\\SourceCode\\Application\\SAHL.Web"	string
                    //Directory.Exists(Path.Combine("C:\\Development\\SourceCode\\Application\\SAHL.Web", _pluginFolder))	true	bool
                    _pluginFolder = Path.Combine(HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath), _pluginFolder);
                }
                _fullPath = Path.Combine(_pluginFolder, _assemblyName);
                _debugFullPath = Path.ChangeExtension(_fullPath, ".pdb");
                if (!File.Exists(_debugFullPath))
                    _debugFullPath = null;

                // ensure that the location actually exists
                if (!File.Exists(_fullPath))
                    throw new RuleException(String.Format("Plugin {0} does not exist", _fullPath));

                return true;
            }
            return false;
        }

        /// <summary>
        /// The folder in which the assembly resides.
        /// </summary>
        public string PluginFolder
        {
            get
            {
                return _pluginFolder;
            }
        }

        /// <summary>
        /// The name of the assembly file.
        /// </summary>
        public string AssemblyName
        {
            get
            {
                return _assemblyName;
            }
        }

        /// <summary>
        /// The full path to the debug fule, or null if it doesn't exist.
        /// </summary>
        public string DebugFullPath
        {
            get
            {
                return _debugFullPath;
            }
        }

        /// <summary>
        /// The full path of the assembly file.
        /// </summary>
        public string FullPath
        {
            get
            {
                return _fullPath;
            }
        }
    }
}
