using System;
using System.Linq;
using System.Collections.Generic;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.Capitec.Tests.TestUIStatements;
using SAHL.Services.Capitec.Tests.TestModels;

namespace SAHL.Services.Capitec.Tests.Fees
{
    public sealed class FeeCalculator
    {
        private decimal buffer;
        public FeeCalculator(decimal initiationFee, decimal buffer)
        {
            this.buffer = buffer;
            this.InitiationFee = initiationFee;
        }
        public void CalculateFees(decimal loanAmount, OfferType offerType, decimal bondRequired, decimal cashOut, bool addInterimInterest)
        {
            var sqlStatement = new GetFees();
            var allFees = StatementRunner.RunSqlStatement<FeeRecord>(sqlStatement);

            decimal amount = 0;
            decimal lval = 0;
            decimal dval = 0;
            decimal bondToRegister;
            bool capitaliseFees;
            decimal bankRate = 0.083M;
            int noOfWeeksForInterimInterest = 12;
            decimal interimInterestAmount = 0;

            capitaliseFees = false;

            if (offerType == OfferType.SwitchLoan)
                capitaliseFees = true;

            if (addInterimInterest)
            {
                interimInterestAmount = (((loanAmount - cashOut) * (bankRate)) / 52) * noOfWeeksForInterimInterest;
                loanAmount = loanAmount + interimInterestAmount;
            }

            this.CancelFee = 0;
            this.RegFee = 0;

            var fees = allFees.Where(x => x.FeeRange >= loanAmount).FirstOrDefault();
            if (offerType == OfferType.SwitchLoan) // only switch gets cancel fee
                this.CancelFee = fees.FeeCancelDuty + fees.FeeCancelConveyancing + fees.FeeCancelVAT;

            this.CalculateRegistrationFee(offerType, fees);

            if (capitaliseFees)
                loanAmount = loanAmount + this.RegFee + this.InitiationFee + this.CancelFee;

            //Calc Bond To Register
            amount = loanAmount;

            bondToRegister = 0;

            if (amount > 0)
            {
                amount = amount + this.buffer;
                lval = Math.Round((amount / 1000), 0);
                dval = lval * 1000;
                if (dval > amount)
                {
                    bondToRegister = dval;
                }
                else
                {
                    bondToRegister = (lval + 1) * 1000;
                    var fees2 = allFees.Where(x => x.FeeRange >= bondToRegister).FirstOrDefault();
                    bondToRegister = fees2.FeeRange;
                }
            }
            else
            {
                if (bondRequired < amount)
                {
                    bondToRegister = amount;
                }
                else
                {
                    bondToRegister = bondRequired;
                }
            } 

            //if the calculated bondtoreg < what the client wants, reset
            if (bondToRegister < bondRequired)
                bondToRegister = bondRequired;

            amount = Math.Round(bondToRegister, 0);

            var feeRangeAfterBondCalc = allFees.Where(x => x.FeeRange >= amount).FirstOrDefault();

            this.CalculateRegistrationFee(offerType, feeRangeAfterBondCalc);

            //set final output variables
            var regFee = Convert.ToDecimal(this.RegFee);
            this.RegFee = Math.Round(regFee, 2);

            var canFee = Convert.ToDecimal(this.CancelFee);
            this.CancelFee = Math.Round(canFee, 2); ;

            this.InterimInterest = Math.Round(interimInterestAmount,2);
        }

        private void CalculateRegistrationFee(OfferType offerType, FeeRecord fees)
        {
            var feeBondStamps = fees.FeeBondStamps;

            if (offerType == OfferType.FurtherLoan || offerType == OfferType.NewPurchaseLoan) // 80% fee for Further Loan & New Purchase
            {
                var feeBondConveyancing80Pct = fees.FeeBondConveyancing80Pct;
                var feeBondVAT80Pct = fees.FeeBondVAT80Pct;

                this.RegFee = feeBondStamps + feeBondConveyancing80Pct + feeBondVAT80Pct;
            }
            else
            {
                var feeBondConveyancing = fees.FeeBondConveyancing;
                var feeBondVAT = fees.FeeBondVAT;
                this.RegFee = feeBondStamps + feeBondConveyancing + feeBondVAT;
            }
        }

        public decimal CancelFee { get; set; }
        public decimal InitiationFee { get; set; }
        public decimal RegFee { get; set; }
        public decimal InterimInterest { get; set; }
    }
}
