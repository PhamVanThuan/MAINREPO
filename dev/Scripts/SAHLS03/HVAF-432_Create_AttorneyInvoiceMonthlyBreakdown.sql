USE [EventProjection]
GO

/****** Object:  Table [dbo].[ThisMonthBreakDownByAttarney]    Script Date: 28/09/2015 02:08:35 PM ******/
If Not Exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'AttorneyInvoiceMonthlyBreakdown' And TABLE_SCHEMA = 'projection')

BEGIN

BEGIN TRANSACTION

CREATE TABLE [projection].[AttorneyInvoiceMonthlyBreakdown](
	[AttorneyId] [uniqueidentifier] NOT NULL,
	[AttorneyName] [varchar](50) NOT NULL,
	[Capitalised] [decimal](22, 10) DEFAULT 0  NOT NULL,
	[PaidBySPV] [decimal](22, 10) DEFAULT 0  NULL,
	[DebtReview] [decimal](22, 10) DEFAULT 0  NULL,
	[Total] AS Capitalised + PaidBySPV,
	[AvgRValuePerInvoice] AS ISNULL((Capitalised + PaidBySPV) / NULLIF(Paid ,0), 0) ,
	[AvgRValuePerAccount] AS ISNULL((Capitalised + PaidBySPV) / NULLIF(AccountsPaid ,0), 0),
	[Paid] [int] DEFAULT 0 NOT NULL,
	[Rejected] [int] DEFAULT 0 NOT NULL,
	[Unprocessed] [int] DEFAULT 0 NOT NULL,
	[Processed] [int] DEFAULT 0 NOT NULL,
	[AccountsPaid] [int] DEFAULT 0 NOT NULL
) ON [PRIMARY]

COMMIT TRANSACTION
END 
GO

GRANT INSERT ON Object::[projection].[AttorneyInvoiceMonthlyBreakdown] TO [AppRole]

GRANT SELECT ON Object::[projection].[AttorneyInvoiceMonthlyBreakdown] TO [AppRole]

GRANT UPDATE ON Object::[projection].[AttorneyInvoiceMonthlyBreakdown] TO [AppRole]

GRANT DELETE ON Object::[projection].[AttorneyInvoiceMonthlyBreakdown] TO [AppRole]