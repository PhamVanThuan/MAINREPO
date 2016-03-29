using System;
using System.Configuration;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Attributes;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Configuration;
using SAHL.Common.Factories.Strategies;

namespace SAHL.Common.Factories
{
    public static class TypeFactory
    {
        const string FACTORY = "FACTORY";
        private static IFactoryStrategy _strategy;
        private static bool _typesLoaded = false;
        private static object _lockObject = new object();
        private static DefaultStrategy _defaultStrategy = new DefaultStrategy();
        public static event EventHandler OnGetTypes;
        public static event EventHandler OnFactoryTypeInfoLoadedSucceeded;
        public static event EventHandler OnFactoryTypeInfoLoadedFail;
        public static event EventHandler OnReflectTypeLoadException;

        public static void LoadTypes(SAHLFactoriesSection Section)
        {
            CacheManager CM = CacheFactory.GetCacheManager(FACTORY);
            FactoryElement FE = null;
            string TypeName = "";
            for (int j = 0; j < Section.Factories.Count; j++)
            {
                Assembly Asm = null;
                // try load the type and extract repositories
                FE = Section.Factories[j];
                try
                {
                    Asm = Assembly.Load(FE.AssemblyName);
                    Type[] _Types = Asm.GetTypes();
                    if (null != OnGetTypes)
                    {
                        OnGetTypes(FE.AssemblyName, null);
                    }
                    for (int k = 0; k < _Types.Length; k++)
                    {
                        FactoryTypeAttribute[] RAs = _Types[k].GetCustomAttributes(typeof(FactoryTypeAttribute), false) as FactoryTypeAttribute[];
                        if (RAs.Length == 1)
                        {
                            try
                            {
                                FactoryTypeAttribute fta = RAs[0];
                                // we have found a repository to load
                                string AssemblyFullName = Asm.FullName;
                                Type RepositoryType = fta.FactoryType;
                                Type RepositoryInstType = _Types[k];
                                TypeName = RepositoryInstType.FullName;
                                FactoryTypeLifeTime LifeTime = fta.LifeTime;

                                // store the above against the type so we can find it

                                string TN = RepositoryType.ToString();
                                if (!CM.Contains(TN))
                                {
                                    CM.Add(TN, new FactoryCacheItem(AssemblyFullName, RepositoryType, RepositoryInstType, LifeTime));
                                    if (null != OnFactoryTypeInfoLoadedSucceeded)
                                    {
                                        OnFactoryTypeInfoLoadedSucceeded(TypeName, null);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                if (null != OnFactoryTypeInfoLoadedFail)
                                {
                                    string s = string.Format("Unable to load Typename:{0}{1}{2}", TypeName, Environment.NewLine, ex.ToString());
                                    OnFactoryTypeInfoLoadedFail(s, null);
                                }
                            }
                        }
                    }
                }
                catch (ReflectionTypeLoadException rtle)
                {
                    if (null != OnReflectTypeLoadException)
                    {
                        LoadArgs l = new LoadArgs();
                        l.TypeName = TypeName;
                        l.AsmName = Asm.FullName;
                        l.ReflectionException = rtle;
                        OnReflectTypeLoadException(null, l);
                    }
                    throw;
                }
                catch (Exception E)
                {
                    throw new Exception(string.Format("Error loading types from asm:{0}", FE.AssemblyName), E);
                }
            }
            _typesLoaded = true;
        }

        public static void EnsureLoaded()
        {
            if (!_typesLoaded)
            {
                SAHLFactoriesSection Section = ConfigurationManager.GetSection("SAHLFactories") as SAHLFactoriesSection;
                if (Section != null)
                {
                    // get the strategy
                    string StategyType = Section.CreationStratergy;
                    Type StratType = Type.GetType(StategyType);
                    _strategy = Activator.CreateInstance(StratType) as IFactoryStrategy;
                    LoadTypes(Section);
                }
                else
                {
                    //_strategy = Activator.CreateInstance(typeof(Common.Factories.Strategies.DefaultStrategy)) as IFactoryStrategy;
                    //LoadTypes(Section);
                    throw new Exception("TypeFactory was unable to find the the 'SAHLFactories' Config Section and thus could not instantiate a FactoryStrategy.");
                }
            }
        }

        public static T CreateType<T>(params object[] Parameters)
        {
            lock (_lockObject)
            {
                try
                {
                    EnsureLoaded();
                    // TODO: this is a nasty hack, but we really don't want the type mapper assuming the mock
                    // strategy - this should be changed so it's a little more configurable
                    if (typeof(T) == typeof(IBusinessModelTypeMapper))
                        return _defaultStrategy.CreateType<T>(Parameters);

                    return _strategy.CreateType<T>(Parameters);
                }
                catch (ReflectionTypeLoadException r)
                {
                    if (null != OnReflectTypeLoadException)
                    {
                        Type TT = typeof(T);
                        LoadArgs l = new LoadArgs();
                        l.ReflectionException = r;
                        l.AsmName = "UNKNOWN - In CreateType<T>()";
                        l.TypeName = TT.FullName;
                        OnReflectTypeLoadException(null, l);
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    Type TT = typeof(T);
                    throw new Exception(string.Format("Unable to create type:{0}", TT.FullName), ex);
                }
            }
        }

        public static void SetStrategy(IFactoryStrategy Strategy)
        {
            SAHLFactoriesSection Section = ConfigurationManager.GetSection("SAHLFactories") as SAHLFactoriesSection;
            lock (_lockObject)
            {
                _strategy = Strategy;
                if (!_typesLoaded)
                    LoadTypes(Section);// do we want to pass this in?
            }
        }
    }
}