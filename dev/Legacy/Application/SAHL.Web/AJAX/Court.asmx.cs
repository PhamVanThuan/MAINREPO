using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ComponentModel;
using System.Web.Script.Services;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.DataAccess;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using AjaxControlToolkit;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Specialized;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Summary description for Court
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Court : System.Web.Services.WebService
    {
        /// <summary>
        /// Gets a list of courts (for the selected courttype) whose name starts with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix">The starting letters of the courts name.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public SAHLAutoCompleteItem[] GetCourtsByTypeAndPrefix(string prefix)
        {
            List<SAHLAutoCompleteItem> items = new List<SAHLAutoCompleteItem>();

            string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetCourtsByTypeAndPrefix");
            using (IDbConnection conn = Helper.GetSQLDBConnection())
            {
                conn.Open();
                ParameterCollection pc = new ParameterCollection();
                pc.Add(new SqlParameter("@CourtTypeKey", (int)SAHL.Common.Globals.CourtTypes.Magistrate));
                pc.Add(new SqlParameter("@Prefix", prefix));

                IDataReader reader = Helper.ExecuteReader(conn, query, pc);
                while (reader.Read())
                {
                    string courtKey = reader.GetValue(reader.GetOrdinal("CourtKey")).ToString();
                    string courtName = reader.GetString(reader.GetOrdinal("CourtName"));
                    string province = reader.GetString(reader.GetOrdinal("Province"));
                    items.Add(new SAHLAutoCompleteItem(courtKey, String.Format("{0} ({1})", courtName, province)));
                }
                reader.Dispose();
            }

            return items.ToArray();
        }

        [WebMethod]
        [ScriptMethod]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Parameter required by the AjaxToolKit even if it is not used")]
        public CascadingDropDownNameValue[] GetAppearanceTypesByHearingType(string knownCategoryValues, string category)
        {
            List<CascadingDropDownNameValue> items = new List<CascadingDropDownNameValue>();
            StringDictionary dictValues = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int hearingTypeKey = -1;

            // this should only ever receive one value
            foreach (DictionaryEntry de in dictValues)
            {
                if (de.Value.ToString() == SAHLDropDownList.PleaseSelectValue)
                    break;

                hearingTypeKey = Convert.ToInt32(de.Value);
                break;
            }

            if (hearingTypeKey > 0)
            {
                string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetAppearanceTypeByHearingType");
                using (IDbConnection conn = Helper.GetSQLDBConnection())
                {
                    conn.Open();
                    ParameterCollection pc = new ParameterCollection();
                    pc.Add(new SqlParameter("@HearingTypeKey", hearingTypeKey));

                    IDataReader reader = Helper.ExecuteReader(conn, query, pc);
                    while (reader.Read())
                    {
                        string hearingAppearanceTypeKey = reader.GetValue(reader.GetOrdinal("HearingAppearanceTypeKey")).ToString();
                        string description = reader.GetString(reader.GetOrdinal("Description"));

                        items.Add(new CascadingDropDownNameValue(description, hearingAppearanceTypeKey));
                    }
                    reader.Dispose();
                }
            }
            return items.ToArray();
        }
    }
}
