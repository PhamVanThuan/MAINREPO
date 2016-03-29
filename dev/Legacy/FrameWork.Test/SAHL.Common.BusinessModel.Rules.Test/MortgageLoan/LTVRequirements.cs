using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Rules;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.BusinessModel.Rules.MortgageRules;

namespace SAHL.Common.BusinessModel.Rules.Test.MortgageLoan
{
    [TestFixture]
    public class LTVRequirements : RuleBase
    {
        MortgageLoanLTVRequirement rule = null;
        [NUnit.Framework.SetUp()]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void LTVRequirementsPass()
        {
            rule = new MortgageLoanLTVRequirement();
            IDomainMessageCollection Messages = new DomainMessageCollection();
            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationProduct apml = _mockery.StrictMock<IApplicationProductMortgageLoan>();
            SetupResult.For(app.CurrentProduct).Return(apml);
            ISupportsVariableLoanApplicationInformation vli = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();
           
            IApplicationInformationVariableLoan vl = _mockery.StrictMock<IApplicationInformationVariableLoan>();
            SetupResult.For(vli.VariableLoanInformation).Return(vl);
            SetupResult.For(vl.LTV).Return((double)105);
            
            ExecuteRule(rule, 0, app);




            SetupResult.For(vli.VariableLoanInformation).Return(vl);
        }

        //[NUnit.Framework.Test]
        //public void LTVRequirementsFail()
        //{
        //    rule = new MortgageLoanLTVRequirement();
        //    IDomainMessageCollection Messages = new DomainMessageCollection();
        //    IApplication app = _mockery.StrictMock<IApplication>();
        //    IApplicationProduct ml = _mockery.StrictMock<IApplicationProductMortgageLoan>();
        //    SetupResult.For(app.CurrentProduct).Return(ml);
        //    ISupportsVariableLoanApplicationInformation vli = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();
        //    IApplicationInformationVariableLoan vl = _mockery.StrictMock<IApplicationInformationVariableLoan>();
        //    SetupResult.For(vl.LTV).Return((double)101);
        //    SetupResult.For(vli.VariableLoanInformation).Return(vl);

        //    ExecuteRule(rule, 1, app);
        //}

        //[NUnit.Framework.Test]
        //public void LTVRequirementsFailAt102()
        //{
        //    rule = new MortgageLoanLTVRequirement();
        //    IDomainMessageCollection Messages = new DomainMessageCollection();
        //    IApplication app = _mockery.StrictMock<IApplication>();
        //    IApplicationProduct ml = _mockery.StrictMock<IApplicationProductMortgageLoan>();
        //    SetupResult.For(app.CurrentProduct).Return(ml);
        //    ISupportsVariableLoanApplicationInformation vli = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();
        //    IApplicationInformationVariableLoan vl = _mockery.StrictMock<IApplicationInformationVariableLoan>();
        //    SetupResult.For(vl.LTV).Return((double)102);
        //    SetupResult.For(vli.VariableLoanInformation).Return(vl);

        //    ExecuteRule(rule, 1, app);
        //}
    }
}
