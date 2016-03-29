using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.TestWTF.DAOHelpers
{
    public class HOCHelper : BaseHelper<HOC_DAO>
    {

        /// <summary>
        /// Creates a new <see cref="HOC_DAO"/> entity.
        /// </summary>
        /// <returns>A new HOC_DAO entity (not yet persisted).</returns>
        public HOC_DAO Create()
        {
            HOC_DAO hoc = new HOC_DAO();
            hoc.Ceded = false;
            hoc.Account = Account_DAO.FindFirst();
            hoc.ChangeDate = DateTime.Now;
            hoc.HOCStatus = HOCStatus_DAO.FindFirst();
            hoc.UserID = TestConstants.UnitTestUserID;

            CreatedEntities.Add(hoc);

            return hoc;
        }

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
