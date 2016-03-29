use [2AM]
go
if (object_id('test.InsertTestMethod') is not null)
	begin
		drop PROCEDURE test.InsertTestMethod
		print 'Dropped PROCEDURE: test.InsertTestMethod'
	end

go

CREATE PROCEDURE test.InsertTestMethod

@TestMethodName VARCHAR(150),
@TestIdentifier VARCHAR(150),
@TestFixture VARCHAR(150)
AS

INSERT INTO test.TestMethod
(
TestMethodName,
TestIdentifier,
TestFixture
)
VALUES
(
@TestMethodName,
@TestIdentifier,
@TestFixture
)