USE [Process]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (	SELECT * 
			FROM Process.dbo.sysobjects 
			WHERE name = 'SolrIndexUpdate')
BEGIN
--	DROP TABLE template.SolrIndexUpdate
--END;


/****** Object:  Table template.SolrIndexUpdate    Script Date: 2015-02-17 03:53:37 PM ******/


CREATE TABLE template.SolrIndexUpdate(
	ID					INT IDENTITY(1,1),	
	ProcessStatusKey	INT,
	GenericKey			XML,
	GenericKeyTypeKey	INT 
) ON [PRIMARY]

END
GO

SET ANSI_PADDING OFF
GO

GRANT SELECT ON OBJECT::[template].[SolrIndexUpdate] TO [Batch];
GRANT SELECT ON OBJECT::[template].[SolrIndexUpdate] TO [ProcessRole];
GRANT SELECT ON OBJECT::[template].[SolrIndexUpdate] TO [AppRole];


-- SELECT * FROM template.SolrIndexUpdate


