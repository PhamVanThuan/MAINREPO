using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Capitec.QueryStatements;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Extensions;
using SAHL.Services.Capitec.Tests.TestModels;
using SAHL.Services.Capitec.Tests.TestUIStatements;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;
using System.Data;

namespace SAHL.Services.Capitec.Tests.Fees
{
    [TestFixture]
    public class Fees
    {
        private FeeCalculator feeCalc;

        [TestFixtureSetUp]
        public void SetUp()
        {
            var sqlStatement = new GetControlRecord();
            var controlTableValues = StatementRunner.RunSqlStatement<ControlRecord>(sqlStatement);

            var initiationFee = controlTableValues.Where(x => x.ControlDescription == "Calc - Initiation Fee Incl. VAT").FirstOrDefault();
            var buffer = controlTableValues.Where(x => x.ControlDescription == "Calc - FurtherLoanCapacity").FirstOrDefault();

            this.feeCalc = new FeeCalculator(
                    Convert.ToDecimal(initiationFee.ControlNumeric),
                    Convert.ToDecimal(buffer.ControlNumeric)
                );
        }

        [Test]
        [TestCaseSource("GetAllFees")]
        public void FeeTests(FeeRecord fee)
        {
            decimal loanAmount = 0;
            decimal bondRequired = 0;
            bool addInterimInterest = false;
            decimal cashOut = 0;

            foreach (decimal d in RangeCheck)
            {
                loanAmount = Convert.ToDecimal((fee.FeeRange)) + d;
                if (loanAmount > 0 && loanAmount < 9950000M)
                {
                    foreach (OfferType ot in Enum.GetValues(typeof(OfferType)))
                    {
                        if (ot == OfferType.SwitchLoan || ot == OfferType.NewPurchaseLoan)
                        {
                            bondRequired = loanAmount - 50;
                            if (ot == OfferType.SwitchLoan)
                            {
                                //we need a cash out value
                                cashOut = loanAmount * 0.1M;
                            }
                            this.AssertCalculations(loanAmount, ot, bondRequired, addInterimInterest, cashOut);
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void AssertCalculations(decimal loanAmount, OfferType appType, decimal bondRequired, bool addInterimInterest, decimal cashOut)
        {
            this.feeCalc.CalculateFees(loanAmount, appType, bondRequired, cashOut, addInterimInterest);

            var sqlStatement = new GetCalculatorFeeQuery((int)appType,loanAmount,cashOut);
            var serviceStatement = new GetCalculatorFeeQueryStatement();
            var results = StatementRunner.RunServiceQueryStatement<GetCalculatorFeeQueryResult>(serviceStatement, sqlStatement);

            Assert.NotNull(results, "GetCalculatorFeeQuery returned no results");
            Assert.AreEqual(this.feeCalc.InitiationFee, results.FirstOrDefault().InitiationFee, "Initiation Fee mismatch for test");
            Assert.AreEqual(this.feeCalc.RegFee, results.FirstOrDefault().RegistrationFee, "Registration Fee mismatch for test");
            Assert.AreEqual(this.feeCalc.CancelFee, results.FirstOrDefault().CancellationFee, "Cancel Fee mismatch for test");
            Assert.AreEqual(this.feeCalc.InterimInterest, results.FirstOrDefault().InterimInterest, "Interim Interest mismatch for test");
        }

        private bool CheckVals(decimal test, decimal sp, decimal fudge)
        {
            if (test <= (sp + fudge) && test >= (sp - fudge))
                return true;

            return false;
        }

        private List<decimal> RangeCheck
        {
            get
            {
                return new List<decimal> { -5.0M, 0.0M, 5.0M};
            }
        }

        private IEnumerable<FeeRecord> GetAllFees()
        {
            var sqlStatement = new GetFees();
            return StatementRunner.RunSqlStatement<FeeRecord>(sqlStatement);
        }
    }
}