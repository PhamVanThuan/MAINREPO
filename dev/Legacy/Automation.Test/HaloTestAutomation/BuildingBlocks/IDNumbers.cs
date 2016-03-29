using BuildingBlocks.Services.Contracts;
using System.Linq;
using NUnit.Framework;
using System;

namespace BuildingBlocks
{
    public static class IDNumbers
    {
        private static ILegalEntityService legalEntityService;

        static IDNumbers()
        {
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
        }

        /// <summary>
        /// Seed id numbers, check if they are valid and that they don’t exist
        /// </summary>
        /// <returns> a single valid Idnumber </returns>
        public static string GetNextIDNumber()
        {
            string _idnumber = String.Empty;
            for (double idnumber = 8001015000000; idnumber < 8412316000000; idnumber++)
            {
                _idnumber = idnumber.ToString();
                bool valid = ValidateID(_idnumber);
                if (valid && !legalEntityService.LegalEntitiesWithIDNumberExist(_idnumber))
                {
                    return _idnumber;
                }
            }
            Assert.That(!String.IsNullOrEmpty(_idnumber), "Could not seed an Id number");
            return String.Empty;
        }

        /// <summary>
        /// Seed id numbers, check if they are valid and that they don’t exist
        /// </summary>
        /// <returns> a single valid Idnumber </returns>
        public static string GetNextIDNumber(string startingDate, string lastGoodIDNumber = null)
        {
            string IDStartDate = string.Format(@"{0}5000000", startingDate);
            string endingDate = string.Format(@"{0}6000000", startingDate);
            double startidnumber = String.IsNullOrEmpty(lastGoodIDNumber) ? Convert.ToDouble(IDStartDate) : Convert.ToDouble(lastGoodIDNumber);
            double endIdNumber = Convert.ToDouble(endingDate);
            string _idnumber = String.Empty;
            for (double idnumber = startidnumber; idnumber < endIdNumber; idnumber++)
            {
                _idnumber = idnumber.ToString();
                bool valid = ValidateID(_idnumber);
                if (valid && !legalEntityService.LegalEntitiesWithIDNumberExist(_idnumber))
                {
                    return _idnumber;
                }
            }
            Assert.That(!String.IsNullOrEmpty(_idnumber), "Could not seed an Id number");
            return String.Empty;
        }

        /// <summary>
        /// Validate idnumber.
        /// </summary>
        /// <param name="IDNumber">id number i.e. 8606185144082</param>
        /// <returns></returns>
        public static bool ValidateID(string IDNumber)
        {
            try
            {
                int odd = 0;
                int ev = 0;
                string[] even = new string[7];
                int total = 0;
                int check = 0;
                string newnum = "0" + IDNumber;

                if (IDNumber.Length != 13)
                    return false;

                Convert.ToDouble(IDNumber);

                for (int i = 0; i < 13; i++)
                {
                    if (i % 2 != 0)
                        odd += Int32.Parse(string.Format("{0}", newnum[i]));
                }

                int evenctr = 0;
                for (int i = 0; i < 13; i++)
                {
                    if (i % 2 == 0)
                    {
                        int tmp = Int32.Parse(string.Format("{0}", newnum[i])) * 2;
                        if (tmp > 0)
                            even[evenctr++] = tmp.ToString();
                    }
                }

                for (int i = 0; i < evenctr; i++)
                {
                    for (int j = 0; j < even[i].Length; j++)
                    {
                        ev += Int32.Parse(string.Format("{0}", even[i][j]));
                    }
                }

                total = odd + ev;
                if (Int32.Parse(string.Format("{0}", total.ToString()[total.ToString().Length - 1])) > 0)
                    check = 10 - Int32.Parse(string.Format("{0}", total.ToString()[total.ToString().Length - 1]));
                else
                    check = 0;

                if (check != Int32.Parse(string.Format("{0}", IDNumber[12])))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DateTime GetDateFromIdNumber(string idNumber)
        {
            var firstSix = String.Join("",idNumber.Take(6));

            var year = string.Format("19{0}",firstSix.Substring(0, 2));
            var month = firstSix.Substring(2, 2);
            var day = firstSix.Substring(4, 2);

            return DateTime.Parse(String.Format("{0},{1},{2}", year, month, day));
        }
    }
}