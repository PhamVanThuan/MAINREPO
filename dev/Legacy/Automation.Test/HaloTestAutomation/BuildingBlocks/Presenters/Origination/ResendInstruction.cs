using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Presenters.Origination
{
    public class ResendInstruction : ResendInstructionControls
    {
        /// <summary>
        /// Clicks the Send Instruction button
        /// </summary>
        public void SendInstruction()
        {
            base.SendInstructionButton.Click();
        }

        /// <summary>
        /// Select a new attorney for the deeds office provided.
        /// </summary>
        public Dictionary<int, string> ChangeAttorney()
        {
            string attorney = base.AttorneyDropDown.SelectedOption.Value;
            //continue
            var newAttorney = (from option in base.AttorneyDropDown.Options
                               where option.Value != attorney
                                   && option.Value != "-select-"
                               select option).First();
            var attorneyDetails = new Dictionary<int, string>
                {
                    {int.Parse(newAttorney.Value), newAttorney.Text}
                };
            base.AttorneyDropDown.SelectByValue(newAttorney.Value);
            base.UpdateButton.Click();
            return attorneyDetails;
        }
    }
}