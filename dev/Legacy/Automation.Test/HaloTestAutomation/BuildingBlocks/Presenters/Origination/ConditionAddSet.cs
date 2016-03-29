using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class ConditionAddSet : ConditionsAddSetControls
    {
        /// <summary>
        /// Saves the default condition set loaded for the application
        /// </summary>
        public void SaveConditionSet(bool expectValidationMessages = false)
        {
            if (base.btnUpdateConditionSet.Exists)
            {
                //then we have already saved conditions so we are updating them
                base.btnUpdateConditionSet.Click();
            }
            else
            {
                base.btnSave.Click();
            }
        }
    }
}