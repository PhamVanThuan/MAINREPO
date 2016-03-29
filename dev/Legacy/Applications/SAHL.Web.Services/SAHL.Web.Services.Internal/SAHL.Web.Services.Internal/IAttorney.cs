using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Activation;
using SAHL.Web.Services.Internal.DataModel;

namespace SAHL.Web.Services.Internal
{
    [ServiceContract]
	public interface IAttorney
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

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        [OperationContract]
        bool RegisterUser(string emailAddress);

        /// <summary>
        /// Render SQL Report
        /// </summary>
        /// <param name="reportkey"></param>
        /// <param name="sqlReportParameters"></param>
        /// <param name="leKey"></param>
        /// <returns></returns>
		[OperationContract]
		byte[] RenderSQLReport(int reportkey, IDictionary<string, string> sqlReportParameters, string username, string password);

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
		bool ForgottenPassword(string userName);

        /// <summary>
        /// Get Notes By Debt Counselling
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
		[OperationContract]
        List<SAHL.Web.Services.Internal.DataModel.NoteDetail> GetNotesByDebtCounselling(int debtCounsellingKey, string username, string password);

      
        /// <summary>
        /// Get Debt Counselling By Key
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
		[OperationContract]
        DebtCounselling GetDebtCounsellingByKey(int debtCounsellingKey, string username, string password);

        /// <summary>
        /// Search for Cases
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="accountNumber"></param>
        /// <param name="idNumber"></param>
        /// <param name="legalEntityName"></param>
        /// <returns></returns>
		[OperationContract]
        List<DebtCounselling> SearchForCases(int legalEntityKey, int accountNumber, string idNumber, string legalEntityName, string username, string password);

        /// <summary>
        /// Save Note Detail
        /// </summary>
        /// <param name="noteDetail"></param>
        /// <param name="legalEntityKey"></param>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveNoteDetail(SAHL.Web.Services.Internal.DataModel.NoteDetail noteDetail, int legalEntityKey, int debtCounsellingKey, string username, string password);

        /// <summary>
        /// Get Proposals
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [OperationContract]
        List<Proposal> GetProposals(int debtCounsellingKey, string username, string password);
	}
}
