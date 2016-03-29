using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Test;
using SAHL.Common.BusinessModel.Rules.Affordability;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.Rules.Test.Affordability
{
    [TestFixture]
    public class Affordability : RuleBase
    {
        ILegalEntityAffordability leAffordability;

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

        [NUnit.Framework.Test]
        public void AffordabilityDescriptionMandatoryIncomeFromInvestmentTestFail()
        {
            AffordabilityDescriptionMandatory rule = new AffordabilityDescriptionMandatory();

            leAffordability = _mockery.StrictMock<ILegalEntityAffordability>();
            IAffordabilityType affordType = _mockery.StrictMock<IAffordabilityType>();
            SetupResult.For(leAffordability.AffordabilityType).Return(affordType);
            SetupResult.For(affordType.Description).Return(AffordabilityTypes.IncomefromInvestments.ToString());
            SetupResult.For(affordType.Key).Return((int)AffordabilityTypes.IncomefromInvestments);
            SetupResult.For(affordType.DescriptionRequired).Return(true);
            string desc = "";
            SetupResult.For(leAffordability.Description).Return(desc);

            ExecuteRule(rule, 1, leAffordability);
        }


        [NUnit.Framework.Test]
        public void AffordabilityDescriptionMandatoryIncomeFromInvestmentTestPass()
        {
            AffordabilityDescriptionMandatory rule = new AffordabilityDescriptionMandatory();

            leAffordability = _mockery.StrictMock<ILegalEntityAffordability>();
            IAffordabilityType affordType = _mockery.StrictMock<IAffordabilityType>();
            SetupResult.For(leAffordability.AffordabilityType).Return(affordType);
            SetupResult.For(affordType.Description).Return(AffordabilityTypes.IncomefromInvestments.ToString());
            SetupResult.For(affordType.Key).Return((int)AffordabilityTypes.IncomefromInvestments);
            SetupResult.For(affordType.DescriptionRequired).Return(true);
            string desc = "IncAndInv";
            SetupResult.For(leAffordability.Description).Return(desc);

            ExecuteRule(rule, 0, leAffordability);
        }

        [NUnit.Framework.Test]
        public void AffordabilityDescriptionMandatoryOtherInstalmentsTestFail()
        {
            AffordabilityDescriptionMandatory rule = new AffordabilityDescriptionMandatory();

            leAffordability = _mockery.StrictMock<ILegalEntityAffordability>();
            IAffordabilityType affordType = _mockery.StrictMock<IAffordabilityType>();
            SetupResult.For(leAffordability.AffordabilityType).Return(affordType);
            SetupResult.For(affordType.Description).Return(AffordabilityTypes.OtherInstalments.ToString());
            SetupResult.For(affordType.Key).Return((int)AffordabilityTypes.OtherInstalments);
            SetupResult.For(affordType.DescriptionRequired).Return(true);
            string desc = "";
            SetupResult.For(leAffordability.Description).Return(desc);

            ExecuteRule(rule, 1, leAffordability);
        }

        [NUnit.Framework.Test]
        public void AffordabilityDescriptionMandatoryOtherInstalmentsTestPass()
        {
            AffordabilityDescriptionMandatory rule = new AffordabilityDescriptionMandatory();

            leAffordability = _mockery.StrictMock<ILegalEntityAffordability>();
            IAffordabilityType affordType = _mockery.StrictMock<IAffordabilityType>();
            SetupResult.For(leAffordability.AffordabilityType).Return(affordType);
            SetupResult.For(affordType.Description).Return(AffordabilityTypes.OtherInstalments.ToString());
            SetupResult.For(affordType.Key).Return((int)AffordabilityTypes.OtherInstalments);
            SetupResult.For(affordType.DescriptionRequired).Return(true);
            string desc = "OtherInst";
            SetupResult.For(leAffordability.Description).Return(desc);

            ExecuteRule(rule, 0, leAffordability);
        }

        [NUnit.Framework.Test]
        public void AffordabilityDescriptionMandatoryOtherExpenseTestFail()
        {
            AffordabilityDescriptionMandatory rule = new AffordabilityDescriptionMandatory();

            leAffordability = _mockery.StrictMock<ILegalEntityAffordability>();
            IAffordabilityType affordType = _mockery.StrictMock<IAffordabilityType>();
            SetupResult.For(leAffordability.AffordabilityType).Return(affordType);
            SetupResult.For(affordType.Description).Return(AffordabilityTypes.OtherInstalments.ToString());
            SetupResult.For(affordType.Key).Return((int)AffordabilityTypes.Other);
            SetupResult.For(affordType.DescriptionRequired).Return(true);
            string desc = "";
            SetupResult.For(leAffordability.Description).Return(desc);

            ExecuteRule(rule, 1, leAffordability);
        }

        [NUnit.Framework.Test]
        public void AffordabilityDescriptionMandatoryOtherExpenseTestPass()
        {
            AffordabilityDescriptionMandatory rule = new AffordabilityDescriptionMandatory();

            leAffordability = _mockery.StrictMock<ILegalEntityAffordability>();
            IAffordabilityType affordType = _mockery.StrictMock<IAffordabilityType>();
            SetupResult.For(leAffordability.AffordabilityType).Return(affordType);
            SetupResult.For(affordType.Description).Return(AffordabilityTypes.OtherInstalments.ToString());
            SetupResult.For(affordType.Key).Return((int)AffordabilityTypes.Other);
            SetupResult.For(affordType.DescriptionRequired).Return(true);
            string desc = "OtherExp";
            SetupResult.For(leAffordability.Description).Return(desc);

            ExecuteRule(rule, 0, leAffordability);
        }

        [NUnit.Framework.Test]
        public void AffordabilityNegativeValueTestFail()
        {
            AffordabilityNegativeValue rule = new AffordabilityNegativeValue();

            leAffordability = _mockery.StrictMock<ILegalEntityAffordability>();
            double amount = -10;
            SetupResult.For(leAffordability.Amount).Return(amount);

            ExecuteRule(rule, 1, leAffordability);
        }

        [NUnit.Framework.Test]
        public void AffordabilityNegativeValueTestPass()
        {
            AffordabilityNegativeValue rule = new AffordabilityNegativeValue();

            leAffordability = _mockery.StrictMock<ILegalEntityAffordability>();
            double amount = 500;
            SetupResult.For(leAffordability.Amount).Return(amount);

            ExecuteRule(rule, 0, leAffordability);
        }

        [NUnit.Framework.Test]
        public void AffordabilityAtLeastOneIncomeTestPass()
        {
            AffordabilityAtLeastOneIncome rule = new AffordabilityAtLeastOneIncome();
            IEventList<ILegalEntityAffordability> leAffordabilities = new EventList<ILegalEntityAffordability>();
            ILegalEntityAffordability afford = _mockery.StrictMock<ILegalEntityAffordability>();
            IApplication _app = _mockery.StrictMock<IApplication>();
            SetupResult.For(_app.Key).Return(1);
            IAffordabilityType affordType = _mockery.StrictMock<IAffordabilityType>();
            SetupResult.For(afford.AffordabilityType).Return(affordType);
            SetupResult.For(affordType.IsExpense).Return(false);
            SetupResult.For(afford.Application).Return(_app);
            leAffordabilities.Add(Messages, afford);

            ExecuteRule(rule, 0, leAffordabilities, _app);
        }

        [NUnit.Framework.Test]
        public void AffordabilityAtLeastOneIncomeTestFail()
        {
            AffordabilityAtLeastOneIncome rule = new AffordabilityAtLeastOneIncome();
            IEventList<ILegalEntityAffordability> leAffordabilities = new EventList<ILegalEntityAffordability>();
            ILegalEntityAffordability afford = _mockery.StrictMock<ILegalEntityAffordability>();
            IApplication _app = _mockery.StrictMock<IApplication>();
            SetupResult.For(_app.Key).Return(1);
            IAffordabilityType affordType = _mockery.StrictMock<IAffordabilityType>();
            SetupResult.For(afford.AffordabilityType).Return(affordType);
            SetupResult.For(affordType.IsExpense).Return(true);
            SetupResult.For(afford.Application).Return(_app);
            leAffordabilities.Add(Messages, afford);

            ExecuteRule(rule, 1, leAffordabilities,_app);
        }
        
        [NUnit.Framework.Test]
        public void AffordabilityStatementMandatoryNoAfforabilityTestFail()
        {
            AffordabilityStatementMandatory rule = new AffordabilityStatementMandatory();

            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            appRoles.Push(applicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            // Setup applicationRole.ApplicationRoleType.Key 
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);
            SetupResult.For(applicationRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();
            
            SetupResult.For(applicationRole.LegalEntity).Return(le);
            SetupResult.For(le.DisplayName).Return("Mr Test");

            IEventList<ILegalEntityAffordability> leAfford = _mockery.StrictMock<IEventList<ILegalEntityAffordability>>();

            SetupResult.For(le.LegalEntityAffordabilities).Return(leAfford);
            SetupResult.For(leAfford.Count).Return(0);

            ExecuteRule(rule, 1, applicationMortgageLoan);
        }

        [NUnit.Framework.Test]
        public void AffordabilityStatementMandatoryNoAfforabilityTestPass()
        {
            AffordabilityStatementMandatory rule = new AffordabilityStatementMandatory();

            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            appRoles.Push(applicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(applicationMortgageLoan.ApplicationRoles).Return(applicationRoles);

            // Setup applicationRole.ApplicationRoleType.Key 
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);
            SetupResult.For(applicationRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();

            SetupResult.For(applicationRole.LegalEntity).Return(le);
            SetupResult.For(le.DisplayName).Return("Mr Test");

            IEventList<ILegalEntityAffordability> leAfford = new EventList<ILegalEntityAffordability>();
            ILegalEntityAffordability affordability = _mockery.StrictMock<ILegalEntityAffordability>();
            leAfford.Add(Messages, affordability);

            SetupResult.For(le.LegalEntityAffordabilities).Return(leAfford);

            ExecuteRule(rule, 0, applicationMortgageLoan);
        }

        /// <summary>
        /// Asset Liability Liability Value Min Helper
        /// </summary>
        [NUnit.Framework.Test]
        public void AssetLiabilityLiabilityValueMinPass()
        {
            AssetLiabilityLiabilityValueMin rule = new AssetLiabilityLiabilityValueMin();

            IAssetLiabilityFixedLongTermInvestment assetLongTerm = _mockery.StrictMock<IAssetLiabilityFixedLongTermInvestment>();
            IAssetLiabilityLiabilityLoan assetLiabilityLiabilityLoan = _mockery.StrictMock<IAssetLiabilityLiabilityLoan>();
            IAssetLiabilityLiabilitySurety assetLiabilityLiabilitySurety = _mockery.StrictMock<IAssetLiabilityLiabilitySurety>();

            //These tests should pass as they all have values > 0
            SetupResult.For(assetLongTerm.LiabilityValue).Return(1000);
            ExecuteRule(rule, 0, assetLongTerm);

            SetupResult.For(assetLiabilityLiabilityLoan.LiabilityValue).Return(1000);
            ExecuteRule(rule, 0, assetLiabilityLiabilityLoan);
            
            SetupResult.For(assetLiabilityLiabilitySurety.LiabilityValue).Return(1000);
            ExecuteRule(rule, 0, assetLiabilityLiabilitySurety);
        }

        /// <summary>
        /// Asset Liability Liability Value Min Helper
        /// </summary>
        [NUnit.Framework.Test]
        public void AssetLiabilityLiabilityValueMinFail()
        {
            AssetLiabilityLiabilityValueMin rule = new AssetLiabilityLiabilityValueMin();

            IAssetLiabilityFixedLongTermInvestment assetLongTerm = _mockery.StrictMock<IAssetLiabilityFixedLongTermInvestment>();
            IAssetLiabilityLiabilityLoan assetLiabilityLiabilityLoan = _mockery.StrictMock<IAssetLiabilityLiabilityLoan>();
            IAssetLiabilityLiabilitySurety assetLiabilityLiabilitySurety = _mockery.StrictMock<IAssetLiabilityLiabilitySurety>();

            //These tests should pass as they all have values > 0
            SetupResult.For(assetLongTerm.LiabilityValue).Return(0);
            ExecuteRule(rule, 1, assetLongTerm);

            SetupResult.For(assetLiabilityLiabilityLoan.LiabilityValue).Return(0);
            ExecuteRule(rule, 1, assetLiabilityLiabilityLoan);

            SetupResult.For(assetLiabilityLiabilitySurety.LiabilityValue).Return(0);
            ExecuteRule(rule, 1, assetLiabilityLiabilitySurety);
        }
    }
}
