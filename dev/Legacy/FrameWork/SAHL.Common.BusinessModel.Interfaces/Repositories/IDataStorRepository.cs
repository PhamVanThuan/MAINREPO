using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IDataStorRepository
    {
        /// <summary>
        /// Creates and Empty Data object
        /// </summary>
        /// <returns>IData</returns>
        IData CreateEmptyData();

        /// <summary>
        /// Adds/Updates Data object
        /// </summary>
        /// <param name="data">The IData entity.</param>
        void SaveData(IData data);

        /// <summary>
        /// Adds/Updates Data object
        /// </summary>
        /// <param name="dataList">The List&lt;IData&gt; entity.</param>
        void SaveDataList(List<IData> dataList);

        /// <summary>
        /// Returns the STOR record for a specified STOR Name
        /// </summary>
        /// <param name="storName">The string Stor Name</param>
        ISTOR GetSTORByName(string storName);
    }
}