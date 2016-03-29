using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CalendarDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CalendarDataModel(DateTime calendarDate, bool isSaturday, bool isSunday, bool isHoliday)
        {
            this.CalendarDate = calendarDate;
            this.IsSaturday = isSaturday;
            this.IsSunday = isSunday;
            this.IsHoliday = isHoliday;
		
        }
		[JsonConstructor]
        public CalendarDataModel(int calendarKey, DateTime calendarDate, bool isSaturday, bool isSunday, bool isHoliday)
        {
            this.CalendarKey = calendarKey;
            this.CalendarDate = calendarDate;
            this.IsSaturday = isSaturday;
            this.IsSunday = isSunday;
            this.IsHoliday = isHoliday;
		
        }		

        public int CalendarKey { get; set; }

        public DateTime CalendarDate { get; set; }

        public bool IsSaturday { get; set; }

        public bool IsSunday { get; set; }

        public bool IsHoliday { get; set; }

        public void SetKey(int key)
        {
            this.CalendarKey =  key;
        }
    }
}