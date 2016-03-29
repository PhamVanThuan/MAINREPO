using Common.Constants;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class ChangeInCircumstance : ChangeOfCircumstanceControls
    {
        public void Enter17pt3Date(DateTime date)
        {
            base.txt173Date.Value = date.ToString(Formats.DateFormat);
        }

        public void EnterComments(string comments)
        {
            base.txtComments.Value = comments;
        }

        public void ClickSave()
        {
            base.btnSave.Click();
        }
    }
}