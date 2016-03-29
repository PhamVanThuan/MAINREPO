using NUnit.Framework;
using SAHL.Core.Services;
using SAHL.X2Engine2.Tests;
using StructureMap;
using System;
using System.Linq;
using SAHL.X2Engine2.Tests.ConventionTests;

namespace SAHL.X2Engine2.Tests.CommandHandlerSpecs
{
    [TestFixture]
	public class can_instantiate_via_dependency_resolver
	{
        [Ignore]
		[Test, TestCaseSource(typeof(CommandAndHandlerTestCaseSource), "GetCommands")]
		public void commands_should_have_an_associated_command_handler_that_ends_in_command_handler(Type commandType)
		{
            try 
            {
                SpecificationIoCBootstrapper.Initialize();
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Unable to get Type: {0} from IOC, Error:{1} Stack: {2}", commandType.ToString(), e.Message, e.StackTrace));
            }

            var serviceCommandHandlerProvider = default(IServiceCommandHandlerProvider);
            try
            {
                serviceCommandHandlerProvider = ObjectFactory.GetInstance<IServiceCommandHandlerProvider>();
            }
            catch(Exception e)
            {
                throw new Exception(String.Format("Unable to get Type: {0} from IOC, Error:{1} Stack: {2}", commandType.ToString(),e.Message ,e.StackTrace));
            }
			var getCommandHandlerMethod = serviceCommandHandlerProvider.GetType().GetMethods().Where(x=>x.Name == "GetCommandHandler" && x.GetGenericArguments() != null).First();
			var genericCommandHandlerMethod = getCommandHandlerMethod.MakeGenericMethod(commandType);
			try
			{
				var commandHandler = genericCommandHandlerMethod.Invoke(serviceCommandHandlerProvider, null);
			}
			catch (Exception ex)
			{
				throw new Exception(String.Format("The {0}'s command handler could not be retrieved from the IoC. It failed with the message {1}", commandType.Name, ex.InnerException.Message));
			}
		}
	}
}