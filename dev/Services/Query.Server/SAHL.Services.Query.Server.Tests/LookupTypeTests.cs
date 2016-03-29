using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.Helper;
using SAHL.Services.Query.Server.Tests.Factory;

namespace SAHL.Services.Query.Server.Tests
{

    [TestFixture]
    public class LookupTypeTests
    {

        private string ValidLookupType
        {
            get { return "generickeytype"; }
        }

        private string InValidLookupType
        {
            get { return "SomeInvalidLookup"; }
        }

        [Test]
        public void IsValidLookup_GivenValidLookup_ShouldReturnTrue()
        {
            
            //arrange
            var lookupTypesHelper = CreateGenericKeyLookupTypeHelper();

            //action
            var isValid = lookupTypesHelper.IsValidLookupType(ValidLookupType);

            //assert
            Assert.IsTrue(isValid);

        }

        [Test]
        public void IsValidLookup_GivenInvalidLookup_ShouldReturnFalse()
        {
            //arrange
            LookupTypesHelper lookupTypesHelper = CreateSomeKeyLookupTypeHelper();

            //action
            var isValid = lookupTypesHelper.IsValidLookupType(InValidLookupType);

            //assert
            Assert.IsFalse(isValid);

        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void FindLookupMetaData_GivenInvalidLookupType_ShouldThrowAnArgumentException()
        {
            //arrange
            LookupTypesHelper lookupTypesHelper = CreateSomeKeyLookupTypeHelper();

            //action
            lookupTypesHelper.FindLookupMetaData(InValidLookupType);

        }

        [Test]
        public void FindLookupMetaData_GivenValidLookupType_ShouldReturnMetaData()
        {

            //arrange
            var lookupTypesHelper = CreateGenericKeyLookupTypeHelper();

            //action
            var lookupMetaData = lookupTypesHelper.FindLookupMetaData(ValidLookupType);
            
            //assert

            ILookupMetaDataModel metaDataModel = MetaDataFactory.GetGenericKeyMetaDataModel();

            Assert.AreEqual(lookupMetaData.Db, metaDataModel.Db);
            Assert.AreEqual(lookupMetaData.Schema, metaDataModel.Schema);
            Assert.AreEqual(lookupMetaData.LookupTable, metaDataModel.LookupTable);
            Assert.AreEqual(lookupMetaData.LookupType, metaDataModel.LookupType);
            Assert.AreEqual(lookupMetaData.LookupKey, metaDataModel.LookupKey);
            Assert.AreEqual(lookupMetaData.LookupDescription, metaDataModel.LookupDescription);

        }

        private static LookupTypesHelper CreateGenericKeyLookupTypeHelper()
        {
            ISupportedLookupDataManager supportedLookupDataManager = new SupportedLookupDataManager(new FakeDbFactory());
            LookupTypesHelper lookupTypesHelper = new LookupTypesHelper(supportedLookupDataManager);
            lookupTypesHelper.ValidLookupTypes.Add("generickeytype", MetaDataFactory.GetGenericKeySupportedLookup());
            return lookupTypesHelper;
        }
        
        private static LookupTypesHelper CreateSomeKeyLookupTypeHelper()
        {
            ISupportedLookupDataManager supportedLookupDataManager = new SupportedLookupDataManager(new FakeDbFactory());
            LookupTypesHelper lookupTypesHelper = new LookupTypesHelper(supportedLookupDataManager);
            return lookupTypesHelper;
        }
    }
}