using System;

namespace SAHL.Common.Utils
{
    /// <summary>
    /// Class to handle all Date-Related functions
    /// </summary>
    public static class DateUtils
    {
        /// <summary>
        /// Month Difference between 2 dates
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="secondDate"></param>
        /// <param name="debitOrderDay"></param>
        /// <returns></returns>
        public static int MonthDifference(this DateTime firstDate, DateTime secondDate, int debitOrderDay)
        {
            return firstDate.MonthDifference(secondDate, debitOrderDay, true);
        }

        /// <summary>
        /// Use the Last Day Of the Month logic
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="secondDate"></param>
        /// <param name="debitOrderDay"></param>
        /// <param name="checkForLastDayOfMonth"></param>
        /// <returns></returns>
        public static int MonthDifference(this DateTime firstDate, DateTime secondDate, int debitOrderDay, bool checkForLastDayOfMonth)
        {
            DateTime firstDateToUse = new DateTime(firstDate.Year, firstDate.Month, firstDate.Day);
            DateTime secondDateToUse = new DateTime(secondDate.Year, secondDate.Month, secondDate.Day);

            DateTime dateToUse = new DateTime(secondDate.Year, secondDate.Month, secondDate.Day);
            DateTime finalDate = firstDate;

            int monthCounter = 0;
            if (secondDateToUse > firstDateToUse)
            {
                dateToUse = firstDateToUse;
                finalDate = secondDate;
            }

            while (dateToUse <= finalDate)
            {
                //If we are using the logic for Last Day Of the Month
                //If the debit order day > the last day of the month
                if (checkForLastDayOfMonth &&
                    debitOrderDay > DateUtils.LastDayOfMonth(dateToUse).Day)
                {
                    //Increment the Month Counter and Increment the Month
                    monthCounter++;
                    dateToUse = dateToUse.AddMonths(1);
                    continue;
                }
                //Continue normal logic
                if (dateToUse.Day == debitOrderDay)
                {
                    monthCounter++;
                }
                dateToUse = dateToUse.AddDays(1);
            }
            return monthCounter;
        }

        /// <summary>
        /// Returns the last day of the month
        /// </summary>
        /// <param name="parmDate"></param>
        /// <returns>last day of the month in DateTime format</returns>
        public static DateTime LastDayOfMonth(DateTime parmDate)
        {
            DateTime _date = new DateTime(parmDate.Year, parmDate.Month, 1);
            return _date.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Returns the First day of the following month
        /// </summary>
        /// <param name="parmDate"></param>
        /// <returns>first day of the following month in DateTime format</returns>
        public static DateTime FirstDayOfNextMonth(DateTime parmDate)
        {
            DateTime _LastDayOfMonth;

            DateTime _date = new DateTime(parmDate.Year, parmDate.Month, 1);
            _LastDayOfMonth = _date.AddMonths(1).AddDays(-1);

            return _LastDayOfMonth.AddDays(1);
        }

        /// <summary>
        /// Calculates an Age Next Birthday as at Today
        /// </summary>
        /// <param name="parmDateOfBirth"></param>
        /// <returns>Age as an Integer</returns>
        public static int CalculateAgeNextBirthday(DateTime parmDateOfBirth)
        {
            return CalculateAgeNextBirthday(parmDateOfBirth, DateTime.Now);
        }

        /// <summary>
        /// Calculates an Age Next Birthday as at a specified Date
        /// </summary>
        /// <param name="parmDateOfBirth"></param>
        /// <param name="parmAgeAsAtDate"></param>
        /// <returns>Age as an Integer</returns>
        public static int CalculateAgeNextBirthday(DateTime parmDateOfBirth, DateTime parmAgeAsAtDate)
        {
            System.Globalization.Calendar ThisCalendar = new System.Globalization.GregorianCalendar();

            int DaysPassedInYear;
            DateTime Now;
            int YearIndex;
            int DiffDays;

            Now = parmAgeAsAtDate;
            DiffDays = (int)Now.Subtract(parmDateOfBirth).TotalDays;
            YearIndex = 0;
            DaysPassedInYear = CalcDaysPassedInYear(Now, parmDateOfBirth, YearIndex,
            ThisCalendar);
            while (DiffDays > DaysPassedInYear)
            {
                DiffDays = DiffDays - DaysPassedInYear;
                YearIndex++; // on to next year
                DaysPassedInYear = CalcDaysPassedInYear(Now, parmDateOfBirth, YearIndex,
                ThisCalendar);
            }
            // Now handle last part year
            if (DiffDays > 0) // if still some days to account for
            {
                int DaysPassedToBirthday;
                DaysPassedInYear = CalcDaysPassedInYear(Now, parmDateOfBirth, -1,
                ThisCalendar);
                DaysPassedToBirthday =
                CalcDaysPassedInYear(parmDateOfBirth.AddYears(YearIndex), parmDateOfBirth, -1, ThisCalendar);
                if (DaysPassedInYear >= DaysPassedToBirthday)
                    YearIndex++; // birthday has passed this year
            }
            return YearIndex;
        }

        private static int CalcDaysPassedInYear(DateTime Now, DateTime DOB, int YearIndex, System.Globalization.Calendar ThisCalendar)
        {
            DateTime PartYearStartDate; // 1st Jan <PartYear>
            int YearDaysPassed;
            int DaysPassedInYear;
            if (YearIndex == 0) // days in starting (DOB) part year
            {
                PartYearStartDate = DateTime.MinValue;
                PartYearStartDate = PartYearStartDate.AddYears(DOB.Year - 1);
                YearDaysPassed = (int)DOB.Subtract(PartYearStartDate).TotalDays;
                DaysPassedInYear = ThisCalendar.GetDaysInYear(DOB.Year) -
                YearDaysPassed;
            }
            else if (YearIndex == -1) // -1 used to indicate last year in period, i.e. Now.Year
            {
                PartYearStartDate = DateTime.MinValue;
                PartYearStartDate.AddYears(Now.Year);
                DaysPassedInYear = (int)Now.Subtract(PartYearStartDate).TotalDays;
            }
            else // a year within the period
            {
                DaysPassedInYear = ThisCalendar.GetDaysInYear(DOB.Year +
                YearIndex);
            }
            return DaysPassedInYear;
        }

        public static string AddDaySuffix(int day)
        {
            string suffix = String.Empty;

            int ones = day % 10;
            int tens = (int)Math.Floor(day / 10M) % 10;

            if (tens == 1)
            {
                suffix = "th";
            }
            else
            {
                switch (ones)
                {
                    case 1:
                        suffix = "st";
                        break;

                    case 2:
                        suffix = "nd";
                        break;

                    case 3:
                        suffix = "rd";
                        break;

                    default:
                        suffix = "th";
                        break;
                }
            }
            return String.Format("{0}{1}", day, suffix);
        }
    }
}