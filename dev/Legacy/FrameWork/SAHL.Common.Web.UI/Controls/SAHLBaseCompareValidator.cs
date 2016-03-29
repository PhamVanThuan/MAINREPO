using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Drawing;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
    public abstract class SAHLBaseCompareValidator : SAHLBaseValidator
    {

        #region Properties

        [
        DefaultValue(false), 
        Category("Behavior"), 
        Themeable(false)
        ]
        public bool CultureInvariantValues
        {
            get
            {
                object obj1 = this.ViewState["CultureInvariantValues"];
                if (obj1 != null)
                {
                    return (bool)obj1;
                }
                return false;
            }
            set
            {
                this.ViewState["CultureInvariantValues"] = value;
            }
        }

        protected static int CutoffYear
        {
            get
            {
                return DateTimeFormatInfo.CurrentInfo.Calendar.TwoDigitYearMax;
            }
        }

        [
        DefaultValue(0), 
        Category("Behavior"), 
        Themeable(false)
        ]
        public ValidationDataType Type
        {
            get
            {
                object obj1 = this.ViewState["Type"];
                if (obj1 != null)
                {
                    return (ValidationDataType)obj1;
                }
                return ValidationDataType.String;
            }
            set
            {
                if ((value < ValidationDataType.String) || (value > ValidationDataType.Currency))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.ViewState["Type"] = value;
            }
        }
 


        #endregion


        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            ValidationDataType type1 = this.Type;
            if (type1 != ValidationDataType.String)
            {
                string text1 = this.ClientID;
                HtmlTextWriter writer1 = null;
                base.AddExpandoAttribute(writer1, text1, "type", PropertyConverter.EnumToString(typeof(ValidationDataType), type1), false);
                NumberFormatInfo info1 = NumberFormatInfo.CurrentInfo;
                switch (type1)
                {
                    case ValidationDataType.Double:
                        {
                            string text2 = info1.NumberDecimalSeparator;
                            base.AddExpandoAttribute(writer1, text1, "decimalchar", text2);
                            return;
                        }
                    case ValidationDataType.Currency:
                        {
                            string text3 = info1.CurrencyDecimalSeparator;
                            base.AddExpandoAttribute(writer1, text1, "decimalchar", text3);
                            string text4 = info1.CurrencyGroupSeparator;
                            if (text4[0] == '\x00a0')
                            {
                                text4 = " ";
                            }
                            base.AddExpandoAttribute(writer1, text1, "groupchar", text4);
                            int num1 = info1.CurrencyDecimalDigits;
                            base.AddExpandoAttribute(writer1, text1, "digits", num1.ToString(NumberFormatInfo.InvariantInfo), false);
                            int num2 = SAHLBaseCompareValidator.GetCurrencyGroupSize(info1);
                            if (num2 > 0)
                            {
                                base.AddExpandoAttribute(writer1, text1, "groupsize", num2.ToString(NumberFormatInfo.InvariantInfo), false);
                            }
                            return;
                        }
                    case ValidationDataType.Date:
                        {
                            base.AddExpandoAttribute(writer1, text1, "dateorder", SAHLBaseCompareValidator.GetDateElementOrder(), false);
                            base.AddExpandoAttribute(writer1, text1, "cutoffyear", SAHLBaseCompareValidator.CutoffYear.ToString(NumberFormatInfo.InvariantInfo), false);
                            int num3 = DateTime.Today.Year;
                            int num4 = num3 - (num3 % 100);
                            base.AddExpandoAttribute(writer1, text1, "century", num4.ToString(NumberFormatInfo.InvariantInfo), false);
                            break;
                        }
                }
            }
        }

        public static bool CanConvert(string text, ValidationDataType type)
        {
            return SAHLBaseCompareValidator.CanConvert(text, type, false);
        }

        public static bool CanConvert(string text, ValidationDataType type, bool cultureInvariant)
        {
            object obj1 = null;
            return SAHLBaseCompareValidator.Convert(text, type, cultureInvariant, out obj1);
        }

        protected static bool Compare(string leftText, string rightText, ValidationCompareOperator op, ValidationDataType type)
        {
            return SAHLBaseCompareValidator.Compare(leftText, false, rightText, false, op, type);
        }

        protected static bool Compare(string leftText, bool cultureInvariantLeftText, string rightText, bool cultureInvariantRightText, ValidationCompareOperator op, ValidationDataType type)
        {
            object obj1;
            int num1;
            if (!SAHLBaseCompareValidator.Convert(leftText, type, cultureInvariantLeftText, out obj1))
            {
                return false;
            }
            if (op != ValidationCompareOperator.DataTypeCheck)
            {
                object obj2;
                if (!SAHLBaseCompareValidator.Convert(rightText, type, cultureInvariantRightText, out obj2))
                {
                    return true;
                }
                switch (type)
                {
                    case ValidationDataType.String:
                        num1 = string.Compare((string)obj1, (string)obj2, false, CultureInfo.CurrentCulture);
                        goto Label_00AC;

                    case ValidationDataType.Integer:
                        num1 = ((int)obj1).CompareTo(obj2);
                        goto Label_00AC;

                    case ValidationDataType.Double:
                        num1 = ((double)obj1).CompareTo(obj2);
                        goto Label_00AC;

                    case ValidationDataType.Date:
                        num1 = ((DateTime)obj1).CompareTo(obj2);
                        goto Label_00AC;

                    case ValidationDataType.Currency:
                        num1 = ((decimal)obj1).CompareTo(obj2);
                        goto Label_00AC;
                }
            }
            return true;
        Label_00AC:
            switch (op)
            {
                case ValidationCompareOperator.Equal:
                    return (num1 == 0);

                case ValidationCompareOperator.NotEqual:
                    return (num1 != 0);

                case ValidationCompareOperator.GreaterThan:
                    return (num1 > 0);

                case ValidationCompareOperator.GreaterThanEqual:
                    return (num1 >= 0);

                case ValidationCompareOperator.LessThan:
                    return (num1 < 0);

                case ValidationCompareOperator.LessThanEqual:
                    return (num1 <= 0);
            }
            return true;
        }

        protected static bool Convert(string text, ValidationDataType type, out object value)
        {
            return SAHLBaseCompareValidator.Convert(text, type, false, out value);
        }

        protected static bool Convert(string text, ValidationDataType type, bool cultureInvariant, out object value)
        {
            value = null;
            try
            {
                string text1;
                string text3;
                switch (type)
                {
                    case ValidationDataType.String:
                        value = text;
                        goto Label_0113;

                    case ValidationDataType.Integer:
                        value = int.Parse(text, CultureInfo.InvariantCulture);
                        goto Label_0113;

                    case ValidationDataType.Double:
                        if (!cultureInvariant)
                        {
                            break;
                        }
                        text1 = SAHLBaseCompareValidator.ConvertDouble(text, CultureInfo.InvariantCulture.NumberFormat);
                        goto Label_0065;

                    case ValidationDataType.Date:
                        if (!cultureInvariant)
                        {
                            goto Label_0094;
                        }
                        value = SAHLBaseCompareValidator.ConvertDate(text, "ymd");
                        goto Label_0113;

                    case ValidationDataType.Currency:
                        if (!cultureInvariant)
                        {
                            goto Label_00EA;
                        }
                        text3 = SAHLBaseCompareValidator.ConvertCurrency(text, CultureInfo.InvariantCulture.NumberFormat);
                        goto Label_00F6;

                    default:
                        goto Label_0113;
                }
                text1 = SAHLBaseCompareValidator.ConvertDouble(text, NumberFormatInfo.CurrentInfo);
            Label_0065:
                if (text1 != null)
                {
                    value = double.Parse(text1, CultureInfo.InvariantCulture);
                }
                goto Label_0113;
            Label_0094:
                if (DateTimeFormatInfo.CurrentInfo.Calendar.GetType() != typeof(GregorianCalendar))
                {
                    value = DateTime.Parse(text, CultureInfo.CurrentCulture);
                    goto Label_0113;
                }
                string text2 = SAHLBaseCompareValidator.GetDateElementOrder();
                value = SAHLBaseCompareValidator.ConvertDate(text, text2);
                goto Label_0113;
            Label_00EA:
                text3 = SAHLBaseCompareValidator.ConvertCurrency(text, NumberFormatInfo.CurrentInfo);
            Label_00F6:
                if (text3 != null)
                {
                    value = decimal.Parse(text3, CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                value = null;
            }
        Label_0113:
            return (value != null);
        }

        internal string ConvertCultureInvariantToCurrentCultureFormat(string valueInString, ValidationDataType type)
        {
            object obj1;
            SAHLBaseCompareValidator.Convert(valueInString, type, true, out obj1);
            if (obj1 is DateTime)
            {
                DateTime time1 = (DateTime)obj1;
                return time1.ToShortDateString();
            }
            return System.Convert.ToString(obj1, CultureInfo.CurrentCulture);
        }

        private static string ConvertCurrency(string text, NumberFormatInfo info)
        {
            string text3;
            string text4;
            string text1 = info.CurrencyDecimalSeparator;
            string text2 = info.CurrencyGroupSeparator;
            int num1 = SAHLBaseCompareValidator.GetCurrencyGroupSize(info);
            if (num1 > 0)
            {
                string text5 = num1.ToString(NumberFormatInfo.InvariantInfo);
                text3 = "{1," + text5 + "}";
                text4 = "{" + text5 + "}";
            }
            else
            {
                text3 = text4 = "+";
            }
            if (text2[0] == '\x00a0')
            {
                text2 = " ";
            }
            int num2 = info.CurrencyDecimalDigits;
            bool flag1 = num2 > 0;
            string text6 = @"^\s*([-\+])?((\d" + text3 + @"(\" + text2 + @"\d" + text4 + @")+)|\d*)" + (flag1 ? (@"\" + text1 + @"?(\d{0," + num2.ToString(NumberFormatInfo.InvariantInfo) + "})") : string.Empty) + @"\s*$";
            Match match1 = Regex.Match(text, text6);
            if (!match1.Success)
            {
                return null;
            }
            if (((match1.Groups[2].Length == 0) && flag1) && (match1.Groups[5].Length == 0))
            {
                return null;
            }
            return (match1.Groups[1].Value + match1.Groups[2].Value.Replace(text2, string.Empty) + ((flag1 && (match1.Groups[5].Length > 0)) ? ("." + match1.Groups[5].Value) : string.Empty));
        }

        private static object ConvertDate(string text, string dateElementOrder)
        {
            int num1;
            int num2;
            int num3;
            string text1 = @"^\s*((\d{4})|(\d{2}))([-/]|\. ?)(\d{1,2})\4(\d{1,2})\s*$";
            Match match1 = Regex.Match(text, text1);
            if (match1.Success && (match1.Groups[2].Success || (dateElementOrder == "ymd")))
            {
                num1 = int.Parse(match1.Groups[6].Value, CultureInfo.InvariantCulture);
                num2 = int.Parse(match1.Groups[5].Value, CultureInfo.InvariantCulture);
                if (match1.Groups[2].Success)
                {
                    num3 = int.Parse(match1.Groups[2].Value, CultureInfo.InvariantCulture);
                }
                else
                {
                    num3 = SAHLBaseCompareValidator.GetFullYear(int.Parse(match1.Groups[3].Value, CultureInfo.InvariantCulture));
                }
            }
            else
            {
                if (dateElementOrder == "ymd")
                {
                    return null;
                }
                string text2 = @"^\s*(\d{1,2})([-/]|\. ?)(\d{1,2})\2((\d{4})|(\d{2}))\s*$";
                match1 = Regex.Match(text, text2);
                if (!match1.Success)
                {
                    return null;
                }
                if (dateElementOrder == "mdy")
                {
                    num1 = int.Parse(match1.Groups[3].Value, CultureInfo.InvariantCulture);
                    num2 = int.Parse(match1.Groups[1].Value, CultureInfo.InvariantCulture);
                }
                else
                {
                    num1 = int.Parse(match1.Groups[1].Value, CultureInfo.InvariantCulture);
                    num2 = int.Parse(match1.Groups[3].Value, CultureInfo.InvariantCulture);
                }
                if (match1.Groups[5].Success)
                {
                    num3 = int.Parse(match1.Groups[5].Value, CultureInfo.InvariantCulture);
                }
                else
                {
                    num3 = SAHLBaseCompareValidator.GetFullYear(int.Parse(match1.Groups[6].Value, CultureInfo.InvariantCulture));
                }
            }
            return new DateTime(num3, num2, num1);
        }

        private static string ConvertDouble(string text, NumberFormatInfo info)
        {
            if (text.Length == 0)
            {
                return "0";
            }
            string text1 = info.NumberDecimalSeparator;
            string text2 = @"^\s*([-\+])?(\d*)\" + text1 + @"?(\d*)\s*$";
            Match match1 = Regex.Match(text, text2);
            if (!match1.Success)
            {
                return null;
            }
            if ((match1.Groups[2].Length == 0) && (match1.Groups[3].Length == 0))
            {
                return null;
            }
            return (match1.Groups[1].Value + ((match1.Groups[2].Length > 0) ? match1.Groups[2].Value : "0") + ((match1.Groups[3].Length > 0) ? ("." + match1.Groups[3].Value) : string.Empty));
        }

        internal string ConvertToShortDateString(string text)
        {
            DateTime time1;
            if (DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out time1))
            {
                text = time1.ToShortDateString();
            }
            return text;
        }

        private static int GetCurrencyGroupSize(NumberFormatInfo info)
        {
            int[] numArray1 = info.CurrencyGroupSizes;
            if ((numArray1 != null) && (numArray1.Length == 1))
            {
                return numArray1[0];
            }
            return -1;
        }


        protected static string GetDateElementOrder()
        {
            string text1 = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            if (text1.IndexOf('y') < text1.IndexOf('M'))
            {
                return "ymd";
            }
            if (text1.IndexOf('M') < text1.IndexOf('d'))
            {
                return "mdy";
            }
            return "dmy";
        }


        protected static int GetFullYear(int shortYear)
        {
            return DateTimeFormatInfo.CurrentInfo.Calendar.ToFourDigitYear(shortYear);
        }


        internal bool IsInStandardDateFormat(string date)
        {
            return Regex.Match(date, @"^\s*(\d+)([-/]|\. ?)(\d+)\2(\d+)\s*$").Success;
        }

 
 
 

 

 
 


    }
}
