using Common.Enums;
using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Presenters.Origination
{
    public class ApplicationAttributesUpdate : ApplicationAttributesUpdateControls
    {
        /// <summary>
        /// Selects the index of marketing source option in the dropdown to the provided index and sets the transferring attorney field to the value provided
        /// </summary>
        /// <param name="TransferringAttorney"></param>
        /// <param name="MarketingSource"></param>
        /// <param name="Button"></param>
        public void UpdateApplicationLoanAttributes(string TransferringAttorney, int MarketingSource, ButtonTypeEnum Button)
        {
            base.txtTransferAttorney.TypeText(TransferringAttorney);
            base.ddlMarketingSource.Options[MarketingSource].Select();
            ClickButton(Button);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="attributeText"></param>
        public void SelectAttributeByDescription(string attributeText)
        {
            var checkBox = base.ApplicationLoanAttributesRow(attributeText).CheckBoxes;
            if (checkBox.Count > 0)
            {
                checkBox[0].Checked = true;
            }
        }

        public void ClickButton(ButtonTypeEnum button)
        {
            switch (button)
            {
                case ButtonTypeEnum.Cancel:
                    base.btnCancel.Click();
                    break;

                case ButtonTypeEnum.Update:
                    base.btnUpdate.Click();
                    break;
            }
        }

        public IEnumerable<WatiN.Core.CheckBox> GetAttributesByEnabledState(bool enabledState)
        {
            var checkboxes = base.Document.CheckBoxes.Where(x => x.Enabled == enabledState).AsEnumerable();
            return checkboxes;
        }
    }
}