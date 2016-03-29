using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.CommonPresenters
{
    public class MemoFollowUpAdd : MemoFollowUpAddControls
    {
        /// <summary>
        /// This method will create a FollowUp on an application at the Application Capture state.
        /// </summary>
        /// <param name="minsFromNow">Minutes to add to the current time for the follow up create, this HAS to be in increments of 5</param>
        /// <param name="SelectedHour">Return variable that stores the hour value selected for the follow up</param>
        /// <param name="SelectedMin">Return variable that stores the minute value selected for the follow up</param>
        public void CreateFollowup(int minsFromNow, out string SelectedHour, out string SelectedMin)
        {
            //fetch hour and minutes
            string hour = string.Empty;
            string min = string.Empty;
            CalcFollowUpTime(minsFromNow, out hour, out min);
            //select option from hour dropdown
            base.HourDropdown.Option(hour).Select();
            //select option from minute dropdown
            base.MinDropdown.Option(min).Select();
            //provide the memo comment
            base.MemoUpdate.Value = "Add Follow Up Comment";
            //create the FollowUp
            base.AddButton.Click();

            SelectedHour = hour;
            SelectedMin = min;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="minutes"></param>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        private static void CalcFollowUpTime(int minutes, out string hour, out string min)
        {
            hour = string.Empty;
            min = string.Empty;

            //get the hour
            var date = DateTime.Now;
            var IntHour = date.Hour;
            //get the minute
            var IntMin = date.Minute;
            //declare a rounding factor
            var factor = 5;
            double d_IntMin = IntMin / factor;
            d_IntMin = (Math.Round(d_IntMin)) * factor;
            IntMin = Convert.ToInt32(d_IntMin);
            IntMin = IntMin + minutes;
            //if minutes >= 60, increment the hour and subtract 60 from min
            if (IntMin >= 60)
            {
                IntHour = IntHour + 1;
                IntMin = IntMin - 60;
            }
            //if hours < 10 add 0 when converting to string
            if (IntHour < 10)
            {
                hour = "0" + IntHour.ToString();
            }
            //if minutes < 10 add 0 when converting to string
            if (IntMin < 10)
            {
                min = "0" + IntMin.ToString();
            }
            //convert hours and minutes to string
            if (hour == string.Empty)
            {
                hour = IntHour.ToString();
            }
            if (min == string.Empty)
            {
                min = IntMin.ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="minsFromNow"></param>
        /// <param name="SelectedHour"></param>
        /// <param name="SelectedMin"></param>
        public void CreateMemo()
        {
            //provide the memo comment
            base.MemoUpdate.Value = "Add Follow Up Comment";
            //create the FollowUp
            base.AddButton.Click();
        }
    }
}