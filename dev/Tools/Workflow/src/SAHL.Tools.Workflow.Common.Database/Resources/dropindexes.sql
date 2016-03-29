-- DROP TEMPORARY INDEXES
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[X2].[ScheduledActivity]') AND name = N'IX_ScheduledActivity_MIGRATE')
DROP INDEX [IX_ScheduledActivity_MIGRATE] ON [X2].[X2].[ScheduledActivity] WITH ( ONLINE = OFF )

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[X2].[InstanceActivitySecurity]') AND name = N'IX_InstanceActivitySecurity_ActivityID_MIGRATE')
DROP INDEX [IX_InstanceActivitySecurity_ActivityID_MIGRATE] ON [X2].[X2].[InstanceActivitySecurity] WITH ( ONLINE = OFF )