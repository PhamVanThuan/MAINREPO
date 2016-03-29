using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
namespace SqlFunctions
{
	public class IDNumberFunctions
	{
		private const string IDExpression = "(?<Year>[0-9][0-9])(?<Month>([0][1-9])|([1][0-2]))(?<Day>([0-2][0-9])|([3][0-1]))(?<Gender>[0-9])(?<Series>[0-9]{3})(?<Citizenship>[0-9])(?<Uniform>[0-9])(?<Control>[0-9])";
		private static readonly Regex expression = new Regex("(?<Year>[0-9][0-9])(?<Month>([0][1-9])|([1][0-2]))(?<Day>([0-2][0-9])|([3][0-1]))(?<Gender>[0-9])(?<Series>[0-9]{3})(?<Citizenship>[0-9])(?<Uniform>[0-9])(?<Control>[0-9])", RegexOptions.Compiled | RegexOptions.Singleline);
		[SqlFunction]
		public static SqlBoolean IsValidIDNumber(SqlString IDNumber)
		{
			Match match = IDNumberFunctions.expression.Match(IDNumber.ToString().Trim());
			SqlBoolean result;
			if (match.Success)
			{
				int num = int.Parse(match.Value.Substring(0, 1)) + int.Parse(match.Value.Substring(2, 1)) + int.Parse(match.Value.Substring(4, 1)) + int.Parse(match.Value.Substring(6, 1)) + int.Parse(match.Value.Substring(8, 1)) + int.Parse(match.Value.Substring(10, 1));
				int num2 = int.Parse(string.Concat(new string[]
				{
					match.Value.Substring(1, 1),
					match.Value.Substring(3, 1),
					match.Value.Substring(5, 1),
					match.Value.Substring(7, 1),
					match.Value.Substring(9, 1),
					match.Value.Substring(11, 1)
				}));
				string text = (num2 * 2).ToString();
				num2 = 0;
				for (int i = 0; i < text.Length; i++)
				{
					num2 += int.Parse(text.Substring(i, 1));
				}
				string text2 = (num + num2).ToString();
				text2 = text2.Substring(text2.Length - 1, 1);
				int num3 = 0;
				if (text2 != "0")
				{
					num3 = 10 - int.Parse(text2.Substring(text2.Length - 1, 1));
				}
				if (match.Groups["Control"].Value == num3.ToString())
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
	}
}
