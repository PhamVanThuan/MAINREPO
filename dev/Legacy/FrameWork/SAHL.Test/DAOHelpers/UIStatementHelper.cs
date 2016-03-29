using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Test.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="UIStatement_DAO"/> domain entity.
    /// </summary>
    public class UIStatementHelper : BaseHelper<UIStatement_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="UIStatement_DAO"/> entity.
        /// </summary>
        /// <returns>A new UIStatement_DAO entity (not yet persisted).</returns>
        public UIStatement_DAO CreateUIStatement()
        {
            UIStatement_DAO UIStatement = new UIStatement_DAO();
            UIStatement.ApplicationName = "Test Application Name 1";
            UIStatement.ModifyDate = DateTime.Now;
            UIStatement.ModifyUser = "Mod User";
            UIStatement.Statement = "Test Statement";
            UIStatement.StatementName = "Test Statement Name";
            UIStatement.uiStatementType = UIStatementType_DAO.FindFirst();
            UIStatement.Version = 1;
            CreatedEntities.Add(UIStatement);
            return UIStatement;
        }

        /// <summary>
        /// Ensures that all StageTransitions created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (UIStatement_DAO UIStatement in CreatedEntities)
            {
                if (UIStatement.Key > 0)
                    TestBase.DeleteRecord("UIStatement", "StatementKey", UIStatement.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}

