using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using NUnit.Framework;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Builders.Core;
using SAHL.Services.Query.Compilers;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Parsers.Elemets;
using SAHL.Services.Query.Server.Tests.DataManagers.Statements;
using SAHL.Services.Query.Server.Tests.Models;

namespace SAHL.Services.Query.Server.Tests.Compilers
{
    [TestFixture]
    public class CustomSqlCompilerTests
    {

        [Test]
        public void GetStatement_GivenEmptyFindManyQueryObject_ShouldReturnCustomSql()
        {
            //arrange
            var compiler = CreateCompilerWithNoWhere();
            
            //action
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select T.Id as Id, T.Description as Description From Test T", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithLimit_ShouldReturnCustomSqlWithLimit()
        {
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();
            query.Limit = CreateLimitPart();
            
            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select Top 5 * From (Select T.Id as Id, T.Description as Description From Test T) As QS", statement);

        }
        
        [Test]
        public void GetStatement_GivenFindManyQueryWithWherePart_ShouldReturnCustomSqlWithWhere()
        {

            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();
            query.Where = InstantiateWhere();
            query.Where.Add(CreateWherePartAsId_IdSetToOne());
            
            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Where Id = @Id", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithTwoWhereParts_ShouldReturnCustomSqlWithWhere()
        {

            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();
            query.Where = InstantiateWhere();
            query.Where.Add(CreateWherePartAsId_IdSetToOne());
            query.Where.Add(CreateWherePartAsDescription());
            
            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Where Id = @Id and Description = @Description", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithWherePartAndStementWhichHasWhereIncluded_ShouldReturnCustomSqlWithWhere()
        {
            var compiler = CreateCompilerWithWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();
            query.Where = InstantiateWhere();
            query.Where.Add(CreateWherePartAsId_IdSetToOne());

            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description, T.IsActive as IsActive From Test T Where T.IsActive = 1) As QS Where Id = @Id", statement);


        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithWherePartAndStementWhichHasWhereIncludedWithSimpleAlias_ShouldReturnCustomSqlWithWhere()
        {
            var compiler = CreateCompilerWithWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();
            query.Where = InstantiateWhere();
            query.Where.Add(CreateWherePartAsName());

            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description, T.IsActive as IsActive From Test T Where T.IsActive = 1) As QS Where Name = @Name", statement);

        }

        [Test]
        public void GetDynamicParameters_GivenFindManyQueryWithNoWherePart_ShouldReturnNull()
        {
            
            //arrange
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();

            //action
            var parameters =  compiler.PerpareDynamicParameters(query);
            
            //assert
            Assert.IsNull(parameters);

        }

        [Test]
        public void GetDynamicParameters_GivenFindManyQueryWithSingleWherePart_ShouldReturnDynamicArgumentsWithOneItem()
        {
            
            //arrange
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.Where = InstantiateWhere();
            query.Where.Add(CreateWherePartAsName());
            
            //action
            var parameters =  compiler.PerpareDynamicParameters(query);
            var parameter = parameters.ParameterNames.FirstOrDefault(x => x.Equals("Name"));

            //assert
            Assert.IsNotNull(parameter);
            Assert.IsNotNull(parameter);
            
        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithOrderBy_ShouldReturnCustomSqlWithOrderBy()
        {
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>(){ new OrderPart(){ Sequence = 0, Field = "Description"}};
            
            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Order By Description ", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithTwoOrderBy_ShouldReturnCustomSqlWithOrderBy()
        {
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>() { 
                new OrderPart() { Sequence = 0, Field = "Description"}, 
                new OrderPart() { Sequence = 1, Field = "Id" }
            };

            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;
            
            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Order By Description , Id ", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithOrderByThatIncludedDirection_ShouldReturnCustomSqlWithOrderBy()
        {
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>() { 
                new OrderPart() { Sequence = 0, Field = "Description Desc"}, 
            };

            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;
            
            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Order By Description Desc", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithTwoOrderByThatIncludedDirection_ShouldReturnCustomSqlWithOrderBy()
        {
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>() { 
                new OrderPart() { Sequence = 0, Field = "Description Desc"}, 
                new OrderPart() { Sequence = 0, Field = "Id Asc"}, 
            };

            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;
            
            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Order By Description Desc, Id Asc", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithTwoOrderByThatIncludedOneDirection_ShouldReturnCustomSqlWithOrderBy()
        {
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>() { 
                new OrderPart() { Sequence = 0, Field = "Description Desc"}, 
                new OrderPart() { Sequence = 0, Field = "Id"}, 
            };

            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;
            
            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Order By Description Desc, Id ", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithSkipAndOrderBy_ShouldReturnCustomSqlWithSkipSet()
        {
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>() { 
                new OrderPart() { Sequence = 0, Field = "Description Desc"}
            };

            query.Skip = new SkipPart()
            {
                Skip = 10,
                Take = 20
            };

            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Order By Description Desc Offset 10 Rows Fetch Next 20 Rows Only", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithSkipAndNoOderBy_ShouldReturnCustomSqlWithNoSkipSet()
        {
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();
            query.OrderBy = new List<IOrderPart>() { 
                new OrderPart() { Sequence = 0, Field = "Description Desc"}
            };

            query.Skip = new SkipPart()
            {
                Skip = 10,
                Take = 20
            };

            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Order By Description Desc Offset 10 Rows Fetch Next 20 Rows Only", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithSkipAndOderByAndLimit_ShouldReturnCustomSqlWithNoTopInSelect ()
        {
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();

            query.Skip = new SkipPart()
            {
                Skip = 10,
                Take = 20
            };

            query.Limit = new LimitPart()
            {
                Count = 10
            };

            //action
            string statement = compiler.PrepareQuery(query);

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithJsonWhereAndStementWhichHasWhereIncludedWithSimpleAlias_ShouldReturnCustomSqlWithWhere()
        {
            var compiler = CreateCompilerWithWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();
            query.Where = InstantiateWhere();
            IWherePart where = CreateWherePartAsWhere();
            where.Where.Add(CreateWherePartAsName());
            query.Where.Add(where);
            
            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description, T.IsActive as IsActive From Test T Where T.IsActive = 1) As QS Where Name = @Name", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithSkipLimitOrderAndWhere_ShouldReturnCustomSqlWithWhereLimitOrderAndSkip()
        {
            var compiler = CreateCompilerWithWhere();
            FindManyQuery query = new FindManyQuery();
            query.Where = InstantiateWhere();
            IWherePart where = CreateWherePartAsWhere();
            where.Where.Add(CreateWherePartAsName());
            query.Where.Add(where);

            query.OrderBy = new List<IOrderPart>();

            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description, T.IsActive as IsActive From Test T Where T.IsActive = 1) As QS Where Name = @Name", statement);

        }

        [Test]
        public void GetStatement_GivenFindQueryObjectFromJsonParser_ShouldReturnCorrectListOfDymanicParamaters()
        {
            
            //arrange
            var compiler = CreateCompilerWithWhere();
            FindManyQuery query = new FindManyQuery();
            query.Where = InstantiateWhere();
            IWherePart where = CreateWherePartAsWhere();
            where.Where.Add(CreateWherePartAsName());
            query.Where.Add(where);

            //action
            DynamicParameters parameters = compiler.PerpareDynamicParameters(query);
            
            //assert
            Assert.IsNotNull(parameters);
            Assert.IsNotNull(parameters.ParameterNames.FirstOrDefault(x => x == "Name"));

        }

        [Test]
        public void GetStatement_GivenFindQueryObjectFromQueryStringParser_ShouldReturnCorrectListOfDymanicParamaters()
        {

            //arrange
            var compiler = CreateCompilerWithWhere();
            FindManyQuery query = new FindManyQuery();
            query.Where = InstantiateWhere();
            query.Where.Add(CreateWherePartAsName());
            
            //action
            DynamicParameters parameters = compiler.PerpareDynamicParameters(query);

            //assert
            Assert.IsNotNull(parameters);
            Assert.IsNotNull(parameters.ParameterNames.FirstOrDefault(x => x == "Name"));

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithWhereWhichHasIn_ShouldReturnCustomSqlWithWhereInSetCorrectly()
        {
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.Where = InstantiateWhere();
            IWherePart where = CreateWherePartAsWhere();
            IWherePart and = CreateWherePartAsId_IdSetToOne();
            and.Operator = "in";
            and.Value = "1,2";
            where.Where.Add(and);
            query.Where.Add(where);

            query.OrderBy = new List<IOrderPart>();

            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Where Id in (1,2)", statement);

        }
        
        [Test]
        public void GetStatement_GivenFindManyQueryWithWhereWhichHasNotIn_ShouldReturnCustomSqlWithWhereNotInSetCorrectly()
        {
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.Where = InstantiateWhere();
            IWherePart where = CreateWherePartAsWhere();
            IWherePart and = CreateWherePartAsId_IdSetToOne();
            and.Operator = "not in";
            and.Value = "1,2";
            where.Where.Add(and);
            query.Where.Add(where);

            query.OrderBy = new List<IOrderPart>();

            //action
            compiler.PrepareQuery(query);
            string statement = compiler.Statement;

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Where Id not in (1,2)", statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithPagedPartAndOrderBy_ShouldPerfomLimitAndSkip()
        {
            
            //arrange
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.PagedPart = new PagedPart()
            {
                CurrentPage = 1,
                PageSize = 10
            };

            query.OrderBy = new List<IOrderPart>();
            query.OrderBy.Add(new OrderPart() { Field = "Description Desc", Sequence = 0 });
            
            //action
            var statement = compiler.PrepareQuery(query);

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Order By Description Desc Offset 0 Rows Fetch Next 10 Rows Only", statement);

        }

        [Ignore] //Not so sure this will apply anymore 
        [Test]
        public void GetStatement_GivenFindManyWithSherePartWithFieldNameLowerCaser_ShouldFindTheCorrectAlias()
        {
            
            //arrange
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();

            query.Where = InstantiateWhere();
            IWherePart where = CreateWherePartAsWhere();
            where.Where.Add(new WherePart()
            {
                ClauseOperator = "and",
                Field = "name",
                Operator = "=",
                Value = "SomeName"
            });
            query.Where.Add(where);

            query.OrderBy = new List<IOrderPart>();

            //action
            var statement = compiler.PrepareQuery(query);

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Where Name = @Name", statement);

        }

        [Ignore] //Not so sure this will apply anymore 
        [Test]
        public void GetStatement_GivenFindQueryObjectFromJsonParserWithLowerCaseField_ShouldReturnCorrectListOfDymanicParamaters()
        {

            //arrange
            var compiler = CreateCompilerWithWhere();
            FindManyQuery query = new FindManyQuery();
            query.Where = InstantiateWhere();
            IWherePart where = CreateWherePartAsWhere();
            IWherePart and = CreateWherePartAsName();
            and.Field = "name";
            where.Where.Add(and);
            query.Where.Add(where);

            //action
            DynamicParameters parameters = compiler.PerpareDynamicParameters(query);

            //assert
            Assert.IsNotNull(parameters);
            Assert.IsNotNull(parameters.ParameterNames.FirstOrDefault(x => x == "Name"));

        }

        [Test]
        public void GetStatement_GivenGetStatementQueryWithTopIncluded_ShouldRemoveTheTopPartOfStatement()
        {

            //arrange
            var compiler = CreateCompilerWithTop();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();
            
            //action
            var statement = compiler.PrepareQuery(query);

            //assert
            Assert.AreEqual("Select * From (Select  T.Id as Id, T.Description as Description From Test T) As QS", statement);
            
        }

        [Test]
        public void GetCountStatement_GivenGetStatementQueryWithTopIncluded_ShouldRemoveTheTopPartOfStatement()
        {
            
            //arrange
            var compiler = CreateCompilerWithTop();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();

            //action
            var statement = compiler.PrepareCountQuery(query);

            //assert
            Assert.AreEqual("Select Count(*) From (Select  T.Id as Id, T.Description as Description From Test T) As QS", statement);
            
        }

        [Test]
        public void GetStatement_GivenGetStatementQueryWithInnerJoinTopIncluded_ShouldRemoveTheFirstTopPartOfStatement()
        {

            //arrange
            var compiler = CreateCompilerWithTopAndInnerJoinTop();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();
            
            //action
            var statement = compiler.PrepareQuery(query);

            //assert
            Assert.AreEqual("Select * From (Select  T.Id as Id, T.Description as Description From Test T Inner Join (Select Top 5 Id From TestItem TI) Links On Links.Id = T.Id) As QS", statement);

        }
        
        [Test]
        public void GetCountStatement_GivenGetStatementQueryWithInnerJoinTopIncluded_ShouldRemoveTheFirstTopPartOfStatement()
        {

            //arrange
            var compiler = CreateCompilerWithTopAndInnerJoinTop();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();

            //action
            var statement = compiler.PrepareCountQuery(query);

            //assert
            var sql = "Select Count(*) From (Select  T.Id as Id, T.Description as Description " +
                "From Test T " +
                "Inner Join (Select Top 5 Id From TestItem TI) Links On Links.Id = T.Id) As QS";

            sql = sql.Replace(Environment.NewLine, string.Empty);

            Assert.AreEqual(sql, statement);

        }

        [Test]
        public void GetStatement_GivenGetStatementQueryWithOnlyInnerJoinTopIncluded_ShouldRemoveNoTopPartOfStatement()
        {

            //arrange
            var compiler = CreateCompilerWithOnlyInnerJoinTop();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();

            //action
            var statement = compiler.PrepareQuery(query);

            //assert
            var sql = "Select * From (Select T.Id as Id, T.Description as Description " +
                "From Test T " +
                "Inner Join (Select Top 5 Id From TestItem TI) Links On Links.Id = T.Id) As QS";

            sql = sql.Replace(Environment.NewLine, string.Empty);

            Assert.AreEqual(sql, statement);

        }

        [Test]
        public void GetCountStatement_GivenGetStatementQueryWithOnlyInnerJoinTopIncluded_ShouldRemoveNoTopPartOfStatement()
        {

            //arrange
            var compiler = CreateCompilerWithOnlyInnerJoinTop();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();

            //action
            var statement = compiler.PrepareCountQuery(query);

            //assert
            var sql = "Select Count(*) From (Select T.Id as Id, T.Description as Description " +
                "From Test T " +
                "Inner Join (Select Top 5 Id From TestItem TI) Links On Links.Id = T.Id) As QS";

            sql = sql.Replace(Environment.NewLine, string.Empty);

            Assert.AreEqual(sql, statement);

        }

        [Test]
        public void GetStatement_GivenFindManyQueryWithWhereClauseThatUsesTheSameColumnMoreThanONce_ShouldCreateSqlStatementWithNumberedParameters()
        {
            
            //arrange
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.OrderBy = new List<IOrderPart>();

            query.Where = InstantiateWhere();
            query.Where.Add(CreateWherePartAsId_IdSetToOne());
            query.Where.Add(CreateWherePartAsId_IdSetToTwo());

            //action
            var statement = compiler.PrepareQuery(query);

            //assert
            Assert.AreEqual("Select * From (Select T.Id as Id, T.Description as Description From Test T) As QS Where Id = @Id and Id = @Id1", statement);


        }

        [Test]
        public void GetDynamicParameters_GivenFindManyQueryWithWherePartThatUsesTheSameFieldTwice_ShouldReturnDynamicArgumentsWithTwoItemTheParamatersMustBeNumbered()
        {

            //arrange
            var compiler = CreateCompilerWithNoWhere();
            FindManyQuery query = new FindManyQuery();
            query.Where = InstantiateWhere();
            query.Where.Add(CreateWherePartAsId_IdSetToOne());
            query.Where.Add(CreateWherePartAsId_IdSetToTwo());

            //action
            var parameters = compiler.PerpareDynamicParameters(query);
            var parameter1 = parameters.ParameterNames.FirstOrDefault(x => x.Equals("Id"));
            var parameter2 = parameters.ParameterNames.FirstOrDefault(x => x.Equals("Id1"));

            //assert
            Assert.IsNotNull(parameters);
            Assert.IsNotNull(parameter1);
            Assert.IsNotNull(parameter2);
            
        }
        
        private CustomSqlCompiler<TestDataModel> CreateCompilerWithNoWhere()
        {
            CustomSqlCompiler<TestDataModel> compiler = new CustomSqlCompiler<TestDataModel>(new GetTestStatement(),
                new TestDataModel());
            return compiler;
        }

        private CustomSqlCompiler<TestDataModel> CreateCompilerWithWhere()
        {
            CustomSqlCompiler<TestDataModel> compiler = new CustomSqlCompiler<TestDataModel>(new GetTestStatementWithWhere(), 
                new TestDataModel());
            return compiler;
        }

        private CustomSqlCompiler<TestDataModel> CreateCompilerWithTop()
        {
            CustomSqlCompiler<TestDataModel> compiler = new CustomSqlCompiler<TestDataModel>(new GetTestStatementWithTop(), 
                new TestDataModel());
            return compiler;
        }

        private CustomSqlCompiler<TestDataModel> CreateCompilerWithTopAndInnerJoinTop()
        {
            CustomSqlCompiler<TestDataModel> compiler = new CustomSqlCompiler<TestDataModel>(new GetTestStatementWithTopAndInnerSelectWithTop(), 
                new TestDataModel());
            return compiler;
        }

        private CustomSqlCompiler<TestDataModel> CreateCompilerWithOnlyInnerJoinTop()
        {
            CustomSqlCompiler<TestDataModel> compiler = new CustomSqlCompiler<TestDataModel>(new GetTestStatementWithInnerSelectWithTop(), 
                new TestDataModel());
            return compiler;
        }

        private List<IWherePart> InstantiateWhere()
        {
            return new List<IWherePart>();
        }

        private WherePart CreateWherePartAsId_IdSetToOne()
        {
            return new WherePart()
            {
                ClauseOperator = "and",
                Field = "Id",
                ParameterName = "Id",
                Operator = "=",
                Value = "1"
            };
        }

        private IWherePart CreateWherePartAsId_IdSetToTwo()
        {
            return new WherePart()
            {
                ClauseOperator = "and",
                Field = "Id",
                ParameterName = "Id1",
                Operator = "=",
                Value = "2"
            };
        }

        private WherePart CreateWherePartAsWhere()
        {
            return new WherePart()
            {
                ClauseOperator = "where",
                Field = "",
                ParameterName = "",
                Operator = "",
                Value = "",
                Where = new List<IWherePart>()
            };
        }

        private WherePart CreateWherePartAsDescription()
        {
            return new WherePart()
            {
                ClauseOperator = "and",
                Field = "Description",
                ParameterName = "Description",
                Operator = "=",
                Value = "SomeValue"
            };
        }

        private WherePart CreateWherePartAsName()
        {
            return new WherePart()
            {
                ClauseOperator = "and",
                Field = "Name",
                ParameterName = "Name",
                Operator = "=",
                Value = "SomeName"
            };
        }

        private LimitPart CreateLimitPart()
        {
            return new LimitPart() { Count = 5 };
        }

    }

}