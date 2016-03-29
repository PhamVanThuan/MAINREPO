using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace BuildingBlocks.Services
{
    public class ValuationService : _2AMDataHelper, IValuationService
    {
        private readonly IWebService webService;
        private IApplicationService appService;
        private IValuationWebService valuationWebService;

        public ValuationService(IWebService webservice, IApplicationService appService, IValuationWebService valuationWebService)
        {
            this.webService = webservice;
            this.appService = appService;
            this.valuationWebService = valuationWebService;
        }

        public void SubmitCompletedEzVal(int genericKey, HOCRoofEnum roof, float conventionalAmount = 0.0f, float thatchAmount = 0.0f, float otherAmount = 0.0f, float valuationAmount = 0.00f)
        {
            var xml = this.GetPopulatedXml(genericKey, roof, conventionalAmount, thatchAmount, otherAmount, valuationAmount,easyValTemplatePath:EzValTemplates.EzVal_Invalid);
            var uniqueId = this.GetUniqueIdFromXml(genericKey);
            var returnData = this.valuationWebService.SubmitCompletedValuationLightstone(uniqueId, xml.ToString());
            this.AssertValidResponse(returnData);
        }

        public void SubmitRejectedEzVal(int genericKey)
        {
            var xml = this.GetPopulatedXml(genericKey, HOCRoofEnum.Conventional, 0.0f, 0.0f, 0.0f, 0.0f);
            var uniqueId = this.GetUniqueIdFromXml(genericKey);
            var returnData = this.valuationWebService.SubmitRejectedValuationLightstone(uniqueId, xml.ToString());
            this.AssertValidResponse(returnData);
        }

        public void SubmitAmendedEzVal(int genericKey, HOCRoofEnum roof, float conventionalAmount = 0.0f, float thatchAmount = 0.0f, float otherAmount = 0.0f, float valuationAmount = 0.00f)
        {
            var xml = this.GetPopulatedXml(genericKey, roof, conventionalAmount, thatchAmount, otherAmount, valuationAmount);
            var uniqueId = this.GetUniqueIdFromXml(genericKey);
            var returnData = this.valuationWebService.SubmitAmendedValuationLightstone(uniqueId, xml.ToString());
            this.AssertValidResponse(returnData);
        }

        public void SubmitInvalidCompletedEzVal(int genericKey)
        {
            var xml = this.GetPopulatedXml(genericKey, HOCRoofEnum.Conventional, 0.0f, 0.0f, 0.0f, 0.0f, EzValTemplates.EzVal_Invalid);
            var uniqueId = this.GetUniqueIdFromXml(genericKey);
            var returnData = this.valuationWebService.SubmitAmendedValuationLightstone(uniqueId, xml.ToString().Replace(">",""));
            this.AssertInvalidResponse(returnData);
        }

        public Automation.DataModels.Valuation GetActiveValuation(int propertykey)
        {
            return (from val in base.GetValuations(propertykey)
                    where val.IsActive
                    select val).FirstOrDefault();
        }

        public void CreateXmlHistoryDummy()
        {
            var xmlHistoryKey = base.GetMaxXmlHistoryKey() + 2;
            base.InsertXmlHistory(xmlHistoryKey, "This is a dummy");
        }

        public bool HasXmlHistoryDummy()
        {
            return base.GetValuationXmlHistory(0).FirstOrDefault() != null;
        }

        public void ClearAllXmlHistoryDummy()
        {
            base.DeleteXmlHistory(0);
        }

        public XElement GetPopulatedXml(int genericKey, HOCRoofEnum roof, float conventionalAmount = 0.0f, float thatchAmount = 0.0f, float otherAmount = 0.0f, float valuationAmount = 0.00f,string easyValTemplatePath=EzValTemplates.EzVal)
        {
            var ezValTemplate = XElement.Load(easyValTemplatePath);

            var uniqueId = GetUniqueIdFromXml(genericKey);

            if (uniqueId == 0)
                throw new Exception("Unique ID is Zero");

            var genericKeyType = (from xmlHistory in base.GetValuationXmlHistory(genericKey)
                                  where xmlHistory.XMLHistoryKey == uniqueId
                                  select xmlHistory.GenericKeyTypeKey).FirstOrDefault();

            var totalSumInsured = (conventionalAmount + thatchAmount + otherAmount);

            #region XmlData

            var xmlData = XElement.Parse(ezValTemplate.ToString());

            var account = xmlData.Descendants("ACCOUNT_NO").First();
            var bond = xmlData.Descendants("BOND_NO").First();
            var purposeOfAssessment = xmlData.Descendants("PURPOSE_OF_ASSESSMENT").First();
            var completionDate = xmlData.Descendants("CompletionDate").First();
            var dateOfAssessment = xmlData.Descendants("DATE_OF_ASSESSMENT").First();
            var roofEle = xmlData.Descendants("ROOF_TYPE").First();
            var marketValue = xmlData.Descendants("PRESENT_MARKET_VALUE").First();
            var replacementThatch = xmlData.Descendants("REPLACEMENT_THATCH").First();
            var replacementConventional = xmlData.Descendants("REPLACEMENT_CONVENTIONAL").First();
            var replacementOther = xmlData.Descendants("REPLACEMENT_OTHER").First();
            var totalReplacementinsuranceValue = xmlData.Descendants("TOTAL_REPLACEMENTINSURANCE_VALUE").First();
            var uniqueID = xmlData.Descendants("UniqueID").First();

            //Set values
            marketValue.SetValue(valuationAmount);
            completionDate.SetValue(DateTime.Now);
            dateOfAssessment.SetValue(DateTime.Now);
            replacementConventional.SetValue(conventionalAmount);
            replacementThatch.SetValue(thatchAmount);
            replacementOther.SetValue(otherAmount);
            roofEle.SetValue(roof);

            if (genericKeyType == GenericKeyTypeEnum.Account_AccountKey)
            {
                purposeOfAssessment.SetValue("HOC");
                account.SetValue(genericKey);
            }
            if (genericKeyType == GenericKeyTypeEnum.Offer_OfferKey)
            {
                var offerResults = this.appService.GetOfferData(genericKey);
                var offerType = offerResults.Select(x => x.Column("OfferTypeKey").GetValueAs<OfferTypeEnum>()).FirstOrDefault();
                purposeOfAssessment.SetValue(offerType);
                bond.SetValue(genericKey);
            }
            totalReplacementinsuranceValue.SetValue(totalSumInsured);
            uniqueID.SetValue(uniqueId);

            #endregion XmlData

            return xmlData;
        }
       
        private void AssertValidResponse(DataSet responseData)
        {
            var response = String.Join(" ", responseData.Tables["Results"].Rows[0].ItemArray);
            Assert.False(response.Contains("Error"),response);
        }

        private void AssertInvalidResponse(DataSet responseData)
        {
            var response = String.Join(" ", responseData.Tables["Results"].Rows[0].ItemArray);
            Assert.True(response.Contains("HALO Error - please notify SAHL"));
        }

        private int GetUniqueIdFromXml(int genericKey)
        {
            return (from xmlHistory in base.GetValuationXmlHistory(genericKey)
                    where xmlHistory.XMLData.Contains("Lightstone.newPhysicalInstruction")
                    orderby xmlHistory.XMLHistoryKey descending
                    select xmlHistory.XMLHistoryKey).FirstOrDefault();
        }
    }
}