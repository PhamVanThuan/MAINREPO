//using System;
//using System.Collections.Generic;
//using System.Text;
//using SAHL.Common.Collections.Interfaces;
//using SAHL.Common.BusinessModel.Interfaces;
//using SAHL.Common.Collections;
//using SAHL.Common.DataAccess;
//using System.Data;
//using SAHL.Common.Globals;
//using System.Data.SqlClient;
//using System.Collections.Specialized;
//using NHibernate;

//namespace SAHL.BusinessModel.Interfaces.Repositories
//{
//    public interface ICommonRepository
//    {
//        object ExecuteConditionTokenStatement(int StatementDefinitionKey, ParameterCollection Parameters);



//        #region moved to application repo
//        //IOriginationSourceProduct GetOriginationSourceProductByKey(int OSPKey);
//        //IOriginationSourceProduct GetOriginationSourceProductBySourceAndProduct(int OriginationSourceKey, int ProductKey);
//        //IOriginationSource GetOriginationSource(OriginationSources source);
//        //ReadOnlyEventList<IProduct> GetOriginationProducts();
//        //ReadOnlyEventList<IMortgageLoanPurpose> GetMortgageLoanPurposes(int[] MortgageLoanPurposeKeys);
//        #endregion

//        TInterface GetByKey<TInterface>(int Key);
//        TInterface CreateEmpty<TInterface, TDAO>() where TDAO : class, new();
//        void Save<TInterface, TDAO>(TInterface ObjectToSave)
//            where TInterface : IDAOObject
//            where TDAO : class;

//        /// <summary>
//        /// Gets the IControl object using the specified control description
//        /// </summary>
//        /// <param name="controlDescription"></param>
//        /// <returns></returns>
//        IControl GetControlByDescription(string controlDescription);

//        #region moved to ?? repo

//        //ReadOnlyEventList<IHelpDeskQuery> GetHelpDeskQueryByInstanceID(long InstanceID);
//        ///// <summary>
//        ///// 
//        ///// </summary>
//        ///// <returns></returns>
//        //IHelpDeskQuery CreateEmptyHelpDeskQuery();

//        ///// <summary>
//        ///// 
//        ///// </summary>
//        ///// <param name="helpDeskquery"></param>
//        //void SaveHelpDeskQuery(IHelpDeskQuery helpDeskquery);

//        #endregion

//        /// <summary>
//        /// Creates and Empty Callback object
//        /// </summary>
//        /// <returns>ICallback</returns>
//        ICallback CreateEmptyCallback();

//        /// <summary>
//        /// Saves a Callback object
//        /// </summary>
//        /// <param name="messages">Collection of Domain Messages that will be added to if the method encounters errors.</param>
//        /// <param name="callback">The <see cref="ICallback">Callback to save.</see></param>
//        /// <returns></returns>
//        void SaveCallback(ICallback callback);

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="StatementName"></param>
//        /// <param name="ApplicationName"></param>
//        /// <param name="parameters"></param>
//        /// <returns></returns>
//        DataTable ExecuteUIStatement(string StatementName, string ApplicationName, List<SqlParameter> parameters);
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="StatementName"></param>
//        /// <param name="ApplicationName"></param>
//        /// <param name="Parameters"></param>
//        /// <param name="TableMappings"></param>
//        /// <returns></returns>
//        DataSet ExecuteUIStatement(string StatementName, string ApplicationName, List<SqlParameter> Parameters, StringCollection TableMappings);
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="statementName"></param>
//        /// <param name="application"></param>
//        /// <param name="Parameters"></param>
//        void ExecuteNonQuery(string statementName, string application, List<SqlParameter> Parameters);
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="DT"></param>
//        /// <param name="statementName"></param>
//        /// <param name="applicationName"></param>
//        /// <param name="Parameters"></param>
//        void ExecuteUIStatementUpdate(DataTable DT, string statementName, string applicationName, List<SqlParameter> Parameters);

//        bool IsBusinessDay(DateTime calendarDate);

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="AccountKey"></param>
//        /// <returns></returns>
//        Dictionary<int, string> GetLinkRatesByAccountKey(int AccountKey);

//        /// <summary>
//        /// Gets a handle on an NHibernate session for the supplied <c>businessObject</c>.
//        /// </summary>
//        /// <param name="businessObject">The business object you want a session for.</param>
//        /// <returns>The NHibernate session.</returns>
//        ISession GetNHibernateSession(object businessObject);

//        /// <summary>
//        /// Gets the NHibernateSession and calls the Update() method.
//        /// </summary>
//        /// <param name="businessObject"></param>
//        void UpdateInCurrentNHibernateSession(object businessObject);
//    }
//}
