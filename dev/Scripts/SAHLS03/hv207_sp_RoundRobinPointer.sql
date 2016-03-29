USE [2AM]
GO

IF object_id('GetNextRoundRobinPointerIndexID') IS NOT NULL
begin
	DROP PROCEDURE [dbo].[GetNextRoundRobinPointerIndexID]
end
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/**************************************************************************************************
Description:	Used to update the [2am].dbo.RoundRobinPointer table and provide the next RoundRobinPointerIndexID. 
				This needs to be atomic and prevent concurrent reads and updates. This is used from 
				the SAHL.Common.BusinessModel.Repositories.WorkflowSecurityRepository

2015-08-28		Clint Stedman	Created

**************************************************************************************************/

create procedure [dbo].[GetNextRoundRobinPointerIndexID]
@RoundRobinPointerKey int,
@MaxRoundRobinPointerIndexID int,
@NextRoundRobinPointerIndexID int out
as
set nocount on;

set transaction isolation level serializable;

begin try

	begin transaction;

	select @NextRoundRobinPointerIndexID = isnull(RoundRobinPointerIndexID, 0) + 1
	from [2am].dbo.RoundRobinPointer
	where RoundRobinPointerKey = @RoundRobinPointerKey;

	if (@NextRoundRobinPointerIndexID > @MaxRoundRobinPointerIndexID)
	begin
		set @NextRoundRobinPointerIndexID = 1

		update [2am].dbo.RoundRobinPointer
		set RoundRobinPointerIndexID = 1
		where RoundRobinPointerKey = @RoundRobinPointerKey;
	end
	else
	begin
		update [2am].dbo.RoundRobinPointer
		set RoundRobinPointerIndexID = @NextRoundRobinPointerIndexID
		where RoundRobinPointerKey = @RoundRobinPointerKey;
	end

	commit transaction;

end try
begin catch

	if @@TRANCOUNT > 0
		rollback transaction;

	set @NextRoundRobinPointerIndexID = -1;

end catch
go

GRANT EXECUTE ON [dbo].[GetNextRoundRobinPointerIndexID] TO [AppRole] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[GetNextRoundRobinPointerIndexID] TO [ProcessRole] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[GetNextRoundRobinPointerIndexID] TO [ITDeveloper] AS [dbo]
GO

GRANT VIEW DEFINITION ON [dbo].[GetNextRoundRobinPointerIndexID] TO [ITDeveloper] AS [dbo]
GO
