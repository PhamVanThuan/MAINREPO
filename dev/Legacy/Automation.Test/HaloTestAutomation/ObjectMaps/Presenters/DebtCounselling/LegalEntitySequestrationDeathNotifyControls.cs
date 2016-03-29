using Common.Enums;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LegalEntitySequestrationDeathNotifyControls : BasePageControls
    {
        /// <summary>
        /// Returns the table
        /// </summary>
        [FindBy(Id = "ctl00_Main_grdLegalEntities")]
        protected Table grdLegalEntities { get; set; }

        /// <summary>
        /// Selects the checkbox for the legal entity
        /// </summary>
        /// <param name="legalEntityKey"></param>
        protected void SelectCheckBoxByLegalEntityKey(int legalEntityKey, NotificationTypeEnum type)
        {
            TableRow row = gridCellsSelected(legalEntityKey);
            CheckBox checkbox = row.CheckBox(Find.ById(new Regex(string.Format(@"ctl00_Main_grdLegalEntities_ctl0\d_{0}", type))));
            checkbox.Checked = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected bool CheckBoxExists(int legalEntityKey, NotificationTypeEnum type)
        {
            TableRow row = gridCellsSelected(legalEntityKey);
            return row != null;
        }

        /// <summary>
        /// The Submit Button
        /// </summary>
        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button btnUpdate { get; set; }

        /// <summary>
        /// The Cancel Button
        /// </summary>
        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        protected TableRow gridCellsSelected(int legalEntityKey)
        {
            TableCellCollection cells = grdLegalEntities.TableCells;
            var cell = Find.ByText(legalEntityKey.ToString());
            var filter = cells.Filter(cell);
            return filter.Count > 0 ? filter[0].ContainingTableRow : null;
        }
    }
}