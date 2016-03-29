using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Core.X2.Exceptions;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.CheckActivityIsValidForStateCommandHandlerSpecs
{
	public class when_checking_if_a_state_is_valid_for_a_state_and_activity_and_it_is_not_valid : WithFakes
	{
		private static AutoMocker<CheckActivityIsValidForStateCommandHandler> autoMocker;
		private static IReadWriteSqlRepository readWriteSqlRepository;
		private static CheckActivityIsValidForStateCommand command;
		private static InstanceDataModel instance;
		private static Activity activity;
		private static System.Exception exception;
        static ServiceRequestMetadata metadata;

		private Establish context = () =>
			{
				readWriteSqlRepository = An<IReadWriteSqlRepository>();
				instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 9, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
				activity = new Activity(1, "Move On", 1, "State A", 2, "State B", 1, false);
                readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
				command = new CheckActivityIsValidForStateCommand(instance, activity.StateId);
				autoMocker = new NSubstituteAutoMocker<CheckActivityIsValidForStateCommandHandler>();
			};

		private Because of = () =>
			{
				exception = Catch.Exception(() =>
					{
                        autoMocker.ClassUnderTest.HandleCommand(command, metadata);
					});
			};

		private It should_check_that_the_from_state_for_the_activity_being_performed_matches_the_current_state_of_the_instance = () =>
			{
			};

		private It should_throw_an_exception = () =>
			{
				exception.ShouldBe(typeof(RuleCommandException));
			};
	}
}