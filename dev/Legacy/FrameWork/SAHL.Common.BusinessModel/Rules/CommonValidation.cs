using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SAHL.Common.BusinessModel.Rules
{
    /// <summary>
    /// Provides common validation functions for use in rules.
    /// </summary>
    public class CommonValidation
    {

        private CommonValidation()
        {
        }

        /// <summary>
        /// Determines if an email address conforms to a standard format using a regular expression.
        /// </summary>
        /// <param name="emailAddress">The email address to be validated.</param>
        /// <returns>True if the email address is valid.  If <c>emailAddress</c> is null, empty or invalid, false.</returns>
        public static bool IsEmail(string emailAddress)
        {
            if (String.IsNullOrEmpty(emailAddress))
                return false;

            // the regular expression below works fine except doesn't catch multiple @ symbols - rather than 
            // hack around with it just check the count first
            if (emailAddress.Split('@').Length != 2)
                return false;

            Regex regxEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return regxEmail.IsMatch(emailAddress);

        }

		/// <summary>
		/// Function to test for Positive Integers.
		/// </summary>
		/// <param name="number">The number to be validated.</param>
		/// <returns>True if the number is valid, else false</returns>
		public static bool IsNaturalNumber(string number)
		{
			if (String.IsNullOrEmpty(number))
				return false;
			Regex objNotNaturalPattern=new Regex("[^0-9]");
			Regex objNaturalPattern=new Regex("0*[1-9][0-9]*");
			return !objNotNaturalPattern.IsMatch(number) &&
			objNaturalPattern.IsMatch(number);
		}

		/// <summary>
		/// Function to test for Positive Integers with zero inclusive
		/// </summary>
		/// <param name="number">The number to be validated.</param>
		/// <returns>True if the number is valid, else false</returns>
		public static bool IsWholeNumber(string number)
		{
			if (String.IsNullOrEmpty(number))
				return false;
			Regex objNotWholePattern=new Regex("[^0-9]");
			return !objNotWholePattern.IsMatch(number);
		}

		/// <summary>
		/// Function to Test for Integers both Positive and Negative
		/// </summary>
		/// <param name="number">The number to be validated.</param>
		/// <returns>True if the number is valid, else false</returns>
		public static bool IsInteger(string number)
		{
			if (String.IsNullOrEmpty(number))
				return false;
			Regex objNotIntPattern=new Regex("[^0-9-]");
			Regex objIntPattern=new Regex("^-[0-9]+$|^[0-9]+$");
			return !objNotIntPattern.IsMatch(number) && objIntPattern.IsMatch(number);
		}

		/// <summary>
		/// Function to Test for Positive Number both Integer and Real
		/// </summary>
		/// <param name="number">The number to be validated.</param>
		/// <returns>True if the number is valid, else false</returns>
		public static bool IsPositiveNumber(string number)
		{
			if (String.IsNullOrEmpty(number))
				return false;
			Regex objNotPositivePattern=new Regex("[^0-9.]");
			Regex objPositivePattern=new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
			Regex objTwoDotPattern=new Regex("[0-9]*[.][0-9]*[.][0-9]*");
			return !objNotPositivePattern.IsMatch(number) &&
			objPositivePattern.IsMatch(number) &&
			!objTwoDotPattern.IsMatch(number);
		}

		/// <summary>
		/// Function to test whether the string is valid number or not
		/// </summary>
		/// <param name="number">The number to be validated.</param>
		/// <returns>True if the number is valid, else false</returns>
		public static bool IsNumber(string number)
		{
			if (String.IsNullOrEmpty(number))
				return false;
			Regex objNotNumberPattern=new Regex("[^0-9.-]");
			Regex objTwoDotPattern=new Regex("[0-9]*[.][0-9]*[.][0-9]*");
			Regex objTwoMinusPattern=new Regex("[0-9]*[-][0-9]*[-][0-9]*");
			String strValidRealPattern="^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
			String strValidIntegerPattern="^([-]|[0-9])[0-9]*$";
			Regex objNumberPattern =new Regex("(" + strValidRealPattern +")|(" + strValidIntegerPattern + ")");
			return !objNotNumberPattern.IsMatch(number) &&
			!objTwoDotPattern.IsMatch(number) &&
			!objTwoMinusPattern.IsMatch(number) &&
			objNumberPattern.IsMatch(number);
		}

		/// <summary>
		/// Function to Check for AlphaNumeric.
		/// </summary>
		/// <param name="strToCheck">The string to be validated.</param>
		/// <returns>True if the string is valid, else false</returns>
		public static bool IsAlphaNumeric(string strToCheck)
		{
			if (String.IsNullOrEmpty(strToCheck))
				return false;
			Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9]");
			return !objAlphaNumericPattern.IsMatch(strToCheck);
		}

		/// <summary>
		/// Function To test for Alphabets.
		/// </summary>
		/// <param name="strToCheck">The string to be validated.</param>
		/// <returns>True if the string is valid, else false</returns>
		public static bool IsAlpha(string strToCheck)
		{
			if (String.IsNullOrEmpty(strToCheck))
				return false;
			Regex objAlphaPattern=new Regex("[^a-zA-Z]");
			return !objAlphaPattern.IsMatch(strToCheck);
		}

        /// <summary>
		/// Function To test for Alphabets, with space, hyphen and slash chars
        /// </summary>
		/// <param name="strToCheck">The string to be validated.</param>
		/// <returns>True if the string is valid, else false</returns>
		public static bool IsAlphaWithSpecial(string strToCheck)
        {
            if (String.IsNullOrEmpty(strToCheck))
                return false;
            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z\\-\\s\\\\\\/]");
			return !objAlphaNumericPattern.IsMatch(strToCheck);
        }
    }
}
