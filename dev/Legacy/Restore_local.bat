@ECHO OFF

SET SERVER=DEV_03
SET USER=sa
SET PASSWORD=W0rdpass
SET SCRIPTS_PATH=I:\Development\Scripts\Data Restore\FrontEndDeveloperRestore
SET RESTORE_PATH=E:\MSSQL\DATA
SET TOOLS_PATH=I:\SAHL.Tools\Binaries
SET BACKUP_PATH=\\sahlm02\Backup\Daily
SET CONNECTION_STRING=Data Source=%SERVER%; Initial Catalog=master; User Id=%USER%; Password=%PASSWORD%;

@ECHO ON

ECHO %TIME%

cd /D %TOOLS_PATH%

ECHO "Start Generate Post Restore Script"
"SAHL.Tools.Restorenator.exe" -c "%CONNECTION_STRING%" -d "%SCRIPTS_PATH%" -r "2am|%RESTORE_PATH%;e-work|%RESTORE_PATH%;ImageIndex|%RESTORE_PATH%;MetroSQL|%RESTORE_PATH%;Process|%RESTORE_PATH%;SAHLDB|%RESTORE_PATH%;Staging|%RESTORE_PATH%;UIPState|%RESTORE_PATH%;Warehouse|%RESTORE_PATH%;Warehouse_Archive|%RESTORE_PATH%;X2|%RESTORE_PATH%;" -p "%BACKUP_PATH%"
ECHO "Complete Generate Post Restore Script"

ECHO "Start Restore Databases"
"SAHL.Tools.Scriptenator.CommandLine.exe" -c "%CONNECTION_STRING%" -d "%SCRIPTS_PATH%" -f "Restore_Databases.xml"
ECHO "Complete Restore Databases"

TIMEOUT 10
sc \\%SERVER% stop SQLSERVERAGENT
TIMEOUT 10
sc \\%SERVER% stop MSSQLSERVER
TIMEOUT 120
sc \\%SERVER% start SQLSERVERAGENT
TIMEOUT 10
sc \\%SERVER% start MSSQLSERVER
TIMEOUT 120

ECHO "Start Restore Databases"
"SAHL.Tools.Scriptenator.CommandLine.exe" -c "%CONNECTION_STRING%" -d "%SCRIPTS_PATH%" -f "Post_Restore_Scripts.xml"
ECHO "Complete Restore Databases"

TIMEOUT 10
sc \\%SERVER% stop SQLSERVERAGENT
TIMEOUT 10
sc \\%SERVER% stop MSSQLSERVER
TIMEOUT 120
sc \\%SERVER% start SQLSERVERAGENT
TIMEOUT 10
sc \\%SERVER% start MSSQLSERVER
TIMEOUT 120