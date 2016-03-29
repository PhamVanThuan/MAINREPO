using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;
using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Tests
{
    [TestFixture]
    public class PropertyFillTests
    {

        private IAssemblyResolver assemblyResolver;

        string directoryName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"SAHL.Tools.DomainServiceDocumenter.Lib.Test.Data.dll");
        
        [Test]
        public void GetProprtyValidation_GivenTheIdProperty_ShouldGiveRequiredValidation()
        {

            //Assign    
            TypeDefinition testModel = GetModel(directoryName, "TestModel");
            PropertyDefinition propertyDefinition = GetPropertyFromModel(testModel, "Id");
            Property property = new Property();

            //Action
            PropertyUtility.GetPropertyValidation(propertyDefinition, property); 

            //Assert
            Assert.AreEqual("Required", property.Validations[0].Name);

        }

        [Test]
        public void GetProprtyValidation_GivenTheKeyProperty_ShouldGiveStringLengthValidation()
        {

            //Assign
            TypeDefinition testModel = GetModel(directoryName, "TestModel");
            PropertyDefinition propertyDefinition = GetPropertyFromModel(testModel, "Key");
            Property property = new Property();

            //Action
            PropertyUtility.GetPropertyValidation(propertyDefinition, property); 

            //Assert
            Assert.AreEqual("Length [10]", property.Validations[0].Name);

        }
        
        [Test]
        public void GetProprtyValidation_GivenTheAlternatePhoneNumberProperty_ShouldGiveRegularExpressionValidation()
        {

            //Assign
            TypeDefinition testModel = GetModel(directoryName, "TestModel");
            PropertyDefinition propertyDefinition = GetPropertyFromModel(testModel, "AlternatePhoneNumber");
            Property property = new Property();

            //Action
            PropertyUtility.GetPropertyValidation(propertyDefinition, property); 

            //Assert
            Assert.AreEqual(@"Regular Expression [^[2-9]\d{2}-\d{3}-\d{4}$]", property.Validations[0].Name);

        }

        [Test]
        public void GetProprtyValidation_GivenMinLengthTwoProperty_ShouldGiveMinLengthValidation()
        {

            //Assign
            TypeDefinition testModel = GetModel(directoryName, "TestModel");
            PropertyDefinition propertyDefinition = GetPropertyFromModel(testModel, "MinLengthTwo");
            Property property = new Property();

            //Action
            PropertyUtility.GetPropertyValidation(propertyDefinition, property); 

            //Assert
            Assert.AreEqual(@"Minimum Length [2]", property.Validations[0].Name);

        }

        [Test]
        public void GetProprtyValidation_GivenMaxLengthTwoProperty_ShouldGiveMaxLengthValidation()
        {

            //Assign
            TypeDefinition testModel = GetModel(directoryName, "TestModel");
            PropertyDefinition propertyDefinition = GetPropertyFromModel(testModel, "MaxLengthTwo");
            Property property = new Property();

            //Action
            PropertyUtility.GetPropertyValidation(propertyDefinition, property); 

            //Assert
            Assert.AreEqual(@"Maximum Length [2]", property.Validations[0].Name);

        }

        [Test]
        public void GetProprtyValidation_GivenThePhoneNumberProperty_ShouldGiveCustomValidation()
        {

            //Assign
            TypeDefinition testModel = GetModel(directoryName, "TestModel");
            PropertyDefinition propertyDefinition = GetPropertyFromModel(testModel, "PhoneNumber");
            Property property = new Property();

            //Action
            PropertyUtility.GetPropertyValidation(propertyDefinition, property); 

            //Assert
            Assert.AreEqual("My Phone Number", property.Validations[0].Name);

        }

        [Test]
        public void GetPropertyValidation_GivenCountryPropertyWithCustomValidation_ShouldReturnCustomValidation()
        {

            //Assign
            TypeDefinition testModel = GetModel(directoryName, "TestModel");
            PropertyDefinition propertyDefinition = GetPropertyFromModel(testModel, "Country");
            Property property = new Property();

            //Action
            PropertyUtility.GetPropertyValidation(propertyDefinition, property); 

            //Assert
            Assert.AreEqual("Country [Allow Country : ZA]", property.Validations[0].Name);

        }

        [Test]
        public void GetPropertyValidation_GivenCreditCardNumberProperty_ShouldReturnCreditCardValidation()
        {

            //Assign
            TypeDefinition testModel = GetModel(directoryName, "TestModel");
            PropertyDefinition propertyDefinition = GetPropertyFromModel(testModel, "CreditCardNumber");
            Property property = new Property();

            //Action
            PropertyUtility.GetPropertyValidation(propertyDefinition, property);

            //Assert
            Assert.AreEqual("Credit Card", property.Validations[0].Name);

        }

        [Test]
        public void GetPropertyValidation_GivenFileNameProperty_ShouldReturnFileExtensionsValidation()
        {

            //Assign
            TypeDefinition testModel = GetModel(directoryName, "TestModel");
            PropertyDefinition propertyDefinition = GetPropertyFromModel(testModel, "FileName");
            Property property = new Property();

            //Action
            PropertyUtility.GetPropertyValidation(propertyDefinition, property);

            //Assert
            Assert.AreEqual("File Extensions [Extensions : exe]", property.Validations[0].Name);

        }

        [Test]
        public void GetPropertyValidation_GivenEmailAddressProperty_ShouldReturnEmailValidation()
        {

            //Assign
            TypeDefinition testModel = GetModel(directoryName, "TestModel");
            PropertyDefinition propertyDefinition = GetPropertyFromModel(testModel, "EmailAddress");
            Property property = new Property();

            //Action
            PropertyUtility.GetPropertyValidation(propertyDefinition, property);

            //Assert
            Assert.AreEqual("Email Address", property.Validations[0].Name);

        }

        private TypeDefinition GetModel(string assemblyFileName, string className)
        {
            assemblyResolver = new ScopedAssemblyResolver(Path.GetDirectoryName(assemblyFileName));
            ReaderParameters readerParameters = new ReaderParameters()
            {
                ReadSymbols = true,
                AssemblyResolver = assemblyResolver
            };

            foreach (TypeDefinition type in AssemblyDefinition.ReadAssembly(assemblyFileName, readerParameters).MainModule.GetTypes())
            {
                if (type.Name.Contains(className))
                {
                    return type;
                }
            }

            return null;
        }

        private PropertyDefinition GetPropertyFromModel(TypeDefinition typeToProcess, string propertyToReturn)
        {
            foreach (var property in typeToProcess.Properties)
            {
                if (property.Name.Equals(propertyToReturn))
                {
                    return property;
                }
            }
            return null;
        }

    }
}