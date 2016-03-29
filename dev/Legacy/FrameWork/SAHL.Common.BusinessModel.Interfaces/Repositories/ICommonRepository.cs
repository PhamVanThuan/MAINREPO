using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using NHibernate;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ICommonRepository
    {
        DateTime GetTodaysDate();

        object ExecuteConditionTokenStatement(int StatementDefinitionKey, ParameterCollection Parameters);

        #region moved to application repo

        //IOriginationSourceProduct GetOriginationSourceProductByKey(int OSPKey);
        //IOriginationSourceProduct GetOriginationSourceProductBySourceAndProduct(int OriginationSourceKey, int ProductKey);
        //IOriginationSource GetOriginationSource(OriginationSources source);
        //ReadOnlyEventList<IProduct> GetOriginationProducts();
        //ReadOnlyEventList<IMortgageLoanPurpose> GetMortgageLoanPurposes(int[] MortgageLoanPurposeKeys);

        #endregion moved to application repo

        void RefreshDAOObject<TInterface>(int key);

        TInterface GetByKey<TInterface>(int Key);

        TInterface CreateEmpty<TInterface, TDAO>() where TDAO : class, new();

        void Save<TInterface, TDAO>(TInterface ObjectToSave)
            where TInterface : IDAOObject
            where TDAO : class;

        #region moved to ?? repo

        //ReadOnlyEventList<IHelpDeskQuery> GetHelpDeskQueryByInstanceID(long InstanceID);
        ///// <summary>
        /////
        ///// </summary>
        ///// <returns></returns>
        //IHelpDeskQuery CreateEmptyHelpDeskQuery();

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="helpDeskquery"></param>
        //void SaveHelpDeskQuery(IHelpDeskQuery helpDeskquery);

        #endregion moved to ?? repo

        /// <summary>
        ///
        /// </summary>
        /// <param name="StatementName"></param>
        /// <param name="ApplicationName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable ExecuteUIStatement(string StatementName, string ApplicationName, List<SqlParameter> parameters);

        /// <summary>
        ///
        /// </summary>
        /// <param name="StatementName"></param>
        /// <param name="ApplicationName"></param>
        /// <param name="Parameters"></param>
        /// <param name="TableMappings"></param>
        /// <returns></returns>
        DataSet ExecuteUIStatement(string StatementName, string ApplicationName, List<SqlParameter> Parameters, StringCollection TableMappings);

        /// <summary>
        ///
        /// </summary>
        /// <param name="statementName"></param>
        /// <param name="application"></param>
        /// <param name="Parameters"></param>
        void ExecuteNonQuery(string statementName, string application, List<SqlParameter> Parameters);

        /// <summary>
        ///
        /// </summary>
        /// <param name="DT"></param>
        /// <param name="statementName"></param>
        /// <param name="applicationName"></param>
        /// <param name="Parameters"></param>
        void ExecuteUIStatementUpdate(DataTable DT, string statementName, string applicationName, List<SqlParameter> Parameters);

        DateTime GetnWorkingDaysFromToday(int nDays);

        DateTime GetnWorkingDaysFromDate(int nDays, DateTime fromDate);

        bool IsBusinessDay(DateTime calendarDate);

		DateTime GetNextBusinessDay(DateTime date);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        Dictionary<int, string> GetLinkRatesByAccountKey(int AccountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="OriginationSourceKey"></param>
        /// <returns></returns>
        Dictionary<int, string> GetLinkRatesByOriginationSource(int OriginationSourceKey);

        /// <summary>
        /// Gets a handle on an NHibernate session for the supplied <c>businessObject</c>.
        /// </summary>
        /// <param name="businessObject">The business object you want a session for.</param>
        /// <returns>The NHibernate session.</returns>
        ISession GetNHibernateSession(object businessObject);

        /// <summary>
        /// Gets a handle on an NHibernate session for the supplied type.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>The NHibernate session.</returns>
        ISession GetNHibernateSession(Type type);

        /// <summary>
        /// Gets the NHibernateSession and calls the Update() method.
        /// </summary>
        /// <param name="businessObject"></param>
        void UpdateInCurrentNHibernateSession(object businessObject);

        /// <summary>
        ///
        /// </summary>
        /// <param name="businessObject"></param>
        void AttachUnModifiedToCurrentNHibernateSession(object businessObject);

        /// <summary>
        /// Remove the object from memory so that we can re-load it as a different type.
        /// </summary>
        /// <param name="businessObject"></param>
        void ClearFromNHibernateSession(object businessObject);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        ILanguage GetLanguageByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="businessObject"></param>
        /// <param name="Identifier"></param>
        void AttachUnModifiedToCurrentNHibernateSession(object businessObject, string Identifier);

        /// <summary>
        /// Get Correspondence By Key
        /// </summary>
        /// <param name="correspondenceTemplates"></param>
        /// <returns></returns>
        ICorrespondenceTemplate GetCorrespondenceTemplateByKey(CorrespondenceTemplates correspondenceTemplates);

        /// <summary>
        /// Get Correspondence By Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ICorrespondenceTemplate GetCorrespondenceTemplateByName(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetDefaultDebitOrderProviderKey();

    }
}