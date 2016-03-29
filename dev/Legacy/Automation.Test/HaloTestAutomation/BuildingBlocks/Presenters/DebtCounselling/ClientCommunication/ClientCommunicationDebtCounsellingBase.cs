using System;
using WatiN.Core;
using System.Text.RegularExpressions;
using ObjectMaps;
using CommonData.Constants;
using CommonData.Enums;
using System.Collections.Generic;

namespace BuildingBlocks
{
    public static partial class Views
    {
        internal static class ClientCommunicationDebtCounsellingBase
        {
            internal static void CheckRecipients(ClientCommunicationDebtCounsellingBaseControls controls)
            {
                Dictionary<string, ExternalRoleTypeEnum> recipients = GetRecipients(controls);
                foreach (string legalName in recipients.Keys)
                {
                    int rowCount = 0;
                    while (rowCount < controls.ctl00_Main_gridRecipients_DXMainTable.OwnTableRows.Count)
                    {
                        TableCell recipientNameCell = controls[rowCount, "Recipient Name"];
                        TableCell checkBoxCell = controls[rowCount, "Sel"];
                        checkBoxCell.CheckBoxes[0].Checked = true;
                        rowCount++;
                    }
                }
            }
           /// <summary>
           /// This will get all the Recipients and external roles and add populate a dictionary
           /// </summary>
           /// <param name="controls"></param>
           /// <returns></returns>
            internal static Dictionary<string, ExternalRoleTypeEnum> GetRecipients(ClientCommunicationDebtCounsellingBaseControls controls)
            {
                Dictionary<string, ExternalRoleTypeEnum> recipients = new Dictionary<string, ExternalRoleTypeEnum>();
                for (int index = 0; index < controls.ctl00_Main_gridRecipients_DXMainTable.TableRows.Count; index++)
                {
                    TableCell recipientNameCell = controls[index, "Recipient Name"];
                    TableCell externalRoleTypeCell = controls[index, "Role"];
                    switch (externalRoleTypeCell.Text)
                    {
                        case "Debt Counselling Client":
                            {
                                recipients.Add(recipientNameCell.Text, ExternalRoleTypeEnum.Client);
                                break;
                            }
                        case "Debt Counsellor":
                            {
                                recipients.Add(recipientNameCell.Text, ExternalRoleTypeEnum.DebtCounsellor);
                                break;
                            }
                        case "NCR - Proposal Complaints":
                            {
                                recipients.Add(recipientNameCell.Text, new ExternalRoleTypeEnum());
                                break;
                            }
                        case "NCR - PDA Complaints":
                            {
                                recipients.Add(recipientNameCell.Text, new ExternalRoleTypeEnum());
                                break;
                            }
                        case "NCR - NCR Termination Department":
                            {
                                recipients.Add(recipientNameCell.Text, new ExternalRoleTypeEnum());
                                break;
                            }
                    }
                }
                return recipients;
            }
        }
    }
}