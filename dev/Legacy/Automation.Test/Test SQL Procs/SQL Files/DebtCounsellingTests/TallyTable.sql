use [2AM]
go
if (object_id('test.Tally') is not null)
	begin
		drop TABLE test.Tally
		print 'Dropped Table: test.Tally'
	end

go

--===== Create and populate the Tally table on the fly
 SELECT TOP 11000 --equates to more than 30 years of dates
        IDENTITY(INT,1,1) AS N
   INTO test.Tally
   FROM Master.dbo.SysColumns sc1,
        Master.dbo.SysColumns sc2

--===== Add a Primary Key to maximize performance
  ALTER TABLE test.Tally
    ADD CONSTRAINT PK_Tally_N 
        PRIMARY KEY CLUSTERED (N) WITH FILLFACTOR = 100

--===== Allow the general public to use it
  GRANT SELECT ON test.Tally TO PUBLIC