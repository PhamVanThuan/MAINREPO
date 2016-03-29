rem copy ".\bin\SAHL.*.*" "..\Internal Binaries"
rem *** this works cause I cheated and made Test reference everything :) ***
del "..\Internal Binaries\*.*" /q
copy ".\sahl.Test\bin\release\SAHL.*.*" "..\Internal Binaries"