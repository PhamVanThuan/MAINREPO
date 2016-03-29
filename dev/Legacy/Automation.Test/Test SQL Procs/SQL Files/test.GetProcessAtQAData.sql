USE [2AM]
GO
/****** Object:  StoredProcedure [test].[GetProcessAtQAData]    Script Date: 04/28/2010 12:42:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetProcessAtQAData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetProcessAtQAData]
	Print 'Dropped procedure [test].[GetProcessAtQAData]'
End

go

create procedure [test].[GetProcessAtQAData]
	@ReasonTypeKey INT,
	@TestIdentifier varchar(max)
as

select 

offersatapplicationcapture.offerkey,
aduser.adusername as AssignedQAUser,
offersatapplicationcapture.username as OrigConsultant,

(select top 01 rdesc.description from [2am].dbo.reasontype rt with (nolock)
		join [2am].dbo.reasontypegroup rtg with (nolock)  on rt.reasontypegroupkey=rtg.reasontypegroupkey
		join [2am].dbo.reasondefinition rd with (nolock)  on rt.reasontypekey=rd.reasontypekey
		join [2am].dbo.reasondescription rdesc with (nolock)  on rd.reasondescriptionkey=rdesc.reasondescriptionkey
	where rt.reasontypekey = @ReasonTypeKey and rdesc.description not like '%Miscellaneous Reason%'
	order by reasondefinitionkey desc) as Reason

from test.offersatapplicationcapture
	inner join x2.x2data.application_management
		on offersatapplicationcapture.offerkey =  application_management.applicationkey
	inner join x2.x2.workflowassignment
		on application_management.instanceid = workflowassignment.instanceid
	inner join dbo.aduser
		on workflowassignment.aduserkey = aduser.aduserkey
where offersatapplicationcapture.testidentifier = @TestIdentifier