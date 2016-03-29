USE [Capitec]
GO

IF OBJECT_ID (N'PersonITC', N'U') IS NULL 
AND OBJECT_ID (N'ApplicantITC', N'U') IS NOT NULL
BEGIN

CREATE TABLE PersonITC
(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	CurrentITCId UNIQUEIDENTIFIER REFERENCES ITC(Id) NOT NULL,
	ITCDate DATETIME NOT NULL
)

INSERT INTO [Capitec].dbo.PersonITC (Id, CurrentITCID, ITCDate)
SELECT a.PersonID AS Id, 
(
	SELECT TOP 1 appITC.CurrentITCID
	FROM [Capitec].dbo.ApplicantITC appITC 
	JOIN [Capitec].dbo.Applicant app ON appITC.ID = app.ID 
	WHERE app.PersonID = a.PersonID 
	ORDER BY appITC.ITCDate DESC
) AS CurrentITCID,
MAX(aitc.ITCDate) AS ITCDate
FROM [Capitec].dbo.ApplicantITC aitc
JOIN [Capitec].dbo.Applicant a ON a.ID = aitc.ID
GROUP BY a.PersonID

END

GO

GRANT SELECT ON dbo.PersonITC to ServiceArchitect

GO

GRANT INSERT ON dbo.PersonITC to ServiceArchitect

GO 

GRANT UPDATE ON dbo.PersonITC to ServiceArchitect

GO 

GRANT DELETE ON dbo.PersonITC to ServiceArchitect

