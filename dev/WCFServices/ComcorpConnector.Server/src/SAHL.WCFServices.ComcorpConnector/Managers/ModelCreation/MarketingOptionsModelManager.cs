using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class MarketingOptionsModelManager : IMarketingOptionsModelManager
    {
        public List<ApplicantMarketingOptionModel> PopulateMarketingOptions(Applicant comcorpApplicant)
        {
            List<ApplicantMarketingOptionModel> applicantMarketingOptions = new List<ApplicantMarketingOptionModel>();
            if (comcorpApplicant.MarketingTelemarketing)
            {
                applicantMarketingOptions.Add(new ApplicantMarketingOptionModel(MarketingOption.Telemarketing, GeneralStatus.Active));
            }
            if (comcorpApplicant.MarketingMarketing)
            {
                applicantMarketingOptions.Add(new ApplicantMarketingOptionModel(MarketingOption.Marketing, GeneralStatus.Active));
            }
            if (comcorpApplicant.MarketingEmail)
            {
                applicantMarketingOptions.Add(new ApplicantMarketingOptionModel(MarketingOption.Email, GeneralStatus.Active));
            }
            if (comcorpApplicant.MarketingSMS)
            {
                applicantMarketingOptions.Add(new ApplicantMarketingOptionModel(MarketingOption.SMS, GeneralStatus.Active));
            }
            if (comcorpApplicant.MarketingConsumerLists)
            {
                applicantMarketingOptions.Add(new ApplicantMarketingOptionModel(MarketingOption.CustomerLists, GeneralStatus.Active));
            }
            return applicantMarketingOptions;
        }
    }
}