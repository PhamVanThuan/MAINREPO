using NUnit.Framework;
using System;
using System.Linq;
using Machine.Specifications;
using Machine.Fakes;

namespace SAHL.X2Engine2.Tests.ConventionTests
{
    [TestFixture]
	public class when_conforming_to_convention
	{
		private const string CommandPostFix = "Command";
		private const string HandlerPostFix = "Handler";

		[Test, TestCaseSource(typeof(CommandAndHandlerTestCaseSource), "GetCommands")]
		public void commands_should_conform_to_convention_and_end_with_command(Type command)
		{
			command.Name.ShouldEndWith(CommandPostFix);
		}

		[Test, TestCaseSource(typeof(CommandAndHandlerTestCaseSource), "GetCommandHandlers")]
		public void command_handlers_should_conform_to_convention_and_end_with_command_handler(Type commandHandler)
		{
			commandHandler.Name.ShouldEndWith(CommandPostFix + HandlerPostFix);
		}

		[Test, TestCaseSource(typeof(CommandAndHandlerTestCaseSource), "GetCommands")]
		public void commands_should_have_an_associated_command_handler_that_ends_in_command_handler(Type command)
		{
			var commandHasAssociativeHandler = CommandAndHandlerTestCaseSource.GetCommandHandlers().Any(x => x.Name == command.Name + HandlerPostFix);
			Assert.IsTrue(commandHasAssociativeHandler, String.Format("The command {0} does not have a handler named {1}", command.Name, command.Name + HandlerPostFix));
		}
	}
}