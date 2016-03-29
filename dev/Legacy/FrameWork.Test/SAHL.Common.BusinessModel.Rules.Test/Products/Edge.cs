using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Rules.Products;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.Test.Products
{
    [TestFixture]
    public class Edge : RuleBase
    {
        [SetUp]
        public new void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public new void TearDown()
        {
            base.TearDown();
        }

        #region Edge Mortgage Loan Term Test
        [NUnit.Framework.Test]
        public void ApplicationProductEdgeTermTest()
        {
			IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
			int edgeMaxTerm = Convert.ToInt32(ctrlRepo.GetControlByDescription("Edge Max Term").ControlNumeric);

            // PASS
			ApplicationProductEdgeTermHelper(0, edgeMaxTerm -1 );
            // FAIL
			ApplicationProductEdgeTermHelper(1, edgeMaxTerm + 1);
        }

		[Test]
		public void ApplicationProductEdgeLTVTest()
		{
			IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
			
			// PASS
            ApplicationProductEdgeLTVHelper(0,  1500000, 79);

			
			// FAIL
			ApplicationProductEdgeLTVHelper(1, 1500001, 81);
		}



        private void ApplicationProductEdgeLTVHelper(int expectedMessageCount, int loanAmount, double LTV)
        {
            MaxEdgeLoanAgreementAmountInBlueBanner rule = new MaxEdgeLoanAgreementAmountInBlueBanner();

            IApplication app = _mockery.StrictMock<IApplication>();
            ISupportsVariableLoanApplicationInformation_ApplicationProduct svlai_appProd = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation_ApplicationProduct>();
            IApplicationInformationVariableLoan aivl = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            SetupResult.For(aivl.LoanAgreementAmount).Return(loanAmount);
            SetupResult.For(aivl.LTV).Return(LTV);

            SetupResult.For(svlai_appProd.ProductType).Return(SAHL.Common.Globals.Products.Edge);
            SetupResult.For(svlai_appProd.VariableLoanInformation).Return(aivl);

            SetupResult.For(app.CurrentProduct).Return(svlai_appProd);

            ExecuteRule(rule, expectedMessageCount, app);
        }

		/// <summary>
        /// This interface is created for mocking purposes only, for rules that cast IApplicationProduct objects 
        /// to ISupportsVariableLoanApplicationInformation objects.
        /// </summary>
        public interface ISupportsVariableLoanApplicationInformation_ApplicationProduct : IApplicationProduct, ISupportsVariableLoanApplicationInformation
		{
		}


		
        private void ApplicationProductEdgeTermHelper(int expectedMessageCount, int term)
        {
            ApplicationProductEdgeTerm rule = new ApplicationProductEdgeTerm();
            
            IApplicationMortgageLoan app = _mockery.StrictMock<IApplicationMortgageLoan>();

            IApplicationProductEdge appProduct = _mockery.StrictMock<IApplicationProductEdge>();
            SetupResult.For(app.CurrentProduct).Return(appProduct);
            SetupResult.For(appProduct.Term).Return(term);
            SetupResult.For(appProduct.ProductType).Return(SAHL.Common.Globals.Products.Edge);
            ExecuteRule(rule, expectedMessageCount, app);
        }
        #endregion
    }
}
