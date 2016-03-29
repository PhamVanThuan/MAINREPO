using Common.Constants;
using Common.Enums;
using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing.AccountMailingAddress
{
    public class AccountMailingAddressUpdate : AccountMailingAddressUpdateControls
    {
        /// <summary>
        /// Selects the first option in the 'Email Address' and 'Address' dropdowns.  OnlineStatementFormat is nullable
        /// </summary>
        /// <param name="formattedAddress"></param>
        /// <param name="correspondenceMedium"></param>
        /// <param name="correspondenceLanguage"></param>
        /// <param name="onlineStatement"></param>
        /// <param name="onlineStatementformat"></param>
        /// <param name="legalentityemailaddress"></param>
        public void UpdateApplicationMailingAddress
            (
                string formattedAddress,
                string correspondenceMedium,
                string correspondenceLanguage,
                bool onlineStatement,
                string onlineStatementformat,
                string legalentityemailaddress = null
            )
        {
            Thread.Sleep(1000);
            base.ddlMailingAddress.Option(formattedAddress).Select();
            Thread.Sleep(1000);
            base.ddlCorrespondenceMedium.Option(correspondenceMedium).Select();
            //we need to wait for a bit here
            Thread.Sleep(1000);
            if (correspondenceMedium == CorrespondenceMedium.Email)
            {
                if (string.IsNullOrEmpty(legalentityemailaddress))
                    throw new System.Exception("CorrespondenceMedium of 'Email' was passed as a parameter, but no emailaddress provided.");
                base.ddlCorrespondenceMailAddress.Option(new Regex(legalentityemailaddress)).Select();
            }
            base.Document.DomContainer.WaitForComplete();
            base.ddlCorrespondenceLanguage.Option(correspondenceLanguage).Select();
            if (!base.chkOnlineStatement.Checked && onlineStatement)
                base.chkOnlineStatement.Click();
            else if (base.chkOnlineStatement.Checked && !onlineStatement)
                base.chkOnlineStatement.Click();
            //we need to wait for a bit here
            Thread.Sleep(2000);
            if (base.chkOnlineStatement.Checked && onlineStatementformat != null)
                base.ddlOnlineStatementFormat.Option(onlineStatementformat).Select();
            base.btnSubmit.Click();
        }

        /// <summary>
        /// Check/uncheck the Online Statement Checkbox
        /// </summary>
        /// <param name="onlineStatment">Check/uncheck</param>
        public void SetOnlineStatmentCheckbox(bool onlineStatment)
        {
            base.chkOnlineStatement.Checked = onlineStatment;
        }

        /// <summary>
        /// Click button
        /// </summary>
        /// <param name="button">Type of button to click</param>
        public void ClickButton(ButtonTypeEnum button)
        {
            switch (button)
            {
                case ButtonTypeEnum.Cancel:
                    base.btnCancel.Click();
                    break;

                case ButtonTypeEnum.Submit:
                    base.btnSubmit.Click();
                    break;
            }
        }

        /// <summary>
        /// Select an option that is not the "- Please select -" or currently selected option
        /// </summary>
        /// <param name="selectList">The select list object to select from</param>
        /// <returns>The new selected option</returns>
        public string SelectNewOptionFromDropDown(SelectList selectList)
        {
            List<string> excludeOptions = new List<string> { "- Please select -", selectList.SelectedOption.Text };

            var option = (from a in selectList.Options
                          where !excludeOptions.Contains(a.Text)
                          select a.Text).FirstOrDefault();

            selectList.Option(option).Select();

            return option;
        }

        /// <summary>
        /// Select an option from the CorrespondenceMedium dropdown list that is not the "- Please select -" or currently selected option
        /// </summary>
        /// <returns>The new selected option</returns>
        public string SelectNewCorrespondenceMedium()
        {
            return SelectNewOptionFromDropDown(base.ddlCorrespondenceMedium);
        }

        /// <summary>
        /// Select the given option from the CorrespondenceMedium dropdown list
        /// </summary>
        /// <param name="correspondenceMedium">Option to select</param>
        public void SelectNewCorrespondenceMedium(string correspondenceMedium)
        {
            base.ddlCorrespondenceMedium.Option(correspondenceMedium).Select();
        }

        /// <summary>
        /// Select an option from the MailingAddress dropdown list that is not the "- Please select -" or currently selected option
        /// </summary>
        /// <returns>The new selected option</returns>
        public string SelectNewMailingAddress()
        {
            return SelectNewOptionFromDropDown(base.ddlMailingAddress);
        }

        /// <summary>
        /// Select an option from the CorrespondenceLanguage dropdown list that is not the "- Please select -" or currently selected option
        /// </summary>
        /// <returns>The new selected option</returns>
        public string SelectNewCorrespondenceLanguage()
        {
            return SelectNewOptionFromDropDown(base.ddlCorrespondenceLanguage);
        }

        /// <summary>
        /// Select an option from the OnlineStatmentFormat dropdown list that is not the "- Please select -" or currently selected option
        /// </summary>
        /// <returns>The new selected option</returns>
        public string SelectNewOnlineStatmentFormat()
        {
            return SelectNewOptionFromDropDown(base.ddlOnlineStatementFormat);
        }

        /// <summary>
        /// Change the OnlineStatement status
        /// </summary>
        /// <returns>New checked/unchecked status</returns>
        public bool SetNewOnlineStatementStatus()
        {
            bool reverseBool = !base.chkOnlineStatement.Checked;
            SetOnlineStatmentCheckbox(reverseBool);
            return reverseBool;
        }

        /// <summary>
        /// Select an option from the EmailAddress dropdown list that is not the "- Please select -" or currently selected option
        /// </summary>
        /// <returns>The new selected option</returns>
        public string SelectNewEmailAddress()
        {
            return SelectNewOptionFromDropDown(base.ddlCorrespondenceMailAddress);
        }

        /// <summary>
        /// Assert the AccountMailingAddressUpdate controls exist
        /// </summary>
        public void AssertAccountMailingAddressUpdateControlsExist()
        {
            var controls = new List<Element>() {
                    base.ddlCorrespondenceMedium,
                    base.ddlMailingAddress,
                    base.ddlCorrespondenceLanguage,
                    base.chkOnlineStatement,
                    base.ddlOnlineStatementFormat,
                    base.btnCancel,
                    base.btnSubmit
                };

            Assertions.WatiNAssertions.AssertFieldsExist(controls);
        }

        /// <summary>
        /// Assert the AccountMailingAddressUpdate controls are enabled
        /// </summary>
        public void AssertAccountMailingAddressUpdateControlsEnabled()
        {
            var controls = new List<Element>() {
                    base.ddlCorrespondenceMedium,
                    base.ddlMailingAddress,
                    base.ddlCorrespondenceLanguage,
                    base.chkOnlineStatement,
                    base.btnCancel,
                    base.btnSubmit
                };

            Assertions.WatiNAssertions.AssertFieldsAreEnabled(controls);
        }

        /// <summary>
        /// Assert the expected options exist in the Correspondence Medium dropdown list
        /// </summary>
        public void AssertCorrespondenceMediumOptions(List<string> correspondenceMedium)
        {
            Assertions.WatiNAssertions.AssertSelectListContents(base.ddlCorrespondenceMedium, correspondenceMedium);
        }

        /// <summary>
        /// Assert the expected options exist in the Correspondence Language dropdown list
        /// </summary>
        public void AssertCorrespondenceLanguageOptions(List<string> correspondenceLanguage)
        {
            Assertions.WatiNAssertions.AssertSelectListContents(base.ddlCorrespondenceLanguage, correspondenceLanguage);
        }

        /// <summary>
        /// Assert the expected options exist in the Online Statement Format dropdown list
        /// </summary>
        public void AssertOnlineStatmentFormatOptions(List<string> onlineStatmentFormat)
        {
            Assertions.WatiNAssertions.AssertSelectListContents(base.ddlOnlineStatementFormat, onlineStatmentFormat);
        }

        /// <summary>
        /// Assert that the online Statement Format dropdown is enabled/dissabled
        /// </summary>
        public void AssertOnlineStatementFormatEnabled(bool enabled)
        {
            var controls = new List<Element>() {
                    base.ddlOnlineStatementFormat
                };

            switch (enabled)
            {
                case true:
                    Assertions.WatiNAssertions.AssertFieldsAreEnabled(controls);
                    break;

                case false:
                    Assertions.WatiNAssertions.AssertFieldsAreDisabled(controls);
                    break;
            }
        }

        /// <summary>
        /// Assert the expected option is selected in the Online Statement Format dropdown list
        /// </summary>
        /// <param name="option"></param>
        public void AssertOnlineStatementFormatSelection(string option)
        {
            Assertions.WatiNAssertions.AssertSelectedOption(option, base.ddlOnlineStatementFormat);
        }
    }
}