using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Security;
//using System.Threading;
//using System.Security.Principal;
using SAHL.Common.Factories;
using SAHL.X2.Framework.Interfaces;
using System.Diagnostics;
using System.Data;
using SAHL.Common.DataAccess;
using System.Threading;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.TestWTF;
using NUnit.Framework;
using System.Security.Principal;

namespace WorkflowTestFramework
{
    public class BaseHelper : TestBase
    {
        private IX2Service _x2Service;

        /// <summary>
        /// Create an X2 instance and call complete on the create
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="ProcessName"></param>
        /// <param name="ProcessVersion"></param>
        /// <param name="WorkFlowName"></param>
        /// <param name="ActivityName"></param>
        /// <param name="InputFields"></param>
        /// <param name="IgnoreWarnings"></param>
        public void CreateCompleteInstance(SAHLPrincipal principal, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings)
        {
            //bool created = false;
            try
            {
                //Login
                IX2Info XI = X2Service.GetX2Info(principal);
                if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                    X2Service.LogIn(principal);

                //Create
                X2Service.CreateWorkFlowInstance(principal, ProcessName, ProcessVersion, WorkFlowName, ActivityName, InputFields, false);

                //created = true;
                
                //Complete
                X2Service.CompleteActivity(principal, InputFields, false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //rollback
                //if (created)
                //    X2Service.CancelActivity(principal);

                throw ex;

            }
        }

        /// <summary>
        /// Start and complete a user activity to progress the instance through the workflow
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="InstanceID"></param>
        /// <param name="ActivityName"></param>
        /// <param name="InputFields"></param>
        /// <param name="IgnoreWarnings"></param>
        public void StartCompleteActivity(SAHLPrincipal principal, long InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings)
        {
            bool started = false;
            try 
	        {	        
        		//Login
                IX2Info XI = X2Service.GetX2Info(principal);
                if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                    X2Service.LogIn(principal);

                //call start activity
                X2Service.StartActivity(principal, InstanceID, ActivityName, InputFields, IgnoreWarnings);
                
                started = true;
                //call complete activity
                X2Service.CompleteActivity(principal, InputFields, IgnoreWarnings);

	        }
	        catch (Exception ex)
	        {
		        Debug.WriteLine(ex.ToString());
                if (started)
                    X2Service.CancelActivity(principal);
                
                throw ex;
	        }
        }

        /// <summary>
        /// Create a WTF application for the X2 instance
        /// </summary>
        /// <param name="appTypeKey"></param>
        /// <param name="OriginationSourceKey"></param>
        /// <returns></returns>
        public IApplication_WTF CreateApplication(int appTypeKey, int OriginationSourceKey)
        {
            Application_WTF_DAO appDAO = new Application_WTF_DAO();

            appDAO.ApplicationStartDate = DateTime.Now;
            appDAO.ApplicationStatusKey = 1;//open
            appDAO.ApplicationTypeKey = appTypeKey;
            appDAO.OriginationSourceKey = OriginationSourceKey;

            appDAO.ApplicationEndDate = DateTime.Now;

            appDAO.SaveAndFlush();

            return new Application_WTF(appDAO);
        }

        /// <summary>
        /// Gets a reference to the X2 service.
        /// </summary>
        protected IX2Service X2Service
        {
            get
            {
                if (_x2Service == null)
                    _x2Service = ServiceFactory.GetService<IX2Service>();
                
                return _x2Service;
            }
        }
    }
}
