using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DecisionTree.Shared;
using SAHL.Services.Interfaces.DecisionTree;
using System;
using System.Reflection;
using System.Collections.Generic;
using SAHL.DecisionTree.Shared.Interfaces;
using SAHL.DecisionTree.Shared.Core;

namespace SAHL.Services.DecisionTree.QueryHandlers
{
    public class DecisionTreeServiceQueryHandler<T, U> : IServiceQueryHandler<T>
        where T : IDecisionTreeServiceQuery<U>
        where U : ITreeQueryResult
    {
        /// <summary>
        /// Extracts query properties from T, and calls manager to process tree
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Core.SystemMessages.ISystemMessageCollection HandleQuery(T query)
        {
            ISystemMessageCollection systemMessageCollection = new SystemMessageCollection();          

            Dictionary<string, object> inputValues = GetInputValuesFromQueryProperties(query);

            //TODO call treebugcontext constructor
            var TreeProcessingContext = new TreeProcessingContext(inputValues, query.TreeName, query.TreeVersion.ToString(), query.GlobalsVersion, systemMessageCollection);
            TreeProcessingContext.Execute();

            var variablesObj = TreeProcessingContext.Variables;

            // Populating the result object with results of script execution
            U resultObject = Activator.CreateInstance<U>();
            resultObject.GlobalsVersionResultIsBasedOn = query.GlobalsVersion;
            ExtractResultsFromVariables(variablesObj.outputs, resultObject);

            query.Result = new ServiceQueryResult<U>(new U[] { resultObject });
            return systemMessageCollection;
        }

        private static Dictionary<string, object> GetInputValuesFromQueryProperties(T query)
        {
            Type queryType = typeof(T);

            Dictionary<string, object> inputValues = new Dictionary<string, object>();

            foreach (PropertyInfo propInfo in queryType.GetProperties())
            {
                if (propInfo.Name.Equals("TreeName", StringComparison.InvariantCultureIgnoreCase)
                    || propInfo.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase)
                    || propInfo.Name.Equals("TreeVersion", StringComparison.InvariantCultureIgnoreCase)
                    || propInfo.Name.Equals("Result", StringComparison.InvariantCultureIgnoreCase)
                    || propInfo.Name.Equals("GlobalsVersion", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    continue;
                }

                object queryValue = propInfo.GetValue(query);

                inputValues.Add(propInfo.Name, queryValue);                
            }

            return inputValues;
        }

        private static void ExtractResultsFromVariables(dynamic outputVariables, U resultObject)
        {
            Type resultType = typeof(U);
            foreach (PropertyInfo propInfo in resultType.GetProperties())
            {
                if (propInfo.Name.Equals("GlobalsVersionResultIsBasedOn", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                var outputPropertyToRead = outputVariables.GetType().GetProperty(propInfo.Name) != null
                     ?  outputVariables.GetType().GetProperty(propInfo.Name)
                     :  outputVariables.GetType().GetProperty(string.Format("{0}_Enumeration",propInfo.Name));

                object output_var = outputPropertyToRead.GetValue(outputVariables);
                propInfo.SetValue(resultObject, output_var);
            }
        }
    }
}