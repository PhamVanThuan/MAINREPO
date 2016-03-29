using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.Valuations;

namespace SAHL.Common.BusinessModel.Rules.Test.Valuation
{
    [TestFixture]
    public class ValuationManualTest : RuleBase
    {
        IValuationDiscriminatedSAHLManual valuation;
        IValuationCottage valCottage;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            valuation = _mockery.StrictMock<IValuationDiscriminatedSAHLManual>();
            valCottage = _mockery.StrictMock<IValuationCottage>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void ManualValuationClassification_Pass()
        {
            ManualValuationClassification rule = new ManualValuationClassification();
            IValuationDiscriminatedSAHLManual valuationDiscriminatedSAHLManual = _mockery.StrictMock<IValuationDiscriminatedSAHLManual>();
            IValuationClassification valuationClassification = _mockery.StrictMock<IValuationClassification>();

            SetupResult.For(valuationDiscriminatedSAHLManual.ValuationClassification).Return(valuationClassification);
            SetupResult.For(valuationDiscriminatedSAHLManual.Key).Return(1);
            SetupResult.For(valuationDiscriminatedSAHLManual.IsActive).Return(true);
            ExecuteRule(rule, 0, valuationDiscriminatedSAHLManual);
        }

        [NUnit.Framework.Test]
        public void ManualValuationClassification_Fail_New()
        {
            ManualValuationClassification rule = new ManualValuationClassification();
            IValuationDiscriminatedSAHLManual valuationDiscriminatedSAHLManual = _mockery.StrictMock<IValuationDiscriminatedSAHLManual>();

            SetupResult.For(valuationDiscriminatedSAHLManual.ValuationClassification).Return(null);
            SetupResult.For(valuationDiscriminatedSAHLManual.Key).Return(0);
            SetupResult.For(valuationDiscriminatedSAHLManual.IsActive).Return(false);
            ExecuteRule(rule, 1, valuationDiscriminatedSAHLManual);
        }

        [NUnit.Framework.Test]
        public void ManualValuationClassification_Fail_Existing()
        {
            ManualValuationClassification rule = new ManualValuationClassification();
            IValuationDiscriminatedSAHLManual valuationDiscriminatedSAHLManual = _mockery.StrictMock<IValuationDiscriminatedSAHLManual>();

            SetupResult.For(valuationDiscriminatedSAHLManual.ValuationClassification).Return(null);
            SetupResult.For(valuationDiscriminatedSAHLManual.Key).Return(1);
            SetupResult.For(valuationDiscriminatedSAHLManual.IsActive).Return(true);
            ExecuteRule(rule, 1, valuationDiscriminatedSAHLManual);
        }
 

        //#region Improvement Validation
        //[NUnit.Framework.Test]
        //public void ManualValuationImprovementMandatory_Pass()
        //{
        //    ManualValuationImprovementMandatory rule = new ManualValuationImprovementMandatory();

        //    IValuationImprovement valuationImprovement = _mockery.StrictMock<IValuationImprovement>();
        //    IValuationImprovementType valuationImprovementType = _mockery.StrictMock<IValuationImprovementType>();

        //    SetupResult.For(valuationImprovement.ValuationImprovementType).Return(valuationImprovementType);
        //    double impovementValue = 100;
        //    DateTime improvementDate = System.DateTime.Now;
        //    SetupResult.For(valuationImprovement.ImprovementValue).Return(impovementValue);
        //    SetupResult.For(valuationImprovement.ImprovementDate).Return(improvementDate);

        //    ExecuteRule(rule, 0, valuationImprovement);
        //}

        //[NUnit.Framework.Test]
        //public void ManualValuationImprovementMandatory_Fail_ImprovementType()
        //{
        //    ManualValuationImprovementMandatory rule = new ManualValuationImprovementMandatory();

        //    IValuationImprovement valuationImprovement = _mockery.StrictMock<IValuationImprovement>();

        //    SetupResult.For(valuationImprovement.ValuationImprovementType).Return(null);
        //    double impovementValue = 100;
        //    DateTime improvementDate = System.DateTime.Now;
        //    SetupResult.For(valuationImprovement.ImprovementValue).Return(impovementValue);
        //    SetupResult.For(valuationImprovement.ImprovementDate).Return(improvementDate);

        //    ExecuteRule(rule, 1, valuationImprovement);
        //}

        //[NUnit.Framework.Test]
        //public void ManualValuationImprovementMandatory_Fail_ImprovementDate()
        //{
        //    ManualValuationImprovementMandatory rule = new ManualValuationImprovementMandatory();

        //    IValuationImprovement valuationImprovement = _mockery.StrictMock<IValuationImprovement>();
        //    IValuationImprovementType valuationImprovementType = _mockery.StrictMock<IValuationImprovementType>();

        //    SetupResult.For(valuationImprovement.ValuationImprovementType).Return(valuationImprovementType);
        //    double impovementValue = 100;
        //    DateTime? improvementDate = null;
        //    SetupResult.For(valuationImprovement.ImprovementValue).Return(impovementValue);
        //    SetupResult.For(valuationImprovement.ImprovementDate).Return(improvementDate);

        //    ExecuteRule(rule, 1, valuationImprovement);
        //}

        //[NUnit.Framework.Test]
        //public void ManualValuationImprovementMandatory_Fail_ImprovementValue()
        //{
        //    ManualValuationImprovementMandatory rule = new ManualValuationImprovementMandatory();

        //    IValuationImprovement valuationImprovement = _mockery.StrictMock<IValuationImprovement>();
        //    IValuationImprovementType valuationImprovementType = _mockery.StrictMock<IValuationImprovementType>();

        //    SetupResult.For(valuationImprovement.ValuationImprovementType).Return(valuationImprovementType);
        //    double impovementValue = 0;
        //    DateTime improvementDate = System.DateTime.Now;
        //    SetupResult.For(valuationImprovement.ImprovementValue).Return(impovementValue);
        //    SetupResult.For(valuationImprovement.ImprovementDate).Return(improvementDate);

        //    ExecuteRule(rule, 1, valuationImprovement);
        //}

        //[NUnit.Framework.Test]
        //public void ManualValuationImprovementMandatory_Fail_ImprovementDateAndValue()
        //{
        //    ManualValuationImprovementMandatory rule = new ManualValuationImprovementMandatory();

        //    IValuationImprovement valuationImprovement = _mockery.StrictMock<IValuationImprovement>();
        //    IValuationImprovementType valuationImprovementType = _mockery.StrictMock<IValuationImprovementType>();

        //    SetupResult.For(valuationImprovement.ValuationImprovementType).Return(valuationImprovementType);
        //    double impovementValue = 0;
        //    DateTime? improvementDate = null;
        //    SetupResult.For(valuationImprovement.ImprovementValue).Return(impovementValue);
        //    SetupResult.For(valuationImprovement.ImprovementDate).Return(improvementDate);

        //    ExecuteRule(rule, 2, valuationImprovement);
        //}
        //#endregion
    }
}
