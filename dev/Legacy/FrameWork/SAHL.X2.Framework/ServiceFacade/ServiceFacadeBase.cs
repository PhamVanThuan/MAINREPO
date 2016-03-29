using System.Reflection;
using System.Resources;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.Authentication;
using SAHL.X2.Framework.DataAccess;
using SAHL.X2.Framework.DataAccess.Workers;

//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.X2.Framework.DataSets;

namespace SAHL.X2.Framework.ServiceFacade
{
    public delegate void FacadeAuthenticatedHandler();
    public delegate void FacadeMetricsCapturedHandler();

    public class ServiceFacadeBase
    {
        #region Variable Declaration

        protected static ResourceManager ResourceManager = new ResourceManager(typeof(ServiceFacadeBase).Namespace + ".Strings", Assembly.GetExecutingAssembly());

        #endregion Variable Declaration

        #region Events

        public event FacadeAuthenticatedHandler FacadeAuthenticated;
        public event FacadeMetricsCapturedHandler FacadeMetricsCaptured;

        #endregion Events

        #region Properties

        #endregion Properties

        #region Methods

        ///// <summary>
        ///// A metric is stored in the db as the final action of every ServiceFacade method.
        ///// This records a user's actions.
        ///// Developers MUST call this as the last line in a method.
        ///// Failure to store a metric i.e data error will not result in rolling back data changes.
        ///// </summary>
        ///// <param name="p_Metric">contains metric info</param>
        ///// <param name="p_MetricAction">Method Name on SF that called this method</param>
        ///// <param name="p_MetricInformation">Method description on SF that called this method</param>
        //protected virtual void //StoreMetric(string p_MetricAction, string p_MetricInformation/*, Metrics p_Metric*/)
        //{
        //  try
        //  {
        //    UserMetrics m_Metrics = new UserMetrics();
        //    TransactionContext m_Context = TransactionController.GetContext("MetricConnectionString");

        //    if (p_Metric._Metrics.Count > 0)
        //    {
        //      p_Metric._Metrics[0].MetricAction = p_MetricAction;
        //      p_Metric._Metrics[0].MetricInformation = p_MetricInformation;

        //      m_Metrics.//StoreMetric(p_Metric, m_Context);
        //    }
        //  }
        //  catch (Exception e)
        //  {
        //    bool rethrow = ExceptionPolicy.HandleException(e, "Metric");
        //    if (rethrow)
        //      throw;
        //  }
        //  finally
        //  {
        //    OnFacadeMetricsCaptured();
        //  }

        //}

        ///// <summary>
        ///// A metric is stored in the db as the final action of every ServiceFacade method.
        ///// This records a user's actions.
        ///// Developers MUST call this as the last line in a method.
        ///// Failure to store a metric i.e data error will not result in rolling back data changes.
        ///// </summary>
        ///// <param name="p_Metric">contains metric info</param>
        ///// <param name="p_MetricAction">Method Name on SF that called this method</param>
        ///// <param name="p_MetricInformation">Method description on SF that called this method</param>
        //protected virtual void StoreMetricWithContext(TransactionContext Context, string p_MetricAction, string p_MetricInformation/*, Metrics p_Metric*/)
        //{
        //  try
        //  {
        //    UserMetrics m_Metrics = new UserMetrics();
        //    TransactionContext m_Context = Context;// TransactionController.GetContext("MetricConnectionString");

        //    if (p_Metric._Metrics.Count > 0)
        //    {
        //      p_Metric._Metrics[0].MetricAction = p_MetricAction;
        //      p_Metric._Metrics[0].MetricInformation = p_MetricInformation;

        //      m_Metrics.//StoreMetric(p_Metric, m_Context);
        //    }
        //  }
        //  catch (Exception e)
        //  {
        //    bool rethrow = ExceptionPolicy.HandleException(e, "Metric");
        //    if (rethrow)
        //      throw;
        //  }
        //  finally
        //  {
        //    OnFacadeMetricsCaptured();
        //  }

        //}

        /// <summary>
        /// A user on the application must be authenticated as being part of an allowed group before proceeding
        /// with the method.  Failure to authenticate results in an error and the method may not be executed.
        /// Developers MUST call this as the first line in a method.
        /// </summary>
        /// <returns>true if authenticated user</returns>
        protected virtual bool AuthenticateUser()
        {
            try
            {
                //Authenticator m_Auth = new Authenticator();
                //m_Auth.AuthenticateCurrentUser();
            }
            catch
            {
                throw;
            }

            OnFacadeAuthenticated();
            return true;
        }

        protected virtual void OnFacadeAuthenticated()
        {
            if (FacadeAuthenticated != null)
            {
                FacadeAuthenticated();
            }
        }

        private void OnFacadeMetricsCaptured()
        {
            if (FacadeMetricsCaptured != null)
            {
                FacadeMetricsCaptured();
            }
        }

        public double GetControlValueNumeric(string p_ControlDescription/*, Metrics p_Mi*/)
        {
            double dControlNumeric = 0;
            AuthenticateUser();
            ITransactionContext context = null;

            try
            {
                context = TransactionController.GetContext();

                dControlNumeric = GetControlValueNumeric(p_ControlDescription);//, p_MI, context);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (context != null)
                    context.Dispose();
            }

            return dControlNumeric;
        }

        internal double GetControlValueNumeric(string p_ControlDescription/*, Metrics p_Mi*/, ITransactionContext p_Context)
        {
            double dControlNumeric = 0;
            AuthenticateUser();

            ControlConstants ControlTbl = new ControlConstants();
            ControlConstants.ControlRow ControlRow = null;

            try
            {
                CommonWorker.GetControlInfo(p_Context, ControlTbl); //Retrieve Control data from DB

                ControlRow = CommonWorker.GetControlValue(ControlTbl, p_ControlDescription);

                dControlNumeric = ControlRow.ControlNumeric;
            }
            catch
            {
                throw;
            }
            finally
            {
                //StoreMetric("Control_SFE.GetControlValueNumeric_Internal", metricInfo);//);//);//, p_Mi);
                if (p_Context != null)
                    p_Context.Dispose();
            }

            return dControlNumeric;
        }

        public string GetControlValueText(string p_ControlDescription/*, Metrics p_Mi*/)
        {
            string sControlText = "";
            AuthenticateUser();

            ITransactionContext context = null;

            try
            {
                context = TransactionController.GetContext();
                sControlText = GetControlValueText(p_ControlDescription, context);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (context != null)
                    context.Dispose();
            }

            return sControlText;
        }

        internal string GetControlValueText(string p_ControlDescription/*, Metrics p_Mi*/, ITransactionContext p_Context)
        {
            string sControlText = "";
            AuthenticateUser();

            ControlConstants ControlTbl = new ControlConstants();
            ControlConstants.ControlRow ControlRow = null;

            try
            {
                CommonWorker.GetControlInfo(p_Context, ControlTbl); //Retrieve Control data from DB

                ControlRow = CommonWorker.GetControlValue(ControlTbl, p_ControlDescription);

                sControlText = ControlRow.ControlText;
            }
            catch
            {
                throw;
            }
            finally
            {
                //StoreMetric("Control_SFE.GetControlValueText_Internal", metricInfo);//);//, p_Mi);
                if (p_Context != null)
                    p_Context.Dispose();
            }

            return sControlText;
        }

        #endregion Methods
    }
}