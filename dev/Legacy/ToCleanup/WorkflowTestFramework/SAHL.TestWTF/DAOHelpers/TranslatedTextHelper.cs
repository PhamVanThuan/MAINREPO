using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.TestWTF.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="TranslatedText_DAO"/> domain entity.
    /// </summary>
    public class TranslatedTextHelper : BaseHelper<TranslatedText_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="TranslatedText_DAO"/> entity.
        /// </summary>
        /// <returns>A new TranslatedText_DAO entity (not yet persisted).</returns>
        public TranslatedText_DAO CreateTranslatedText()
        {
            TranslatedText_DAO TranslatedText = new TranslatedText_DAO();
            TranslatedText.Language = Language_DAO.FindFirst();
            TranslatedText.Text = "Text here";
            TranslatedText.TranslatableItem = TranslatableItem_DAO.FindFirst();
            CreatedEntities.Add(TranslatedText);
            return TranslatedText;
        }

        /// <summary>
        /// Ensures that all StageTransitions created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (TranslatedText_DAO TranslatedText in CreatedEntities)
            {
                if (TranslatedText.Key > 0)
                    TestBase.DeleteRecord("TranslatedText", "TranslatedTextKey", TranslatedText.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}

