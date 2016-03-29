use [x2]
go

declare @InstanceID bigint

set @InstanceID = :InstanceID

INSERT INTO [staging].[SecurityRecalculation] ([InstanceID]) VALUES (@InstanceID)
		   
		   
