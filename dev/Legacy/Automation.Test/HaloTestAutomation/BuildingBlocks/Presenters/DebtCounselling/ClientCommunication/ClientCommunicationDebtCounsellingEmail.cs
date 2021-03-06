﻿using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using ObjectMaps.Pages;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class ClientCommunicationDebtCounsellingEmail : ClientCommunicationDebtCounsellingEmailControls
    {
        private readonly IWatiNService watinService;

        public ClientCommunicationDebtCounsellingEmail()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Send email
        /// </summary>
        public void Send()
        {
            base.Send.Click();
        }

        /// <summary>
        /// Populate the view with an email subject and email message.
        /// </summary>
        /// <param name="emailSubject"></param>
        /// <param name="emailMessage"></param>
        public void Populate(string emailSubject, string emailMessage)
        {
            base.EmailSubject.Value = emailSubject;
            string js = string.Format("document.getElementById('ctl00_Main_htmlEmailEditor_DesignIFrame').contentWindow.document.body.innerHTML = '{0}';", emailMessage);
            base.Document.Eval(js);
        }

        /// <summary>
        /// Check all the ceheckboxes in the recipients grid.
        /// </summary>
        public void CheckRecipients()
        {
            watinService.CheckTableCheckboxes(true, base.RecipientsTable);
        }

        /// <summary>
        /// This will get all the legalnames by external role type in the grid
        /// </summary>
        /// <param name="externalRoleType"></param>
        /// <returns></returns>
        public List<string> GetRecipientNamesByExternalRole(ExternalRoleTypeEnum externalRoleType)
        {
            var recipientNames = new List<string>();
            var rows = new List<TableRow>();
            switch (externalRoleType)
            {
                case ExternalRoleTypeEnum.Client:
                    rows = watinService.FindRows("Debt Counselling Client", base.RecipientsTable);
                    break;

                case ExternalRoleTypeEnum.DebtCounsellor:
                    rows = watinService.FindRows("Debt Counsellor", base.RecipientsTable);
                    break;

                case ExternalRoleTypeEnum.LitigationAttorney:
                    rows = watinService.FindRows("Litigation Attorney Company", base.RecipientsTable);
                    break;

                case ExternalRoleTypeEnum.PaymentDistributionAgent:
                    rows = watinService.FindRows("Payment Distribution Agent", base.RecipientsTable);
                    break;

                case ExternalRoleTypeEnum.NationalCreditRegulator:
                    rows = watinService.FindRows("NCR - ", base.RecipientsTable);
                    break;
            }
            foreach (var row in rows)
            {
                if (row.TableCells[1].Text != null)
                {
                    var ricipientName = row.TableCells[1].Text;
                    recipientNames.Add(ricipientName.RemoveWhiteSpace());
                }
            }
            return recipientNames;
        }

        /// <summary>
        /// This will get all the legalnames by legalentity role type in the grid
        /// </summary>
        /// <param name="roleType"></param>
        /// <returns></returns>
        public List<string> GetRecipientNamesByLegalEntityRole(RoleTypeEnum roleType)
        {
            var recipientNames = new List<string>();
            var rows = new List<TableRow>();

            switch (roleType)
            {
                case RoleTypeEnum.MainApplicant:
                    rows = watinService.FindRows(RoleType.MainApplicant, base.RecipientsTable);
                    break;

                case RoleTypeEnum.Suretor:
                    rows = watinService.FindRows(RoleType.Suretor, base.RecipientsTable);
                    break;
            }
            foreach (var row in rows)
            {
                if (row.TableCells[1].Text != null)
                {
                    var ricipientName = row.TableCells[1].Text;
                    recipientNames.Add(ricipientName.RemoveWhiteSpace());
                }
            }
            return recipientNames;
        }
    }
}