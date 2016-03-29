using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    /// <summary>
    ///
    /// </summary>
    public class LegalEntitySequestrationDeathNotify : LegalEntitySequestrationDeathNotifyControls
    {
        /// <summary>
        /// Selects the legal entity from the grid.
        /// </summary>
        /// <param name="legalEntityKey">Legal Entity to Select</param>
        public void SelectLegalEntityCheckBox(int legalEntityKey, NotificationTypeEnum type)
        {
            base.SelectCheckBoxByLegalEntityKey(legalEntityKey, type);
        }

        /// <summary>
        /// Clicks the update button
        /// </summary>
        public void ClickUpdateButton()
        {
            base.btnUpdate.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="leKeys"></param>
        /// <param name="notificationType"></param>
        public void AssertLegalEntityCheckBoxes(Dictionary<int, LegalEntityTypeEnum> leKeys, NotificationTypeEnum notificationType)
        {
            int key = 0;
            key = (from l in leKeys
                   where l.Value == LegalEntityTypeEnum.NaturalPerson
                   && base.CheckBoxExists(l.Key, notificationType) == false
                   select l.Key).FirstOrDefault();
            Assert.That(key == 0, string.Format(@"Legal Entity {0} was not found on the screen", key));
        }
    }
}