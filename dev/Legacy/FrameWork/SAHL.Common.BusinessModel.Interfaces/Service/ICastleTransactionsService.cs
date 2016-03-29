using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel.Interfaces.Service
{
    public interface ICastleTransactionsService
    {
        DataSet ExecuteQueryOnCastleTran(string query, Common.Globals.Databases database, ParameterCollection parameters, StringCollection tablemappings, DataSet ds);

        int ExecuteNonQueryOnCastleTran(string query, Common.Globals.Databases database, ParameterCollection parameters);

        DataSet ExecuteQueryOnCastleTran(string query, Common.Globals.Databases database, ParameterCollection parameters);

        object ExecuteScalarOnCastleTran(string query, Common.Globals.Databases database, ParameterCollection parameters);

        object ExecuteScalarOnCastleTran(string query, Type Ty, ParameterCollection parameters);

        int ExecuteNonQueryOnCastleTran(string query, Type Ty, ParameterCollection parameters);

        DataSet ExecuteQueryOnCastleTran(string query, Type Ty, ParameterCollection parameters);

        void FillDataSetFromQueryOnCastleTran(DataSet dataSet, string tableName, string query, Common.Globals.Databases database, ParameterCollection parameters);

        void FillDataTableFromQueryOnCastleTran(DataTable dataTable, string query, Common.Globals.Databases database, ParameterCollection parameters);

        IList<T> Many<T>(Common.Globals.QueryLanguages language, string query, Common.Globals.Databases database, params object[] parameters);

        IList<T> Many<T>(Common.Globals.QueryLanguages language, string query, string sqlReturnDefinition, Common.Globals.Databases database, params object[] parameters);

        T Single<T>(Common.Globals.QueryLanguages language, string query, Common.Globals.Databases database, params object[] parameters);

        T Single<T>(Common.Globals.QueryLanguages language, string query, string sqlReturnDefinition, Common.Globals.Databases database, params object[] parameters);

        void ExecuteHaloAPI_uiS_OnCastleTranForUpdate(string applicationName, string statementName, ParameterCollection parameters);

        void ExecuteHaloAPI_uiS_OnCastleTranForRead(string applicationName, string statementName, ParameterCollection parameters);
    }
}