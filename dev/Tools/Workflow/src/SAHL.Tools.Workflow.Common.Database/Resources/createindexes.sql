-- SETUP SOME TEMPORARY INDEXES

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[X2].[ScheduledActivity]') AND name = N'IX_ScheduledActivity_MIGRATE')
	CREATE NONCLUSTERED INDEX [IX_ScheduledActivity_MIGRATE]
	ON [X2].[X2].[ScheduledActivity] ([WorkFlowProviderName])
	INCLUDE ([InstanceID],[Time],[ActivityID],[Priority],[ID])

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[X2].[InstanceActivitySecurity]') AND name = N'IX_InstanceActivitySecurity_ActivityID_MIGRATE')
	CREATE NONCLUSTERED INDEX [IX_InstanceActivitySecurity_ActivityID_MIGRATE] ON [X2].[X2].[InstanceActivitySecurity] 
	(
		[ActivityID] ASC
	)
	INCLUDE ( [ID],
	[InstanceID],
	[ADUserName]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = ON, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

