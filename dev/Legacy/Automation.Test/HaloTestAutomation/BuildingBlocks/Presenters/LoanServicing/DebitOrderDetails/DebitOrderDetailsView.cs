using BuildingBlocks.Assertions;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing.DebitOrderDetails
{
    public class DebitOrderDetailsView : DebitOrderDetailsBaseControls
    {
        private readonly IWatiNService watinService;

        public DebitOrderDetailsView()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void AddDebitOrderDetails(Automation.DataModels.FinancialServiceBankAccountModel fsb, NodeTypeEnum node = NodeTypeEnum.Add)
        {
            SelectPaymentType(fsb.FinancialServicePaymentTypeKey);
            if (base.BankAccount.Exists)
            {
                SelectBankAccount(fsb.BankAccountKey);
            }
            PopulateDebitOrderDay(fsb.DebitOrderDay);
            PopulateEffectiveDate(fsb.ChangeDate);
            if (node == NodeTypeEnum.Add)
            {
                ClickAdd();
            }
            else
            {
                ClickUpdate();
            }
        }

        /// <summary>
        /// Selects a payment type. Use PaymentType.None if you do not want to select.
        /// </summary>
        /// <param name="paymentType"></param>
        public void SelectPaymentType(FinancialServicePaymentTypeEnum paymentType)
        {
            if (paymentType == FinancialServicePaymentTypeEnum.None)
            {
                base.DOPaymentType.Options[0].Select();
            }
            else
            {
                base.DOPaymentType.SelectByValue(((int)paymentType).ToString());
            }
        }

        /// <summary>
        /// Selects a value from the debit order day dropdown. Pass 0 to not select.
        /// </summary>
        /// <param name="debitOrderDay"></param>
        public void PopulateDebitOrderDay(int? debitOrderDay)
        {
            if (!debitOrderDay.HasValue)
            {
                base.DebitOrderDay.Options[0].Select();
            }
            else
            {
                base.DebitOrderDay.Select(debitOrderDay.ToString());
            }
        }

        /// <summary>
        /// Inserts an effective date
        /// </summary>
        /// <param name="effectiveDate"></param>
        public void PopulateEffectiveDate(DateTime? effectiveDate)
        {
            if (!effectiveDate.HasValue)
            {
                base.EffectiveDate.Clear();
            }
            else
            {
                base.EffectiveDate.Value = effectiveDate.Value.ToString(Formats.DateFormat);
            }
        }

        /// <summary>
        /// Click Add
        /// </summary>
        public void ClickAdd()
        {
            base.Add.Click();
        }

        /// <summary>
        /// Selects a bank account
        /// </summary>
        /// <param name="bankAccountKey"></param>
        public void SelectBankAccount(int? bankAccountKey)
        {
            if (!bankAccountKey.HasValue)
            {
                base.BankAccount.Options[0].Select();
            }
            else
            {
                base.BankAccount.SelectByValue(bankAccountKey.Value.ToString());
            }
        }

        /// <summary>
        /// Checks that the bank account exists in the dropdown list
        /// </summary>
        /// <param name="bankAccountKey"></param>
        public void AssertBankAccountDropdownContainsBankAccountKey(int bankAccountKey)
        {
            WatiNAssertions.AssertSelectListContents(base.BankAccount, new List<int> { bankAccountKey }, false);
        }

        /// <summary>
        ///
        /// </summary>
        public void SelectRow(int identifierKey)
        {
            var row = (from rows in base.DebitOrdersRows
                       where rows.TableCell(Find.ByText(identifierKey.ToString())).Exists
                       select rows).FirstOrDefault();
            row.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void ClickUpdate()
        {
            base.Update.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void ClickDelete()
        {
            watinService.HandleConfirmationPopup(base.Delete);
        }

        public void AssertSalaryPaymentDays(IEnumerable<Automation.DataModels.LegalEntityRole> leRoles, IEnumerable<Automation.DataModels.Employment> legalEntityEmployments)
        {
            foreach (var ler in leRoles)
            {
                var emp = (from e in legalEntityEmployments
                           where e.EmploymentStatusKey == EmploymentStatusEnum.Current
                                        && ler.LegalEntityKey == e.LegalEntityKey
                           select e).FirstOrDefault();

                var matchingRow = (from r in base.SalaryPaymentDays.OwnTableRows
                                   where (from cell in r.TableCells
                                          where cell.Text.Contains(ler.LegalEntityLegalName)
                                          select cell.Text).FirstOrDefault() != null
                                   select r).FirstOrDefault();

                var cell1ContainsLegalName = (from cell1 in matchingRow.OwnTableCells
                                              where cell1.Text.Contains(ler.LegalEntityLegalName)
                                              select cell1).FirstOrDefault();

                var cell2ContainsSalaryPaymentDay = (from cell2 in matchingRow.OwnTableCells
                                                     where cell2.Text.Contains(emp.SalaryPaymentDay.Value.ToString())
                                                     select cell2).FirstOrDefault();

                Assert.That(cell1ContainsLegalName != null && cell2ContainsSalaryPaymentDay != null,
                    "Salary Payment is not dispplayed for LegalEntity:\"{0}\", LegalEntityKey:{1}, AccountKey:{2}", ler.LegalEntityLegalName, ler.LegalEntityKey, ler.AccountKey);
            }
        }

        public void AssertSalaryPaymentDayNotDisplayed()
        {
            Assert.AreEqual(base.SalaryPaymentDays.TableRows.Count, 0, "Salary Payment Day is being displayed but none of the legal entity's employment have a salary payment day.");
        }
    }
}