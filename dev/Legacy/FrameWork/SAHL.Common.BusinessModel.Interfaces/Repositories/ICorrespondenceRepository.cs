using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Globals;
using System;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public class CorrespondenceExt
    {
        public CorrespondenceExt(ICorrespondence correspondence, bool excludeFromDataSTOR)
        {
            this.Correspondence = correspondence;
            this.ExcludeFromDataSTOR = excludeFromDataSTOR;
        }

        public ICorrespondence Correspondence { get; protected set; }
        public bool ExcludeFromDataSTOR { get; protected set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface ICorrespondenceRepository
    {

        void SendCorrespondenceReportToLegalEntity(ISendCorrespondenceRequest sendCorrespondenceRequest);

        /// <summary>
        /// Creates and Empty Correspondence object
        /// </summary>
        /// <returns>ICorrespondence</returns>
        ICorrespondence CreateEmptyCorrespondence();

        /// <summary>
        /// Creates and Empty CorrespondenceParameter object
        /// </summary>
        /// <returns>ICorrespondenceParameters</returns>
        ICorrespondenceParameters CreateEmptyCorrespondenceParameter();

        /// <summary>
        /// Returns the correspondence record for a specified CorrespondenceKey
        /// </summary>
        /// <param name="CorrespondenceKey">The int CorrespondenceKey</param>
        ICorrespondence GetCorrespondenceByKey(int CorrespondenceKey);

        /// <summary>
        /// Returns all correspondence records for a specified ReportStatementKey and GenericKey
        /// </summary>
        /// <param name="ReportStatementKey"></param>
        /// <param name="GenericKey"></param>
        /// <param name="GenericKeyTypeKey"></param>
        /// <param name="UnprocessedOnly"></param>
        /// <returns></returns>
        IEventList<ICorrespondence> GetCorrespondenceByReportStatementAndGenericKey(int ReportStatementKey, int GenericKey, int GenericKeyTypeKey, bool UnprocessedOnly);

        /// <summary>
        /// Returns all correspondence records for a specified GenericKey
        /// </summary>
        /// <param name="genericKeyValue"></param>
        /// <param name="genericKeyTypeValue"></param>
        /// <param name="UnprocessedOnly"></param>
        /// <returns></returns>
        IEventList<ICorrespondence> GetCorrespondenceByGenericKey(int genericKeyValue, int genericKeyTypeValue, bool UnprocessedOnly);

        /// <summary>
        /// Returns all correspondence records for a specified GenericKeys
        /// </summary>
        /// <param name="genericKeyValues"></param>
        /// <param name="UnprocessedOnly"></param>
        /// <returns></returns>
        IEventList<ICorrespondence> GetCorrespondenceByGenericKeys(Dictionary<int, int> genericKeyValues, bool UnprocessedOnly);

        /// <summary>
        /// Returns all correspondence parameter records for a specified CorrespondenceKey
        /// </summary>
        /// <param name="CorrespondenceKey">The int CorrespondenceKey</param>
        IEventList<ICorrespondenceParameters> GetCorrespondenceParametersByCorrespondenceKey(int CorrespondenceKey);

        /// <summary>
        /// Adds/Updates Correspondence object
        /// </summary>
        /// <param name="correspondence">The Correspondence entity.</param>
        void SaveCorrespondence(ICorrespondence correspondence);

        /// <summary>
        /// Adds/Updates Correspondence object
        /// </summary>
        /// <param name="correspondence">The List&lt;ICorrespondence&gt; entity.</param>
        void SaveCorrespondenceList(List<ICorrespondence> correspondence);

        /// <summary>
        /// Deletes all correspondence records for a specified CorrespondenceKey
        /// </summary>
        /// <param name="CorrespondenceKey">The int ReportStatementKey</param>
        void RemoveCorrespondenceByKey(int CorrespondenceKey);

        /// <summary>
        /// Deletes all correspondence records for a specified ReportStatementKey and GenericKey
        /// </summary>
        /// <param name="ReportStatementKey">The int ReportStatementKey</param>
        /// <param name="GenericKey">The int GenericKey of the reocrds to delete</param>
        /// <param name="GenericKeyTypeKey"></param>
        /// <param name="UnprocessedOnly">A bool value to determine whether to only remove unprocessed correspondence records or not</param>
        void RemoveCorrespondenceByReportStatementAndGenericKey(int ReportStatementKey, int GenericKey, int GenericKeyTypeKey, bool UnprocessedOnly);

        /// <summary>
        /// Deletes all correspondence records for a specified GenericKey
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="GenericKeyType"></param>
        /// <param name="UnprocessedOnly"></param>
        /// <param name="KeepReportStatementKey"></param>
        void RemoveCorrespondenceByGenericKey(int GenericKey, int GenericKeyType, bool UnprocessedOnly, int KeepReportStatementKey);

        /// <summary>
        /// Create and empty correspondence detail object
        /// </summary>
        /// <returns></returns>
        ICorrespondenceDetail CreateEmptyCorrespondenceDetail();

        /// <summary>
        /// Get Correspondence Detail Record by Key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        ICorrespondenceDetail GetCorrespondenceDetailByKey(int Key);

        /// <summary>
        /// Adds/Updates Correspondence Detail object
        /// </summary>
        /// <param name="correspondencedetail">The Correspondence entity.</param>
        void SaveCorrespondenceDetail(ICorrespondenceDetail correspondencedetail);

        void GetEmailTemplate(ILegalEntity legalEntity, string consultantName, string emailFrom, Int32 genericKey, CorrespondenceTemplates correspondenceTemplate, out string subject, out string body);

        /// <summary>
        /// This will remove all client email entries for the specified accountkey and email subject
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="emailSubject"></param>
        /// <returns></returns>
        void RemoveClientEmailByAccountKeyAndSubject(int accountKey, string emailSubject);
    }
}