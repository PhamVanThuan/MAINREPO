--================================================
-- Drop function template
--================================================
USE [2AM]
GO

IF OBJECT_ID (N'dbo.fCalculateAge') IS NOT NULL
   DROP FUNCTION dbo.fCalculateAge
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		AndrewK
-- Create date: 2015-03-10
-- Description:	Calculate approximate age given start and end date
-- ============================================= 

CREATE FUNCTION dbo.fCalculateAge 
(
	-- Add the parameters for the function here
	@StartDate datetime,
	@EndDate datetime
)
RETURNS varchar(10)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Age int
	DECLARE @DatePart varchar(10)

	if (@StartDate > @EndDate)
		begin
			RETURN null
		end

	if (datediff(month,@StartDate, @EndDate) >= 12)
		begin 
			set @Age = floor(datediff(month,@StartDate, @EndDate)/12)
			set @DatePart = ' year'
		end
	else if (datediff(day,@StartDate, @EndDate) >= 31)
		begin 
			set @Age = datediff(month,@StartDate, @EndDate)
			set @DatePart = 'month'
		end
	else if (datediff(hour,@StartDate, @EndDate) >= 24)
		begin
			set @Age = floor(datediff(hour,@StartDate, @EndDate)/24)
			set @DatePart = 'day'
		end
	else if (datediff(minute,@StartDate, @EndDate) >= 60)
		begin
			set @Age = floor(datediff(minute,@StartDate, @EndDate)/60)
			set @DatePart = 'hour'
		end
	else if (datediff(second,@StartDate, @EndDate) >= 60)
		begin 
			set @Age = floor(datediff(second,@StartDate, @EndDate)/60)
			set @DatePart = 'minute'
		end
	else
		begin
			set @Age = datediff(second,@StartDate, @EndDate)
			set @DatePart = 'second'
		end

	-- Return the result of the function
	RETURN cast(@Age as varchar) + ' ' + @DatePart + case when @Age <> 1 then 's' else '' end

END
GO

GRANT EXECUTE ON dbo.fCalculateAge TO AppRole