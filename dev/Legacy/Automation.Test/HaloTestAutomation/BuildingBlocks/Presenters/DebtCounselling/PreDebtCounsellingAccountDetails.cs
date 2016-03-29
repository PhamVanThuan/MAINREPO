using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;

using System;
using System.Linq;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class PreDebtCounsellingAccountDetails : ObjectMaps.Pages.PreDebtCounsellingAccountDetailControls
    {
        public void AssertSnapshotDisplay
            (
                ProductEnum expectedProduct,
                params Automation.DataModels.SnapshotAccount[] accountSnapshots
            )
        {
            //Assert the product.
            Assert.That((base.Product.Text == expectedProduct.ToString() ||
                            base.Product.Exists
                         ), "Product decription not displayed.");

            var installmentAmount = 0.00;
            var interestRate = 0.00;
            var calculatedInterestRate = 0.00;
            var term = 0;
            var hocPremium = 0.00;
            var lifePremium = 0.00;
            var marketRate = 0.00;
            var linkRate = 0.00;

            #region Check Varifix Leg

            var fixedSnapshot = (from acc in accountSnapshots
                                 where acc.SnapshotFinancialService != null
                                     && acc.SnapshotFinancialService.FinancialServiceTypeKey == FinancialServiceTypeEnum.FixedLoan
                                 select acc).FirstOrDefault();
            if (fixedSnapshot != null)
            {
                fixedSnapshot.SnapshotFinancialService.ActiveMarketRate
                    = Math.Round(fixedSnapshot.SnapshotFinancialService.ActiveMarketRate * 100, 2);
                fixedSnapshot.SnapshotFinancialService.Margin = Math.Round(fixedSnapshot.SnapshotFinancialService.Margin * 100, 2);

                installmentAmount = Double.Parse(base.FixedInstallment.Text.Replace("R ", ""));
                interestRate = Double.Parse(base.FixedInterestRate.Text.Replace("%", ""));
                marketRate = Double.Parse(base.FixedMarketRate.Text.Replace("%", ""));
                linkRate = Double.Parse(base.FixedLinkRate.Text.Replace("%", ""));

                calculatedInterestRate
                    = (fixedSnapshot.SnapshotFinancialService.Margin + fixedSnapshot.SnapshotFinancialService.ActiveMarketRate);
                calculatedInterestRate = Math.Round(calculatedInterestRate, 2);

                Assert.AreEqual(installmentAmount, fixedSnapshot.SnapshotFinancialService.Installment);
                Assert.AreEqual(interestRate, calculatedInterestRate);
                Assert.AreEqual(marketRate, fixedSnapshot.SnapshotFinancialService.ActiveMarketRate);
                Assert.AreEqual(linkRate, fixedSnapshot.SnapshotFinancialService.Margin);
            }

            #endregion Check Varifix Leg

            #region Check Variable Leg

            var variableSnapshot = (from acc in accountSnapshots
                                    where acc.SnapshotFinancialService != null
                                        && acc.SnapshotFinancialService.FinancialServiceTypeKey == FinancialServiceTypeEnum.VariableLoan
                                    select acc).FirstOrDefault();

            variableSnapshot.SnapshotFinancialService.ActiveMarketRate
                = Math.Round(variableSnapshot.SnapshotFinancialService.ActiveMarketRate * 100, 2);
            variableSnapshot.SnapshotFinancialService.Margin = Math.Round(variableSnapshot.SnapshotFinancialService.Margin * 100, 2);

            calculatedInterestRate
                = (variableSnapshot.SnapshotFinancialService.Margin + variableSnapshot.SnapshotFinancialService.ActiveMarketRate);
            calculatedInterestRate = Math.Round(calculatedInterestRate, 2);

            installmentAmount = Double.Parse(base.VariableInstallment.Text.Replace("R ", ""));
            interestRate = Double.Parse(base.VariableInterestRate.Text.Replace("%", ""));
            marketRate = Double.Parse(base.VariableMarketRate.Text.Replace("%", ""));
            linkRate = Double.Parse(base.VariableLinkRate.Text.Replace("%", ""));
            term = Int32.Parse(base.Term.Text);
            hocPremium = Double.Parse(base.HOCPremium.Text.Replace("R ", ""));
            lifePremium = Double.Parse(base.LifePremium.Text.Replace("R ", ""));

            Assert.AreEqual(installmentAmount, variableSnapshot.SnapshotFinancialService.Installment);
            Assert.AreEqual(interestRate, calculatedInterestRate);
            Assert.AreEqual(term, variableSnapshot.RemainingInstallments);
            Assert.AreEqual(marketRate, variableSnapshot.SnapshotFinancialService.ActiveMarketRate);
            Assert.AreEqual(linkRate, variableSnapshot.SnapshotFinancialService.Margin);
            Assert.AreEqual(hocPremium, variableSnapshot.HOCPremium);
            Assert.AreEqual(lifePremium, variableSnapshot.LifePremium);

            #endregion Check Variable Leg

            #region Check RateOverrides

            var expectedRateoverrides = from acc in accountSnapshots
                                        where acc.SnapshotFinancialService.SnapshotRateOverride != null
                                        select acc.SnapshotFinancialService.SnapshotRateOverride;
            foreach (var rateOverride in expectedRateoverrides)
            {
                var rateOverrideRow = base.RateOverrides.FindRow(rateOverride.RateOverrideTypeDescription.RemoveWhiteSpace(), 4);
                Assert.That(rateOverrideRow != null, "Expected a rate override of type: {0}", rateOverride.RateOverrideTypeDescription.RemoveWhiteSpace());
                //re-used the rateoverride grid from the update rate override screen and this grid displays the dates
                // in yyyy/MM/dd
                if (rateOverride.FromDate != null)
                    Assert.AreEqual(rateOverrideRow.TableCells[1].Text, rateOverride.FromDate.Value.ToString(Formats.DateFormat2));
                if (rateOverride.EndDate != null)
                    Assert.AreEqual(rateOverrideRow.TableCells[2].Text, rateOverride.EndDate.Value.ToString(Formats.DateFormat2));
                Assert.AreEqual(rateOverrideRow.TableCells[3].Text, rateOverride.Discount.ToString());
            }

            #endregion Check RateOverrides
        }
    }
}