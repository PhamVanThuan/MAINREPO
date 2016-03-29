using ObjectMaps.Presenters.LoanServicing;

namespace BuildingBlocks.Presenters.LoanServicing
{
    public class RollbackTransaction : TransactionRollBackControls
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="financialTransactionKey"></param>
        public void SelectFinancialTransactionAndRollback(int financialTransactionKey)
        {
            bool result = false;
            //page through the worklist
            if (base.dxgvPagerBottomPanel.Exists)
            {
                while (!base.DisabledNext.Exists)
                {
                    result = base.TransactionExists(financialTransactionKey);
                    if (result)
                    {
                        base.SelectTransactionRow(financialTransactionKey).Click();
                        break;
                    }
                    if (base.Next.Enabled)
                    {
                        base.Next.Click();
                    }
                }
                //check again on the last page
                result = base.TransactionExists(financialTransactionKey);
                if (result)
                {
                    base.SelectTransactionRow(financialTransactionKey).Click();
                }
            }
            else
            {
                base.SelectTransactionRow(financialTransactionKey).Click();
            }
            base.btnRollback.Click();
        }
    }
}