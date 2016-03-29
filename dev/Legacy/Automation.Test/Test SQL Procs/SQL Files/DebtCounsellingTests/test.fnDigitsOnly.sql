use [2AM]
go
if (object_id('test.fnDigitsOnly ') is not null)
	begin
		drop function test.fnDigitsOnly 
		print 'Dropped Function: test.fnDigitsOnly '
	end

go

CREATE FUNCTION test.fnDigitsOnly (@pString VARCHAR(8000))
     
RETURNS VARCHAR(8000) AS
  BEGIN
        DECLARE @CleanString VARCHAR(8000)
         SELECT @CleanString = ISNULL(@CleanString,'')+SUBSTRING(@pString,N,1)
           FROM test.Tally WITH (NOLOCK)
          WHERE N<=LEN(@pString)
            AND SUBSTRING(@pString,N,1) LIKE ('[0-9]')    
 RETURN rtrim(@CleanString)
    END
    