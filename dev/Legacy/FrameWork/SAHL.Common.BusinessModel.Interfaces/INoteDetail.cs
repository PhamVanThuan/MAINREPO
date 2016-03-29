using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO
    /// </summary>
    public partial interface INoteDetail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.Tag
        /// </summary>
        System.String Tag
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.WorkflowState
        /// </summary>
        System.String WorkflowState
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.InsertedDate
        /// </summary>
        System.DateTime InsertedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.NoteText
        /// </summary>
        System.String NoteText
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.Note
        /// </summary>
        INote Note
        {
            get;
            set;
        }
    }
}