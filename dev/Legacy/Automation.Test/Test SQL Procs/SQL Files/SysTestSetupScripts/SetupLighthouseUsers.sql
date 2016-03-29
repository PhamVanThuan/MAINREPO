USE [sahldb]

GO

INSERT INTO [sahldb].dbo.SAHLEmployee
(
EmployeeTypeNumber,
EmployeeTeamNumber,
SAHLBranchNumber,
UserGroupNumber,
SAHLEmployeeCode,
SAHLEmployeeName,
SAHLEmployeeFullName,
SAHLEmployeeStatusFlag,
SAHLEmployeeCreditAuthority,
SAHLEmployeeSecondedToCredit,
SAHLEmployeeExceptionCommittee,
SAHLEmployeePreProspectToken,
SAHLEmployeeTakeOnIndicator
)
VALUES
(
1, 1, 1, 3, 0, 'MarchuanV', 'Marchuan van der Merwe', 1 , 0, 0, 0, 0, 0
)

UPDATE [sahldb].dbo.SAHLEmployee
SET UserGroupNumber=3
where SAHLEmployeeName in 
('ClintonS',
'TristanZ',
'AndrewK',
'MarchuanV')



