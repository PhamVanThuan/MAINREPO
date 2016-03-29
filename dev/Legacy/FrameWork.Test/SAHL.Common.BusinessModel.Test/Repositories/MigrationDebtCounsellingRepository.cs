using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Rhino.Mocks;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
	[TestFixture]
	public class MigrationDebtCounsellingRepository : TestBase
	{
		[SetUp]
		public void Setup()
		{
			// set the strategy to default so we actually go to the database
			SetRepositoryStrategy(TypeFactoryStrategy.Default);
			_mockery = new MockRepository();
		}

		[TearDown]
		public void TearDown()
		{
			MockCache.Flush();
		}

		/// <summary>
		/// Create Empty Debt Counselling
		/// </summary>
		[Test]
		public void CreateEmptyDebtCounselling()
		{
			using (new SessionScope())
			{
				IMigrationDebtCounsellingRepository debtCounsellingRepository = RepositoryFactory.GetRepository<IMigrationDebtCounsellingRepository>();
				IMigrationDebtCounselling debtCounsellingCase = debtCounsellingRepository.CreateEmptyDebtCounselling();
				Assert.IsNotNull(debtCounsellingCase);
			}
		}

		/// <summary>
		/// Create Empty Proposal
		/// </summary>
		[Test]
		public void CreateEmptyProposal()
		{
			using (new SessionScope())
			{
				IMigrationDebtCounsellingRepository debtCounsellingRepository = RepositoryFactory.GetRepository<IMigrationDebtCounsellingRepository>();
				IMigrationDebtCounsellingProposal debtCounsellingProposal = debtCounsellingRepository.CreateEmptyProposal();
				Assert.IsNotNull(debtCounsellingProposal);
			}
		}

		/// <summary>
		/// Create Empty Proposal Item
		/// </summary>
		[Test]
		public void CreateEmptyProposalItem()
		{
			using (new SessionScope())
			{
				IMigrationDebtCounsellingRepository debtCounsellingRepository = RepositoryFactory.GetRepository<IMigrationDebtCounsellingRepository>();
				IMigrationDebtCounsellingProposalItem debtCounsellingProposalItem = debtCounsellingRepository.CreateEmptyProposalItem();
				Assert.IsNotNull(debtCounsellingProposalItem);
			}
		}

		/// <summary>
		/// Create Empty External Role
		/// </summary>
		[Test]
		public void CreateEmptyExternalRole()
		{
			using (new SessionScope())
			{
				IMigrationDebtCounsellingRepository debtCounsellingRepository = RepositoryFactory.GetRepository<IMigrationDebtCounsellingRepository>();
				IMigrationDebtCounsellingExternalRole externalRole = debtCounsellingRepository.CreateEmptyExternalRole();
				Assert.IsNotNull(externalRole);
			}
		}
	}
}
