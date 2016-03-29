using Common.Constants;
using System;
using System.Linq;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DebitOrderDetailsBaseControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_DOPaymentTypeUpdate")]
        protected SelectList DOPaymentType { get; set; }

        [FindBy(Id = "ctl00_Main_BankAccountUpdate")]
        protected SelectList BankAccount { get; set; }

        [FindBy(Id = "ctl00_Main_DebitOrderDayUpdate")]
        protected SelectList DebitOrderDay { get; set; }

        [FindBy(Id = "ctl00_Main_EffectiveDateUpdate")]
        protected TextField EffectiveDate { get; set; }

        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button Add { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button Update { get; set; }

        [FindBy(Id = "ctl00_Main_btnDelete")]
        protected Button Delete { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button Cancel { get; set; }

        [FindBy(Id = "ctl00_Main_gridOrder")]
        protected Table gridOrder { get; set; }

        [FindBy(Id = "ctl00_Main_tblSalaryPaymentDays")]
        protected Table SalaryPaymentDays { get; set; }

        protected TableRowCollection DebitOrdersRows
        {
            get
            {
                return gridOrder.TableRows;
            }
        }

        protected TableRow DebitOrdersRow(string effectiveDate, DateTime dateChanged, int debitOrderDay, string paymentType)
        {
            var row = (from r in DebitOrdersRows
                       where r.TableCell(Find.ByText(effectiveDate)).Exists
                       && r.TableCell(Find.ByText(dateChanged.ToString(Formats.DateFormat))).Exists
                       && r.TableCell(Find.ByText(debitOrderDay.ToString())).Exists
                       && r.TableCell(Find.ByText(paymentType)).Exists
                       select r).FirstOrDefault();

            return row;
        }
    }
}