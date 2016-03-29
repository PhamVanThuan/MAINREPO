using SAHL.Core.Data;
using SAHL.Core.Attributes;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    [InsertConventionExclude]
    public class CreateDisabilityClaimPaymentScheduleStatement : ISqlStatement<string>
    {
        public int DisabilityClaimKey { get; protected set; }
        public string UserName { get; protected set; }

        public CreateDisabilityClaimPaymentScheduleStatement(int disabilityClaimKey, string adUserName)
        {
            DisabilityClaimKey = disabilityClaimKey;
            UserName = adUserName;
        }

        public string GetStatement()
        {
            return @"BEGIN TRY
	                    BEGIN TRANSACTION
                            DECLARE @RC int
                            DECLARE @Msg varchar(1024)
                            EXECUTE @RC = [process].[halo].[pApproveLifeDisabilityPayment] 
                               @DisabilityClaimKey
                              ,@UserName
                              ,@Msg OUTPUT
                    
                            select @Msg as Message      
	                            COMMIT
                    END TRY
                        BEGIN CATCH
	                        ROLLBACK
                        END CATCH";
        }
    }
}
