using BuildingBlocks.Services;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.LoanServicing.CATSDisbursement
{
    public class CATSDisbursementDelete : CATSDisbursementDeleteControls
    {
        private readonly IDisbursementService disbursementService;

        public CATSDisbursementDelete()
        {
            disbursementService = new DisbursementService();
        }

        /// <summary>
        /// Perform the Delete action on the CATSDisbursementDelete view
        /// </summary>
        public void DeleteDisbursement()
        {
            ClickButton(ButtonTypeEnum.Delete);
        }

        /// <summary>
        /// Click the specified button on the CATSDisbursementDelete view
        /// </summary>
        /// <param name="button">ButtonType</param>
        public void ClickButton(ButtonTypeEnum button)
        {
            switch (button)
            {
                case ButtonTypeEnum.Delete:
                    base.btnDelete.Click();
                    break;

                default:
                    break;
            }
        }

        #region Validation

        /// <summary>
        /// Assert the expected controls exist on the CATSDisbursementDelete view
        /// </summary>
        public void AssertCATSDisbursementDeleteControlsExist_NoDisbursementRecords()
        {
            var enabledAddControls = new List<Element>() {
                    base.btnCancel
                };

            Assertions.WatiNAssertions.AssertFieldsExist(enabledAddControls);
            Assertions.WatiNAssertions.AssertFieldsAreEnabled(enabledAddControls);

            var disabledAddControls = new List<Element>() {
                    base.btnDelete
                };

            Assertions.WatiNAssertions.AssertFieldsExist(disabledAddControls);
            Assertions.WatiNAssertions.AssertFieldsAreDisabled(disabledAddControls);
        }

        /// <summary>
        /// Assert the expected controls exist on the CATSDisbursementDelete view when pending disbursement records exist
        /// </summary>
        public void AssertCATSDisbursementDeleteControlsExist_PendingDisbursementRecords()
        {
            var enabledAddControls = new List<Element>() {
                    base.btnCancel,
                    base.btnDelete
                };

            Assertions.WatiNAssertions.AssertFieldsExist(enabledAddControls);
            Assertions.WatiNAssertions.AssertFieldsAreEnabled(enabledAddControls);
        }

        /// <summary>
        /// Assert that the disbursement record has been deleted from the db
        /// </summary>
        /// <param name="disbursementKey"></param>
        public void AssertDisbursementRecordDeleted(int disbursementKey)
        {
            Logger.LogAction("Asserting that Disbursement {0} has been deleted", disbursementKey);

            var disbursement = (from d in disbursementService.GetDisbursementByDisbursementKey(new List<int> { disbursementKey })
                                select d).FirstOrDefault<Automation.DataModels.Disbursement>();

            Assert.True(disbursement == null, string.Format(@"Disbursement {0} has not been deleted", disbursementKey));
        }

        #endregion Validation
    }
}