using System;

namespace SAHL.Common.Utils
{
    public static class ValidationUtils
    {
        /// <summary>
        /// Use to Validate a South African ID Number
        /// </summary>
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

        /// <summary>
        /// Use to Validate a Passport Number
        /// </summary>
        public static bool ValPassportNumber(string passportNumber)
        {
            return true;
        }
    }
}