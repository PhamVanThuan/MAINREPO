USE [EventProjection]
GO

/****** Object:  Table [projection].[AccountsPaidForThirdPartyInvoicesMonthly]    Script Date: 28/09/2015 02:08:35 PM ******/
If Not Exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'AccountsPaidForAttorneyInvoicesMonthly' And TABLE_SCHEMA = 'projection')

BEGIN

BEGIN TRANSACTION

CREATE TABLE [projection].AccountsPaidForAttorneyInvoicesMonthly
(
	[AttorneyId] [uniqueidentifier] NOT NULL,
	[ThirdPartyInvoiceKey] [int] NOT NULL,
	[AccountKey] [int] NOT NULL
) ON [PRIMARY]

COMMIT TRANSACTION
END 
GO

GRANT INSERT ON Object::[projection].AccountsPaidForAttorneyInvoicesMonthly TO [AppRole]

GRANT SELECT ON Object::[projection].AccountsPaidForAttorneyInvoicesMonthly TO [AppRole]

GRANT UPDATE ON Object::[projection].AccountsPaidForAttorneyInvoicesMonthly TO [AppRole]

GRANT DELETE ON Object::[projection].AccountsPaidForAttorneyInvoicesMonthly TO [AppRole]