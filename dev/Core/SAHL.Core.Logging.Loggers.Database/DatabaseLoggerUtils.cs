using SAHL.Core.Data;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Core.Serialisation;
using SAHL.Core.Tasks;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging.Loggers.Database
{
    public class DatabaseLoggerUtils : IDatabaseLoggerUtils
    {
        public TaskManager LogTaskManager { get; set; }

        private IIocContainer iocContainer;

        public DatabaseLoggerUtils(IIocContainer iocContainer)
        {
            LogTaskManager = new TaskManager();
            this.iocContainer = iocContainer;
        }

        IEnumerable<MessageParametersDataModel> ConvertParamsDictToMessageParametersDataModels(LogMessageDataModel datamodel, IDictionary<string, object> parameters)
        {
            var datamodels = new List<MessageParametersDataModel>();

            foreach (var key in parameters.Keys)
            {
                string value = (parameters[key].GetType() == typeof(string)) ? (string)parameters[key] : SerializeObject(parameters[key]);
                datamodels.Add(new MessageParametersDataModel(datamodel.Id, key, value));
            }

            return datamodels;
        }

        IEnumerable<LatencyMetricParametersDataModel> ConvertParamsDictToLatencyMetricParametersDataModels(LatencyMetricMessageDataModel datamodel, IDictionary<string, object> parameters)
        {
            var datamodels = new List<LatencyMetricParametersDataModel>();

            foreach (var key in parameters.Keys)
            {
                string value = (parameters[key].GetType() == typeof(string)) ? (string)parameters[key] : SerializeObject(parameters[key]);
                datamodels.Add(new LatencyMetricParametersDataModel(datamodel.Id, key, value));
            }
            return datamodels;
        }

        IEnumerable<ThroughputMetricParametersDataModel> ConvertParamsDictToThroughputMetricParametersDataModels(ThroughputMetricMessageDataModel datamodel, IDictionary<string, object> parameters)
        {
            var datamodels = new List<ThroughputMetricParametersDataModel>();

            foreach (var key in parameters.Keys)
            {
                string value = (parameters[key].GetType() == typeof(string)) ? (string)parameters[key] : SerializeObject(parameters[key]);
                datamodels.Add(new ThroughputMetricParametersDataModel(datamodel.Id, key, value));
            }
            return datamodels;
        }

        IEnumerable<IDataModel> ConvertParamsDictToDataModels(InstantaneousValueMetricMessageDataModel datamodel, IDictionary<string, object> parameters)
        {
            var datamodels = new List<IDataModel>();
            return datamodels;
        }

        public void DatabaseInsert<T>(T obj, IDictionary<string, object> parameters = null) where T : IDataModelWithPrimaryKeyId
        {
            var dbFactory = this.iocContainer.GetInstance(typeof(IDbFactory)) as IDbFactory;
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.InsertNoLog<T>(obj);
                
                if (parameters != null)
                {
                    var dataModelType = obj.GetType().Name;
                    switch (dataModelType)
                    {
                        case "LogMessageDataModel":
                            db.InsertNoLog<MessageParametersDataModel>(ConvertParamsDictToMessageParametersDataModels(obj as LogMessageDataModel, parameters));
                            break;

                        case "LatencyMetricMessageDataModel":
                            db.InsertNoLog<LatencyMetricParametersDataModel>(ConvertParamsDictToLatencyMetricParametersDataModels(obj as LatencyMetricMessageDataModel, parameters));
                            break;

                        case "ThroughputMetricMessageDataModel":
                            db.InsertNoLog<ThroughputMetricParametersDataModel>(ConvertParamsDictToThroughputMetricParametersDataModels(obj as ThroughputMetricMessageDataModel, parameters));
                            break;

                        default:
                            break;
                    }
                }

                db.Complete();
            }
        }

        public string SerializeObject(object obj)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, JsonSerialisation.Settings);
            }
            catch (Exception e)
            {
                return string.Format("Unable to Serialize object: {0}. Error was: {1}", obj.GetType(), e.Message);
            }
        }

        public void EnsureParametersCreated(ref IDictionary<string, object> parameters)
        {
            if (null == parameters)
            {
                parameters = new Dictionary<string, object>();
            }
        }

        public void MergeThreadLocalParameters(ref IDictionary<string, object> parameters)
        {
            this.EnsureParametersCreated(ref parameters);

            IDictionary<string, object> threadParameters = Logger.ThreadContext;
            if (parameters != null && threadParameters != null)
            {
                foreach (KeyValuePair<string, object> kvp in threadParameters)
                {
                    string parameterKey = kvp.Key;
                    if (parameters.ContainsKey(parameterKey))
                    {
                        parameterKey = string.Format("{0}_{1}", parameterKey, Guid.NewGuid());
                    }

                    parameters.Add(parameterKey, kvp.Value);
                }
            }
        }
    }
}