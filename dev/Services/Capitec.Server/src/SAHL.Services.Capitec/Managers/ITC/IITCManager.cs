using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Interfaces.ITC.Models;
using System;

namespace SAHL.Services.Capitec.Managers.ITC
{
    public interface IITCManager
    {
        ITCDataModel GetValidITCModelForPerson(string identityNumber);

        ITCDataModel GetITC(Guid itcID);

        ItcProfile GetITCProfile(Guid itcID);

        bool DoesITCPassQualificationRules(ItcProfile itc, Guid applicantID);

        ApplicantITCRequestDetailsModel CreateApplicantITCRequest(Interfaces.Capitec.ViewModels.Apply.Applicant applicant);

        void LinkItcToPerson(Guid personID, Guid itcID, DateTime itcDate);
    }
}