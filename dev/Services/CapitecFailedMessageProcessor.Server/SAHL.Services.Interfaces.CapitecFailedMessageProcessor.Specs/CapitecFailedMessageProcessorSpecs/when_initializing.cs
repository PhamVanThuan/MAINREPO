using Machine.Fakes;
using Machine.Specifications;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CapitecFailedMessageProcessor.CapitecFailedMessageProcessorSpecs
{
	public class when_initializing : WithFakes
	{
		private static NSubstituteAutoMocker<CapitecFailedMessageProcessor> mock;
		Establish context = () =>
		{
			mock = new NSubstituteAutoMocker<CapitecFailedMessageProcessor>();
		};

		Because of = () =>
		{
			mock.ClassUnderTest.Initialize();
		};

		It should_start_a_timer_to_pick_up_failed_messages = () =>
		{
			mock.Get<ITimerFactory>().WasToldTo(x => x.Get(mock.ClassUnderTest.ProcessFailedMessages));
		};
	}
}
