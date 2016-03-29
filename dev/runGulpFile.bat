IF "%1" == "" (
	SET task=default
) ELSE ( 
SET task=%1
)

echo %task% 

gulp %task% --gulpfile .\Build\NodeJS\gulpfile.js