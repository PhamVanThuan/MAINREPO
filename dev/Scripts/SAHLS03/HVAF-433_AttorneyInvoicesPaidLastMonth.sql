USE [EventProjection]
GO

/****** Object:  Table [projection].[AttorneyInvoicesPaidLastMonth]    Script Date: 28/09/2015 02:08:35 PM ******/
If Not Exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'AttorneyInvoicesPaidLastMonth' And TABLE_SCHEMA = 'projection')

BEGIN

BEGIN TRANSACTION

CREATE TABLE [projection].AttorneyInvoicesPaidLastMonth
(
	[Count] [int] NOT NULL,
	[Value] [decimal](22, 10) NOT NULL,
) ON [PRIMARY]

COMMIT TRANSACTION
END 
GO

GRANT INSERT ON Object::[projection].AttorneyInvoicesPaidLastMonth TO [AppRole]

GRANT SELECT ON Object::[projection].AttorneyInvoicesPaidLastMonth TO [AppRole]

GRANT UPDATE ON Object::[projection].AttorneyInvoicesPaidLastMonth TO [AppRole]

GRANT DELETE ON Object::[projection].AttorneyInvoicesPaidLastMonth TO [AppRole]