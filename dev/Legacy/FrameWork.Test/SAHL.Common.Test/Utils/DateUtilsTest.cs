using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Utils;

namespace SAHL.Common.Utils.Test
{
    [TestFixture]
    public class DateUtilsTest : TestBase
    {
        [Test]
        public void GetLastDayOfMonth()
        {
            DateTime dteCurrentDate = new DateTime(2007, 09, 03);
            DateTime dteExpectedDate = new DateTime(2007, 09, 30);

            DateTime dteReturnDate = DateUtils.LastDayOfMonth(dteCurrentDate);

            Assert.IsTrue(dteReturnDate == dteExpectedDate);
        }

        [Test]
        public void GetFirstDayOfNextMonth()
        {
            DateTime dteCurrentDate = new DateTime(2007, 09, 03);
            DateTime dteExpectedDate = new DateTime(2007, 10, 01);

            DateTime dteReturnDate = DateUtils.FirstDayOfNextMonth(dteCurrentDate);

            Assert.IsTrue(dteReturnDate == dteExpectedDate);

            // check for date overlapping year
            dteCurrentDate = new DateTime(2007, 12, 03);
            dteExpectedDate = new DateTime(2008, 01, 01);

            dteReturnDate = DateUtils.FirstDayOfNextMonth(dteCurrentDate);

            Assert.IsTrue(dteReturnDate == dteExpectedDate);


        }

        [Test]
        public void CalculateAgeNextBirthday()
        {
            int iAge = 0;
            DateTime dteDateOfBirth = new DateTime(1968, 12, 09);
            // return age as at today
            //int iAge = DateUtils.CalculateAgeNextBirthday(dteDateOfBirth);
            //Assert.IsTrue(iAge == 39);

            // return age as at specified date
            DateTime dteAgeAsAtDate = new DateTime(2008, 11, 01);
            iAge = DateUtils.CalculateAgeNextBirthday(dteDateOfBirth, dteAgeAsAtDate);

            Assert.IsTrue(iAge == 40);

            // return age as at specified date
            dteDateOfBirth = new DateTime(1959, 02, 01);
            dteAgeAsAtDate = new DateTime(2008, 01, 31);
            iAge = DateUtils.CalculateAgeNextBirthday(dteDateOfBirth, dteAgeAsAtDate);
            Assert.IsTrue(iAge == 49);

            dteAgeAsAtDate = new DateTime(2008, 02, 01);
            iAge = DateUtils.CalculateAgeNextBirthday(dteDateOfBirth, dteAgeAsAtDate);
            Assert.IsTrue(iAge == 50);

            dteAgeAsAtDate = new DateTime(2008, 02, 02);
            iAge = DateUtils.CalculateAgeNextBirthday(dteDateOfBirth, dteAgeAsAtDate);
            Assert.IsTrue(iAge == 50);

        }

    }
}
