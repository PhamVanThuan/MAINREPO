
USe [2AM]
Go

--DROP Trigger ti_Attorney_ThirdParty
If Not Exists (SELECT TriggerName  = T.name FROM sys.triggers T LEFT JOIN sys.all_objects O ON T.parent_id = o.object_id LEFT JOIN sys.schemas S ON S.schema_id = O.schema_id 
	Where S.name = 'dbo' And O.Name = 'Attorney' And T.Name = 'ti_Attorney_ThirdParty')
BEGIN

	Execute sp_executesql N'CREATE Trigger ti_Attorney_ThirdParty
	On [dbo].[Attorney]
	For Insert
	As
		
		Declare @ThirdPartyTypeId uniqueidentifier
		Set @ThirdPartyTypeId = ''990B66D5-BB00-4114-A2F9-A42800E3755D''

		Insert Into [dbo].[ThirdParty] (Id, LegalEntityKey, ThirdPartyTypeId, IsPanel, GeneralStatusKey, GenericKey)
		Select dbo.CombGuid(), inserted.LegalEntityKey, @ThirdPartyTypeId, 1, inserted.GeneralStatusKey, inserted.AttorneyKey
		From inserted
	'

END

--DROP Trigger tu_Attorney_ThirdParty
If Not Exists (SELECT TriggerName  = T.name FROM sys.triggers T LEFT JOIN sys.all_objects O ON T.parent_id = o.object_id LEFT JOIN sys.schemas S ON S.schema_id = O.schema_id 
	Where S.name = 'dbo' And O.Name = 'Attorney' And T.Name = 'tu_Attorney_ThirdParty')
BEGIN

	Execute sp_executesql N'CREATE Trigger tu_Attorney_ThirdParty
	On [dbo].[Attorney]
	After Update
	As

		IF NOT UPDATE(GeneralStatusKey) RETURN

		Update [dbo].[ThirdParty]
			Set GeneralStatusKey = inserted.GeneralStatusKey
			From [dbo].[ThirdParty] TP
			Inner Join inserted
				On inserted.AttorneyKey = TP.GenericKey
				
	'
END


--DROP Trigger td_Attorney_ThirdParty
If Not Exists (SELECT TriggerName  = T.name FROM sys.triggers T LEFT JOIN sys.all_objects O ON T.parent_id = o.object_id LEFT JOIN sys.schemas S ON S.schema_id = O.schema_id 
	Where S.name = 'dbo' And O.Name = 'Attorney' And T.Name = 'td_Attorney_ThirdParty')
BEGIN
	Execute sp_executesql N'CREATE Trigger td_Attorney_ThirdParty
		On [dbo].[Attorney]
		After Delete
		As
			Delete From [dbo].[ThirdParty] 
			From [dbo].[ThirdParty] TP
			Inner Join deleted
				On deleted.AttorneyKey = TP.GenericKey'
END
 