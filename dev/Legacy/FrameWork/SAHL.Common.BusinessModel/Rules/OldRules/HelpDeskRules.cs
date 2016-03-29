using System;
using System.Collections.Generic;
using System.Text;




namespace SAHL.Common.BusinessModel.Rules
{
    //public class HelpDeskRules : RulesBase
    //{
    //    public HelpDeskRules()
    //    {
    //    }

    //    private bool ValidateAdd(System.Data.DataSet p_DataSet)
    //    {
    //        HelpDesk.HelpDeskQueryDataTable q = null;
    //        Account.AccountMemoDataTable a = null;
    //        LegalEntity.LegalEntityMemoDataTable l = null;

    //        foreach( System.Data.DataTable t in p_DataSet.Tables )
    //        {
    //            if( t.GetType() == typeof(HelpDesk.HelpDeskQueryDataTable) )
    //                q = (HelpDesk.HelpDeskQueryDataTable)t;
    //            if (t.GetType() == typeof(Account.AccountMemoDataTable))
    //                a = (Account.AccountMemoDataTable)t;
    //            if (t.GetType() == typeof(LegalEntity.LegalEntityMemoDataTable))
    //                l = (LegalEntity.LegalEntityMemoDataTable)t;
    //        }

    //        if (q == null || a == null || l == null )
    //            throw new Exception("HelpDeskRules: invalid data");


    //        HelpDesk.HelpDeskQueryRow qr = q.FindByHelpDeskQueryKey(-1);
    //        Account.AccountMemoRow ar = a.FindByAccountMemoKey(-1);
    //        LegalEntity.LegalEntityMemoRow lr = l.FindByLegalEntityMemoKey(-1);

    //        if( qr == null || ( ar == null && lr == null))
    //            throw new Exception("HelpDeskRules: invalid data");

    //        if( ar != null )
    //        {
    //            Account.AccountMemoRow m_Row = ar;

    //            if (m_Row.AccountMemoStatusKey == (int)SAHL.Datasets.AccountMemoStatus.Unresolved)
    //            {
    //                if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                    AppendError("The Expiry Date must be greater than or equal to the reminder date");

    //                if (m_Row.ReminderDate < DateTime.Today)
    //                    AppendError("The Reminder Date must be greater than or equal to today's date");

    //                if (m_Row.ExpiryDate < DateTime.Today)
    //                    AppendError("The Expiry Date must be greater than or equal to today's date");
    //            }
    //            else // resolved
    //            {
    //                if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                    AppendError("The Expiry Date must be greater than or equal to the reminder date");
    //            }
                
    //        }
    //        else
    //        {

    //            LegalEntity.LegalEntityMemoRow m_Row = lr;

    //            if (m_Row.AccountMemoStatusKey == (int)SAHL.Datasets.AccountMemoStatus.Unresolved)
    //            {
    //                if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                    AppendError("The Expiry Date must be greater than or equal to the reminder date");

    //                if (m_Row.ReminderDate < DateTime.Today)
    //                    AppendError("The Reminder Date must be greater than or equal to today's date");

    //                if (m_Row.ExpiryDate < DateTime.Today)
    //                    AppendError("The Expiry Date must be greater than or equal to today's date");
    //            }
    //            else // resolved
    //            {
    //                if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                    AppendError("The Expiry Date must be greater than or equal to the reminder date");
    //            }

    //        }

    //        return ReturnValue();

    //    }
    //    private bool ValidateUpdate(System.Data.DataSet p_DataSet)
    //    {

    //        HelpDesk.HelpDeskQueryDataTable q = null;
    //        Account.AccountMemoDataTable a = null;
    //        LegalEntity.LegalEntityMemoDataTable l = null;

    //        foreach (System.Data.DataTable t in p_DataSet.Tables)
    //        {
    //            if (t.GetType() == typeof(HelpDesk.HelpDeskQueryDataTable))
    //                q = (HelpDesk.HelpDeskQueryDataTable)t;
    //            if (t.GetType() == typeof(Account.AccountMemoDataTable))
    //                a = (Account.AccountMemoDataTable)t;
    //            if (t.GetType() == typeof(LegalEntity.LegalEntityMemoDataTable))
    //                l = (LegalEntity.LegalEntityMemoDataTable)t;
    //        }

    //        if (q == null || a == null || l == null)
    //            throw new Exception("HelpDeskRules: invalid data");


    //        HelpDesk.HelpDeskQueryRow qr = null;
    //        Account.AccountMemoRow ar = null;
    //        LegalEntity.LegalEntityMemoRow lr = null;


    //        foreach (HelpDesk.HelpDeskQueryRow tmp in q)
    //        {
    //            if (tmp.RowState == System.Data.DataRowState.Modified)
    //            {
    //                qr = tmp;
    //                break;
    //            }
    //        }

    //        foreach (Account.AccountMemoRow tmp in a)
    //        {
    //            if (tmp.RowState == System.Data.DataRowState.Modified)
    //            {
    //                ar = tmp;
    //                break;
    //            }
    //        }

    //        foreach (LegalEntity.LegalEntityMemoRow tmp in l)
    //        {
    //            if (tmp.RowState == System.Data.DataRowState.Modified)
    //            {
    //                lr = tmp;
    //                break;
    //            }
    //        }

    //        if (qr == null || (ar == null && lr == null))
    //            throw new Exception("HelpDeskRules: invalid data");


    //        if (ar != null)
    //        {
    //            Account.AccountMemoDataTable OriginalDT = new Account.AccountMemoDataTable();
    //            Account.AccountMemoRow m_Row = ar;

    //            Account_SFE sfe = new Account_SFE();
    //            sfe.GetAccountMemoByAccountMemoKey(OriginalDT, m_Row.AccountMemoKey, new Metrics());
    //            if (OriginalDT.Rows.Count != 1)
    //                throw new Exception("Validator - invalid input");


    //            if (m_Row.UserID != OriginalDT[0].UserID)
    //                AppendError("The Help Desk memo record was created by another user and may not be modified");

    //            if (m_Row.AccountMemoStatusKey == (int)SAHL.Datasets.AccountMemoStatus.Unresolved)
    //            {
    //                if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                    AppendError("The Expiry Date must be greater than or equal to the reminder date");

    //                if (m_Row.ReminderDate < DateTime.Today)
    //                    AppendError("The Reminder Date must be greater than or equal to today's date");

    //                if (m_Row.ExpiryDate < DateTime.Today)
    //                    AppendError("The Expiry Date must be greater than or equal to today's date");
    //            }
    //            else // resolved
    //            {
    //                if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                    AppendError("The Expiry Date must be greater than or equal to the reminder date");
    //            }
    //        }
    //        else
    //        {
    //            LegalEntity.LegalEntityMemoDataTable OriginalDT = new LegalEntity.LegalEntityMemoDataTable();
    //            LegalEntity.LegalEntityMemoRow m_Row = lr;

    //            LegalEntity_SFE sfe = new LegalEntity_SFE();
    //            sfe.GetLegalEntityMemoByLegalEntityMemoKey(OriginalDT, m_Row.LegalEntityMemoKey, new Metrics());
    //            if (OriginalDT.Rows.Count != 1)
    //                throw new Exception("Validator - invalid input");


    //            if (m_Row.UserID != OriginalDT[0].UserID)
    //                AppendError("The Help Desk memo record was created by another user and may not be modified");


    //            if (m_Row.AccountMemoStatusKey == (int)SAHL.Datasets.AccountMemoStatus.Unresolved)
    //            {
    //                if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                    AppendError("The Expiry Date must be greater than or equal to the reminder date");

    //                if (m_Row.ReminderDate < DateTime.Today)
    //                    AppendError("The Reminder Date must be greater than or equal to today's date");

    //                if (m_Row.ExpiryDate < DateTime.Today)
    //                    AppendError("The Expiry Date must be greater than or equal to today's date");
    //            }
    //            else // resolved
    //            {
    //                if (m_Row.ExpiryDate < m_Row.ReminderDate)
    //                    AppendError("The Expiry Date must be greater than or equal to the reminder date");
    //            }




    //        }





    //        return ReturnValue();

    //    }
    //    public override bool Validate(ValidationBasis p_Mode, System.Data.DataSet p_DataSet)
    //    {
    //        switch (p_Mode)
    //        {
    //            case ValidationBasis.ModeIsAdd:
    //                return ValidateAdd(p_DataSet);

    //            case ValidationBasis.ModeIsUpdate:
    //                return ValidateUpdate(p_DataSet);

    //            default:
    //                throw new Exception("HelpDeskRules: unknown rule");
    //        }

    //    }
    //}
}
