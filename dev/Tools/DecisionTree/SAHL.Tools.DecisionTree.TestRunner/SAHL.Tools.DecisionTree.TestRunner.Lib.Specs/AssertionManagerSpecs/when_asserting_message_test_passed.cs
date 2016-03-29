using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions;
using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.AssertionManagerSpecs
{
    public class when_asserting_message_test_passed : WithFakes
    {
        private static AssertionManagerTestFactory testFactory;
        private static IAssertionFactory assertionFactory;
        private static AssertionManager manager;

        private static OutputMessage expected;
        private static List<ISystemMessage> actualMessages;
        private static string assertion;

        private static TestMessageResult result;
        private static CollectionAssertion fakeAssertion;

        private Establish context = () =>
        {
            testFactory = new AssertionManagerTestFactory();
            manager = testFactory.AssertionManager;
            assertionFactory = testFactory.AssertionFactory;

            expected = new OutputMessage("You need more money", "to contain", "Warning Messages");
            actualMessages = new List<ISystemMessage> { new SystemMessage("You need more money", SystemMessageSeverityEnum.Warning) };

            fakeAssertion = An<CollectionAssertion>();
            assertionFactory.WhenToldTo(x => x.GetCollectionAssertion("to contain")).Return(fakeAssertion);
            fakeAssertion.WhenToldTo(x => x.Assert<string>(Param.IsAny<IEnumerable<string>>(), "You need more money")).Return(true);
        };

        private Because of = () =>
        {
            result = manager.AssertMessageTestPassed(actualMessages, expected);
        };

        private It should_create_a_collection_assertion_for_the_expectation = () =>
        {
            assertionFactory.WasToldTo(x => x.GetCollectionAssertion("to contain"));
        };

        private It should_call_assert_on_the_returned_assertion_with_the_messages = () =>
        {
            fakeAssertion.WasToldTo(x => x.Assert(Param.IsAny<List<string>>(), "You need more money"));
        };

        private It should_return_the_assertion_result = () =>
        {
            result.Passed.ShouldBeTrue();
            result.Name.ShouldEqual("Warning Messages");
            result.ActualMessages.First(x => x.Message == "You need more money").ShouldNotBeNull();
            result.Expected.ShouldEqual("You need more money");
        };
    }
}