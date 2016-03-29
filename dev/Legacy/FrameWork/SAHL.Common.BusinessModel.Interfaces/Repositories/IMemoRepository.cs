using System;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IMemoRepository
    {
        IMemo GetMemoByKey(int Key);

        IEventList<IMemo> GetMemoRelatedToAccount(int GenericKey, int GenericKeyTypeKey, int Status);

        IEventList<IMemo> GetMemoByGenericKey(int GenericKey, int GenericKeyTypeKey);

        IEventList<IMemo> GetMemoByGenericKey(int GenericKey, int GenericKeyTypeKey, int GeneralStatusKey);

        IMemo CreateMemo();

        void SaveMemo(IMemo memo);

        IInstance GetInstanceForMemoKeyAndApplicationKeyForOriginationWorkflow(int GenericKey, int emoKey);

        /// <summary>
        /// Get all Memos for a GenericKey created by the ADUser on the date specified
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="dt"></param>
        /// <param name="adUser"></param>
        /// <returns></returns>
        IEventList<IMemo> GetMemoByGenericKeyADUserAndDate(int genericKey, int genericKeyTypeKey, DateTime dt, IADUser adUser);
    }
}