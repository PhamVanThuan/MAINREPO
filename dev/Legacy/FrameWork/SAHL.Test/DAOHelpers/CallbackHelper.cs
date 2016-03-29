using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Test.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="Callback_DAO"/> domain entity.
    /// </summary>
    public class CallbackHelper : BaseHelper<Callback_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="Callback_DAO"/> entity.
        /// </summary>
        /// <returns>A new Callback_DAO entity (not yet persisted).</returns>
        public Callback_DAO CreateCallback()
        {
            DateTime today = System.DateTime.Now;

            Callback_DAO callback = new Callback_DAO();
            callback.GenericKey = 9876789;
            callback.GenericKeyType = GenericKeyType_DAO.FindFirst();
            callback.CallbackDate = today.AddDays(1);
            callback.CallbackUser = "Test";
            callback.EntryDate = today;
            callback.EntryUser = "Test";
            callback.Reason = Reason_DAO.FindFirst();

            CreatedEntities.Add(callback);

            return callback;
        }

        /// <summary>
        /// Ensures that all Callbacks created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (Callback_DAO callback in CreatedEntities)
            {
                if (callback.Key > 0)
                    TestBase.DeleteRecord("Callback", "CallbackKey", callback.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}

