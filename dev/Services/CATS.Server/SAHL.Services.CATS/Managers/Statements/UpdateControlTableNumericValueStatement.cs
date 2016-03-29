using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.CATS.Managers.Statements
{
    public class UpdateControlTableNumericValueStatement : ISqlStatement<object>
    {
        public double ControlNumbericValue { get; protected set; }
        public int ControlNumber { get; protected set; }

        public UpdateControlTableNumericValueStatement(double sequenceNumber, int controlNumericValue)
        {
            this.ControlNumbericValue = sequenceNumber;
            this.ControlNumber = controlNumericValue;
        }

        public string GetStatement()
        {
            return @"UPDATE [2AM].[dbo].[Control]
                       SET [ControlNumeric] = @ControlNumbericValue
                     WHERE [ControlNumber] = @ControlNumber";
        }
    }
}
