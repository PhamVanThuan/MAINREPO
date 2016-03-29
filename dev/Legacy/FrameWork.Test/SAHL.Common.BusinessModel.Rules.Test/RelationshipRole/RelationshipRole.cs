using System;
using SAHL.Common.Collections;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using Rhino.Mocks;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.RelationshipRole;

namespace SAHL.Common.BusinessModel.Rules.Test.RelationshipRole
{
    [TestFixture]
    public class RelationshipRole: RuleBase
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

        #region AddRelationshipRoleBetween2LegalEntity
        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void AddRelationshipRoleBetween2LegalEntityNoArgumentsPassed()
        {
            AddRelationshipRoleBetween2LegalEntity rule = new AddRelationshipRoleBetween2LegalEntity();
            ExecuteRule(rule, 0);

        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void AddRelationshipRoleBetween2LegalEntityIncorrectArgumentsPassed()
        {
            AddRelationshipRoleBetween2LegalEntity rule = new AddRelationshipRoleBetween2LegalEntity();

            // Setup an incorrect Argumnt to pass along
            IApplication application = _mockery.StrictMock<IApplication>();
            ExecuteRule(rule, 0, application);

        }

        /// <summary>
        /// Expects 0 Domain messages.
        /// </summary>
        [NUnit.Framework.Test]
        public void AddRelationshipRoleBetween2LegalEntitySuccess()
        {
            AddRelationshipRoleBetween2LegalEntity rule = new AddRelationshipRoleBetween2LegalEntity();

            // Setup a correct Argumnt to pass along
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();

            // Setup legalEntity.LegalEntityRelationships
            IEventList<ILegalEntityRelationship> legalEntityRelationships = new EventList<ILegalEntityRelationship>();
            SetupResult.For(legalEntity.LegalEntityRelationships).Return(legalEntityRelationships);
            
            // Setup legalEntityRelationship.RelatedLegalEntity
            ILegalEntityRelationship legalEntityRelationship = _mockery.StrictMock<ILegalEntityRelationship>();
            legalEntityRelationships.Add(Messages, legalEntityRelationship);

            // Setup legalEntityRelationship.LegalEntity
            SetupResult.For( legalEntityRelationship.RelatedLegalEntity).Return(legalEntity);

            ExecuteRule(rule, 0, legalEntity);

        }

        /// <summary>
        /// Expects 1 Domain message.
        /// </summary>
        [NUnit.Framework.Test]
        public void AddRelationshipRoleBetween2LegalEntityFailure()
        {
            AddRelationshipRoleBetween2LegalEntity rule = new AddRelationshipRoleBetween2LegalEntity();

            // Setup a correct Argumnt to pass along
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();

            // Setup legalEntity.LegalEntityRelationships
            IEventList<ILegalEntityRelationship> legalEntityRelationships = new EventList<ILegalEntityRelationship>();
            SetupResult.For(legalEntity.LegalEntityRelationships).Return(legalEntityRelationships);

            // Setup legalEntityRelationship.RelatedLegalEntity
            ILegalEntityRelationship legalEntityRelationship = _mockery.StrictMock<ILegalEntityRelationship>();
            legalEntityRelationships.Add(Messages, legalEntityRelationship);

            // Setup legalEntityRelationship.LegalEntity
            SetupResult.For(legalEntityRelationship.RelatedLegalEntity).Return(null);

            ExecuteRule(rule, 1, legalEntity);
        }

        #endregion

        #region AddRelationshipRoleMortgageLoanAccount
        /// <summary>
        /// Expects 0 Domain messages.
        /// </summary>
        [NUnit.Framework.Test]
        [Ignore("To be fixed")]
        public void AddRelationshipRoleMortgageLoanAccountSuccess()
        {
            AddRelationshipRoleMortgageLoanAccount rule = new AddRelationshipRoleMortgageLoanAccount();
            // Setup a correct Argumnt to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);

            // Setup role.LegalEntity.LegalEntityRelationships
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(role.LegalEntity).Return(legalEntity);

            // Setup legalEntity.LegalEntityRelationships
            IEventList<ILegalEntityRelationship> legalEntityRelationships = new EventList<ILegalEntityRelationship>();
            SetupResult.For(legalEntity.LegalEntityRelationships).Return(legalEntityRelationships);

            // Setup legalEntityRelationship.RelatedLegalEntity
            ILegalEntityRelationship legalEntityRelationship = _mockery.StrictMock<ILegalEntityRelationship>();
            legalEntityRelationships.Add(Messages, legalEntityRelationship);

            // Setup legalEntityRelationship.LegalEntity
            SetupResult.For(legalEntityRelationship.RelatedLegalEntity).Return(legalEntity);

            ExecuteRule(rule, 0, mortgageLoanAccount);

        }

        /// <summary>
        /// Expects 1 Domain message.
        /// </summary>
        [NUnit.Framework.Test]
        [Ignore("To be fixed")]
        public void AddRelationshipRoleMortgageLoanAccountFailure()
        {
            AddRelationshipRoleMortgageLoanAccount rule = new AddRelationshipRoleMortgageLoanAccount();

            // Setup a correct Argument to pass along
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();

            // Setup legalEntity.LegalEntityRelationships
            IEventList<ILegalEntityRelationship> legalEntityRelationships = new EventList<ILegalEntityRelationship>();
            SetupResult.For(legalEntity.LegalEntityRelationships).Return(legalEntityRelationships);

            // Setup legalEntityRelationship.RelatedLegalEntity
            ILegalEntityRelationship legalEntityRelationship = _mockery.StrictMock<ILegalEntityRelationship>();
            legalEntityRelationships.Add(Messages, legalEntityRelationship);

            // Setup legalEntityRelationship.LegalEntity
            SetupResult.For(legalEntityRelationship.RelatedLegalEntity).Return(null);

            ExecuteRule(rule, 1, legalEntity);
        }
        #endregion


    }
}
