echo off

REM get the version number of the deploy - if nothing entered then just exit out
set VERSION=
set /P VERSION=Enter the version number of the deploy: %=%
if "%VERSION%"=="" goto noinput
echo Your input was: %VERSION%

REM set up the share to sahls216b
ECHO Adding SAHLS216b share
net use W: \\sahls216b\D$

SET LOCALDIR=E:

REM copy all the Scripts an Scriptenator file
rem ECHO Copying SQL Scripts to backup
rem xcopy "%LOCALDIR%\Development\Scripts\Development\Releases\%VERSION%\*.sql*" "W:\Inetpub\wwwroot\halo\backupforrollback\%VERSION%\Scripts" /E /Q /Y
rem xcopy "%LOCALDIR%\Development\Scripts\Development\Releases\%VERSION%\*.xml*" "W:\Inetpub\wwwroot\halo\backupforrollback\%VERSION%\Scripts" /E /Q /Y

ECHO Copying web to halo
xcopy "%LOCALDIR%\Deploy\*.*" "W:\Inetpub\wwwroot\halo" /E /Q /Y

ECHO Copying web to halodev
xcopy "%LOCALDIR%\Deploy\*.*" "W:\Inetpub\wwwroot\halodev" /E /Q /Y

ECHO Copying config files to halo
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\UIPConfigSchema.xsd" "W:\Inetpub\wwwroot\halo\Config\UIPConfigSchema.xsd" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\ConnectionStrings.config.stest" "W:\Inetpub\wwwroot\halo\Config\ConnectionStrings.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\Logging.config.stest" "W:\Inetpub\wwwroot\halo\Config\Logging.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\SAHL.Common.Service.config.stest" "W:\Inetpub\wwwroot\halo\Config\SAHL.Common.Service.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\SAHL.Common.WebServices.config.stest" "W:\Inetpub\wwwroot\halo\Config\SAHL.Common.WebServices.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\SAHL.Common.X2.config.stest" "W:\Inetpub\wwwroot\halo\Config\SAHL.Common.X2.config" /Y

ECHO Copying config files to halodev
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\UIPConfigSchema.xsd" "W:\Inetpub\wwwroot\halodev\Config\UIPConfigSchema.xsd" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\ConnectionStrings.config.dev" "W:\Inetpub\wwwroot\halodev\Config\ConnectionStrings.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\Logging.config.dev" "W:\Inetpub\wwwroot\halodev\Config\Logging.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\SAHL.Common.Service.config.dev" "W:\Inetpub\wwwroot\halodev\Config\SAHL.Common.Service.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\SAHL.Common.WebServices.config.dev" "W:\Inetpub\wwwroot\halodev\Config\SAHL.Common.WebServices.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\SAHL.Common.X2.config.dev" "W:\Inetpub\wwwroot\halodev\Config\SAHL.Common.X2.config" /Y

ECHO Backing up halo
SET BACKUPFOLDER="W:\Inetpub\wwwroot\halo\backupforrollback\%VERSION%"
xcopy "%LOCALDIR%\Deploy\*.*" "%BACKUPFOLDER%\Web" /E /Q /Y
xcopy "W:\Inetpub\wwwroot\halo\Config\*.*" "%BACKUPFOLDER%\Web\Config" /E /Q /Y

ECHO Creating Folders for Release Build
SET TARGETDIR=%LOCALDIR%\Temp\HaloRelease
SET SOURCEDIR=%LOCALDIR%\Development\SourceCode\Halo
SET FRAMEDIR=%SOURCEDIR%\Framework
SET APPDIR=%TARGETDIR%\Application
mkdir %APPDIR%\SAHL.Web
mkdir "%TARGETDIR%\Internal Binaries"
mkdir "%TARGETDIR%\External Binaries"

ECHO Copying Source for Release Build (this may take a while)
copy "%SOURCEDIR%\Application\Application.sln" %APPDIR% /Y
xcopy "%SOURCEDIR%\Application\SAHL.Web" %APPDIR%\SAHL.Web /E /Q /Y 
xcopy "%SOURCEDIR%\External Binaries" "%TARGETDIR%\External Binaries" /Y /E /Q

ECHO Building Framework (Release Version)
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe "%FRAMEDIR%\Framework.sln" /noconsolelogger /target:Clean /v:quiet /p:Configuration=Release
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe "%FRAMEDIR%\Framework.sln" /noconsolelogger /v:quiet /p:Configuration=Release
copy "%FRAMEDIR%\SAHL.Test\bin\Release\SAHL.*.*" "%TARGETDIR%\Internal Binaries" /Y

ECHO Building HALO (Release Version)
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe "%APPDIR%\Application.sln" /noconsolelogger /target:Clean /v:quiet /p:Configuration=Release
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe "%APPDIR%\SAHL.Web\SAHL.Web.csproj" /noconsolelogger /v:quiet /t:Build /p:Configuration=Release
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe %APPDIR%\SAHL.Web\SAHL.Web.csproj /noconsolelogger /v:quiet /target:ResolveAssemblyReferences,_CopyWebApplication /p:OutDir=%TARGETDIR%\ /p:WebOutputDir=%TARGETDIR%\ /p:Configuration=Release
ren "%TARGETDIR%\_PublishedWebsites" Output
xcopy "%TARGETDIR%\Internal Binaries\*.dll" "%TARGETDIR%\Output\SAHL.Web\bin\" /D /E /Q
xcopy "%TARGETDIR%\External Binaries\*.dll" "%TARGETDIR%\Output\SAHL.Web\bin\" /D /E /Q

ECHO Copying Release version to backup folder
rd %BACKUPFOLDER%\Web.Release.Test /S /Q
mkdir "%BACKUPFOLDER%\Web.Release\Config"
xcopy "%TARGETDIR%\Output\SAHL.Web" "%BACKUPFOLDER%\Web.Release" /D /E /Q

ECHO Cleaning up Release Folders
rd /S /Q %TARGETDIR%


REM we're done - get out of here
goto end

REM code block for when no version number is entered
:noinput
ECHO No version number entered.

REM clean-up - this should always be called
:end
ECHO Removing SAHLS216b share
net use /D W:

PAUSE
