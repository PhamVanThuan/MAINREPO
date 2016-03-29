using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Services.FinancialDomain.Managers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class GetValidSPVStatement : ISqlStatement<GetValidSPVResultModel>
    {
        public string XML { get; protected set; }

        public int GenericKey { get; protected set; }

        public int? ExistingSPVKey { get; protected set; }

        public int SPVDetermineSource { get; protected set; }

        public GetValidSPVStatement(decimal LTV, string offerAttributesCSV)
        {
            this.XML = Functions.GenerateGetValidSPVxml(LTV, offerAttributesCSV);
            this.GenericKey = -1;
            this.ExistingSPVKey = null;
            this.SPVDetermineSource = 3;
        }

        public string GetStatement()
        {
            return @"
declare @ValidSPVKey int,
        @RC          int,
        @Msg         varchar(255)

exec @RC = [Process].[halo].[pGetValidSPV] @GenericKey, 
                                           @ExistingSPVKey, 
		                                   @SPVDetermineSource, 
		                                   @XML, 
		                                   @ValidSPVKey output, 
		                                   @Msg output 

select @ValidSPVKey as ValidSPVKey, @Msg as Message;";

        }
    }
}
