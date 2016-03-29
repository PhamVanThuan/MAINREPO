using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.CATS.Managers.Statements
{
    public class GetControlTableValueByControlNumberStatement : ISqlStatement<double>
    {
        public int ControlNumber { get; protected set; }

        public GetControlTableValueByControlNumberStatement(int controlNumber)
        {
            this.ControlNumber = controlNumber;
        }

        public string GetStatement()
        {
            var query = @"
                        select top 1
	                        ControlNumeric 
                        from [2am].dbo.[Control] 
                        where 
	                        ControlNumber = @ControlNumber";

            return query;
        }
    }
}
