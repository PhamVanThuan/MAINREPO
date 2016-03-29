using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using System.Data;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class LegalEntitySequestrationNotify : LegalEntityNotification
    {
        /// <summary>
        /// Constructor for LegalEntitySequestrationNotify
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public LegalEntitySequestrationNotify(ILegalEntityNotification view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            _view.UpdateSequestration = true;
        }

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            using (TransactionScope txn = new TransactionScope())
            {
                try
                {
                    //bool workdone = false;
                    Dictionary<int, bool> selectedGridItems = _view.GetSequestrationItems;
                    foreach (BindableLEReasonRole ler in leList)
                    {
                        foreach (KeyValuePair<int, bool> kv in selectedGridItems)
                        {
                            if (kv.Key == ler.LEKey)// only assess this le
                            {
                                if (kv.Value && !ler.Sequestration)//checked in the grid, no reason exists, create one
                                {
                                    IReason r = ReasonRepo.CreateEmptyReason();
                                    r.Comment = "";
                                    r.GenericKey = ler.LEKey;
                                    r.ReasonDefinition = ReasonRepo.GetReasonDefinitionsByReasonDescriptionKey(ReasonTypes.DebtCounsellingNotification, (int)ReasonDescriptions.NotificationofSequestration)[0];
                                    ReasonRepo.SaveReason(r);
                                    //workdone = true;
                                }

                                if (!kv.Value && ler.Sequestration)//not selected in the grid, delete the reason (if it exists)
                                {
                                    IReadOnlyEventList<IReason> reasons = ReasonRepo.GetReasonByGenericKeyAndReasonTypeKey(ler.LEKey, (int)ReasonTypes.DebtCounsellingNotification);
                                    foreach (IReason r in reasons)
                                    {
                                        if (r.ReasonDefinition.ReasonDescription.Key == (int)ReasonDescriptions.NotificationofSequestration)
                                        {
                                            ReasonRepo.DeleteReason(r);
                                            //workdone = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //if (!workdone)
                    //    _view.Messages.Add(new Warning("No changes have been made, do you want to continue?", "No changes have been made, do you want to continue?"));
                    string subject = String.Empty;
                    string body = String.Empty;
                    string affectedLE = String.Empty;
                    bool exists = false;
                    bool leLeft = false;

                    DataTable dt = DebtCounselling.Account.GetAccountRoleNotificationByTypeAndDecription(ReasonTypes.DebtCounsellingNotification, ReasonDescriptions.DCCancelledClientSequestrated);
                    foreach (DataRow dr in dt.Rows)
                    {
                        exists = Convert.ToBoolean(dr[1]);
                        if (exists)
                            affectedLE += dr[0].ToString() + ", ";
                        else
                            leLeft = true;
                    }

                    if (!String.IsNullOrEmpty(affectedLE))
                    {
                        affectedLE = affectedLE.Trim();
                        affectedLE = affectedLE.TrimEnd(',');
                    }


                    // send the mails if there are affected LE's
                    if (_view.IsValid && !String.IsNullOrEmpty(affectedLE))
                    {
                        string wfrt = String.Empty;
                        IADUser dcUser = AssignedDCUser(out wfrt);

                        ICorrespondenceTemplate template = null;
                        if (leLeft)
                            template = CRepo.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.SequestrationNotificationOthersExist);
                        else
                            template = CRepo.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.SequestrationNotificationNoOthers);

                        subject = String.Format(template.Subject, DebtCounselling.Account.Key);
                        body = String.Format(template.Template, affectedLE, DebtCounselling.Account.Key, dcUser.LegalEntity.DisplayName, wfrt);

                        //send internal mail
                        msgSvc.SendEmailInternal("HALO@SAHomeloans.com", template.DefaultEmail, "", "", subject, body);

                        //from Assigned DC user
                        string from = dcUser.LegalEntity.EmailAddress;

                        //to Assigned DC
                        string to = DebtCounselling.DebtCounsellor.EmailAddress;
						//if there was no email addy for the person....
						if (String.IsNullOrEmpty(to) &&
							//Ensure that the Debt Counsellor Company is not null before attempting to get the Debt Counselling Company Email Address
							DebtCounselling.DebtCounsellorCompany != null &&
							String.IsNullOrEmpty(DebtCounselling.DebtCounsellorCompany.EmailAddress))
							to = DebtCounselling.DebtCounsellorCompany.EmailAddress;

                        //send external mail to DC
                        msgSvc.SendEmailExternal(GenericKey, from, to, "", "", subject, body, String.Empty, String.Empty, String.Empty);
                    }
                    if (_view.IsValid)
                    {
                        txn.VoteCommit();

                        CompleteAndNavigate();
                    }
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
            }
        }
    }
}