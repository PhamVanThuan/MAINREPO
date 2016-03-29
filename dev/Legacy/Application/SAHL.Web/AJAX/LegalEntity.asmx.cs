using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Script.Services;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using System.Text;
using SAHL.Common.Utils;
using System.Collections.Generic;
using SAHL.Common.DataAccess;
using System.Data.SqlClient;

namespace SAHL.Web.AJAX
{

	/// <summary>
	/// Provides web methods pertaining to Legal Entities.
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	[ScriptService]
	public class LegalEntity : System.Web.Services.WebService
	{
		/// <summary>
		/// Used to discover if the webservice is responding to requests.
		/// </summary>
		[WebMethod]
		public bool Ping()
		{
			return true;
		}

		/// <summary>
		/// Gets a list of legal entities where the ID number starts with <c>prefix</c>.
		/// </summary>
		/// <param name="prefix">The starting letters of the legal entity's ID number.</param>
		/// <returns></returns>
		[WebMethod]
		[ScriptMethod]
		public SAHLAutoCompleteItem[] GetLegalEntitiesByIDNumber(string prefix)
		{
			List<SAHLAutoCompleteItem> items = new List<SAHLAutoCompleteItem>();

			// TODO: This will all get ripped out once the UIStatementRepository gets pulled through into branch
			string query = UIStatementRepository.GetStatement("LegalEntity", "GetLegalEntitiesByIdNumberPartial");
			using (IDbConnection conn = Helper.GetSQLDBConnection())
			{
				conn.Open();
				ParameterCollection pc = new ParameterCollection();
				pc.Add(new SqlParameter("@IDNumber", prefix));

				IDataReader reader = Helper.ExecuteReader(conn, query, pc);
				while (reader.Read())
				{
					string leKey = reader.GetValue(reader.GetOrdinal("LegalEntityKey")).ToString();
					string idNum = reader.GetString(reader.GetOrdinal("IDNumber"));
					string legalName = reader.GetString(reader.GetOrdinal("LegalName"));
					items.Add(new SAHLAutoCompleteItem(leKey, String.Format("{0} - {1}", idNum, legalName)));
				}
				reader.Dispose();
			}

			return items.ToArray();
		}

		/// <summary>
		/// Gets a list of legal entities where the Passport number starts with <c>prefix</c>.
		/// </summary>
		/// <param name="prefix">The starting letters of the legal entitiy's Passport number.</param>
		/// <returns></returns>
		[WebMethod]
		[ScriptMethod]
		public SAHLAutoCompleteItem[] GetLegalEntitiesByPassportNumber(string prefix)
		{
			List<SAHLAutoCompleteItem> items = new List<SAHLAutoCompleteItem>();

			// TODO: This will all get ripped out once the UIStatementRepository gets pulled through into branch
			string query = UIStatementRepository.GetStatement("LegalEntity", "GetLegalEntitiesByPassportNumberPartial");
			using (IDbConnection conn = Helper.GetSQLDBConnection())
			{
				conn.Open();
				ParameterCollection pc = new ParameterCollection();
				pc.Add(new SqlParameter("@PassportNumber", prefix));

				IDataReader reader = Helper.ExecuteReader(conn, query, pc);
				while (reader.Read())
				{
					string leKey = reader.GetValue(reader.GetOrdinal("LegalEntityKey")).ToString();
					string passportNum = reader.GetString(reader.GetOrdinal("PassportNumber"));
					string legalName = reader.GetString(reader.GetOrdinal("LegalName"));
					items.Add(new SAHLAutoCompleteItem(leKey, String.Format("{0} - {1}", passportNum, legalName)));
				}
				reader.Dispose();
			}

			return items.ToArray();

		}

		/// <summary>
		/// Get a list of Debt Counsellors<c>ncrRegistrationNumber</c>.
		/// </summary>
		/// <param name="ncrRegistrationNumber">NCR Registration Number.</param>
		/// <returns></returns>
		[WebMethod]
		[ScriptMethod]
		public SAHLAutoCompleteItem[] GetDebtCounsellorByNCRRegistrationNumber(string ncrRegistrationNumber)
		{
			List<SAHLAutoCompleteItem> items = new List<SAHLAutoCompleteItem>();

			// TODO: This will all get ripped out once the UIStatementRepository gets pulled through into branch
			string query = UIStatementRepository.GetStatement("LegalEntity", "GetDebtCounsellorByNCRNumber");
			using (IDbConnection conn = Helper.GetSQLDBConnection())
			{
				conn.Open();
				ParameterCollection pc = new ParameterCollection();
				pc.Add(new SqlParameter("@ncrRegistrationNumber", ncrRegistrationNumber));

				IDataReader reader = Helper.ExecuteReader(conn, query, pc);
				while (reader.Read())
				{
					string name = reader.GetString(reader.GetOrdinal("Name"));
					string legalEntityOrganisationStructureKey = reader.GetInt32(reader.GetOrdinal("LegalEntityOrganisationStructureKey")).ToString();
					items.Add(new SAHLAutoCompleteItem(legalEntityOrganisationStructureKey, String.Format("{0}",name)));
				}
				reader.Dispose();
			}

			return items.ToArray();

		}

		/// <summary>
		/// Get Legal Entities by ID or Passport Number
		/// </summary>
		/// <param name="prefix"></param>
		/// <returns></returns>
		[WebMethod]
		[ScriptMethod]
		public string GetLegalEntitiesByIDOrPassportNumber(string prefix)
		{
			List<JSonPersonResult> results = new List<JSonPersonResult>();

			// TODO: This will all get ripped out once the UIStatementRepository gets pulled through into branch
			string query = UIStatementRepository.GetStatement("LegalEntity", "GetLegalEntitiesByIDOrPassportNumberPartial");
			using (IDbConnection conn = Helper.GetSQLDBConnection())
			{
				conn.Open();
				ParameterCollection pc = new ParameterCollection();
				pc.Add(new SqlParameter("@IDOrPassportNumber", prefix));

				IDataReader reader = Helper.ExecuteReader(conn, query, pc);
				while (reader.Read())
				{
					string leKey = reader.GetValue(reader.GetOrdinal("LegalEntityKey")).ToString();
					string passportNum = reader.GetString(reader.GetOrdinal("PassportNumber"));
					string idNumber = reader.GetString(reader.GetOrdinal("IDNumber"));
					string legalName = reader.GetString(reader.GetOrdinal("LegalName"));
					results.Add(new JSonPersonResult(){Key = leKey, IDNumber = idNumber, PassportNumber = passportNum, LegalName = legalName});
				}
				reader.Dispose();
			}

			System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			return serializer.Serialize(results);
		}

		/// <summary>
		/// Gets a list of legal entity registration numbers, given a legal entity type.
		/// </summary>
		/// <param name="prefix">The first letters of the registration number to search on.</param>
		/// <param name="legalEntityTypeKey">The legal entity type: only Companies, Close Corporations and Trusts are supported.  This must cast into a <see cref="LegalEntityTypes"/> object.</param>
		/// <returns></returns>
		[WebMethod]
		[ScriptMethod]
		public SAHLAutoCompleteItem[] GetLegalEntitiesByRegistrationNumber(string prefix, string legalEntityTypeKey)
		{
			ILegalEntityRepository repository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
			SAHLAutoCompleteItem[] items;

			LegalEntityTypes legalEntityType = new LegalEntityTypes();

			try
			{
				legalEntityType = (LegalEntityTypes)Int32.Parse(legalEntityTypeKey);
			}
			catch (Exception)
			{

				throw new ArgumentException("Invalid legal entity type");
			}



			switch (legalEntityType)
			{
				case LegalEntityTypes.CloseCorporation:
				case LegalEntityTypes.Company:
				case LegalEntityTypes.Trust:
					// get the registration numbers for companies
					IDictionary<string, string> companies = repository.GetRegistrationNumbersForCompanies(prefix, 10);
					int i = 0;
					items = new SAHLAutoCompleteItem[companies.Count];
					foreach (KeyValuePair<string, string> company in companies)
					{
						items[i] = new SAHLAutoCompleteItem(company.Key, company.Value);
						i++;
					}

					break;
				default:
					throw new ArgumentException("Invalid legal entity type");
			}

			return items;
		}

		/// <summary>
		/// Web service call written specifically for the client search function to improve performance.  This returns 
		/// HTML for displaying legal entity details.
		/// </summary>
		/// <param name="legalEntityKey"></param>
		/// <param name="clientData">Data supplied by the client for use in the asynchronous callback. This is returned as item 0 in the result array.</param>
		/// <returns>An array containing two items: 0: <c>clientData</c>, and 1: The HTML required for displaying the legal entity details</returns>
		[WebMethod]
		[ScriptMethod]
		public string[] GetClientSearchLegalEntityDetails(string legalEntityKey, string clientData, bool enableHyperLinks)
		{
			ILegalEntityRepository repository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
			ILegalEntity legalEntity = repository.GetLegalEntityByKey(Convert.ToInt32(legalEntityKey));
			StringBuilder sbHtml = new StringBuilder();
			string image = @"<img src=""../../images/{0}"" alt="""" />";

			// extract natural person details
			ILegalEntityNaturalPerson leNaturalPerson = legalEntity as ILegalEntityNaturalPerson;
			if (leNaturalPerson != null)
			{
				BuildClientDetailsItem(sbHtml, "Passport number: ", leNaturalPerson.PassportNumber);
				if (leNaturalPerson.MaritalStatus != null)
					BuildClientDetailsItem(sbHtml, "Marital status: ", leNaturalPerson.MaritalStatus.Description);
				BuildClientDetailsItem(sbHtml, "Home phone:", StringUtils.JoinNullableStrings(leNaturalPerson.HomePhoneCode, leNaturalPerson.HomePhoneNumber));
				BuildClientDetailsItem(sbHtml, "Work phone:", StringUtils.JoinNullableStrings(leNaturalPerson.WorkPhoneCode, leNaturalPerson.WorkPhoneNumber));
				BuildClientDetailsItem(sbHtml, "Cell phone:", leNaturalPerson.CellPhoneNumber);
				BuildClientDetailsItem(sbHtml, "Fax:", StringUtils.JoinNullableStrings(leNaturalPerson.FaxCode, leNaturalPerson.FaxNumber));
				BuildClientDetailsItem(sbHtml, "Email:", leNaturalPerson.EmailAddress);
			}

			// extract company information
			switch ((LegalEntityTypes)legalEntity.LegalEntityType.Key)
			{
				case LegalEntityTypes.CloseCorporation:
				case LegalEntityTypes.Company:
				case LegalEntityTypes.Trust:
					BuildClientDetailsItem(sbHtml, "Company phone:", StringUtils.JoinNullableStrings(legalEntity.WorkPhoneCode, legalEntity.WorkPhoneNumber));
					BuildClientDetailsItem(sbHtml, "Fax:", StringUtils.JoinNullableStrings(legalEntity.FaxCode, legalEntity.FaxNumber));
					BuildClientDetailsItem(sbHtml, "Email:", legalEntity.EmailAddress);
					break;
			}

			// add addresses
			if (legalEntity.LegalEntityAddresses.Count > 0)
			{
				BuildClientDetailsItem(sbHtml, "Address Details:", "&nbsp;");
				foreach (ILegalEntityAddress leAddress in legalEntity.LegalEntityAddresses)
				{
					BuildClientDetailsItem(sbHtml, String.Format(@"<span style=""margin-left:10px"">{0}</span>: ", leAddress.AddressType.Description), leAddress.Address.GetFormattedDescription(AddressDelimiters.Comma));
				}
			}

			// account roles
			if (legalEntity.Roles.Count > 0)
			{
				BuildClientDetailsItem(sbHtml, "Account Roles:", "&nbsp;");
				foreach (IRole role in legalEntity.Roles)
				{
					IAccount acc = role.Account;
					IAccountHOC hoc = acc as IAccountHOC;
					IAccountLifePolicy life = acc as IAccountLifePolicy;

					string productDescription = acc.Product.Description;

					// determine the image to display, and if the account is an HOC account, include the name 
					// of the insurer in the product description
					string img = "";
					if (hoc != null && hoc.HOC != null && hoc.HOC.HOCInsurer != null)
					{
						img = String.Format(image, GetHocIcon(hoc, (int)AccountStatuses.Closed));
						productDescription = String.Concat(productDescription, " with <strong><i>", hoc.HOC.HOCInsurer.Description, "</i></strong> ");
					}
					else if (acc.OriginationSource != null)
					{
						string icon = String.Format("originationsources\\{0}.gif", acc.OriginationSource.Key);
						img = String.Format(image, icon);
					}

					// check the role status
					string roleStatus = "";
					if (role.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
						roleStatus = @" (<span class=""error"">Inactive Role</span>)";

					string accountStatus = acc.AccountStatus.Description;

					// if this is a life account then show the lifepolicystatus description
					if (life != null && life.LifePolicy != null)
						accountStatus = life.LifePolicy.LifePolicyStatus.Description;

					BuildClientDetailsItem(sbHtml,
						"",
						String.Format(@"<span style=""margin-left:10px"">{0} {1} in account {2} : {3} ({4}){5}</span>",
							img,
							role.RoleType.Description,
							acc.Key,
							productDescription,
							accountStatus,
							roleStatus)
					);

				}
			}

			// only get non-operator roles
			IReadOnlyEventList<IApplicationRole> clientRoles = legalEntity.GetApplicationRolesByRoleTypeGroups(OfferRoleTypeGroups.Client);

			// application roles
			if (clientRoles.Count > 0)
			{
				BuildClientDetailsItem(sbHtml, "Application Roles:", "&nbsp;");
				foreach (IApplicationRole role in clientRoles)
				{
					IApplication app = role.Application;

					// determine the image to display
					string img = "";
					if (app.OriginationSource != null)
					{
						string icon = String.Format("originationsources\\{0}.gif", app.OriginationSource.Key);
						img = String.Format(image, icon);
					}

					// check the role status
					string roleStatus = "";
					if (role.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
						roleStatus = @" (<span class=""error"">Inactive Role</span>)";

					// build up the application string - if the application is open then we create a link to the 
					// workflow super search
					bool showLink = (app.ApplicationStatus.Key == (int)OfferStatuses.Open && enableHyperLinks);

					StringBuilder sbApp = new StringBuilder();
					sbApp.Append("<span style=\"margin-left:10px\">");
					sbApp.Append(img).Append(" ");
					sbApp.Append(role.ApplicationRoleType.Description);
					sbApp.Append(" in ");
					if (showLink)
						sbApp.Append("<a class=\"colored\" href=\"#\" onclick=\"openApplication({0})\">");
					sbApp.Append("application {0}");
					if (showLink)
						sbApp.Append("</a>");

					sbApp.Append(" : ").Append(app.ApplicationType.Description).Append(" ");
					sbApp.Append("(").Append(app.ApplicationStatus.Description).Append(")");
					sbApp.Append(roleStatus);
					sbApp.Append("</span>");

					BuildClientDetailsItem(sbHtml,
						"",
						String.Format(sbApp.ToString(), app.Key)
					);
				}

			}

			if (sbHtml.Length == 0)
				sbHtml.Append(@"<span class=""LabelText"">No additional details found.</span>");

			sbHtml.Insert(0, @"<div class=""backgroundLight"" style=""float:left;padding:2px;width:99%;"">");
			sbHtml.Append("</div>");

			return new string[] { clientData, sbHtml.ToString() };
		}

        /// <summary>
        /// Web service call written specifically for the correspondence screen.  This returns HTML for displaying legal entity details.
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="clientData">Data supplied by the client for use in the asynchronous callback. This is returned as item 0 in the result array.</param>
        /// <returns>An array containing two items: 0: <c>clientData</c>, and 1: The HTML required for displaying the legal entity details</returns>
        [WebMethod]
        [ScriptMethod]
        public string[] GetCorrespondenceExtraLegalEntityDetails(string legalEntityKey, string genericKey, string genericKeyTypeKey, string clientData)
        {
            ILegalEntityRepository repository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            ILegalEntity legalEntity = repository.GetLegalEntityByKey(Convert.ToInt32(legalEntityKey));
            StringBuilder sbHtml = new StringBuilder();

            // extract natural person details
            ILegalEntityNaturalPerson leNaturalPerson = legalEntity as ILegalEntityNaturalPerson;
            if (leNaturalPerson != null)
            {
                BuildClientDetailsItem(sbHtml, "ID Number: ", leNaturalPerson.IDNumber);
                BuildClientDetailsItem(sbHtml, "Status   : ", leNaturalPerson.LegalEntityStatus.Description);              
            }

            // extract company information
            switch ((LegalEntityTypes)legalEntity.LegalEntityType.Key)
            {
                case LegalEntityTypes.CloseCorporation:
                case LegalEntityTypes.Company:
                case LegalEntityTypes.Trust:
                    BuildClientDetailsItem(sbHtml, "Company Number:", legalEntity.LegalNumber);
                    break;
            }

            int genericKeyType = Convert.ToInt32(genericKeyTypeKey);
            int accountKey = -1;

            switch (genericKeyType)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                case (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount:
                    // get the legalentities role for this account
                    if (String.IsNullOrEmpty(genericKey) == false)
                    {
                        accountKey = Convert.ToInt32(genericKey);
                    }
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM:
                    if (!string.IsNullOrEmpty(genericKey))
                    {
                        int dcKey = Convert.ToInt32(genericKey);
                        IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                        IDebtCounselling dc = dcRepo.GetDebtCounsellingByKey(dcKey);
                        accountKey = dc.Account.Key;
                    }
                    break;
                default:
                    break;
            }

            if (accountKey > 0)
            {
                IRole role = legalEntity.GetRole(accountKey);
                if (role != null)
                {
                    BuildClientDetailsItem(sbHtml, "Role :", role.RoleType.Description);
                }
            }

            foreach (var add in legalEntity.LegalEntityAddresses)
            {
                if (add.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                {
                    BuildClientDetailsItem(sbHtml, add.AddressType.Description + " Address : ", add.Address.GetFormattedDescription(AddressDelimiters.Comma));
                }
            }

            //check for domicilium address - only if the legalentity has an account role
            if (accountKey > 0)
            {
                foreach (IRole role in legalEntity.Roles)
                {
                    if (role.Account.Key == accountKey)
                    {
                        IAddressRepository repo = RepositoryFactory.GetRepository<IAddressRepository>();
                        ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                        if (null != role.LegalEntity.ActiveDomiciliumAddress)
                        {
                            BuildClientDetailsItem(sbHtml, "Domicilium Address : ", role.LegalEntity.ActiveDomiciliumAddress.Address.GetFormattedDescription(AddressDelimiters.Comma));
                        }

                        break;
                    }
                }
            }

            if (sbHtml.Length == 0)
                sbHtml.Append(@"<span class=""LabelText"">No additional details found.</span>");

            sbHtml.Insert(0, @"<div class=""backgroundLight"" style=""float:left;padding:2px;width:99%;"">");
            sbHtml.Append("</div>");

            return new string[] { clientData, sbHtml.ToString() };
        }

		#region Helper Methods

		/// <summary>
		/// Helper method for GetClientSearchLegalEntityDetails.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="label"></param>
		/// <param name="text"></param>
		private static void BuildClientDetailsItem(StringBuilder sb, string label, string text)
		{
			if (String.IsNullOrEmpty(text))
				return;

			sb.Append(@"<div class=""row"" style=""float:left;height:16px;"">");
			sb.Append(String.Format(@"<span class=""TitleText"">{0}</span> ", label));
			sb.Append(String.Format(@"<span class=""LabelText"">{0}</span> ", text));
			sb.Append("</div>");
		}

		/// <summary>
		/// Gets the image to use for HOC accounts in GetClientSearchLegalEntityDetails. 
		/// </summary>
		/// <param name="hoc"></param>
		/// <param name="closedKey"></param>
		/// <returns></returns>
		private static string GetHocIcon(IAccountHOC hoc, int closedKey)
		{
			if (hoc.AccountStatus.Key == closedKey)
				return "ClosedHOC.gif";
			else
				return "OpenHOC.gif";

		}

		#endregion


	}

	/// <summary>
	/// Web Service Urls
	/// </summary>
	public static partial class WebServiceUrls
	{
		public const string GetDebtCounsellorByNCRRegistrationNumber = "SAHL.Web.AJAX.LegalEntity.GetDebtCounsellorByNCRRegistrationNumber";
	}
}
