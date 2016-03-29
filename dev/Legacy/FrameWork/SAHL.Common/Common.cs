using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SAHL.Common
{
    public class DateFormatting
    {
        public static DateTime ToDateTimeYYYYMMDD(string StringToConvert)
        {
            DateTime retval = new DateTime(1900, 1, 1);

            if (StringToConvert != null)
            {
                string[] szVals = StringToConvert.Split('/');
                if (szVals.Length == 3)
                {
                    try
                    {
                        retval = new DateTime(int.Parse(szVals[0]), int.Parse(szVals[1]), int.Parse(szVals[2]));
                    }
                    catch { }
                }
            }

            return retval;
        }

        public static DateTime ToDateTimeDDMMYYYY(string StringToConvert)
        {
            DateTime retval = new DateTime(1900, 1, 1);

            if (StringToConvert != null)
            {
                string[] szVals = StringToConvert.Split('/');
                if (szVals.Length == 3)
                {
                    try
                    {
                        retval = new DateTime(int.Parse(szVals[2]), int.Parse(szVals[1]), int.Parse(szVals[0]));
                    }
                    catch { }
                }
            }

            return retval;
        }
    }

    public struct param
    {
        public string name;
        public object value;
    }

    public class paramlist : Dictionary<string, object>
    {
        public new void Add(string ParameterName, Object ParameterValue)
        {
            base.Add(ParameterName, ParameterValue);
        }
    }

    public interface ICalculator
    {
        bool CanCalc(string p_sCalculationName);

        object CalcValue(string p_sCalculationName, object[] p_Parameters);

        object CalcValue(string p_sCalculationName, paramlist p_Parameters);
    }

    public interface IOrderBySpecification
    {
        string OrderBy { get; }
    }

    public static class MathCalcs
    {
        public static decimal SQLRound(decimal Value, int length)
        {
            if (length == 0)
            {
                return System.Math.Round(Value, 0);
            }
            else if (length > 0)
            {
                return System.Math.Round(Value, length);
            }
            else if (length < 0)
            {
                length = System.Math.Abs(length);

                decimal divisor = Convert.ToDecimal(System.Math.Pow(10, length));

                Value = System.Math.Round(Value / divisor, 0);
                Value = Value * divisor;
                return Value;
            }
            else
            {
                return 0;
            }
        }

        private static Regex _isInteger = new Regex(@"^\d+$");

        /// <summary>
        /// Test the numericness of a string, returns true is the string is Numeric
        /// </summary>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public static bool IsInteger(string theValue)
        {
            Match m = _isInteger.Match(theValue);
            return m.Success;
        }
    }
}