using BuildingBlocks.Services.Contracts;
using Common.Constants;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class PaymentReceivedUpdate : PaymentReviewUpdateControls
    {
        private IAccountService accountservice;

        public PaymentReceivedUpdate()
        {
            accountservice = ServiceLocator.Instance.GetService<IAccountService>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        public void EnterPaymentReceivedDateGreaterThanToday()
        {
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            dt = dt.AddDays(2);
            base.txtPaymentReceived.Value = dt.ToString(Formats.DateFormat);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        /// <param name="accountKey"></param>
        public void EnterPaymentReceivedDateThatDoesNotMatchDebitOrderDay(int accountKey)
        {
            var debitOrderDay = accountservice.GetDebitOrderDayIncludingFutureDatedChanges(accountKey);
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            if (dt.Day == debitOrderDay)
            {
                dt = dt.AddDays(-1);
            }
            base.txtPaymentReceived.Value = dt.ToString(Formats.DateFormat);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        public void EnterBlankPaymentReceivedDate()
        {
            base.txtPaymentReceived.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        /// <param name="accountKey"></param>
        public string EnterPaymentReceivedDateThatMatchesDebitOrderDay(int accountKey)
        {
            var i = accountservice.GetDebitOrderDayIncludingFutureDatedChanges(accountKey);
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            while (dt.Day != i)
            {
                dt = dt.AddDays(-1);
            }
            base.txtPaymentReceived.Value = dt.ToString(Formats.DateFormat);
            return dt.ToString(Formats.DateFormat);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        public void EnterBlankPaymentReceivedAmount()
        {
            base.txtPaymentReceivedAmountRands.Clear();
            base.txtPaymentReceivedAmountCents.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        public void ClickUpdateButton()
        {
            base.btnUpdate.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        public void EnterZeroPaymentReceivedAmount()
        {
            base.txtPaymentReceivedAmountRands.TypeText("0");
            base.txtPaymentReceivedAmountCents.TypeText("00");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        public string EnterValidReviewDate()
        {
            var dt = new DateTime();
            dt = DateTime.Now;
            dt = dt.AddMonths(9);
            base.txtTermReviewDate.Value = dt.ToString(Formats.DateFormat);
            return dt.ToString(Formats.DateFormat);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        public decimal EnterValidPaymentReceivedAmount()
        {
            string rands = "1500";
            string cents = "00";
            base.txtPaymentReceivedAmountCents.Clear();
            base.txtPaymentReceivedAmountRands.Clear();
            base.txtPaymentReceivedAmountRands.TypeText(rands);
            base.txtPaymentReceivedAmountCents.TypeText(cents);
            decimal amt = decimal.Parse(string.Format("{0}.{1}", rands, cents));
            return amt;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        public void EnterReviewDateGreaterThan18Months()
        {
            var dt = new DateTime();
            dt = DateTime.Now;
            dt = dt.AddMonths(19);
            base.txtTermReviewDate.Value = dt.ToString(Formats.DateFormat);
        }
    }
}