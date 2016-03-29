use [2am]

go


ALTER TABLE [dbo].ITC ALTER COLUMN AccountKey INT NULL
go

ALTER TABLE [archive].ITC ALTER COLUMN AccountKey INT NULL
go


-- ===============================================
-- Author:		GaryD
-- Create date: 2007-03-07
-- Description:	Archive any previous ITC enquiries

-- 2014/12/22	GaryD	removed AccountKey from the archive insert and subsequent delete. ITC queries are distinct per Legal Entity
-- ===============================================
ALTER TRIGGER [dbo].[ti_ITC]
   ON  [dbo].[ITC] AFTER INSERT
AS 
BEGIN
  SET NOCOUNT ON;

  INSERT INTO archive.ITC 
  --(ITCKey, LegalEntityKey, AccountKey, ChangeDate, ResponseXML, ResponseStatus, UserID, ArchiveUser, ArchiveDate, RequestXML)
  SELECT itc.ITCKey, 
		itc.LegalEntityKey, 
		itc.AccountKey, 
		itc.ChangeDate, 
		itc.ResponseXML, 
		itc.ResponseStatus, 
		itc.UserID, 
		i.UserID, 
		GetDate(),
		itc.RequestXML
  FROM dbo.ITC itc
  INNER JOIN inserted i ON itc.LegalEntityKey = i.LegalEntityKey
  WHERE itc.ITCKey NOT IN (SELECT ITCKey FROM inserted);

  DELETE FROM dbo.ITC
  WHERE ITCKey IN (
		SELECT itc.ITCKey 
		FROM dbo.ITC itc
		INNER JOIN inserted i 
			ON itc.LegalEntityKey = i.LegalEntityKey
			WHERE itc.ITCKey NOT IN (SELECT ITCKey FROM inserted)
		) ;

END
GO
