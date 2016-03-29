using System;

namespace DomainService2.SharedServices.Common
{
    public class GetnWorkingDaysFromTodayCommand : StandardDomainServiceCommand
    {
        public GetnWorkingDaysFromTodayCommand(int nDays)
        {
            this.NDays = nDays;
        }

        public int NDays
        {
            get;
            protected set;
        }

        public DateTime Result
        {
            get;
            set;
        }
    }
}