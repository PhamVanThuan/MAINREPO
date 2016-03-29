using SAHL.Web.Services.Internal.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SAHL.Web.Services.Internal
{
    [ServiceContract]
    public interface IClientSecure
    {
        [OperationContract]
        ServiceMessage GetServiceMessage(string legalEntityKey);

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="leKey"></param>
        /// <returns></returns>
        [OperationContract]
        bool Login(string username, string password, out string leKey);

        ///// <summary>
        ///// Register
        ///// </summary>
        ///// <param name="emailAddress"></param>
        ///// <returns></returns>
        //[OperationContract]
        //bool RegisterUser(string emailAddress);

        /// <summary>
        /// Render SQL Report
        /// </summary>
        /// <param name="reportkey"></param>
        /// <param name="sqlReportParameters"></param>
        /// <param name="leKey"></param>
        /// <returns></returns>
        [OperationContract]
        byte[] RenderSQLReport(out string contentType, out string fileExtension, int reportkey, IDictionary<string, string> sqlReportParameters, int reportFormatTypeKey, string username, string password);

        [OperationContract]
        IDictionary<int, string> ReportFormats(string username, string password);

        /// <summary>
        /// Get Report Params By Key
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        [OperationContract]
        List<SAHL.Web.Services.Internal.DataModel.ReportParameter> GetReportParametersByStatementKey(int ReportStatementKey, string username, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [OperationContract]
        bool ResetPassword(string userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [OperationContract]
        bool LegalEntityChangePassword(string userName, string password, string newPassword);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [OperationContract]
        IList<Int32> MortgageLoanAccountKeys(string username, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="accountKey"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [OperationContract]
        bool RequestFunds(string userName, string password, int accountKey, decimal amount);


        /// <summary>
        /// Fetched the subsidised AccountKey for this particular Legal Entity, will return null if no Subsidy Account.
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        [OperationContract]
        int? GetSubsidyAccountKey(int legalEntityKey);
    }
}
