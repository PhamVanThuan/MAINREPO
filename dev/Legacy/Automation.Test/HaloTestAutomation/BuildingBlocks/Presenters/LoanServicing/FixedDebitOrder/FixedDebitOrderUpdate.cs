using BuildingBlocks.Services.Contracts;
using Common.Constants;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.LoanServicing.FixedDebitOrders
{
    public class FixedDebitOrderUpdate : FixedDebitOrderUpdateControls
    {
        private readonly ICommonService commonService;

        public FixedDebitOrderUpdate()
        {
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="businessDay"></param>
        /// <param name="prevDayNonBusinessDay"></param>
        public void UpdateFixedDebitOrder(double amount, bool businessDay, bool prevDayNonBusinessDay)
        {
            var date = commonService.GetDateWithBusinessDayCheck(businessDay, prevDayNonBusinessDay);
            base.FixedDebitOrderAmount.Clear();
            base.FixedDebitOrderAmount.Value = amount.ToString();
            base.EffectiveDate.Clear();
            base.EffectiveDate.Value = date.ToString(Formats.DateFormat);
            base.btnSubmit.Click();
        }

        public void UpdateFixedDebitOrder(double amount, DateTime? dateTime)
        {
            base.FixedDebitOrderAmount.Value = amount.ToString();
            if (dateTime == null)
            {
                base.EffectiveDate.Clear();
            }
            else
            {
                base.EffectiveDate.Value = dateTime.Value.ToString(Formats.DateFormat);
            }
            base.btnSubmit.Click();
        }
    }
}