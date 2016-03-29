using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ComponentModel;
using System.Web.Script.Services;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.DataAccess;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using AjaxControlToolkit;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Specialized;
using System.Globalization;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Summary description for Diary
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Diary : System.Web.Services.WebService
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="diaryDate"></param>
        /// <param name="aduserKey"></param>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public string GetCasesWithSameDiaryDate(int genericKey, string diaryDate, string aduserKey, string workflowName)
        {
            string results = "No Diary Date to check.";

            if (!String.IsNullOrEmpty(diaryDate))
            {
                
                results = "This case is not assigned to a Consultant.";
                int cnt = 0;
                DateTime dteDiaryDate = DateTime.ParseExact(diaryDate, SAHL.Common.Constants.DateFormat, CultureInfo.InvariantCulture);
                bool courtConsultant = false;

                IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                INoteRepository noteRepo = RepositoryFactory.GetRepository<INoteRepository>();
                IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

                switch (workflowName)
                {
                    case SAHL.Common.Constants.WorkFlowName.DebtCounselling://(int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM:
                        {
                            IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                            IList<IWorkflowRole> workflowRoles;

                            // check if there are any active workflow role records for 'Debt Counselling Court Counsultant'
                            workflowRoles = x2Repo.GetWorkflowRoleForGenericKey(genericKey, (int)SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingCourtConsultantD, (int)SAHL.Common.Globals.GeneralStatuses.Active);
                            if (workflowRoles.Count > 0)
                            {
                                courtConsultant = true;
                            }
                            else
                            {
                                // check if there are any active workflow role records for 'Debt Counselling Counsultant'
                                workflowRoles = x2Repo.GetWorkflowRoleForGenericKey(genericKey, (int)SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, (int)SAHL.Common.Globals.GeneralStatuses.Active);
                            }

                            if (workflowRoles.Count > 0)
                            {
                                IADUser adUser = osRepo.GetAdUserByLegalEntityKey(workflowRoles[0].LegalEntityKey);

                                // lets get all the active workflowroles for this consultant
                                IList<IWorkflowRole> workflowRolesForConsultant = x2Repo.GetWorkflowRoleForLegalEntityKey(workflowRoles[0].LegalEntityKey, workflowRoles[0].WorkflowRoleType.Key, (int)SAHL.Common.Globals.GeneralStatuses.Active);
                                if (workflowRolesForConsultant.Count > 0)
                                {
                                    // for each of these roles lets go get the debtcounselling record
                                    foreach (var workflowRole in workflowRolesForConsultant)
                                    {
                                        bool incrementCount = true;
                                        if (courtConsultant == false)
                                        {
                                            // if we are looking for debt counselling consultant cases - make sure the case does not have a court consultant assigned
                                            workflowRoles = x2Repo.GetWorkflowRoleForGenericKey(workflowRole.GenericKey, (int)SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingCourtConsultantD, (int)SAHL.Common.Globals.GeneralStatuses.Active);
                                            if (workflowRoles.Count > 0)
                                                incrementCount = false;
                                        }

                                        if (incrementCount)
                                        {
                                            IDebtCounselling debtCounselling = dcRepo.GetDebtCounsellingByKey(workflowRole.GenericKey);
                                            // lets check the diary date
                                            if (debtCounselling != null && debtCounselling.DebtCounsellingStatus.Key == (int)SAHL.Common.Globals.DebtCounsellingStatuses.Open
                                                && debtCounselling.DiaryDate.HasValue && debtCounselling.DiaryDate.Value.Date == dteDiaryDate.Date)
                                            {
                                                if (debtCounselling.Key != genericKey) // dont count the current case
                                                    cnt++;
                                            }
                                        }
                                    }
                                }

                                if (cnt > 0)
                                    results = "There are " + cnt + " accounts diarised for " + adUser.ADUserName + " on " + dteDiaryDate.ToString(SAHL.Common.Constants.DateFormat);
                                else
                                    results = "There are no accounts diarised for " + adUser.ADUserName + " on " + dteDiaryDate.ToString(SAHL.Common.Constants.DateFormat);
                            }
                            break;
                        }
                    case SAHL.Common.Constants.WorkFlowName.PersonalLoans:
                        {
                            IADUser aduser = osRepo.GetADUserByKey(Convert.ToInt32(aduserKey));
                            int workflowRoleTypeGroupKey = (int)SAHL.Common.Globals.WorkflowRoleTypeGroups.PersonalLoan;
                            IList<IWorkflowRole> workflowRoles = x2Repo.GetWorkflowRoleForGenericKey(aduser.ADUserName, workflowRoleTypeGroupKey, (int)SAHL.Common.Globals.GeneralStatuses.Active);
                            foreach (IWorkflowRole workflowRole in workflowRoles)
                            {
                                INote note = noteRepo.GetNoteByGenericKeyAndType(workflowRole.GenericKey, workflowRole.WorkflowRoleType.WorkflowRoleTypeGroup.GenericKeyType.Key);
                                if (note != null && note.GenericKey != genericKey 
                                    && note.DiaryDate.HasValue && note.DiaryDate.Value.Date == dteDiaryDate.Date)
                                {
                                        cnt++;
                                }
                            }

                            if (cnt > 0)
                                results = "There are " + cnt + " Applications diarised for " + aduser.ADUserName + " on " + dteDiaryDate.ToString(SAHL.Common.Constants.DateFormat);
                            else
                                results = "There are no Applications diarised for " + aduser.ADUserName + " on " + dteDiaryDate.ToString(SAHL.Common.Constants.DateFormat);

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Serialize(results);
        }
    }
}
