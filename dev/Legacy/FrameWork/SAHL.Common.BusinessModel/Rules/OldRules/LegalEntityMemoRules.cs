using System;
using System.Collections.Generic;
using System.Text;




namespace SAHL.Common.BusinessModel.Rules
{
    //public class LegalEntityMemoRules : RulesBase
    //{
    //    public LegalEntityMemoRules()
    //    {
    //    }

    //    public override bool Validate(ValidationBasis p_Mode,System.Data.DataRow p_Row)
    //    {
    //        LegalEntity.LegalEntityMemoDataTable OriginalDT = new LegalEntity.LegalEntityMemoDataTable();
    //        LegalEntity.LegalEntityMemoRow m_Row = (LegalEntity.LegalEntityMemoRow)p_Row;

    //        ClearErrors();
    //        switch (p_Mode)
    //        {
    //            case ValidationBasis.ModeIsAdd:
    //                {

    //                    if (m_Row.AccountMemoStatusKey == (int)SAHL.Datasets.AccountMemoStatus.Unresolved)
    //                    {
    //                        if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                            AppendError("The Expiry Date must be greater than or equal to the reminder date");

    //                        if (m_Row.ReminderDate < DateTime.Today)
    //                            AppendError("The Reminder Date must be greater than or equal to today's date");

    //                        if (m_Row.ExpiryDate < DateTime.Today)
    //                            AppendError("The Expiry Date must be greater than or equal to today's date");
    //                    }
    //                    else // resolved
    //                    {
    //                        if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                            AppendError("The Expiry Date must be greater than or equal to the reminder date");
    //                    }
    //                }
    //                break;

    //            case ValidationBasis.ModeIsUpdate:
    //                {

    //                    LegalEntity_SFE sfe = new LegalEntity_SFE();
    //                    sfe.GetLegalEntityMemoByLegalEntityMemoKey(OriginalDT, m_Row.LegalEntityMemoKey, new Metrics());
    //                    if (OriginalDT.Rows.Count != 1)
    //                        throw new Exception("Validator - invlid input");


    //                    if (m_Row.UserID != OriginalDT[0].UserID)
    //                        AppendError("The LegalEntity memo record was created by another user and may not be modified");

    //                    HelpDesk_SFE hdsfe = new HelpDesk_SFE();
    //                    HelpDesk.HelpDeskQueryDataTable h = new HelpDesk.HelpDeskQueryDataTable();
    //                    hdsfe.GetHelpDeskQueryByLegalEntityMemoKey(m_Row.LegalEntityMemoKey, h, new Metrics());
    //                    if( h.Rows.Count > 0 )
    //                        AppendError("The LegalEntity memo record is linked to a help desk query and may not be modified.");

    //                    if (m_Row.AccountMemoStatusKey == (int)SAHL.Datasets.AccountMemoStatus.Unresolved)
    //                    {
    //                        if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                            AppendError("The Expiry Date must be greater than or equal to the reminder date");

    //                        if (m_Row.ReminderDate < DateTime.Today)
    //                            AppendError("The Reminder Date must be greater than or equal to today's date");

    //                        if (m_Row.ExpiryDate < DateTime.Today)
    //                            AppendError("The Expiry Date must be greater than or equal to today's date");
    //                    }
    //                    else // resolved
    //                    {
    //                        if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                            AppendError("The Expiry Date must be greater than or equal to the reminder date");
    //                    }



    //                }
    //                break;



    //        }
    //        return ReturnValue();
    //    }
    //}
}
