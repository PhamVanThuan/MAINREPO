using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.TranslatedText_DAO
    /// </summary>
    public partial interface ITranslatedText : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TranslatedText_DAO.Text
        /// </summary>
        System.String Text
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TranslatedText_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TranslatedText_DAO.TranslatableItem
        /// </summary>
        ITranslatableItem TranslatableItem
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TranslatedText_DAO.Language
        /// </summary>
        ILanguage Language
        {
            get;
            set;
        }
    }
}