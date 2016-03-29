using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.TestWTF.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="UIStatementType_DAO"/> domain entity.
    /// </summary>
    public class UIStatementTypeHelper : BaseHelper<UIStatementType_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="UIStatementType_DAO"/> entity.
        /// </summary>
        /// <returns>A new UIStatementType_DAO entity (not yet persisted).</returns>
        public UIStatementType_DAO CreateUIStatementType()
        {
            UIStatementType_DAO UIStatementType = new UIStatementType_DAO();
            UIStatementType.Description = "Test Description"; 
            CreatedEntities.Add(UIStatementType);
            return UIStatementType;
        }

        /// <summary>
        /// Ensures that all StageTransitions created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (UIStatementType_DAO UIStatementType in CreatedEntities)
            {
                if (UIStatementType.Key > 0)
                    TestBase.DeleteRecord("UIStatementType", "StatementTypeKey", UIStatementType.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}

