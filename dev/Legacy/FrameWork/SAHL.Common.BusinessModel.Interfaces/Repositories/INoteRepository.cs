using System.Collections.Generic;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface INoteRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        INote GetNoteByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        INoteDetail GetNoteDetailByKey(int key);

		INote GetNoteByGenericKeyAndType(int genericKey, int genericKeyTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        INote CreateEmptyNote();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        INoteDetail CreateEmptyNoteDetail();

        /// <summary>
        ///
        /// </summary>
        /// <param name="note"></param>
        void SaveNote(INote note);

        /// <summary>
        ///
        /// </summary>
        /// <param name="notedetail"></param>
        void SaveNoteDetail(INoteDetail notedetail);

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="GenericKeyTypeKey"></param>
        /// <returns></returns>
        List<INoteDetail> GetNoteDetailsByGenericKeyAndType(int GenericKey, int GenericKeyTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        List<INoteDetail> GetAllDebtcounsellingNoteDetailsForAccount(int accountKey);
    }
}