using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;

namespace BuildingBlocks.Services
{
    public class ControlService : _2AMDataHelper, IControlService
    {
        public int GetControlNumericValue(string controlDescription)
        {
            QueryResultsRow controlEntry = base.GetControlValue(controlDescription);
            return controlEntry.Column("ControlNumeric").GetValueAs<int>();
        }

        public string GetControlTextValue(string controlDescription)
        {
            QueryResultsRow controlEntry = base.GetControlValue(controlDescription);
            return controlEntry.Column("ControlText").GetValueAs<string>();
        }
    }
}