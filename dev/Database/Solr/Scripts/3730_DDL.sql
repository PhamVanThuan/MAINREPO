USE [2am]
GO

/************************************* THIRD PARTY TABLE ****************************/

IF OBJECT_ID('[2am].solr.ThirdParty') IS NOT NULL
 DROP TABLE [2am].solr.ThirdParty
 
CREATE TABLE [2AM].solr.ThirdParty(
	ID					INT IDENTITY (1,1),
	LegalEntityKey		VARCHAR(10)  NOT NULL,
	LegalEntityType		VARCHAR(10)  NULL,		--FACET
	ThirdPartyType		VARCHAR(10)  NULL,		--FACET
	ThirdPartySubType	VARCHAR(50)  NULL,		--FACET
	LegalName			VARCHAR(100) NULL,
	TradingName			VARCHAR(100) NULL,
	LegalIdentityType	VARCHAR(20)  NULL,
	LegalIdentity		VARCHAR(30)  NULL,
	TaxNumber			VARCHAR(100) NULL,
	Contact				VARCHAR(100) NULL,
	WorkPhoneNumber		VARCHAR(100) NULL,
	CellPhoneNumber		VARCHAR(100) NULL,
	EmailAddress		VARCHAR(100) NULL,
	FaxNumber			VARCHAR(100) NULL,		
	OfferKey			VARCHAR(MAX) NULL,
	AccountKey			VARCHAR(MAX) NULL,
	OfferType			VARCHAR(MAX) NULL,
	[Address]			VARCHAR(5000) NULL,
	LastModifiedDate		DATETIME )			

/************************************* TASK TABLE ****************************/

IF OBJECT_ID('[2am].solr.Task') IS NOT NULL
 DROP TABLE [2am].solr.Task

CREATE TABLE [2AM].solr.Task(
	ID					INT IDENTITY (1,1),
	InstanceID			INT,
	UserName			VARCHAR(MAX) NULL,		
	Process				VARCHAR(100) NULL,		
	Workflow			VARCHAR(100) NULL,		
	[State]				VARCHAR(100) NULL,
	[Subject]			VARCHAR(200) NULL,
	GenericKeyTypeKey	VARCHAR(30)  NULL,
	GenericKeyType		VARCHAR(100) NULL,
	GenericKeyValue		VARCHAR(100) NULL,
	Attribute1Type		VARCHAR(100) NULL,
	Attribute1Value		VARCHAR(100) NULL,
	Attribute1DataType	VARCHAR(20)  NULL,
	Attribute2Type		VARCHAR(100) NULL,
	Attribute2Value		VARCHAR(100) NULL,
	Attribute2DataType	VARCHAR(20)  NULL,
	Attribute3Type		VARCHAR(100) NULL,
	Attribute3Value		VARCHAR(100) NULL,
	Attribute3DataType	VARCHAR(20)  NULL,		
	[Status]			VARCHAR(20)  NULL,					
	LastModifiedDate	DATETIME )

/************************************* CLIENT TABLE ****************************/
IF OBJECT_ID('[2am].solr.Client') IS NOT NULL
 DROP TABLE [2am].solr.Client
	
CREATE TABLE [2am].solr.Client(
	ID					 INT IDENTITY (1,1),
	LegalEntityKey		 VARCHAR(10)  NOT NULL,
	LegalEntityStatusKey VARCHAR(10)  NULL,		
	LegalEntityType		VARCHAR(50)	  NULL,		
	LegalName			VARCHAR(200)  NULL,		
	FirstNames			VARCHAR(80)	  NULL,		
	Surname				VARCHAR(80)  NULL,		
	LegalIdentity		VARCHAR(50)   NULL,
	LegalIdentityType	VARCHAR(50)   NULL,
	PreferredName		VARCHAR(50)   NULL,
	TaxNumber			VARCHAR(100)  NULL,
	HomePhoneNumber		VARCHAR(50)   NULL,
	WorkPhoneNumber		VARCHAR(50)   NULL,
	CellPhoneNumber		VARCHAR(50)   NULL,
	EmailAddress		VARCHAR(100)  NULL,		
	FaxNumber			VARCHAR(100)  NULL,
	RoleType			VARCHAR(100)  NULL,		
	AccountKey			VARCHAR(30)   NULL,
	Accounts			VARCHAR(MAX)  NULL,		
	AccountStatus		VARCHAR(200)  NULL,
	OfferKey			VARCHAR(MAX)  NULL,
	OfferStatus			VARCHAR(200)  NULL,
	OfferType			VARCHAR(200)  NULL,
	Product				VARCHAR(200)  NULL,
	LegalEntityAddress	VARCHAR(5000) NULL,
	PropertyAddress		VARCHAR(5000) NULL,
	ClientLead			VARCHAR(30)	  NULL,
	LastModifiedDate		DATETIME )
	
/************************************* THIRD PARTY INVOICE TABLE ****************************/
IF OBJECT_ID('[2am].solr.ThirdPartyInvoice') IS NOT NULL
 DROP TABLE [2am].solr.ThirdPartyInvoice

CREATE TABLE [2am].solr.ThirdPartyInvoice		
	(	 
		 ID INT IDENTITY(1,1)
		,ThirdPartyInvoiceKey INT
		,SahlReference VARCHAR(100)
		,InvoiceStatusKey INT
		,InvoiceStatusDescription VARCHAR(100)
		,AccountKey		INT
		,ThirdPartyID	VARCHAR(100)
		,InvoiceNumber	VARCHAR(100)
		,InvoiceDate	DATETIME
		,ReceivedFromEmailAddress VARCHAR(100)
		,AmountExcludingVAT DECIMAL(22,10)
		,VATAmount			DECIMAL(22,10)
		,TotalAmountIncludingVAT DECIMAL(22,10)
		,CapitaliseInvoice	BIT
		,ReceivedDate		DATETIME
		,SpvDescription		VARCHAR(100)
		,WorkflowProcess	VARCHAR(100)
		,WorkflowStage		VARCHAR(100)
		,AssignedTo			VARCHAR(100)
		,ThirdParty			VARCHAR(100)
		,InstanceID			VARCHAR(50)
		,GenericKey			VARCHAR(50)
		,LastModifiedDate	DATETIME
		,DocumentGuid		VARCHAR(100)			
	)

--	SELECT * FROM [2AM].solr.ThirdPartyInvoice	(NOLOCK)					