using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ITC.Models;

namespace SAHL.Services.ITC.Managers.Itc
{
    public interface IItcManager
    {
        ItcRequest CreateITCRequestForApplicant(ApplicantITCRequestDetailsModel applicantITCRequestDetails);

        ItcRequest CreateITCRequestForApplicant(ClientDetailsQueryResult clientDetails, IEnumerable<GetClientStreetAddressByClientKeyQueryResult> clientStreetAddresses);

        void SaveITC(Guid itcID, ItcResponse itcResponse);

        void SaveITC(int clientId, int? accountNumber, ItcResponse itcResponse, string userId);

        void LogFailedITCRequestAndResponse(ItcRequest itcRequest, ItcResponse itcResponse, string callingMethod);

        ItcProfile GetITCProfile(Guid itcID);

        ItcProfile GetCurrentItcProfileForLegalEntity(string identityNumber);
    }
}