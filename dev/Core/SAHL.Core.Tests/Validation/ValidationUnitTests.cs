using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Capitec.Validation.Fakes;
using NUnit.Framework;
using SAHL.Core.Tests.Validation.Fakes;
using SAHL.Core.Tests.Validation.Fakes.DeeplyNested;

namespace SAHL.Core.Tests.Validation
{
    [TestFixture]
    public class ValidationUnitTests
    {
        [Test]
        public void Validate_GivenInvalidCommand_ShouldReturnListOfErrors()
        {
            //Assign
            var fakeValidationCommand = new FakeValidationCommandSingleProperty();
            fakeValidationCommand.Key = null;

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(fakeValidationCommand);

            //Assert
            CollectionAssert.IsNotEmpty(results);
        }

        [Test]
        public void Validate_GivenInvalidCompositeCommandWhereInnerHasNoValue_ShouldReturnEmptyListOfErrors()
        {
            //Assign
            var commandComposite = ValidationFakesHelpers.CreateNewFakeValidationCommand();
            commandComposite.Composite = null;

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandComposite);

            //Assert
            Assert.AreEqual(2, results.Count());
        }

        [Test]
        public void Validate_GivenInvalidCompositeListCommandWhereInnerHasNoValue_ShouldReturnListOfOneError()
        {
            //Assign
            var commandCompositeList = new FakeValidationCommandCompositeList
            {
                Id = "a",
                Key = "b",
                List = new List<FakeValidationCommandSingleProperty>
                {
                    new FakeValidationCommandSingleProperty {Key = "c"},
                    new FakeValidationCommandSingleProperty {Key = ""},
                    new FakeValidationCommandSingleProperty {Key = "f"}
                }
            };

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandCompositeList);

            //Assert
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void Validate_GivenInvalidCompositeListHoldingListOfList_ShouldReturnListOfSingleFailure()
        {
            //Assign
            var commandCompositeList = new FakeValidationCommandCompositeListWithList
            {
                Id = "1",
                Key = "a",
                IdN = 1,
                Composite = new List<FakeValidationCommandCompositeList>
                {
                    new FakeValidationCommandCompositeList
                    {
                        Id = "1",
                        Key = "v",
                        List = new List<FakeValidationCommandSingleProperty>
                        {
                            new FakeValidationCommandSingleProperty
                            {
                                Key = ""
                            }
                        }
                    },
                    new FakeValidationCommandCompositeList
                    {
                        Id = "1",
                        Key = "b",
                        List = new List<FakeValidationCommandSingleProperty>
                        {
                            new FakeValidationCommandSingleProperty
                            {
                                Key = "n"
                            }
                        }
                    }
                }
            };

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandCompositeList).ToList();

            //Assert
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void Validate_GivenInvalidCompositeListOfGenrics_ShouldReturnOneError()
        {
            //Assign
            var commandCompositeList = new FakeValidationCommandCompositeHoldingGeneric
            {
                GenericItems = new List<FakeValidationCommandCompositeGeneric<string>>()
                {
                    new FakeValidationCommandCompositeGeneric<string> {GenericItem = "a"},
                    new FakeValidationCommandCompositeGeneric<string> {GenericItem = ""},
                    new FakeValidationCommandCompositeGeneric<string> {GenericItem = "c"}
                }
            };

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandCompositeList);

            //Assert
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void Validate_GivenInvalidCommandWithMultipleProperties_ShouldReturnListOfTwoErrors()
        {
            //Assign
            var commandMultipleProperty = new FakeValidationCommandMultipleProperty();

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandMultipleProperty);

            //Assert
            Assert.AreEqual(2, results.Count());
        }

        [Test]
        public void Validate_GivenInvalidCompositeCommand_ShouldReturnListofThreeErrors()
        {
            //Assign
            var commandComposite = ValidationFakesHelpers.CreateNewFakeValidationCommand();

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandComposite);

            //Assert
            Assert.AreEqual(3, results.Count());
        }

        [Test]
        public void Validate_GivenInvalidCompositeWithArrayCommand_ShouldReturnListofThreeErrors()
        {
            //Assign
            var commandComposite = new FakeValidationCommandCompositeWithArray
            {
                Id = "a",
                Key = "",
                Composite =
                    new[]
                    {
                        new FakeValidationCommandSingleProperty {Key = ""},
                        new FakeValidationCommandSingleProperty {Key = ""},
                    }
            };

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandComposite);

            //Assert
            Assert.AreEqual(3, results.Count());
        }

        [Test]
        public void Validate_GivenValidCommand_ShouldReturnEmptyListOfErrors()
        {
            //Assign
            var fakeValidationCommand = new FakeValidationCommandSingleProperty();
            fakeValidationCommand.Key = "1";

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(fakeValidationCommand);

            //Assert
            CollectionAssert.IsEmpty(results);
        }

        [Test]
        public void Validate_GivenValidCompositeCommandWhereInnerHasNoValue_ShouldReturnEmptyListOfErrors()
        {
            //Assign
            var commandComposite = ValidationFakesHelpers.CreateNewFakeValidationCommand("a", "b");
            commandComposite.Composite = null;

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandComposite);

            //Assert
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Validate_GivenValidCompositeCommand_ShouldReturnEmptyListOfErrors()
        {
            //Assign
            var commandComposite = ValidationFakesHelpers.CreateNewFakeValidationCommand("a", "b", "c");

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandComposite);

            //Assert
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Validate_GivenValidCompositeListCommandWhereInnerHasValue_ShouldReturnEmptyListOfErrors()
        {
            //Assign
            var commandCompositeList = new FakeValidationCommandCompositeList
            {
                Id = "a",
                Key = "b",
                List = new List<FakeValidationCommandSingleProperty>
                {
                    new FakeValidationCommandSingleProperty {Key = "c"},
                    new FakeValidationCommandSingleProperty {Key = "d"}
                }
            };

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandCompositeList);

            //Assert
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Validate_GivenValidCompositeListOfGenrics_ShouldReturnEmptyList()
        {
            //Assign
            var commandCompositeList = new FakeValidationCommandCompositeHoldingGeneric
            {
                GenericItems = new List<FakeValidationCommandCompositeGeneric<string>>()
                {
                    new FakeValidationCommandCompositeGeneric<string> {GenericItem = "a"},
                    new FakeValidationCommandCompositeGeneric<string> {GenericItem = "b"},
                    new FakeValidationCommandCompositeGeneric<string> {GenericItem = "c"}
                }
            };

            //Action

            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandCompositeList);

            //Assert
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Validate_GivenValidCompositeWithArrayCommand_ShouldReturnEmptyListOfErrors()
        {
            //Assign
            var commandComposite = new FakeValidationCommandCompositeWithArray
            {
                Id = "a",
                Key = "b",
                Composite =
                    new[]
                    {
                        new FakeValidationCommandSingleProperty {Key = "c"},
                        new FakeValidationCommandSingleProperty {Key = "d"},
                    }
            };

            //Action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(commandComposite);

            //Assert
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Validate_GivenValidDictionaryWithPrimitiveValueType_ShouldReturnEmptyList()
        {
            var command = new FakeValidationCommandWithDictionary<string, int>
            {
                Validating = new Dictionary<string, int> { { "1", 1 } }
            };
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Validate_GivenValidDictionaryWithReferenceValueType_ShouldReturnListOfSingleError()
        {
            var command = new FakeValidationCommandWithDictionary<int, FakeValidationCommandSingleProperty>
            {
                Validating = new Dictionary<int, FakeValidationCommandSingleProperty>
                {
                    {
                        1, new FakeValidationCommandSingleProperty {Key = ""}
                    }
                }
            };
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void Validate_GivenValidDictionaryWithValueType_ShouldReturnNoErrors()
        {
            var command = new FakeValidationCommandWithDictionary<int, int>
            {
                Validating = new Dictionary<int, int>
                {
                    {
                        1, 1
                    }
                }
            };
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Validate_GivenMultipleValidationAttributes_ShouldReturnListOfThreeErrors()
        {
            var command = new FakeValidationCommandUniqueValidationProperties();
            command.StringLengthProperty = "ThisStringIsTooLong";

            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            Assert.AreEqual(3, results.Count());
            Assert.IsNotNull(results.SingleOrDefault(a => a.ErrorMessage.Contains("The RequiredProperty field is required")));
            Assert.IsNotNull(results.SingleOrDefault(a => a.ErrorMessage.Contains("The field RangeProperty must be between")));
            Assert.IsNotNull(results.SingleOrDefault(a => a.ErrorMessage.Contains("The field StringLengthProperty must be a string with a maximum length of")));
        }

        [Test]
        public void Validate_GivenMultipleValidationAttributes_ShouldReturnContainingObjectInErrorMessage()
        {
            var command = new FakeValidationCommandSingleProperty();

            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            Assert.AreEqual(1, results.Count());
            Assert.IsTrue(results.Single().ErrorMessage.Contains("{in: root}"));
        }

        [Test]
        public void Validate_GivenDeepNestedObjects_ShouldReturnContainingObjectInErrorMessages()
        {
            var command = FakeValidationCompositeDeeplyNested.Create();

            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            var errorMessages = results.Select(a => a.ErrorMessage).ToList();

            Assert.AreEqual(5, results.Count());
            Assert.IsTrue(errorMessages.Any(a => a.Equals("The Id field is required. {in: root}", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(errorMessages.Any(a => a.Equals("The Id1 field is required. {in: Node1}", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(errorMessages.Any(a => a.Equals("The Id2 field is required. {in: Node2}", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(errorMessages.Any(a => a.Equals("The Id3 field is required. {in: Node3}", StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(errorMessages.Any(a => a.Equals("The Id4 field is required. {in: Node4}", StringComparison.OrdinalIgnoreCase)));
        }

        [Test]
        public void Validate_GivenCompositeWithList_ShouldReturnContainingObjectInErrorMessages()
        {
            var command = new FakeValidationCommandCompositeList();
            command.Id = "Id";
            command.Key = "Key";

            command.List = new List<FakeValidationCommandSingleProperty>
            {
                new FakeValidationCommandSingleProperty(),
                new FakeValidationCommandSingleProperty()
            };

            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            var errorMessages = results.Select(a => a.ErrorMessage).ToList();

            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(errorMessages.All(a => a.Equals("The Key field is required. {in: List}", StringComparison.OrdinalIgnoreCase)));
        }

        [Test]
        public void Validate_GivenListOfObject_ShouldReturnListOfTwoErrorMessages()
        {
            var command = new FakeValidationCommandCompositeListOfObject();

            command.List = new List<object>
            {
                new FakeValidationCommandSingleProperty(),
                new FakeValidationCommandSingleProperty(),
            };

            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            Assert.AreEqual(2, results.Count());
            var firstMessage = results.First().ErrorMessage;
            Assert.IsTrue(firstMessage.Contains("Key"));
            Assert.IsTrue(firstMessage.Contains("is required"));
            Assert.IsTrue(firstMessage.Contains("List"));

            var secondMessage = results.Last().ErrorMessage;
            Assert.IsTrue(secondMessage.Contains("Key"));
            Assert.IsTrue(secondMessage.Contains("is required"));
            Assert.IsTrue(secondMessage.Contains("List"));
        }

        [Test]
        public void Validate_GivenGlobalNamespaceObjectAsRoot_ShouldReturnListOfSingleErrorMessage()
        {
            var command = new FakeCommandWithGlobalNamespace();

            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void Validate_GivenGlobalNamespaceObjectAsNonRoot_ShouldReturnEmptyListWithErrorMessages()
        {
            var command = new FakeCommandWithGlobalNamespaceComposite
            {
                Item = new FakeCommandWithGlobalNamespace(),
            };

            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Validate_GivenCapitecNamespaceObject_ShouldReturnListWithSingleErrorMessage()
        {
            var command = new CapitecFakeCommandComposite
            {
                Item = new CapitecFakeCommandSingleProperty(),
            };

            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void Validate_GivenObjectThatInheritsFromDictionary_ShouldReturnListWithNoErrorMessage()
        {

            //arrange
            var command = new FakeValidationCommandWithSubTypeOfDictionary();
            command.Key = "SomeKey";
            command.MetaData = new ObjectWhichInheritsFromDictionary();

            //action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            //assert
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Validate_GivenObjectThatInheritsFromDictionaryWithNullDictionary_ShouldReturnListWithOneErrorMessage()
        {
            //arrange
            var command = new FakeValidationCommandWithSubTypeOfDictionary();
            command.Key = "SomeKey";

            //action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            //assert
            Assert.AreEqual(1, results.Count());

            var message = results.First().ErrorMessage;
            Assert.IsTrue(message.Contains("MetaData"));
            Assert.IsTrue(message.Contains("is required"));
            Assert.IsTrue(message.Contains("root"));
        }

        [Test]
        public void Validate_GivenObjectThatInheritsFromDictionaryWithCustomPropertyAndRequiredAttributeAndNoElements_ShouldReturnListWithNoErrorMessage()
        {
            //arrange
            var command = new FakeValidationCommandWithSubTypeOfDictionary();
            command.Key = "SomeKey";
            command.MetaData = new ObjectWhichInheritsFromDictionaryWithRequiredAttribute();

            //action
            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            //assert
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Validate_GivenObjectThatInheritsFromDictionaryWithValueTypeThatContainsARequiredField_ShouldReturnListWithTwoErrorMessages()
        {
            var command = new FakeValidationCommandWithSubTypeOfDictionaryWithReferenceValue();
            command.Key = "SomeKey";
            command.MetaData = new ObjectWhichInheritsFromDictionaryWithReferenceValueWithRequiredAttribute();
            command.MetaData.Add("SomeItem1", new FakeValidationCommandSingleProperty());
            command.MetaData.Add("SomeItem2", new FakeValidationCommandSingleProperty { Key = "SomeKey2" });
            command.MetaData.Add("SomeItem3", new FakeValidationCommandSingleProperty());

            var results = ValidationFakesHelpers.CreateValidateCommand().Validate(command);

            Assert.AreEqual(2, results.Count());

            var firstMessage = results.First().ErrorMessage;
            Assert.IsTrue(firstMessage.Contains("Key"));
            Assert.IsTrue(firstMessage.Contains("is required"));
            Assert.IsTrue(firstMessage.Contains("MetaData"));

            var secondMessage = results.Last().ErrorMessage;
            Assert.IsTrue(secondMessage.Contains("Key"));
            Assert.IsTrue(secondMessage.Contains("is required"));
            Assert.IsTrue(secondMessage.Contains("MetaData"));
        }
    }
}