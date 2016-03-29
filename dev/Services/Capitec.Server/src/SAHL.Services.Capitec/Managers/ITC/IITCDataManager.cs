using SAHL.Core.Data.Models.Capitec;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Capitec.Managers.ITC
{
    public interface IITCDataManager
    {
        List<ITCDataModel> GetItcModelsForPerson(string identityNumber);

        ITCDataModel GetItcById(Guid itcID);

        PersonITCDataModel GetPersonITC(Guid personID);

        void UpdatePersonItc(Guid personID, Guid itcID, DateTime itcDate);

        void SavePersonItc(Guid personID, Guid itcID, DateTime itcDate);

        void SaveITC(Guid itcID, DateTime itcDate, string itcData);
    }
}