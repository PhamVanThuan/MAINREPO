using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Collections.Generic;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.Origination
{
    public class BankingDetailsSearchLegalEntity : BankingDetailsSearchLegalEntityControls
    {
        /// <summary>
        /// Select a record from the 'Clients Related to this Account' grid
        /// </summary>
        /// <param name="idNumber">IDNumber</param>
        /// <param name="button">Button</param>
        public void UseLegalEntityBankAccount(string idNumber, ButtonTypeEnum button)
        {
            BankDetailsSearchGrid_Row(idNumber).Click();
            switch (button)
            {
                case ButtonTypeEnum.Use:
                    base.UseButton.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.CancelButton.Click();
                    break;

                default:
                    break;
            }
        }

        #region Assertions

        /// <summary>
        /// Assert that the Legal Entity record identified by idnumber is displayed in the 'Clients Related to this Account' grid
        /// </summary>
        /// <param name="idNumber">IDNumber</param>
        public void AssertLegalEntityDisplayed(string idNumber)
        {
            Logger.LogAction(string.Format(@"Asserting that IdNumber: {0} exists int the 'Clients Related to this Account' grid", idNumber));
            Assert.True(base.BankDetailsSearchGrid_Row(idNumber).Exists, "The Legal Entity record is not displayed in the 'Clients Related to this Account' grid");
        }

        /// <summary>
        /// Assert that the Legal Entity records identified by IDnumber are displayed in the 'Clients Related to this Account' grid
        /// </summary>
        /// <param name="idNumber">List of IDNumbers</param>
        public void AssertLegalEntityDisplayed(List<string> idNumbers)
        {
            foreach (string idNumber in idNumbers)
            {
                Logger.LogAction(string.Format(@"Asserting that IdNumber: {0} exists in the 'Clients Related to this Account' grid", idNumber));
                Assert.True(base.BankDetailsSearchGrid_Row(idNumber).Exists,
                    string.Format(@"The Legal Entity identified by IdNumber: {0} is not displayed in the 'Clients Related to this Account' grid",
                    idNumber));
            }
        }

        /// <summary>
        /// Assert that the text message is displayed on screen
        /// </summary>
        /// <param name="text">Text Message</param>
        public void AssertTextMessageExists(string text)
        {
            Logger.LogAction(@"Asserting that the text message: {0} is displayed", text);
            Assert.True(base.Document.ContainsText(text), string.Format(@"The text message: {0} is not dispalyed", text));
        }

        #endregion Assertions
    }
}