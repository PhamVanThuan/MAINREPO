using System.Collections.Generic;
using NUnit.Framework;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Builders.Core;
using SAHL.Services.Query.Parsers.Elemets;
using SAHL.Services.Query.Validators;

namespace SAHL.Services.Query.Server.Tests.Validators
{
    public class FindQueryValidatorTests
    {

        private IFindQueryValidator FindQueryValidator;

        [SetUp]
        public void SetupFixture()
        {
            FindQueryValidator = new FindQueryValidator();
        }
       
        [Test]
        public void IsValid_GivenFindQueryWithPagingSetButNoOrderBy_ShouldReturnIsValidSetToFalseAndSetErrorOnList()
        {
            
            //arrange
            IFindQuery findQuery = new FindManyQuery();
            CreatePagedPart(findQuery);

            findQuery.OrderBy = new List<IOrderPart>();

            //action
            FindQueryValidator.IsValid(findQuery);
            
            //assert
            Assert.IsFalse(findQuery.IsValid);
            Assert.AreEqual(1, findQuery.Errors.Count);

        }

        private static void CreatePagedPart(IFindQuery findQuery)
        {
            findQuery.PagedPart = new PagedPart()
            {
                CurrentPage = 0,
                PageSize = 10,
            };
        }
    }
}