﻿using Machine.Fakes;
using Machine.Specifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Messaging;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Models.Shared;
using SAHL.Services.Capitec.Models.Shared.Capitec;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;

namespace SAHL.Services.Interfaces.CapitecFailedMessageProcessor.Specs.CapitecFailedMessageProcessorSpecs
{
    public class when_there_are_messages : WithFakes
    {
        private static NSubstituteAutoMocker<CapitecFailedMessageProcessor> mock;
        private static List<PublishMessageFailureDataModel> failedMessages;
        private static SwitchLoanApplication switchLoanApplication = Stubs.SwitchLoanApplicationRequest();
        private static NewPurchaseApplication newPurchaseApplication = Stubs.NewPurchaseApplicationRequest();
        private static ITimer timer;
        private static IReadWriteSqlRepository readWriteRepository;
        private static Guid switchLoanApplicationFailedMessageKey;
        private static Guid newPurchaseApplicationFailedMessageKey;

        private Establish context = () =>
        {
            failedMessages = new List<PublishMessageFailureDataModel>();
            var newPurchaseApplicationFailedMessage = new PublishMessageFailureDataModel(Serialize(newPurchaseApplication), DateTime.Now);
            var switchLoanApplicationFailedMessage = new PublishMessageFailureDataModel(Serialize(switchLoanApplication), DateTime.Now);
            newPurchaseApplicationFailedMessageKey = newPurchaseApplicationFailedMessage.Id;
            switchLoanApplicationFailedMessageKey = switchLoanApplicationFailedMessage.Id;
            failedMessages.Add(new PublishMessageFailureDataModel(Serialize(newPurchaseApplication), DateTime.Now));
            failedMessages.Add(new PublishMessageFailureDataModel(Serialize(switchLoanApplication), DateTime.Now));
            readWriteRepository = MockRepositoryProvider.GetReadWriteRepository();
            timer = An<ITimer>();

            mock = new NSubstituteAutoMocker<CapitecFailedMessageProcessor>();
            mock.Get<ITimerFactory>().WhenToldTo(x => x.Get(Param.IsAny<Action<object>>())).Return(timer);
            mock.Inject<ISerializationProvider>(new JsonSerializationProvider());
            readWriteRepository.WhenToldTo(x => x.Select<PublishMessageFailureDataModel>("select * from [Capitec].dbo.PublishMessageFailure", Param.IsAny<object>())).Return(failedMessages);

            mock.ClassUnderTest.Initialize();
        };

        private static string Serialize(object objectToSerialize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(objectToSerialize, new Newtonsoft.Json.JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                },
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
        }

        private Because of = () =>
        {
            mock.ClassUnderTest.ProcessFailedMessages(null);
        };

        private It should_suspend_the_timer = () =>
        {
            timer.WasToldTo(x => x.Stop());
        };

        private It should_publish_the_failed_messages = () =>
        {
            mock.Get<IMessageBus>().WasToldTo(x => x.Publish<CreateCapitecApplicationRequest>(Arg.Is<CreateCapitecApplicationRequest>(request => request.CapitecApplication.ReservedApplicationKey == newPurchaseApplication.ReservedApplicationKey)));
            mock.Get<IMessageBus>().WasToldTo(x => x.Publish<CreateCapitecApplicationRequest>(Arg.Is<CreateCapitecApplicationRequest>(request => request.CapitecApplication.ReservedApplicationKey == switchLoanApplication.ReservedApplicationKey)));
        };

        private It should_start_another_timer = () =>
        {
            mock.Get<ITimerFactory>().WasToldTo(x => x.Get(Param.IsAny<Action<object>>())).Twice();
        };

        private It should_remove_the_message_after_a_successful_publish = () =>
        {
            readWriteRepository.WasToldTo(x => x.DeleteByKey<PublishMessageFailureDataModel, Guid>(newPurchaseApplicationFailedMessageKey));
            readWriteRepository.WasToldTo(x => x.DeleteByKey<PublishMessageFailureDataModel, Guid>(switchLoanApplicationFailedMessageKey));
        };
    }
}