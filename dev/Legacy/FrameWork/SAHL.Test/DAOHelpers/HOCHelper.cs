using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.Test.DAOHelpers
{
    public class HOCHelper : BaseHelper<HOC_DAO>
    {

        /// <summary>
        /// Ensures that all hoc accounts created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (HOC_DAO hoc in CreatedEntities)
            {
                if (hoc.Key > 0)
                {
                    TestBase.DeleteRecord("HOC", "FinancialServiceKey", hoc.Key.ToString());
                    TestBase.DeleteRecord("FinancialService", "FinancialServiceKey", hoc.Key.ToString());
                }
            }

            CreatedEntities.Clear();
        }
    }
}
