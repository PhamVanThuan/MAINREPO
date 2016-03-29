using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.UnlockInstanceCommandHandlerSpecs
{
	public class when_unlocking_an_instance_for_a_user : WithFakes
	{
		private static AutoMocker<UnlockInstanceCommandHandler> automocker = new NSubstituteAutoMocker<UnlockInstanceCommandHandler>();
		private static UnlockInstanceCommand command;
		private static long instanceID;
		private static InstanceDataModel instanceDataModel;
		private static IReadWriteSqlRepository readWriteSqlRepository;
        static ServiceRequestMetadata metadata;

		private Establish context = () =>
		{
			instanceID = 5;
			instanceDataModel = new InstanceDataModel(instanceID, 1, null, "name", "subject", string.Empty, 1, "someone", DateTime.Now, null, null, null, null, null, null, null, null);
			command = new UnlockInstanceCommand(instanceID);

			var dictionary = new Dictionary<string, object>();
			dictionary.Add("PrimaryKey", command.InstanceID);

            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
			readWriteSqlRepository.WhenToldTo(x => x.SelectOne<InstanceDataModel>(Param.IsAny<String>(), Arg.Is<object>(anonymousObject => anonymousObject.CheckValue(dictionary)))).Return(instanceDataModel);
		};

		private Because of = () =>
		{
            automocker.ClassUnderTest.HandleCommand(command, metadata);
		};

		private It should_empty_out_the_user_on_the_instance = () =>
		{
			instanceDataModel.ActivityADUserName.ShouldBeNull();
		};

		private It should_empty_out_the_activityID = () =>
		{
			instanceDataModel.ActivityID.ShouldBeNull();
		};

		private It should_empty_out_the_activity_date = () =>
		{
			instanceDataModel.ActivityDate.ShouldBeNull();
		};

		private It should_update_the_database_with_the_updated_instance = () =>
		{
            readWriteSqlRepository.WasToldTo(x => x.Update<InstanceDataModel>(Arg.Is<UnlockInstanceSqlStatement>(y => y.InstanceId == command.InstanceID)));
		};
	}
}