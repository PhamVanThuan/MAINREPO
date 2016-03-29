using Common.Constants;
using Common.Enums;
using Common.Extensions;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.LoanServicing.DebitOrderDetails
{
    public sealed class DebitOrderDetailsAddView : DebitOrderDetailsBaseControls
    {
        public void ClickButton(ButtonTypeEnum button)
        {
            switch (button)
            {
                case ButtonTypeEnum.Add:
                    base.Add.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.Cancel.Click();
                    break;

                default:
                    break;
            }
        }

        public void AddDebitOrder(string acbBankDescription, string acbBranchCode, string acbBranchDescription, string acbTypeDescription, string accountNumber, string accountName, int debitOrderDay, DateTime effectiveDate, ButtonTypeEnum button)
        {
            string bankDetails = string.Format(@"{0} - {1} - {2} - {3} - {4} - {5}",
                 acbBankDescription,
                 acbBranchCode,
                 acbBranchDescription,
                 acbTypeDescription,
                 accountNumber,
                 accountName).RemoveDoubleSpaces();

            base.DOPaymentType.Option(PaymentType.DebitOrderPayment).Select();
            base.DebitOrderDay.Option(debitOrderDay.ToString()).Select();
            base.EffectiveDate.Value = effectiveDate.ToString(Formats.DateFormat);
            ClickButton(button);
        }

        public void AddDebitOrder(PaymentType paymentType, int debitOrderDay, DateTime effectiveDate, ButtonTypeEnum button)
        {
            base.DOPaymentType.Option(paymentType.ToString()).Select();
            base.DebitOrderDay.Option(debitOrderDay.ToString()).Select();
            base.EffectiveDate.Value = effectiveDate.ToString(Formats.DateFormat);
            ClickButton(button);
        }
    }
}