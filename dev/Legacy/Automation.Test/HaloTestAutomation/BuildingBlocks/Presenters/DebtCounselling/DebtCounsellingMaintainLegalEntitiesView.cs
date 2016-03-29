using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class DebtCounsellingMaintainLegalEntitiesView : DebtCounsellingMaintainLegalEntitiesControls
    {
        private readonly IExternalRoleService externalRoleService;
        private readonly ILegalEntityService legalEntityService;

        public DebtCounsellingMaintainLegalEntitiesView()
        {
            externalRoleService = ServiceLocator.Instance.GetService<IExternalRoleService>();
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
        }

        public void ClickRemoveLegalEntity()
        {
            base.Remove.Click();
        }

        public void ClickAddLegalEntity()
        {
            base.Add.Click();
        }

        public void ClickCancel()
        {
            base.Cancel.Click();
        }

        //reselect a legal entity once it has already been removed/added to be put back to original state
        public void SelectLegalEntity(int legalEntityKey)
        {
            var legalentity = legalEntityService.GetLegalEntityIDNumber(legalEntityKey);
            base.ctl00MaingrdApplicantsMainTableRow(legalentity).Click();
        }

        public int SelectFirstActiveLegalEntity(int debtCounsellingKey)
        {
            //select the legal entity
            var externalRole = externalRoleService.GetFirstActiveExternalRole(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.Client, true);
            if (externalRole == null)
                throw new AssertionException("Not active external role exist for client against debtcounselling case");
            string legalEntityToSelect = legalEntityService.GetLegalEntityIDNumber(externalRole.LegalEntityKey);
            base.ctl00MaingrdApplicantsMainTableRow(legalEntityToSelect).Click();
            return externalRole.LegalEntityKey;
        }

        /// <summary>
        /// Fetches the value of the warning label
        /// </summary>
        /// <returns></returns>
        public string GetGroupedAccountsWarningLabel()
        {
            return GroupedAccountWarningLabel.Text;
        }
    }
}