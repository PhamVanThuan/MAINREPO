use [2am]

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.DeleteProposal') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.DeleteProposal
	Print 'Dropped Proc test.DeleteProposal'
End
Go

CREATE PROCEDURE test.DeleteProposal
@debtcounsellingkey int,
@proposaltypekey int,
@proposalstatuskey int

AS
begin
--we need the keys that we are going to delete
declare @keys table (proposalKey int)

insert into @keys
select proposalKey from debtcounselling.proposal 
where debtcounsellingkey = @debtcounsellingkey
and proposaltypekey = @proposaltypekey
and proposalstatuskey = @proposalstatuskey

--remove proposal item records
delete from debtcounselling.proposalitem 
where proposalkey in (select proposalKey from @keys)

--remove proposal records
delete from debtcounselling.proposal 
where proposalkey in (select proposalKey from @keys)

--remove any reasons
delete from [2am].dbo.reason
where reasonkey in (
select reasonKey from reason r
join [2am].dbo.reasondefinition rd on r.reasonDefinitionKey=rd.reasonDefinitionKey
join [2am].dbo.reasonType rt on rd.reasontypekey=rt.reasontypekey and genericKeyTypeKey=28
join @keys k on r.generickey=k.proposalKey
)

end
