using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.DataAccess;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Summary description for Account
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Account : System.Web.Services.WebService
    {

        /// <summary>
        /// Get Open Account keys
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public SAHLAutoCompleteItem[] GetAccount(string prefix)
        {
            List<SAHLAutoCompleteItem> items = new List<SAHLAutoCompleteItem>();

            string query = UIStatementRepository.GetStatement("Account", "GetAccountsByKey");
            using (IDbConnection conn = Helper.GetSQLDBConnection())
            {
                conn.Open();
                ParameterCollection pc = new ParameterCollection();
                pc.Add(new SqlParameter("@AccountKey", prefix));

                IDataReader reader = Helper.ExecuteReader(conn, query, pc);
                while (reader.Read())
                {
                    string accKey = reader.GetValue(reader.GetOrdinal("AccountKey")).ToString();
                    items.Add(new SAHLAutoCompleteItem(accKey, accKey));
                }
                reader.Dispose();
            }

            return items.ToArray();
        }

        /// <summary>
        /// Get Open Account keys
        /// </summary>
        /// <param name="days"></param>
        /// <param name="dtStr"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public string GetnWorkingDaysFromDate(int days, string dtStr)
        {
            ICommonRepository cRepo = SAHL.Common.Factories.RepositoryFactory.GetRepository<ICommonRepository>();

            string[] dts = dtStr.Split('/');
            DateTime dt = new DateTime(Convert.ToInt16(dts[2]), Convert.ToInt16(dts[1]), Convert.ToInt16(dts[0]));
            
            return cRepo.GetnWorkingDaysFromDate(days, dt).ToString(SAHL.Common.Constants.DateFormat);
        }
    }

    /// <summary>
    /// Web Service Urls
    /// </summary>
    public static partial class WebServiceUrls
    {
        public const string SearchForAccountsByKey = "SAHL.Web.AJAX.Account.GetAccount";
    }
}
