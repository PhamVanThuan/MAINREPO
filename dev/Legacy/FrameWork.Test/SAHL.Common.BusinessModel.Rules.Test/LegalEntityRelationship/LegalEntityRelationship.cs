using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Rules.LegalEntityRelationship;

namespace SAHL.Common.BusinessModel.Rules.Test.LegalEntityRelationship
{
    [TestFixture]
    public class LegalEntityRelationship : RuleBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        #region ValidateCircularLegalEntityRelationship
        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ValidateCircularLegalEntityRelationship_NoArgumentsPassed()
        {
            LegalEntityRelationshipCircular rule = new LegalEntityRelationshipCircular();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ValidateCircularLegalEntityRelationship_IncorrectArgumentsPassed()
        {
            LegalEntityRelationshipCircular rule = new LegalEntityRelationshipCircular();

            // Setup an incorrect Argument to pass -- the rule should accept an ILegalEntityRelationship
            IApplication application = _mockery.StrictMock<IApplication>();
            ExecuteRule(rule, 0, application);

        }

        [NUnit.Framework.Test]
        public void ValidateCircularLegalEntityRelationship_Success()
        {
            LegalEntityRelationshipCircular rule = new LegalEntityRelationshipCircular();

            // Setup an correct Argument to pass 
            ILegalEntityRelationship legalEntityRelationship = _mockery.StrictMock<ILegalEntityRelationship>();
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            ILegalEntity relatedLegalEntity = _mockery.StrictMock<ILegalEntity>();

            // Setup the different legalentity as the related legalentity
            SetupResult.For(legalEntityRelationship.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.Key).Return(99);
            SetupResult.For(legalEntityRelationship.RelatedLegalEntity).Return(relatedLegalEntity);
            SetupResult.For(relatedLegalEntity.Key).Return(999);

            // Execute the rule 
            ExecuteRule(rule, 0, legalEntityRelationship);
        }

        [NUnit.Framework.Test]
        public void ValidateCircularLegalEntityRelationship_Failure()
        {
            LegalEntityRelationshipCircular rule = new LegalEntityRelationshipCircular();

            // Setup an correct Argument to pass 
            ILegalEntityRelationship legalEntityRelationship = _mockery.StrictMock<ILegalEntityRelationship>();
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            ILegalEntity relatedLegalEntity = _mockery.StrictMock<ILegalEntity>();

            // Setup the same legalentity as the related legalentity
            SetupResult.For(legalEntityRelationship.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.Key).Return(99);
            SetupResult.For(legalEntityRelationship.RelatedLegalEntity).Return(relatedLegalEntity);
            SetupResult.For(relatedLegalEntity.Key).Return(99);

            // Execute the rule 
            ExecuteRule(rule, 1, legalEntityRelationship);
        }
        #endregion

    }
}
