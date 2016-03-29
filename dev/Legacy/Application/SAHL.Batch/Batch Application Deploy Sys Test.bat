echo off

REM get the version number of the deploy - if nothing entered then just exit out
set VERSION=
set /P VERSION=Enter the version number of the deploy: %=%
if "%VERSION%"=="" goto noinput
echo Your input was: %VERSION%

REM set up the share to sahls216b
ECHO Adding SAHLS216b share
net use W: \\sahls216b\D$

REM set up the share to sahls215
ECHO Adding SAHLS215 share
net use V: \\sahls215\D$\SAHL.Batches

SET LOCALDIR=E:

REM create the backup config folders
SET BACKUPVERSIONFOLDER="W:\Inetpub\wwwroot\halo\backupforrollback\%VERSION%"

SET BACKUPFOLDER="%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.dev"
ECHO Creating %BACKUPFOLDER%
mkdir %BACKUPFOLDER%
SET BACKUPFOLDER="%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.stest"
ECHO Creating %BACKUPFOLDER%
mkdir %BACKUPFOLDER%
SET BACKUPFOLDER="%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.uat"
ECHO Creating %BACKUPFOLDER%
mkdir %BACKUPFOLDER%
SET BACKUPFOLDER="%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.prod"
ECHO Creating %BACKUPFOLDER%
mkdir %BACKUPFOLDER%

ECHO Clean BatchBin in Application Folder
DEL "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchBin\*.pdb"
DEL "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchBin\*.xml"
DEL "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchBin\*.config"

ECHO Copying Batch Application Files from BatchBin Folder to System Test Report Server
xcopy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchBin\*.*" "V:\" /E /Q /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\SAHL.Common.Service.config.stest" "V:\SAHL.Common.Service.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapExtract\Config\Batch_CapExtract.exe.config.stest" "V:\Batch_CapExtract.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapImport\Config\Batch_CapImport.exe.config.stest" "V:\Batch_CapImport.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapMailingHouseExtract\Config\Batch_CapMailingHouseExtract.exe.config.stest" "V:\Batch_CapMailingHouseExtract.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_DataReport\Config\Batch_DataReport.exe.config.stest" "V:\Batch_DataReport.exe.config" /Y

ECHO Backing up the Batch Applications
xcopy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchBin\*.*" "%BACKUPVERSIONFOLDER%\SAHL.Batches" /E /Q /Y

REM copy dev config files
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\SAHL.Common.Service.config.dev" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.dev\SAHL.Common.Service.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapExtract\Config\Batch_CapExtract.exe.config.dev" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.dev\Batch_CapExtract.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapImport\Config\Batch_CapImport.exe.config.dev" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.dev\Batch_CapImport.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapMailingHouseExtract\Config\Batch_CapMailingHouseExtract.exe.config.dev" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.dev\Batch_CapMailingHouseExtract.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_DataReport\Config\Batch_DataReport.exe.config.dev" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.dev\Batch_DataReport.exe.config" /Y

REM copy System Test config files
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\SAHL.Common.Service.config.stest" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.stest\SAHL.Common.Service.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapExtract\Config\Batch_CapExtract.exe.config.stest" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.stest\Batch_CapExtract.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapImport\Config\Batch_CapImport.exe.config.stest" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.stest\Batch_CapImport.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapMailingHouseExtract\Config\Batch_CapMailingHouseExtract.exe.config.stest" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.stest\Batch_CapMailingHouseExtract.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_DataReport\Config\Batch_DataReport.exe.config.stest" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.stest\Batch_DataReport.exe.config" /Y

REM copy UAT config files
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\SAHL.Common.Service.config.uat" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.uat\SAHL.Common.Service.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapExtract\Config\Batch_CapExtract.exe.config.uat" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.uat\Batch_CapExtract.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapImport\Config\Batch_CapImport.exe.config.uat" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.uat\Batch_CapImport.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapMailingHouseExtract\Config\Batch_CapMailingHouseExtract.exe.config.uat" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.uat\Batch_CapMailingHouseExtract.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_DataReport\Config\Batch_DataReport.exe.config.uat" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.uat\Batch_DataReport.exe.config" /Y

REM copy Prod config files
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\SAHL.Web\Config\SAHL.Common.Service.config.prod" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.prod\SAHL.Common.Service.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapExtract\Config\Batch_CapExtract.exe.config.prod" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.prod\Batch_CapExtract.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapImport\Config\Batch_CapImport.exe.config.prod" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.prod\Batch_CapImport.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_CapMailingHouseExtract\Config\Batch_CapMailingHouseExtract.exe.config.prod" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.prod\Batch_CapMailingHouseExtract.exe.config" /Y
copy "%LOCALDIR%\Development\SourceCode\Halo\Application\BatchApplications\Batch_DataReport\Config\Batch_DataReport.exe.config.prod" "%BACKUPVERSIONFOLDER%\SAHL.Batches\Config.prod\Batch_DataReport.exe.config" /Y

REM we're done - get out of here
goto end

REM code block for when no version number is entered
:noinput
ECHO No version number entered.

REM clean-up - this should always be called
:end
ECHO Removing SAHLS216b share
net use /D W:

ECHO Removing SAHLS215 share
net use /D V:

PAUSE




