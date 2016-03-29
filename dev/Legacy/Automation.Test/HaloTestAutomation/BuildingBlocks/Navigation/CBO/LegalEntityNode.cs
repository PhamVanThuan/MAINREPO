using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using Common.Extensions;
using ObjectMaps;

namespace BuildingBlocks.Navigation
{
    public class LegalEntityNode : LegalEntityNodeControls
    {
        private readonly ILegalEntityService legalEntityService;

        public LegalEntityNode()
        {
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
        }

        public void LegalEntity(int OfferKey)
        {
            var results = new QueryResults();
            results = legalEntityService.GetLegalEntityLegalNamesForOffer(OfferKey);
            string LegalEntityName = results.Rows(0).Column("LegalEntityLegalName").Value;
            base.LegalEntityMain(LegalEntityName.EscapeCharactersBeforeRegexProcessing()).Click();
        }

        public void LegalEntity_ByLegalEntityKey(int LegalEntityKey)
        {
            var results = new QueryResults();
            results = legalEntityService.GetLegalEntityLegalNameByLegalEntityKey(LegalEntityKey);
            string LegalEntityName = results.SQLScalarValue;
            //this method call replaces a double space with a single space
            LegalEntityName = LegalEntityName.RemoveDoubleSpace();
            LegalEntityName = LegalEntityName.EscapeCharactersBeforeRegexProcessing();
            base.LegalEntityMain(LegalEntityName).Click();
        }

        public void LegalEntity_ByLegalEntityByIdNumber(string IdNumber)
        {
            string LegalEntityName = legalEntityService.GetLegalEntityLegalNameByIDNumber(IdNumber);
            //this method call replaces a double space with a single space
            LegalEntityName = LegalEntityName.RemoveDoubleSpace();
            LegalEntityName = LegalEntityName.EscapeCharactersBeforeRegexProcessing();
            base.LegalEntityMain(LegalEntityName).Click();
        }

        public void LegalEntityDetails(NodeTypeEnum Node)
        {
            base.LegalEntityDetails.Click();
            switch (Node)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateLegalEntityDetails.Click();
                    break;
            }
        }

        public void LegalEntityAddress(NodeTypeEnum Node)
        {
            if (base.LegalEntityAddressDetails.Exists)
            {
                base.LegalEntityAddressDetails.Click();
                switch (Node)
                {
                    case NodeTypeEnum.Update:
                        {
                            base.UpdateLegalEntityAddress.Click();
                        }
                        break;

                    case NodeTypeEnum.Add:
                        base.AddLegalEntityAddress.Click();
                        break;

                    case NodeTypeEnum.Delete:
                        base.DeleteLegalEntityAddress.Click();
                        break;

                    case NodeTypeEnum.View:
                        break;
                }
            }
        }

        public void ApplicationDeclarations(NodeTypeEnum Node)
        {
            base.ApplicationDeclarations.Click();
            switch (Node)
            {
                case NodeTypeEnum.Update:
                    base.UpdateDeclarations.Click();
                    break;

                case NodeTypeEnum.View:
                    break;
            }
        }

        public void EmploymentDetails(NodeTypeEnum Node)
        {
            base.EmploymentDetails.Click();
            switch (Node)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateEmploymentDetails.Click();
                    break;

                case NodeTypeEnum.Add:
                    base.AddEmploymentDetails.Click();
                    break;
            }
        }

        public void BankingDetails(NodeTypeEnum Node)
        {
            base.BankingDetails.Click();
            switch (Node)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateBankingDetails.Click();
                    break;

                case NodeTypeEnum.Add:
                    base.AddBankingDetails.Click();
                    break;

                case NodeTypeEnum.Delete:
                    base.DeleteBankingDetails.Click();
                    break;
            }
        }

        public void AffordabilityAndExpenses(NodeTypeEnum Node)
        {
            base.AffordabilityandExpenses.Click();
            switch (Node)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateAffordabilityDetails.Click();
                    break;
            }
        }

        public void AssetsAndLiabilities(NodeTypeEnum Node)
        {
            base.AssetsAndLiabilities.Click();
            switch (Node)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Add:
                    base.AddAssetLiability.Click();
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateAssetLiability.Click();
                    break;

                case NodeTypeEnum.Delete:
                    base.DeleteAssetLiability.Click();
                    break;

                case NodeTypeEnum.Associate:
                    base.AssociateAssetLiability.Click();
                    break;
            }
        }

        public void LegalEntityMemo(NodeTypeEnum Node)
        {
            base.LegalEntityMemo.Click();
            switch (Node)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Add:
                    base.AddLegalEntityMemo.Click();
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateLegalEntityMemo.Click();
                    break;
            }
        }

        public void LegalEntityRelationships(NodeTypeEnum Node)
        {
            base.LegalEntityRelationships.Click();
            switch (Node)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Add:
                    base.AddLegalEntityRelationship.Click();
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateLegalEntityRelationship.Click();
                    break;

                case NodeTypeEnum.Delete:
                    base.DeleteLegalEntityRelationship.Click();
                    break;
            }
        }

        public void DomiciliuAddressDetails(NodeTypeEnum Node)
        {
            base.DomiciliumAddressDetails.Click();
            switch (Node)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateDomiciliumAddress.Click();
                    break;
            }
        }

        /// <summary>
        /// Look for the legalentity node by getting the legalentity legalname by provided the passport
        /// </summary>
        /// <param name="passportNumber"></param>
        public void LegalEntity_ByLegalEntityPassportNumber(string passportNumber)
        {
            string LegalEntityName = legalEntityService.GetLegalEntityLegalNameByPassportNumber(passportNumber);
            //this method call replaces a double space with a single space
            LegalEntityName = LegalEntityName.RemoveDoubleSpace();
            LegalEntityName = LegalEntityName.EscapeCharactersBeforeRegexProcessing();
            base.LegalEntityMain(LegalEntityName).Click();
        }

        /// <summary>
        /// Select Legal Entities node if View is passed in, or select Maintain Legal Entities action if Maintain passed in
        /// </summary>
        /// <param name="Node"></param>
        public void ClickLegalEntities(NodeTypeEnum Node)
        {
            switch (Node)
            {
                case NodeTypeEnum.View:
                    base.DCLegalEntityMain.Click();
                    break;

                case NodeTypeEnum.Maintain:
                    base.DCLegalEntityMain.Click();
                    base.MaintainLegalEntities.Click();
                    break;
            }
        }

        public int GetNumberOfLegalEntities(int accountKey)
        {
            QueryResults results = legalEntityService.GetLegalEntityNamesAndRoleByAccountKey(accountKey);
            return results.RowList.Count;
        }
    }
}