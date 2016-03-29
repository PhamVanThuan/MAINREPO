using System.Collections.Generic;
using NUnit.Framework;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Builders;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Models.Core;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Services.Query.Server.Tests.Builders
{
    [TestFixture]
    public class WhereBuilderTests
    {
        
        [Test]
        public void BuildWhereFilter_GivenRelationshipDefenitionWithSingleItem_ShouldPrepareJsonWhereWithDefaultAndClause()
        {
            
            //arrange
            WhereBuilder whereBuilder = new WhereBuilder();
            RelationshipDefinition relationshipDefinition = CreateRelationshipDefinition();
            relationshipDefinition.RelatedFields.Add(CreateIdRelatedField());

            //action
            string jsonWhere = whereBuilder.BuildWhereFilter(relationshipDefinition);

            //assert
            Assert.AreEqual(@"filter={""where"":{""id"":""2""}}", jsonWhere);

        }

        [Test]
        public void BuildWhereFilter_GivenRelationshipDefenitionWithTwoItems_ShouldPrepareJsonWhereWithDefaultAndClause()
        {

            //arrange
            WhereBuilder whereBuilder = new WhereBuilder();
            RelationshipDefinition relationshipDefinition = CreateRelationshipDefinition();
            relationshipDefinition.RelatedFields.Add(CreateIdRelatedField());
            relationshipDefinition.RelatedFields.Add(CreateNameRelatedField());

            //action
            string jsonWhere = whereBuilder.BuildWhereFilter(relationshipDefinition);

            //assert
            Assert.AreEqual(@"filter={""where"":{""id"":""2"",""namelink"":""somevalue""}}", jsonWhere);

        }

        private RelationshipDefinition CreateRelationshipDefinition()
        {
            return new RelationshipDefinition()
            {
                DataModelType = typeof (AttorneyContactDataModel),
                RelatedEntity = "AttorneyContact",
                RelatedFields = new List<IRelatedField>(),
                RelationshipType = RelationshipType.OneToMany
            };
        }

        private IRelatedField CreateIdRelatedField()
        {
            return new RelatedField()
            {
                LocalKey = "AttorneyId",
                RelatedKey = "Id",
                Value = "2"
            };
        }
        
        private IRelatedField CreateNameRelatedField()
        {
            return new RelatedField()
            {
                LocalKey = "Name",
                RelatedKey = "NameLink",
                Value = "SomeValue"
            };
        }
    }
}