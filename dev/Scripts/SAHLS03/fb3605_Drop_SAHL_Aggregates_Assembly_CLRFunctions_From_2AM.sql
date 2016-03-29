
USE [2AM]
Go

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'AndDelimit' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP AGGREGATE [dbo].[AndDelimit]
End

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'CommaDelimit' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP AGGREGATE [dbo].[CommaDelimit]
End

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'FirstDate' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP AGGREGATE [dbo].[FirstDate]
End

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'FirstFloat' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP AGGREGATE [dbo].[FirstFloat]
End

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'FirstInt' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP AGGREGATE [dbo].[FirstInt]
End

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'FirstString' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP AGGREGATE [dbo].[FirstString]
End

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'ForwardSlashDelimit' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP AGGREGATE [dbo].[ForwardSlashDelimit]
End

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'NarrowAndDelimit' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP AGGREGATE [dbo].[NarrowAndDelimit]
End

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'NarrowForwardSlashDelimit' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP AGGREGATE [dbo].[NarrowForwardSlashDelimit]
End


If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'SecondFloat' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP AGGREGATE [dbo].[SecondFloat]
End

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'SecondString' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP AGGREGATE [dbo].[SecondString]
End

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'CapitecApplicantsToJson' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP FUNCTION [dbo].[CapitecApplicantsToJson]
End

If Exists (Select * From INFORMATION_SCHEMA.ROUTINES Where ROUTINE_SCHEMA = 'dbo' And ROUTINE_NAME = 'CombGuid' And ROUTINE_TYPE='FUNCTION')
Begin 
	DROP FUNCTION [dbo].[CombGuid]
End

If Exists (Select * From sys.assemblies Where name = 'SAHLAggregates')
Begin 
	DROP ASSEMBLY [SAHLAggregates]
End
