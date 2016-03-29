using System;
using System.Collections.Generic;
using EWorkConnector;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DomainMessages;
using SAHL.Common;

namespace DomainService2.SharedServices.Common
{
    public class PerformEWorkActionCommandHandler : IHandlesDomainServiceCommand<PerformEWorkActionCommand>
    {
        private IeWork eWorkEngine;
        private IDebtCounsellingRepository debtCounsellingRepository;

        public PerformEWorkActionCommandHandler(IeWork eWorkEngine, IDebtCounsellingRepository debtCounsellingRepository)
        {
            this.eWorkEngine = eWorkEngine;
            this.debtCounsellingRepository = debtCounsellingRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, PerformEWorkActionCommand command)
        {
            command.Result = false;
            messages.Clear();
            //string SessionID = eWorkEngine.LogIn("LighthouseUser");
            string SessionID = eWorkEngine.LogIn("X2");
            string ChangedField = "";
            Dictionary<string, string> Vars = new Dictionary<string, string>();
            string resp = null;

            //strip our the SAHL\ from the username
            string eWorkUser = command.AssignedUser.Replace(@"SAHL\", "");

            try
            {
                switch (command.ActionToPerform)
                {
                    #region Application Management Actions

                    case SAHL.Common.Constants.EworkActionNames.X2NTUAdvise:
                        {
                            Vars.Add(SAHL.Common.Constants.EworkActionNames.X2NTUReason, "19");
                            resp = eWorkEngine.InvokeAndSubmitAction(SessionID, command.EFolderID, SAHL.Common.Constants.EworkActionNames.X2NTUAdvise, Vars, SAHL.Common.Constants.EworkActionNames.X2NTUReason);
                            command.Result = true;
                            break;
                        }
                    case SAHL.Common.Constants.EworkActionNames.X2ClientWonOver:
                    case SAHL.Common.Constants.EworkActionNames.X2ClientRefused:
                    case SAHL.Common.Constants.EworkActionNames.X2UNREGISTER:
                    case SAHL.Common.Constants.EworkActionNames.X2REINSTRUCTED:
                    case SAHL.Common.Constants.EworkActionNames.X2ARCHIVE:
                    case SAHL.Common.Constants.EworkActionNames.X2ROLLBACKDISBURSEMENT:
                    case SAHL.Common.Constants.EworkActionNames.X2DISBURSEMENTTIMER:
                    case SAHL.Common.Constants.EworkActionNames.X2RESUB:
                    case SAHL.Common.Constants.EworkActionNames.X2HOLDOVER:
                    case SAHL.Common.Constants.EworkActionNames.X2DECLINEFINAL:
                        {
                            resp = eWorkEngine.InvokeAndSubmitAction(SessionID, command.EFolderID, command.ActionToPerform, Vars, ChangedField);
                            command.Result = true;
                            break;
                        }

                    #endregion Application Management Actions

                    #region Loss Control Actions

                    case Constants.EworkActionNames.X2DebtCounselling:
                        // only fire this flag if there is a e-work case and the case IS NOT at a DC stage
                        if (String.IsNullOrEmpty(command.CurrentStage) == false)
                        {
                            if (String.Compare(command.CurrentStage, "Debt Counselling (Facilitation)", true) != 0// Facilitation sub-map
                                && String.Compare(command.CurrentStage, "Debt Counselling (Seq)", true) != 0// Sequestration sub-map
                                && String.Compare(command.CurrentStage, "Debt Counselling (Estates)", true) != 0 // Estates sub-map
                                && String.Compare(command.CurrentStage, "Debt Counselling (Collections)", true) != 0 // Collections sub-map
                                && String.Compare(command.CurrentStage, "Debt Counselling (Arrears)", true) != 0 // Arrears sub-map
                                && String.Compare(command.CurrentStage, "Debt Counselling", true) != 0) // Legal Action sub-map
                            {
                                Vars.Add("UserToDo", eWorkUser);
                                resp = eWorkEngine.InvokeAndSubmitAction(SessionID, command.EFolderID, SAHL.Common.Constants.EworkActionNames.X2DebtCounselling, Vars, ChangedField);
                                command.Result = true;
                            }
                            else
                            {
                                command.Result = true;
                            }
                        }
                        break;


                    case Constants.EworkActionNames.X2ReturnDebtCounselling:
                    case Constants.EworkActionNames.CancelRCSDebtCounselling:
                    case Constants.EworkActionNames.TerminateDebtCounselling:
						// only fire this flag if there is a e-work case and the case IS at a DC stage
						if (String.IsNullOrEmpty(command.CurrentStage) == false)
						{
							if (String.Compare(command.CurrentStage, "Debt Counselling (Facilitation)", true) == 0// Facilitation sub-map
								|| String.Compare(command.CurrentStage, "Debt Counselling (Seq)", true) == 0// Sequestration sub-map
								|| String.Compare(command.CurrentStage, "Debt Counselling (Estates)", true) == 0 // Estates sub-map
								|| String.Compare(command.CurrentStage, "Debt Counselling (Collections)", true) == 0 // Collections sub-map
								|| String.Compare(command.CurrentStage, "Debt Counselling (Arrears)", true) == 0 // Arrears sub-map
								|| String.Compare(command.CurrentStage, "Debt Counselling", true) == 0) // Legal Action sub-map
							{
								Vars.Add("UserToDo", eWorkUser);
                                resp = eWorkEngine.InvokeAndSubmitAction(SessionID, command.EFolderID, command.ActionToPerform, Vars, ChangedField);
								command.Result = true;
							}
							else
							{
								command.Result = true;
							}
						}
						break;

                    #endregion Loss Control Actions
                }
            }
            catch (eWorkException)
            {
                messages.Add(new Error(string.Format("Unable to perform EWork Action: {0}", command.ActionToPerform), ""));
            }
        }
    }
}