using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SAHL.Common.BusinessModel.SAHLComparers
{
    /// <summary>
    /// Used to order collections by date
    /// </summary>
    public class SAHLDateComparer : IComparer<DateTime>
    {

        #region IComparer<DateTime> Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(DateTime x, DateTime y)
        {
            if (x.Date >= y.Date)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        #endregion
    }
}
