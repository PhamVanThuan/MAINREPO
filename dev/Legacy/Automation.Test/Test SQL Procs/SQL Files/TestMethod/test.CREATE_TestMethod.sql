use [2AM]
go
if (object_id('test.TestMethod') is not null)
	begin
		drop TABLE test.TestMethod
		print 'Dropped TABLE: test.TestMethod'
	end

go

CREATE TABLE test.TestMethod
(
TestMethodKey INT IDENTITY(1,1),
TestMethodName VARCHAR(250) NOT NULL,
TestIdentifier VARCHAR(50) NOT NULL,
TestFixture VARCHAR(50) NOT NULL
)

go

if (object_id('test.TestMethodParameter') is not null)
	begin
		drop TABLE test.TestMethodParameter
		print 'Dropped TABLE: test.TestMethodParameter'
	end

go

CREATE TABLE test.TestMethodParameter
(
TestMethodParameterKey INT IDENTITY(1,1),
TestMethodKey INT NOT NULL,
ParameterTypeKey INT NOT NULL,
ParameterValue VARCHAR(50)
)

go

if (object_id('test.ParameterType') is not null)
	begin
		drop TABLE test.ParameterType
		print 'Dropped TABLE: test.ParameterType'
	end

go

CREATE TABLE test.ParameterType
(
ParameterTypeKey INT IDENTITY(1,1),
Description VARCHAR(50)
)
