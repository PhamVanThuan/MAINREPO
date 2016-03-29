using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.X2Engine2.Tests.ConventionTests
{
	public class CommandAndHandlerTestCaseSource
	{
		public static IEnumerable<Type> GetCommands()
		{
			var commandTypesToExclude = new Type[]{
				typeof(IRuleCommand),
				typeof(RuleCommand), typeof(HandleSystemRequestBaseCommand)
			};
            var types = GetTypes(Assembly.GetAssembly(typeof(ClearWorkListCommandHandler)), typeof(IServiceCommand))
				.Except(commandTypesToExclude);
            return types;
		}

		public static IEnumerable<Type> GetCommandHandlers()
		{
            var types = GetTypes(Assembly.GetAssembly(typeof(ClearWorkListCommandHandler)), typeof(IServiceCommandHandler<>));
            return types;
		}

		private static IList<Type> GetTypes(Assembly assembly, Type interfaceType)
		{
			return assembly.GetTypes().Where(x =>
								!x.IsInterface &&
								!x.IsGenericType &&
								!x.IsAbstract &&
								x.GetInterfaces().Where(i => i.Name == interfaceType.Name).Count() > 0
							  ).ToList();
		}
	}
}