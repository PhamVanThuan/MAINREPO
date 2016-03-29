using System;
using System.Data;
using NUnit.Framework;
using SAHL.Services.Query.Helper;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Core.Testings
{

    [TestFixture]
    public class LookupMetaDataTests
    {

        [Test]
        public void GetLookupMetaData_GivenValidLookupMetaDataString_ShouldSetTheLookupMetaDataObjectCorrectly()
        {
            
            //arrange
            string metaData = "LookupType,Db,Schema,Table,Key,Description";

            //action
            MetaDataHelper metaDataHelper = new MetaDataHelper();
            LookupMetaDataModel lookupMetaDataModel = metaDataHelper.GenerateMetaDataLookup(metaData);

            //assert
            Assert.AreEqual(lookupMetaDataModel.LookupType, "LookupType");
            Assert.AreEqual(lookupMetaDataModel.Db, "Db");
            Assert.AreEqual(lookupMetaDataModel.Schema, "Schema");
            Assert.AreEqual(lookupMetaDataModel.LookupTable, "Table");
            Assert.AreEqual(lookupMetaDataModel.LookupKey, "Key");
            Assert.AreEqual(lookupMetaDataModel.LookupDescription, "Description");

        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetLookupMetaData_GivenInvalidMetaDataLength_ShouldRaiseAnException()
        {
            MetaDataHelper metaDataHelper = new MetaDataHelper();
            metaDataHelper.GenerateMetaDataLookup("");
        }
    }
}
