declare @InstanceID bigint

set @InstanceID = :InstanceID

if (not exists(select InstanceID from [x2].[staging].[SecurityRecalculation] where InstanceID=@InstanceID))
	insert into [x2].[staging].[SecurityRecalculation] ([InstanceID]) values (@InstanceID)
		   
		   
