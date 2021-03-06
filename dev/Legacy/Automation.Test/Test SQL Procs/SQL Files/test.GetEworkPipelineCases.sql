USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetEworkPipelineCases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetEworkPipelineCases]
	Print 'Dropped procedure [test].[GetEworkPipelineCases]'
End
Go

/*
This procedure will retrieve applications where the ework interaction can be tested.
Option 1:	Pass @ework_stage as NULL, then use a combination of the @x2_state and the
			@ework_action to find cases where an interaction can occur. e.g. Find cases
			at 'Disbursement Review' where the e-work action 'Hold Over' can be performed
Usage: exec test.GetEworkPipelineCases 'hold over','disbursement review',null
Option 2:	Pass @ework_action and @x2_state as NULL to find e-work cases at a particular 
			state. e.g.	@ework_stage = 'Resubmitted' will return cases that you can resub
			in HALO.
Usage: exec test.GetEworkPipelineCases NULL,NULL,'Resubmitted'
*/

CREATE PROCEDURE [test].[GetEworkPipelineCases]
@ework_action varchar(100), 
@x2_state varchar(100), 
@ework_stage varchar(100) 

 AS 

IF (@ework_stage IS NULL) 
  BEGIN 
    SELECT DISTINCT TOP 50 applicationkey, 
                           loannumber 
    FROM   x2.x2data.application_management am(nolock) 
           JOIN [2am].dbo.offer o(nolock) 
             ON am.applicationkey = o.offerkey 
           JOIN x2.x2.instance i(nolock) 
             ON am.instanceid = i.id 
           JOIN x2.x2.state s(nolock) 
             ON i.stateid = s.id 
           JOIN (SELECT efoldername [loannumber] 
                 FROM   [e-work]..efolder e(nolock) 
                 WHERE  estagename IN (SELECT estagename 
                                FROM   [e-work]..eaction (nolock) 
                                WHERE  emapname = 'Pipeline' 
                                AND eactionname = @ework_action) 
                 AND emapname = 'Pipeline' 
                 AND ISNUMERIC(efoldername) = 1) ework 
             ON o.accountkey = ework.loannumber 
    WHERE  s.name = @x2_state 
  END 
ELSE 
  BEGIN 
    SELECT DISTINCT TOP 50 applicationkey, 
                           loannumber 
    FROM   x2.x2data.application_management am(nolock) 
           JOIN [2am].dbo.offer o(nolock) 
             ON am.applicationkey = o.offerkey 
           JOIN x2.x2.instance i(nolock) 
             ON am.instanceid = i.id 
           JOIN x2.x2.state s(nolock) 
             ON i.stateid = s.id 
           JOIN (SELECT efoldername [loannumber] 
                 FROM   [e-work]..efolder e(nolock) 
                 WHERE  estagename IN (SELECT estagename 
                                FROM   [e-work]..eaction (nolock) 
                                WHERE  emapname = 'Pipeline' 
                                AND estagename = @ework_stage) 
                 AND emapname = 'Pipeline' 
                 AND ISNUMERIC(efoldername) = 1) ework 
             ON o.accountkey = ework.loannumber 
  END