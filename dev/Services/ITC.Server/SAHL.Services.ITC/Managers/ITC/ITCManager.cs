using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Managers.Itc
{
    public class ItcManager : IItcManager
    {
        private ILogger logger;
        private ILoggerSource loggerSource;
        private IItcDataManager dataManager;
        private ICombGuid combGuid;

        public ItcManager(ILogger logger, ILoggerSource loggerSource, IItcDataManager dataManager, ICombGuid combGuid)
        {
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.dataManager = dataManager;
            this.combGuid = combGuid;
        }

        public ItcRequest CreateITCRequestForApplicant(ApplicantITCRequestDetailsModel applicantITCRequestDetails)
        {
            string birthDate = String.Empty;
            if (applicantITCRequestDetails.DateOfBirth.HasValue)
            {
                birthDate = applicantITCRequestDetails.DateOfBirth.Value.ToString("yyyyMMdd");
            }
            var itcRequest = new ItcRequest
            {
                Forename1 = applicantITCRequestDetails.FirstName,
                Surname = applicantITCRequestDetails.Surname,
                BirthDate = birthDate,
                IdentityNo1 = applicantITCRequestDetails.IdentityNumber,
                Title = applicantITCRequestDetails.Title,
                HomeTelNo = applicantITCRequestDetails.HomePhoneNumber,
                WorkTelNo = applicantITCRequestDetails.WorkPhoneNumber,
                CellNo = applicantITCRequestDetails.CellPhoneNumber,
                EmailAddress = applicantITCRequestDetails.EmailAddress,
                AddressLine1 = applicantITCRequestDetails.AddressLine1,
                AddressLine2 = applicantITCRequestDetails.AddressLine2,
                Suburb = applicantITCRequestDetails.Suburb,
                City = applicantITCRequestDetails.City,
                PostalCode = applicantITCRequestDetails.PostalCode
            };
            return itcRequest;
        }

        public ItcRequest CreateITCRequestForApplicant(ClientDetailsQueryResult clientDetail, IEnumerable<GetClientStreetAddressByClientKeyQueryResult> clientStreetAddresses)
        {
            ItcRequest itcRequest = null;

            if (clientStreetAddresses != null && clientDetail != null && clientStreetAddresses.Any())
            {
                var clientAddressList = clientStreetAddresses.ToList();

                itcRequest = new ItcRequest
                {
                    Forename1 = clientDetail.FirstNames,
                    Surname = clientDetail.Surname,
                    BirthDate = clientDetail.DateOfBirth.Value.ToString("yyyyMMdd"),
                    IdentityNo1 = clientDetail.IDNumber,
                    Title = ((SalutationType)clientDetail.SalutationKey).ToString(),
                    HomeTelNo = string.Format("{0}{1}", clientDetail.HomePhoneCode, clientDetail.HomePhoneNumber),
                    WorkTelNo = string.Format("{0}{1}", clientDetail.WorkPhoneCode, clientDetail.WorkPhoneNumber),
                    CellNo = clientDetail.CellPhoneNumber,
                    EmailAddress = clientDetail.EmailAddress,

                    AddressLine1 = clientAddressList[0].StreetNumber,
                    AddressLine2 = clientAddressList[0].StreetName,
                    Suburb = clientAddressList[0].Suburb,
                    City = clientAddressList[0].City,
                    PostalCode = clientAddressList[0].PostalCode,
                };

                if (clientAddressList.Count > 1)
                {
                    itcRequest.Address2Line1 = clientAddressList[1].StreetNumber;
                    itcRequest.Address2Line2 = clientAddressList[1].StreetName;
                    itcRequest.Address2Suburb = clientAddressList[1].Suburb;
                    itcRequest.Address2PostalCode = clientAddressList[1].PostalCode;
                    itcRequest.Address2City = clientAddressList[1].City;
                }
            }

            return itcRequest;
        }

        public void SaveITC(Guid itcID, ItcResponse itcResponse)
        {
            dataManager.SaveITC(itcID, itcResponse.ITCDate, itcResponse.Response.Root.ToString());
        }

        public void SaveITC(int clientId, int? accountKey, ItcResponse itcResponse, string userId)
        {
            dataManager.SaveITC(clientId, accountKey, itcResponse.ITCDate, itcResponse.Request.Root.ToString(),
                itcResponse.Response.Root.ToString(), itcResponse.ResponseStatus.ToString(), userId);
        }

        private JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                ContractResolver = new DefaultContractResolver()
            };
        }

        public void LogFailedITCRequestAndResponse(ItcRequest itcRequest, ItcResponse itcResponse, string callingMethod)
        {
            var serializedRequest = JsonConvert.SerializeObject(itcRequest, GetJsonSerializerSettings());
            logger.LogError(loggerSource, "System", callingMethod, String.Format("Failed ITC Request object: {0}", serializedRequest));
            if (itcResponse != null)
            {
                var serializedResponse = JsonConvert.SerializeObject(itcResponse, GetJsonSerializerSettings());
                logger.LogError(loggerSource, "System", callingMethod, String.Format("Failed ITC Response object: {0}", serializedResponse));
            }
        }

        public ItcProfile GetITCProfile(Guid itcID)
        {
            var itcDataModel = dataManager.GetITCByID(itcID);
            if (itcDataModel == null)
            {
                return null;
            }
            var itcData = XDocument.Parse(itcDataModel.ITCData);
            var itcReader = new ItcReader(itcData);
            var itcProfile = itcReader.GetITCProfile;
            return itcProfile;
        }

        private const int daysItcIsValid = 31;

        public ItcProfile GetCurrentItcProfileForLegalEntity(string identityNumber)
        {
            var legalEntityItcs = dataManager.GetItcsForLegalEntity(identityNumber);
            var validItcDate = DateTime.Now.AddDays(daysItcIsValid * -1);
            var currentItc = legalEntityItcs.FirstOrDefault(x => x.ChangeDate >= validItcDate);
            if (currentItc != null)
            {
                var itcData = XDocument.Parse(currentItc.ResponseXML);
                var itcReader = new ItcReader(itcData);
                return itcReader.GetITCProfile;
            }
            return null;
        }
    }
}