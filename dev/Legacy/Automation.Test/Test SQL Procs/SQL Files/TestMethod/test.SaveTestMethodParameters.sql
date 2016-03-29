USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.SaveTestMethodParameters') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.SaveTestMethodParameters
	Print 'Dropped procedure test.SaveTestMethodParameters'
End
Go

CREATE PROCEDURE test.SaveTestMethodParameters

@TestMethodName VARCHAR(150),
@TestIdentifier VARCHAR(150),
@ParameterDescription VARCHAR(150),
@ParameterValue VARCHAR(150)

AS

--first we get the TestMethodKey
DECLARE @TestMethodKey INT
DECLARE @ParameterTypeKey INT

SELECT @TestMethodKey = TestMethodKey 
FROM test.TestMethod tm 
WHERE 
tm.TestMethodName = @TestMethodName
AND tm.TestIdentifier = @TestIdentifier

--lets get the ParameterKey
--SELECT @ParameterTypeKey = ParameterTypeKey 
--FROM test.ParameterType pt 
--WHERE pt.Description = @ParameterDescription
set @ParameterTypeKey = CAST(@ParameterDescription as INT)

INSERT INTO test.TestMethodParameter
(
TestMethodKey,
ParameterTypeKey,
ParameterValue
)
VALUES
(
@TestMethodKey,
@ParameterTypeKey,
@ParameterValue
)



