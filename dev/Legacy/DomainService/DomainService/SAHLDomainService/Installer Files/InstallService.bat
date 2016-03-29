@ECHO OFF
CLS
:GetServiceDrive

ECHO *****************************************************************
ECHO *** Select Drive Letter of the Domain Service and press Enter ***
ECHO *****************************************************************
ECHO C. Drive
ECHO D. Drive
ECHO.
ECHO Q. Quit
set DRIVE=
set /P DRIVE=Select : %=%
IF NOT '%DRIVE%'=='' SET DRIVE=%DRIVE:~0,1%
ECHO.
:: /I makes the IF comparison case-insensitive
IF /I '%DRIVE%'=='C' GOTO Environment
IF /I '%DRIVE%'=='D' GOTO Environment
IF /I '%DRIVE%'=='Q' GOTO End
ECHO "%DRIVE%" is not valid. Please try again.
ECHO.
GOTO GetServiceDrive

:Environment
ECHO ***************************************************************
ECHO *** Select Domain Service setup Environment and press Enter ***
ECHO ***************************************************************
ECHO D. Dev
ECHO A. Sys Test A
ECHO S. Sys Test S
ECHO U. UAT
ECHO P. PRODUCTION
ECHO.
ECHO Q. Quit
:: SET /P prompts for input and sets the variable
:: to whatever the user types
SET Choice=
SET /P Choice=Select : 
:: The syntax in the next line extracts the substring
:: starting at 0 (the beginning) and 1 character long
IF NOT '%Choice%'=='' SET Choice=%Choice:~0,1%
ECHO.
:: /I makes the IF comparison case-insensitive
IF /I '%Choice%'=='D' GOTO Development
IF /I '%Choice%'=='A' GOTO SystemTestA
IF /I '%Choice%'=='S' GOTO SystemTestS
IF /I '%Choice%'=='U' GOTO UAT
IF /I '%Choice%'=='P' GOTO Production
IF /I '%Choice%'=='Q' GOTO End
ECHO "%Choice%" is not valid. Please try again.
ECHO.
GOTO Environment

:Development
ECHO Setting up the DEV config file.
SET EXTENSION=DEV
GOTO GetMachineArchitecture

:SystemTestA
ECHO Setting up the SYSTEM TEST A config file.
SET EXTENSION=203A
GOTO GetMachineArchitecture

:SystemTestS
ECHO Setting up the SYSTEM TEST S config file.
SET EXTENSION=203S
GOTO GetMachineArchitecture

:UAT
ECHO Setting up the UAT config file.
SET EXTENSION=UAT
GOTO GetMachineArchitecture

:Production
ECHO Setting up the PRODUCTION config file.
SET EXTENSION=PROD
GOTO GetMachineArchitecture

:GetMachineArchitecture

ECHO *********************************************
ECHO *** Select 32 or 64 bit and press Enter   ***
ECHO *********************************************
ECHO 32. bit
ECHO 64. bit
ECHO.
ECHO Q. Quit
set MACHINE=
set /P MACHINE=Select : %=%
IF NOT '%MACHINE%'=='' SET MACHINE=%MACHINE:~0,2%
ECHO.
:: /I makes the IF comparison case-insensitive
IF /I '%MACHINE%'=='32' GOTO 32bit
IF /I '%MACHINE%'=='64' GOTO 64bit
IF /I '%MACHINE%'=='Q' GOTO End
ECHO "%MACHINE%" is not valid. Please try again.
ECHO.
GOTO GetMachineArchitecture

:32bit
ECHO Setting up the program files folder for 32bit.
SET FOLDER=Program Files
GOTO ConfigFileCopy

:64bit
ECHO Setting up the program files folder for 64bit.
SET FOLDER=Program Files (x86)
GOTO ConfigFileCopy

:ConfigFileCopy
copy "%DRIVE%:\%FOLDER%\SA Home Loans\SAHL Domain Service\SAHLDomainService.exe.config.%EXTENSION%" "%DRIVE%:\%FOLDER%\SA Home Loans\SAHL Domain Service\SAHLDomainService.exe.config" /Y
GOTO ServiceInstall

:ServiceInstall
ECHO UnInstall the Service if it exists
sc delete "SAHLDomainService"
ECHO Install the Service.
sc create "SAHLDomainService" binpath= "%DRIVE%:\%FOLDER%\SA Home Loans\SAHL Domain Service\SAHLDomainService.exe" start= auto displayname= "X2 SAHL Domain Service"
sc description "SAHLDomainService" "SAHL Domain Service"
ECHO ***********************************************
ECHO *** SAHL Domain Service has been Installed. ***
ECHO ***********************************************
PAUSE

:End

