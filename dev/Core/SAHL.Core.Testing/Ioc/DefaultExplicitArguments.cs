using System.Reflection;
using SAHL.Core.Testing.Ioc;
using SAHL.Core.Testing.Providers;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Linq;
using System.Collections.Generic;
using SAHL.Core.Testing.FileConventions;
using StructureMap.Configuration.DSL;
using System.IO;
using System.Diagnostics;
using StructureMap.Pipeline;

namespace SAHL.Core.Testing.Ioc
{
    public sealed class DefaultExplicitArguments
    {

        private Dictionary<Type, Action<ConfiguredInstance, string>> configuredInstances; 
        public DefaultExplicitArguments()
        {
            PopulateDefaultTypeActions();
        }

        private void PopulateDefaultTypeActions()
        {
            configuredInstances = new Dictionary<Type, Action<ConfiguredInstance, string>>();
            configuredInstances.Add(typeof(Guid), SetGuid);
            configuredInstances.Add(typeof(int[]), SetIntArray);
            configuredInstances.Add(typeof(String), SetString);
            configuredInstances.Add(typeof(Boolean), SetBoolean);
            configuredInstances.Add(typeof(int?), SetNullableInteger);
            configuredInstances.Add(typeof(Int32), SetInteger);
            configuredInstances.Add(typeof(Int64), SetLong);
            configuredInstances.Add(typeof(DateTime?), SetDate);
            configuredInstances.Add(typeof(DateTime), SetDate);
            configuredInstances.Add(typeof(Decimal), SetDecimal);
            configuredInstances.Add(typeof(Double), SetDouble);
            configuredInstances.Add(typeof(Double?), SetDouble);
        }

        

        internal static ITestingIoc Container { get; set; }
        public void Configure(ConfiguredInstance configuredInstance, Type type)
        {

            var ctors = type.GetConstructors().Where(x => x.GetParameters().Count() > 0);
            if (ctors.Count() == 0)
            {
                return;
            }

            var parameters = ctors.FirstOrDefault().GetParameters();
            foreach (var par in parameters)
            {

                if (IsTypeSupported(par))
                {
                    SetConstructorValue(configuredInstance, par);
                }

                if (par.ParameterType.IsEnum)
                {
                    var enums = (int[])Enum.GetValues(par.ParameterType);
                    configuredInstance.CtorDependency<Enum>(par.Name).Is(enums[0]);
                }

                if (par.ParameterType.Namespace.StartsWith("SAHL.") && par.ParameterType.IsClass)
                {
                    Container.Configure(x =>
                    {
                        var configuredInstanceSAHL = x.For(par.ParameterType).Use(par.ParameterType);
                        this.Configure(configuredInstanceSAHL, par.ParameterType);
                    });
                    configuredInstance.CtorDependency<int?>(par.Name).Is(Container.GetInstance(par.ParameterType));
                }
            }
        }

        private void SetConstructorValue(ConfiguredInstance configuredInstance, ParameterInfo par)
        {
            Action<ConfiguredInstance, string> setValue = configuredInstances[par.ParameterType];
            setValue(configuredInstance, par.Name);
        }

        private bool IsTypeSupported(ParameterInfo par)
        {
            return configuredInstances.ContainsKey(par.ParameterType);
        }

        private void SetGuid(ConfiguredInstance configuredInstance, string parameterName)
        {
            configuredInstance.CtorDependency<Guid>(parameterName).Is(Guid.Parse("9B70CEF1-45FF-4FE8-BDC5-4F3B28FF45FA"));
        }
        
        private void SetString(ConfiguredInstance configuredInstance, string parameterName)
        {
            configuredInstance.CtorDependency<String>(parameterName).Is("str");
        }

        private void SetBoolean(ConfiguredInstance configuredInstance, string parameterName)
        {
            configuredInstance.CtorDependency<Boolean>(parameterName).Is(true);
        }

        private void SetNullableInteger(ConfiguredInstance configuredInstance, string parameterName)
        {
            configuredInstance.CtorDependency<int?>(parameterName).Is(1);
        }

        private void SetInteger(ConfiguredInstance configuredInstance, string parameterName)
        {
            configuredInstance.CtorDependency<Int32>(parameterName).Is(1);
        }

        private void SetLong(ConfiguredInstance configuredInstance, string parameterName)
        {
            configuredInstance.CtorDependency<Int64>(parameterName).Is(Int64.MaxValue);
        }

        private void SetDate(ConfiguredInstance configuredInstance, string parameterName)
        {
            configuredInstance.CtorDependency<DateTime>(parameterName).Is(DateTime.Now);
        }

        private void SetDecimal(ConfiguredInstance configuredInstance, string parameterName)
        {
            configuredInstance.CtorDependency<Decimal>(parameterName).Is(1M);
        }

        private void SetDouble(ConfiguredInstance configuredInstance, string parameterName)
        {
            configuredInstance.CtorDependency<Double>(parameterName).Is(1D);
        }
        private void SetIntArray(ConfiguredInstance configuredInstance, string parameterName)
        {
            configuredInstance.CtorDependency<int[]>(parameterName).Is(new int[] {1,2});
        }
    }
}