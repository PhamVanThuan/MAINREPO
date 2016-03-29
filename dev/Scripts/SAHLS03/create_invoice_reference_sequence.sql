use [2am]

go


IF NOT EXISTS (
SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[Invoice_Seq]') AND type IN ('SO')
)

BEGIN

	CREATE SEQUENCE dbo.Invoice_Seq
	 AS INTEGER
	 START WITH 1
	 INCREMENT BY 1
	 MINVALUE 1
	 MAXVALUE 99999
	 NO CYCLE;

END


GRANT UPDATE on dbo.Invoice_Seq TO AppRole

GRANT ALTER on dbo.Invoice_Seq TO AppRole

