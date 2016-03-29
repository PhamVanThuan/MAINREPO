using Machine.Fakes;
using Machine.Specifications;
using System;

namespace SAHL.Core.Data.Specs.LinkedKeySpecs
{
    public class when_retrieving_a_linked_key_given_an_empty_guid : WithFakes
    {
        private static LinkedKeyManager linkedKeyManager;
        private static ILinkedKeyDataManager linkedKeyDataManager;
        private static int key;
        private static Guid guid;
        private static Exception exception;

        private Establish context = () =>
        {
            linkedKeyDataManager = An<ILinkedKeyDataManager>();
            linkedKeyManager = new LinkedKeyManager(linkedKeyDataManager);
            guid = Guid.Empty;
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => { key = linkedKeyManager.RetrieveLinkedKey(guid); });
        };

        private It should_throw_an_argument_exception = () =>
        {
            exception.ShouldBeOfType<ArgumentException>();
        };

        private It should_specify_the_invalid_argument = () =>
        {
            ((ArgumentException)exception).ParamName.ShouldEqual("guid");
        };
    }
}