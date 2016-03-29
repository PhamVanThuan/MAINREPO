using System;
using System.Collections.Generic;
using SAHL.Core.Data.Models.Capitec;

namespace SAHL.Services.ITC.Managers.Itc
{
    public interface IItcDataManager
    {
        void SaveITC(Guid itcID, DateTime itcDate, string itcData);

        void SaveITC(int clientKey, int? accountKey, DateTime itcDate, string itcRequestXML, string itcResponseXML, string responseStatus, string userId);

        SAHL.Core.Data.Models._2AM.ITCRequestDataModel GetITCByID(Guid itcID);

        IEnumerable<Core.Data.Models._2AM.ITCDataModel> GetItcsForLegalEntity(string identityNumber);
    }
}