using ClientSuperSearch;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
public class UserDefinedFunctions
{
	private static readonly Regex Reg = new Regex("(?<QUOTES>\".*?\")|(?<NOTAND>\\band\\b\\s\\bnot\\b)|(?<AND>\\band\\b)|(?<OR>\\bor\\b)|(?<NUMBER>\\d+\\*?)|(?<WORD>\\b\\w+\\b)|\\w+\\*?");
	private static readonly int ACCOUNTRANKING = 20;
	private static readonly int ACCOUNTKEYINDEX = 37;
	private static readonly string[] IgnoreWords = new string[]
	{
		"co",
		"za",
		"com"
	};
	[SqlFunction(FillRowMethodName = "FillRow", SystemDataAccess = SystemDataAccessKind.Read, DataAccess = DataAccessKind.Read, TableDefinition = "[LegalEntityKey] [int],\r\n\t[LegalEntityTypeKey] [int],\r\n\t[MaritalStatusKey] [int],\r\n\t[GenderKey] [int],\r\n\t[PopulationGroupKey] [int],\r\n\t[IntroductionDate] [datetime],\r\n\t[Salutationkey] [int],\r\n\t[FirstNames] [nvarchar](50),\r\n\t[Initials] [nvarchar](5),\r\n\t[Surname] [nvarchar](50),\r\n\t[PreferredName] [nvarchar](50),\r\n\t[IDNumber] [nvarchar](20),\r\n\t[PassportNumber] [nvarchar](20),\r\n\t[TaxNumber] [nvarchar](20),\r\n\t[RegistrationNumber] [nvarchar](25),\r\n\t[RegisteredName] [nvarchar](50),\r\n\t[TradingName] [nvarchar](50),\r\n\t[DateOfBirth] [datetime],\r\n\t[HomePhoneCode] [nvarchar](10),\r\n\t[HomePhoneNumber] [nvarchar](15),\r\n\t[WorkPhoneCode] [nvarchar](10),\r\n\t[WorkPhoneNumber] [nvarchar](15),\r\n\t[CellPhoneNumber] [nvarchar](15),\r\n\t[EmailAddress] [nvarchar](50),\r\n\t[FaxCode] [nvarchar](10),\r\n\t[FaxNumber] [nvarchar](15),\r\n\t[Password] [nvarchar](50),\r\n\t[CitizenTypeKey] [int],\r\n\t[LegalEntityStatusKey] [int],\r\n\t[Comments] [nvarchar](255),\r\n\t[LegalEntityExceptionStatusKey] [int],\r\n\t[UserID] [nvarchar](25),\r\n\t[ChangeDate] [datetime],\r\n\t[EducationKey] [int],\r\n\t[HomeLanguageKey] [int],\r\n\t[DocumentLanguageKey] [int],\r\n\t[ResidenceStatusKey] [int],\r\n    [RANK] [int] ")]
	public static IEnumerable LegalEntitySearchRank(SqlString SearchString, SqlString LegalEntityTypes)
	{
		ArrayList arrayList = new ArrayList();
		IEnumerable result;
		if (SearchString.IsNull || SearchString.ToString().Length == 0)
		{
			result = arrayList;
		}
		else
		{
			MatchCollection matchCollection = UserDefinedFunctions.Reg.Matches(SearchString.ToString());
			string[] groupNames = UserDefinedFunctions.Reg.GetGroupNames();
			string text = "";
			int[] indeces = new int[]
			{
				7,
				8,
				9,
				11,
				14,
				15,
				16
			};
			int[] indeces2 = new int[]
			{
				10,
				19,
				21,
				22,
				23,
				25
			};
			List<And> list = new List<And>();
			List<NAnd> list2 = new List<NAnd>();
			List<string> list3 = new List<string>();
			string text2 = "";
			string a = "";
			for (int i = 0; i < matchCollection.Count; i++)
			{
				string text3 = matchCollection[i].ToString().ToLower();
				string matchGroup = UserDefinedFunctions.GetMatchGroup(groupNames, matchCollection[i].Groups);
				string text4;
				if (text3.StartsWith("\"") && text3.EndsWith("\""))
				{
					text4 = "word";
				}
				else
				{
					if (text3 != "or" && text3 != "and" && text3 != "and not")
					{
						text4 = "word";
					}
					else
					{
						text4 = text3;
					}
				}
				string text5 = text4;
				if (text5 != null)
				{
					if (!(text5 == "word"))
					{
						if (!(text5 == "or"))
						{
							if (!(text5 == "and"))
							{
								if (!(text5 == "notand"))
								{
									if (!(text5 == "number"))
									{
									}
								}
								else
								{
									if (i < matchCollection.Count - 1)
									{
										string matchGroup2 = UserDefinedFunctions.GetMatchGroup(groupNames, matchCollection[i + 1].Groups);
										if (matchGroup2 != "NUMBER")
										{
											text2 = "";
											a = "";
										}
									}
								}
							}
							else
							{
								if (i < matchCollection.Count - 1)
								{
									string matchGroup2 = UserDefinedFunctions.GetMatchGroup(groupNames, matchCollection[i + 1].Groups);
									if (matchGroup2 == "NUMBER")
									{
										text2 = "";
										a = "";
									}
								}
							}
						}
					}
					else
					{
						list3.Add(text3.Replace("*", ""));
						if (!UserDefinedFunctions.ShouldIgnore(text3))
						{
							if (text3.Length > 3)
							{
								text3 = "\"" + text3 + "*\"";
							}
							text = text + text3 + " or ";
							if (text2 != "")
							{
								if (a == "and")
								{
									list.Add(new And(text2, text3));
								}
								else
								{
									list2.Add(new NAnd(text2, text3));
								}
								text2 = "";
								a = "";
							}
							else
							{
								if (i < matchCollection.Count - 1)
								{
									string text6 = matchCollection[i + 1].ToString();
									if (text6 == "and" || text6 == "notand")
									{
										text2 = text3;
										a = text6;
									}
								}
							}
						}
					}
				}
			}
			if (text.Length > 0 && text.EndsWith("or "))
			{
				text = text.Substring(0, text.Length - 4);
			}
			else
			{
				if (text.Length == 0)
				{
					result = arrayList;
					return result;
				}
			}
			string text7 = string.Format("select le.*, 1 from legalentity le inner join containstable(LegalEntity, (FirstNames, Surname, PreferredName, RegisteredName, TradingName, IDNumber, PassportNumber, RegistrationNumber, Initials, CellPhoneNumber, EmailAddress, FaxNumber, HomePhoneNumber, WorkPhoneNumber), '{0}') ct on le.legalentitykey = ct.[KEY] ", text);
			if (!LegalEntityTypes.IsNull && LegalEntityTypes.ToString().Length > 0)
			{
				text7 += string.Format(" and le.legalentitytypekey in ({0}) ", LegalEntityTypes);
			}
			text7 += " group by le.[LegalEntityKey],le.[LegalEntityTypeKey],le.[MaritalStatusKey],le.[GenderKey],le.[PopulationGroupKey],le.[IntroductionDate],le.[Salutationkey],le.[FirstNames],le.[Initials],le.[Surname],le.[PreferredName],le.[IDNumber],le.[PassportNumber],le.[TaxNumber],le.[RegistrationNumber],le.[RegisteredName],le.[TradingName],le.[DateOfBirth],le.[HomePhoneCode],le.[HomePhoneNumber],le.[WorkPhoneCode],le.[WorkPhoneNumber],le.[CellPhoneNumber],le.[EmailAddress],le.[FaxCode],le.[FaxNumber],le.[Password],le.[CitizenTypeKey],le.[LegalEntityStatusKey],le.[Comments],le.[LegalEntityExceptionStatusKey],le.[UserID],le.[ChangeDate],le.[EducationKey],le.[HomeLanguageKey],le.[DocumentLanguageKey],le.[ResidenceStatusKey] ";
			using (SqlConnection sqlConnection = new SqlConnection("context connection=true"))
			{
				sqlConnection.Open();
				SqlPipe pipe = SqlContext.Pipe;
				SqlCommand sqlCommand = sqlConnection.CreateCommand();
				sqlCommand.CommandText = text7;
				SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
				while (sqlDataReader.Read())
				{
					int num = 0;
					object[] array = new object[sqlDataReader.FieldCount];
					sqlDataReader.GetSqlValues(array);
					for (int i = 0; i < list.Count; i++)
					{
						num += UserDefinedFunctions.RankConstrainedMatch(array, indeces, list[i].LHS, list[i].RHS);
					}
					for (int i = 0; i < list2.Count; i++)
					{
						num += UserDefinedFunctions.RankConstrainedMatch(array, indeces, list2[i].LHS, list2[i].RHS);
					}
					if (list.Count == 0 && list2.Count == 0)
					{
						num += UserDefinedFunctions.RankMatch(array, indeces, list3) * 2;
						num += UserDefinedFunctions.RankMatch(array, indeces2, list3);
					}
					if (list3.Count > 3 && (list.Count > 0 || list2.Count > 0))
					{
						num += UserDefinedFunctions.RankMatch(array, indeces, list3) * 2;
						num += UserDefinedFunctions.RankMatch(array, indeces2, list3);
					}
					array[sqlDataReader.FieldCount - 1] = new SqlInt32(num);
					arrayList.Add(array);
				}
				result = arrayList;
			}
		}
		return result;
	}
	[SqlFunction(FillRowMethodName = "FillRow", SystemDataAccess = SystemDataAccessKind.Read, DataAccess = DataAccessKind.Read, TableDefinition = "[LegalEntityKey] [int],\r\n\t[LegalEntityTypeKey] [int],\r\n\t[MaritalStatusKey] [int],\r\n\t[GenderKey] [int],\r\n\t[PopulationGroupKey] [int],\r\n\t[IntroductionDate] [datetime],\r\n\t[Salutationkey] [int],\r\n\t[FirstNames] [nvarchar](50),\r\n\t[Initials] [nvarchar](5),\r\n\t[Surname] [nvarchar](50),\r\n\t[PreferredName] [nvarchar](50),\r\n\t[IDNumber] [nvarchar](20),\r\n\t[PassportNumber] [nvarchar](20),\r\n\t[TaxNumber] [nvarchar](20),\r\n\t[RegistrationNumber] [nvarchar](25),\r\n\t[RegisteredName] [nvarchar](50),\r\n\t[TradingName] [nvarchar](50),\r\n\t[DateOfBirth] [datetime],\r\n\t[HomePhoneCode] [nvarchar](10),\r\n\t[HomePhoneNumber] [nvarchar](15),\r\n\t[WorkPhoneCode] [nvarchar](10),\r\n\t[WorkPhoneNumber] [nvarchar](15),\r\n\t[CellPhoneNumber] [nvarchar](15),\r\n\t[EmailAddress] [nvarchar](50),\r\n\t[FaxCode] [nvarchar](10),\r\n\t[FaxNumber] [nvarchar](15),\r\n\t[Password] [nvarchar](50),\r\n\t[CitizenTypeKey] [int],\r\n\t[LegalEntityStatusKey] [int],\r\n\t[Comments] [nvarchar](255),\r\n\t[LegalEntityExceptionStatusKey] [int],\r\n\t[UserID] [nvarchar](25),\r\n\t[ChangeDate] [datetime],\r\n\t[EducationKey] [int],\r\n\t[HomeLanguageKey] [int],\r\n\t[DocumentLanguageKey] [int],\r\n\t[ResidenceStatusKey] [int],\r\n    [RANK] [int] ")]
	public static IEnumerable ClientSearchRank(SqlString SearchString, SqlString OriginationSources, SqlString LegalEntityTypes, SqlString AccountTypes)
	{
		ArrayList arrayList = new ArrayList();
		IEnumerable result;
		if (SearchString.IsNull || SearchString.ToString().Length == 0 || OriginationSources.IsNull || OriginationSources.ToString().Length == 0)
		{
			result = arrayList;
		}
		else
		{
			MatchCollection matchCollection = UserDefinedFunctions.Reg.Matches(SearchString.ToString());
			string[] groupNames = UserDefinedFunctions.Reg.GetGroupNames();
			string text = "";
			int[] indeces = new int[]
			{
				7,
				8,
				9,
				11,
				14,
				15,
				16
			};
			int[] indeces2 = new int[]
			{
				10,
				19,
				21,
				22,
				23,
				25
			};
			List<And> list = new List<And>();
			List<NAnd> list2 = new List<NAnd>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			string text2 = "";
			string a = "";
			for (int i = 0; i < matchCollection.Count; i++)
			{
				string text3 = matchCollection[i].ToString().ToLower();
				string matchGroup = UserDefinedFunctions.GetMatchGroup(groupNames, matchCollection[i].Groups);
				string text4;
				if (text3.StartsWith("\"") && text3.EndsWith("\""))
				{
					text4 = "word";
				}
				else
				{
					if (text3 != "or" && text3 != "and" && text3 != "and not")
					{
						text4 = "word";
					}
					else
					{
						text4 = text3;
					}
				}
				string text5 = text4;
				if (text5 != null)
				{
					if (!(text5 == "word"))
					{
						if (!(text5 == "or"))
						{
							if (!(text5 == "and"))
							{
								if (!(text5 == "notand"))
								{
									if (!(text5 == "number"))
									{
									}
								}
								else
								{
									if (i < matchCollection.Count - 1)
									{
										string matchGroup2 = UserDefinedFunctions.GetMatchGroup(groupNames, matchCollection[i + 1].Groups);
										if (matchGroup2 != "NUMBER")
										{
											text2 = "";
											a = "";
										}
									}
								}
							}
							else
							{
								if (i < matchCollection.Count - 1)
								{
									string matchGroup2 = UserDefinedFunctions.GetMatchGroup(groupNames, matchCollection[i + 1].Groups);
									if (matchGroup2 == "NUMBER")
									{
										text2 = "";
										a = "";
									}
								}
							}
						}
					}
					else
					{
						list3.Add(text3.Replace("*", ""));
						if (!UserDefinedFunctions.ShouldIgnore(text3))
						{
							if (text3.Length > 3)
							{
								text3 = "\"" + text3 + "*\"";
							}
							text = text + text3 + " or ";
							if (text2 != "")
							{
								if (a == "and")
								{
									list.Add(new And(text2, text3));
								}
								else
								{
									list2.Add(new NAnd(text2, text3));
								}
								text2 = "";
								a = "";
							}
							else
							{
								if (i < matchCollection.Count - 1)
								{
									string text6 = matchCollection[i + 1].ToString();
									if (text6 == "and" || text6 == "notand")
									{
										text2 = text3;
										a = text6;
									}
								}
								if (matchGroup == "NUMBER")
								{
									string text7 = text3.Replace("*", "");
									text7 = text7.Replace("\"", "");
									if (text7.Length == 7)
									{
										list4.Add(text7);
									}
								}
							}
						}
					}
				}
			}
			if (text.Length > 0 && text.EndsWith("or "))
			{
				text = text.Substring(0, text.Length - 4);
			}
			else
			{
				if (text.Length == 0)
				{
					result = arrayList;
					return result;
				}
			}
			string text8 = string.Format("select le.*, [2am].dbo.commadelimit(a.accountkey) from legalentity le inner join containstable(LegalEntity, (FirstNames, Surname, PreferredName, RegisteredName, TradingName, IDNumber, PassportNumber, RegistrationNumber, Initials, CellPhoneNumber, EmailAddress, FaxNumber, HomePhoneNumber, WorkPhoneNumber), '{0}') ct on le.legalentitykey = ct.[KEY] inner join [role] r on r.legalentitykey = le.legalentitykey inner join account a on a.accountkey = r.accountkey and a.RRR_OriginationSourceKey in ({1}) ", text, OriginationSources);
			if (!LegalEntityTypes.IsNull && LegalEntityTypes.ToString().Length > 0)
			{
				text8 += string.Format(" and le.legalentitytypekey in ({0}) ", LegalEntityTypes);
			}
			if (!AccountTypes.IsNull && AccountTypes.ToString().Length > 0)
			{
				text8 += string.Format(" inner join financialservice fs on fs.accountkey = a.accountkey inner join financialservicetype fst on fst.financialservicetypekey = fs.financialservicetypekey and fst.financialservicetypekey in ({0})", AccountTypes);
			}
			text8 += " group by le.[LegalEntityKey],le.[LegalEntityTypeKey],le.[MaritalStatusKey],le.[GenderKey],le.[PopulationGroupKey],le.[IntroductionDate],le.[Salutationkey],le.[FirstNames],le.[Initials],le.[Surname],le.[PreferredName],le.[IDNumber],le.[PassportNumber],le.[TaxNumber],le.[RegistrationNumber],le.[RegisteredName],le.[TradingName],le.[DateOfBirth],le.[HomePhoneCode],le.[HomePhoneNumber],le.[WorkPhoneCode],le.[WorkPhoneNumber],le.[CellPhoneNumber],le.[EmailAddress],le.[FaxCode],le.[FaxNumber],le.[Password],le.[CitizenTypeKey],le.[LegalEntityStatusKey],le.[Comments],le.[LegalEntityExceptionStatusKey],le.[UserID],le.[ChangeDate],le.[EducationKey],le.[HomeLanguageKey],le.[DocumentLanguageKey],le.[ResidenceStatusKey]";
			if (list4.Count > 0)
			{
				string text9 = "";
				for (int j = 0; j < list4.Count; j++)
				{
					text9 += list4[j].ToString();
					if (j < list4.Count - 1)
					{
						text9 += ", ";
					}
				}
				text8 += "\n union select le.*, [2am].dbo.commadelimit(a.accountkey) from legalentity le inner join [role] r on le.legalentitykey = r.legalentitykey inner join account a on a.accountkey = r.accountkey ";
				if (!LegalEntityTypes.IsNull && LegalEntityTypes.ToString().Length > 0)
				{
					text8 += string.Format(" and le.legalentitytypekey in ({0}) ", LegalEntityTypes);
				}
				if (!AccountTypes.IsNull && AccountTypes.ToString().Length > 0)
				{
					text8 += string.Format(" inner join financialservice fs on fs.accountkey = a.accountkey inner join financialservicetype fst on fst.financialservicetypekey = fs.financialservicetypekey and fst.financialservicetypekey in ({0}) ", AccountTypes);
				}
				text8 += string.Format(" where a.accountkey in ({0}) ", text9);
				text8 += " group by le.[LegalEntityKey],le.[LegalEntityTypeKey],le.[MaritalStatusKey],le.[GenderKey],le.[PopulationGroupKey],le.[IntroductionDate],le.[Salutationkey],le.[FirstNames],le.[Initials],le.[Surname],le.[PreferredName],le.[IDNumber],le.[PassportNumber],le.[TaxNumber],le.[RegistrationNumber],le.[RegisteredName],le.[TradingName],le.[DateOfBirth],le.[HomePhoneCode],le.[HomePhoneNumber],le.[WorkPhoneCode],le.[WorkPhoneNumber],le.[CellPhoneNumber],le.[EmailAddress],le.[FaxCode],le.[FaxNumber],le.[Password],le.[CitizenTypeKey],le.[LegalEntityStatusKey],le.[Comments],le.[LegalEntityExceptionStatusKey],le.[UserID],le.[ChangeDate],le.[EducationKey],le.[HomeLanguageKey],le.[DocumentLanguageKey],le.[ResidenceStatusKey]";
			}
			using (SqlConnection sqlConnection = new SqlConnection("context connection=true"))
			{
				sqlConnection.Open();
				SqlPipe pipe = SqlContext.Pipe;
				SqlCommand sqlCommand = sqlConnection.CreateCommand();
				sqlCommand.CommandText = text8;
				SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
				while (sqlDataReader.Read())
				{
					int num = 0;
					object[] array = new object[sqlDataReader.FieldCount];
					sqlDataReader.GetSqlValues(array);
					if (list4.Count > 0)
					{
						num += UserDefinedFunctions.RankAccountKeyMatch(array, UserDefinedFunctions.ACCOUNTKEYINDEX, list4);
					}
					for (int i = 0; i < list.Count; i++)
					{
						num += UserDefinedFunctions.RankConstrainedMatch(array, indeces, list[i].LHS, list[i].RHS);
					}
					for (int i = 0; i < list2.Count; i++)
					{
						num += UserDefinedFunctions.RankConstrainedMatch(array, indeces, list2[i].LHS, list2[i].RHS);
					}
					if (list.Count == 0 && list2.Count == 0)
					{
						num += UserDefinedFunctions.RankMatch(array, indeces, list3) * 2;
						num += UserDefinedFunctions.RankMatch(array, indeces2, list3);
					}
					if (list3.Count > 3 && (list.Count > 0 || list2.Count > 0))
					{
						num += UserDefinedFunctions.RankMatch(array, indeces, list3) * 2;
						num += UserDefinedFunctions.RankMatch(array, indeces2, list3);
					}
					array[sqlDataReader.FieldCount - 1] = new SqlInt32(num);
					arrayList.Add(array);
				}
				result = arrayList;
			}
		}
		return result;
	}
	private static bool ShouldIgnore(string Word)
	{
		bool result;
		for (int i = 0; i < UserDefinedFunctions.IgnoreWords.Length; i++)
		{
			if (Word == UserDefinedFunctions.IgnoreWords[i])
			{
				result = true;
				return result;
			}
		}
		result = false;
		return result;
	}
	public static void FillRow(object row, out SqlInt32 LegalEntityKey, out SqlInt32 LegalEntityTypeKey, out SqlInt32 MaritalStatusKey, out SqlInt32 GenderKey, out SqlInt32 PopulationGroupKey, out SqlDateTime IntroductionDate, out SqlInt32 Salutationkey, out SqlString FirstNames, out SqlString Initials, out SqlString Surname, out SqlString PreferredName, out SqlString IDNumber, out SqlString PassportNumber, out SqlString TaxNumber, out SqlString RegistrationNumber, out SqlString RegisteredName, out SqlString TradingName, out SqlDateTime DateOfBirth, out SqlString HomePhoneCode, out SqlString HomePhoneNumber, out SqlString WorkPhoneCode, out SqlString WorkPhoneNumber, out SqlString CellPhoneNumber, out SqlString EmailAddress, out SqlString FaxCode, out SqlString FaxNumber, out SqlString Password, out SqlInt32 CitizenTypeKey, out SqlInt32 LegalEntityStatusKey, out SqlString Comments, out SqlInt32 LegalEntityExceptionStatusKey, out SqlString UserID, out SqlDateTime ChangeDate, out SqlInt32 EducationKey, out SqlInt32 HomeLanguageKey, out SqlInt32 DocumentLanguageKey, out SqlInt32 ResidenceStatusKey, out SqlInt32 RANK)
	{
		object[] array = row as object[];
		LegalEntityKey = (SqlInt32)array[0];
		LegalEntityTypeKey = (SqlInt32)array[1];
		MaritalStatusKey = (SqlInt32)array[2];
		GenderKey = (SqlInt32)array[3];
		PopulationGroupKey = (SqlInt32)array[4];
		IntroductionDate = (SqlDateTime)array[5];
		Salutationkey = (SqlInt32)array[6];
		FirstNames = (SqlString)array[7];
		Initials = (SqlString)array[8];
		Surname = (SqlString)array[9];
		PreferredName = (SqlString)array[10];
		IDNumber = (SqlString)array[11];
		PassportNumber = (SqlString)array[12];
		TaxNumber = (SqlString)array[13];
		RegistrationNumber = (SqlString)array[14];
		RegisteredName = (SqlString)array[15];
		TradingName = (SqlString)array[16];
		DateOfBirth = (SqlDateTime)array[17];
		HomePhoneCode = (SqlString)array[18];
		HomePhoneNumber = (SqlString)array[19];
		WorkPhoneCode = (SqlString)array[20];
		WorkPhoneNumber = (SqlString)array[21];
		CellPhoneNumber = (SqlString)array[22];
		EmailAddress = (SqlString)array[23];
		FaxCode = (SqlString)array[24];
		FaxNumber = (SqlString)array[25];
		Password = (SqlString)array[26];
		CitizenTypeKey = (SqlInt32)array[27];
		LegalEntityStatusKey = (SqlInt32)array[28];
		Comments = (SqlString)array[29];
		LegalEntityExceptionStatusKey = (SqlInt32)array[30];
		UserID = (SqlString)array[31];
		ChangeDate = (SqlDateTime)array[32];
		EducationKey = (SqlInt32)array[33];
		HomeLanguageKey = (SqlInt32)array[34];
		DocumentLanguageKey = (SqlInt32)array[35];
		ResidenceStatusKey = (SqlInt32)array[36];
		RANK = (SqlInt32)array[UserDefinedFunctions.ACCOUNTKEYINDEX];
	}
	private static string GetMatchGroup(string[] Names, GroupCollection groupCollection)
	{
		string result;
		for (int i = 0; i < groupCollection.Count; i++)
		{
			if (Names[i] != "0")
			{
				if (groupCollection[i].Success)
				{
					result = Names[i];
					return result;
				}
			}
		}
		result = "";
		return result;
	}
	private static int RankAccountKeyMatch(object[] Data, int AccountKeyIndex, List<string> Numbers)
	{
		int result;
		for (int i = 0; i < Numbers.Count; i++)
		{
			string[] array = Data[UserDefinedFunctions.ACCOUNTKEYINDEX].ToString().Split(new char[]
			{
				','
			});
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].Trim() == Numbers[i])
				{
					result = UserDefinedFunctions.ACCOUNTRANKING;
					return result;
				}
			}
		}
		result = 0;
		return result;
	}
	private static int RankMatch(object[] Data, int[] Indeces, List<string> Words)
	{
		int num = 0;
		List<string> list = new List<string>();
		for (int i = 0; i < Indeces.Length; i++)
		{
			for (int j = 0; j < Words.Count; j++)
			{
				string text = Words[j].ToLower().Trim();
				string text2 = Convert.ToString(Data[Indeces[i]]).ToLower().Trim();
				if (text2 == text)
				{
					if (!list.Contains(text))
					{
						num += 2;
						list.Add(text);
					}
				}
				if (text2.Contains(text))
				{
					if (!list.Contains(text))
					{
						num++;
						list.Add(text);
					}
				}
			}
		}
		return num;
	}
	private static int RankConstrainedMatch(object[] Data, int[] Indeces, string LHS, string RHS)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		string text = LHS.ToLower().Trim();
		string text2 = RHS.ToLower().Trim();
		for (int i = 0; i < Indeces.Length; i++)
		{
			string text3 = Convert.ToString(Data[Indeces[i]]).ToLower().Trim();
			if (text3 == text)
			{
				flag3 = true;
			}
			else
			{
				if (text3.Contains(text))
				{
					flag = true;
				}
			}
			if (text3 == text2)
			{
				flag4 = true;
			}
			else
			{
				if (text3.Contains(text2))
				{
					flag2 = true;
				}
			}
		}
		bool flag5 = flag2 || flag4;
		bool flag6 = flag || flag3;
		int result;
		if (flag5 && flag6)
		{
			if (flag3 && flag4)
			{
				result = 2;
			}
			else
			{
				result = 1;
			}
		}
		else
		{
			result = 0;
		}
		return result;
	}
}
