using NUnit.Framework;
using SAHL.Services.Query.Mappers;

namespace SAHL.Services.Query.Server.Tests.Mapper
{
    [TestFixture]
    public class OperatorMapperTests
    {
        [Test]
        public void MapOperator_GivenEmptyString_ShouldDefaultEquals()
        {
            //arrange
           
            //action
            string operation = OperatorMapper.MapOperator("");

            //assert
            Assert.AreEqual("=", operation);

        }

        [TestCase ("eq", "=")]
        [TestCase ("gt", ">")]
        [TestCase ("gte", ">=")]
        [TestCase ("lt", "<")]
        [TestCase ("lte", "<=")]
        [TestCase ("like", "like")]
        [TestCase ("in", "in")]
        [TestCase ("inq", "not in")]
        [TestCase ("startswith", "like")]
        [TestCase ("endswith", "like")]
        [TestCase ("contains", "like")]
        public void MapOperator_GivenOperatorString_ShouldAssociatedQueryOperator(string operation, string queryOperation)
        {
            //arrange

            //action
            string mappedItem = OperatorMapper.MapOperator(operation);

            //assert
            Assert.AreEqual(queryOperation, mappedItem);

        }

        [TestCase("test", false)]
        [TestCase("eq", true)]
        [TestCase("gt", true)]
        [TestCase("gte", true)]
        [TestCase("lt", true)]
        [TestCase("lte", true)]
        [TestCase("like", true)]
        [TestCase("in", true)]
        [TestCase("inq", true)]
        [TestCase("startswith", true)]
        [TestCase("endswith", true)]
        [TestCase("contains", true)]
        public void MapOperator_GivenOperatorString_ShouldReturnTrueIfItExistsAsAnOperator(string operation, bool exists)
        {
            //arrange

            //action
            bool isOperator = OperatorMapper.IsOperator(operation);

            //assert
            Assert.AreEqual(exists, isOperator);

        }

    }
}