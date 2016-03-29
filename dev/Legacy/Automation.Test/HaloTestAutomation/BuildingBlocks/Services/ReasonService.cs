using Automation.DataAccess;
using Automation.DataAccess.DataHelper._2AM.Contracts;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class ReasonService : IReasonService
    {
        private IReasonDataHelper dataHelper;
        private IApplicationDataHelper applicationDataHelper;

        public ReasonService(IReasonDataHelper dataHelper, IApplicationDataHelper applicationDataHelper)
        {
            this.dataHelper = dataHelper;
            this.applicationDataHelper = applicationDataHelper;
        }

        /// <summary>
        /// Get all the reasondescriptions and keys in 2am by reasontype.
        /// </summary>
        /// <param name="reasonType"></param>
        /// <param name="allowComment"></param>
        /// <returns>Dictionary (key=ReasonDescriptionKey and value=Description</returns>
        public Dictionary<int, string> GetReasonDescriptionsByReasonType(string reasonType, bool allowComment)
        {
            var results = (from r in dataHelper.GetActiveReasonsByReasonType(reasonType)
                           where r.Column("AllowComment").GetValueAs<bool>() == allowComment
                           select r).AsQueryable();
            var reasonDescriptions = new Dictionary<int, string>();
            foreach (QueryResultsRow description in results)
            {
                reasonDescriptions.Add(int.Parse(description.Column("ReasonDescriptionKey").Value), description.Column("Description").Value);
            }
            return reasonDescriptions;
        }

        public bool ReasonExistsAgainstGenericKey(string reasonDescription, string reasonTypeDescription, int genericKey, GenericKeyTypeEnum genKey)
        {
            QueryResults results = null;
            switch (genKey)
            {
                case GenericKeyTypeEnum.OfferInformation_OfferInformationKey:
                    QueryResults oiKeys = applicationDataHelper.GetLatestOfferInformationByOfferKey(genericKey);
                    int offerInformationKey = oiKeys.Rows(0).Column("OfferInformationKey").GetValueAs<int>();
                    oiKeys.Dispose();
                    results = dataHelper.GetReasonsByGenericKeyAndGenericKeyType(offerInformationKey, genKey);
                    break;

                default:
                    results = dataHelper.GetReasonsByGenericKeyAndGenericKeyType(genericKey, genKey);
                    break;
            }
            //we need to loop through the reasons
            bool exists = false;
            QueryResultsRow reasonCount = null;
            if (results != null)
            {
                reasonCount = (from r in results
                               where r.Column("ReasonDescription").Value == reasonDescription && r.Column("ReasonTypeDescription").Value == reasonTypeDescription
                               select r).FirstOrDefault();
            }
            return exists = results != null && reasonCount != null ? true : false;
        }

        public Automation.DataModels.ReasonDefinition GetReasonDefinition(ReasonTypeEnum reasonType, string reasonDescription)
        {
            var reasonDefinition = dataHelper.GetReasonDefinition(reasonDescription, reasonType);
            return reasonDefinition.FirstOrDefault();
        }

        public void RemoveReasonsAgainstGenericKeyByReasonType(int genericKey, GenericKeyTypeEnum genericKeyType, ReasonTypeEnum reasonTypeKey)
        {
            dataHelper.RemoveReasonsAgainstGenericKeyByReasonType(genericKey, genericKeyType, reasonTypeKey);
        }

        public void InsertReason(int genericKey, string reasonDescription, ReasonTypeEnum reasonType, GenericKeyTypeEnum genericKeyType)
        {
            dataHelper.InsertReason(genericKey, reasonDescription, reasonType, genericKeyType);
        }

        public QueryResults GetActiveReasonsByReasonType(string ReasonType)
        {
            return dataHelper.GetActiveReasonsByReasonType(ReasonType);
        }

        public QueryResults GetReasonsByGenericKeyAndGenericKeyType(int genericKey, GenericKeyTypeEnum genericKeyType)
        {
            return dataHelper.GetReasonsByGenericKeyAndGenericKeyType(genericKey, genericKeyType);
        }

        public QueryResults GetNotificationReasonsForLegalEntity(int reasonDefinition, int accountKey)
        {
            return dataHelper.GetNotificationReasonsForLegalEntity(reasonDefinition, accountKey);
        }
    }
}