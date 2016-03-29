//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace SAHL.X2.Framework.DataAccess
{
    //public class UserMetrics
    //{
    //  #region Events

    //  public event MetricStoredHandler MetricDataSaved;

    //  #endregion

    //  #region Methods

    //  /// <summary>
    //  /// Takes a typed Metrics dataset as input and inserts the Metric data in the Metrics database table
    //  /// </summary>
    //  /// <param name="p_MetricInfo"></param>
    //  /// <param name="p_Context">A context with a valid data connection.</param>
    //  public void //StoreMetric(Metrics p_MetricInfo, TransactionContext p_Context)
    //  {
    //    try
    //    {
    //      // we have to pop the metric rows into a new table
    //      Metrics InsertMetric = new Metrics();

    //      Metrics.MetricsRow NewRow = null, Row = null;

    //      for (int i = 0; i < p_MetricInfo._Metrics.Count; i++)
    //      {
    //        Row = p_MetricInfo._Metrics[i];
    //        if (Row.RowState == DataRowState.Added)
    //        {
    //          InsertMetric._Metrics.ImportRow(Row);
    //        }
    //        else
    //        {
    //          NewRow = InsertMetric._Metrics.NewMetricsRow();
    //          Common.CloneDataRow(Row, NewRow);
    //          InsertMetric._Metrics.AddMetricsRow(NewRow);
    //        }
    //      }

    //      //
    //      UpdateInformation UpdateInfo = new UpdateInformation();
    //      UpdateInfo.InsertName = "MetricInsert";
    //      UpdateInfo.ApplicationName = "RCS";
    //      UpdateInfo.InsertParameters = new ParameterCollection();

    //      // Insert Parameters
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@ApplicationName", "ApplicationName");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@HostName", "HostName");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@WorkStationID", "WorkStationID");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@WindowsLogon", "WindowsLogon");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@FormName", "FormName");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@MetricFrom", "MetricFrom");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@MetricTo", "MetricTo");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@MetricAction", "MetricAction");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@MetricInformation", "MetricInformation");

    //      WorkerHelper.Update(InsertMetric._Metrics, p_Context, UpdateInfo);

    //    }
    //    catch (Exception e)
    //    {
    //      ///TODO:make sure of exception policy
    //      bool rethrow = ExceptionPolicy.HandleException(e, "DataAccess");
    //      if (rethrow)
    //        throw;// new MetricException("Saving Metric failed");
    //    }

    //    OnMetricSave();
    //  }

    //  /// <summary>
    //  /// Takes a typed Metrics dataset as input and inserts the Metric data in the Metrics database table
    //  /// </summary>
    //  /// <param name="p_MetricInfo"></param>
    //  /// <param name="p_Context">A context with a valid data connection.</param>
    //  public void StoreMetricWithContext(Metrics p_MetricInfo, TransactionContext p_Context)
    //  {
    //    try
    //    {
    //      // we have to pop the metric rows into a new table
    //      Metrics InsertMetric = new Metrics();

    //      Metrics.MetricsRow NewRow = null, Row = null;

    //      for (int i = 0; i < p_MetricInfo._Metrics.Count; i++)
    //      {
    //        Row = p_MetricInfo._Metrics[i];
    //        if (Row.RowState == DataRowState.Added)
    //        {
    //          InsertMetric._Metrics.ImportRow(Row);
    //        }
    //        else
    //        {
    //          NewRow = InsertMetric._Metrics.NewMetricsRow();
    //          Common.CloneDataRow(Row, NewRow);
    //          InsertMetric._Metrics.AddMetricsRow(NewRow);
    //        }
    //      }

    //      //
    //      UpdateInformation UpdateInfo = new UpdateInformation();
    //      UpdateInfo.InsertName = "MetricInsert";
    //      UpdateInfo.ApplicationName = "RCS";
    //      UpdateInfo.InsertParameters = new ParameterCollection();

    //      // Insert Parameters
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@ApplicationName", "ApplicationName");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@HostName", "HostName");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@WorkStationID", "WorkStationID");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@WindowsLogon", "WindowsLogon");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@FormName", "FormName");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@MetricFrom", "MetricFrom");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@MetricTo", "MetricTo");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@MetricAction", "MetricAction");
    //      WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@MetricInformation", "MetricInformation");

    //      WorkerHelper.Update(InsertMetric._Metrics, p_Context, UpdateInfo);

    //    }
    //    catch (Exception e)
    //    {
    //      ///TODO:make sure of exception policy
    //      bool rethrow = ExceptionPolicy.HandleException(e, "DataAccess");
    //      if (rethrow)
    //        throw;// new MetricException("Saving Metric failed");
    //    }

    //    OnMetricSave();
    //  }

    //  /// <summary>
    //  /// Raise Metric Save event
    //  /// </summary>
    //  protected virtual void OnMetricSave()
    //  {
    //    if (MetricDataSaved != null)
    //    {
    //      MetricDataSaved();
    //    }
    //  }
    //  #endregion

    //  #region Delegates

    //  public delegate void MetricStoredHandler();

    //  #endregion
    //}
}