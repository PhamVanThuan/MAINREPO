PAUSE "Press any key to start..."

cd .\Tools\SAHL.Tools\Tool Binaries
"SAHL.Tools.Restorenator.exe" -c "Data Source=SAHLS215A; Initial Catalog=master; User Id=EworkAdmin2; Password=W0rdpass;" -d "D:\scripts\Data Restore\FrontEndDeveloperRestore" -f "Full_Database_Restore.xml" -r "2am|D:\MSSQL\Data;e-work|D:\MSSQL\Data;ImageIndex|D:\MSSQL\Data;MetroSQL|D:\MSSQL\Data;Process|D:\MSSQL\Data;SAHLDB|D:\MSSQL\Data;Staging|D:\MSSQL\Data;UIPState|D:\MSSQL\Data;Warehouse|D:\MSSQL\Data;Warehouse_Archive|D:\MSSQL\Data;X2|D:\MSSQL\Data;DWWarehousePre|E:\MSSQL\Data;DWStaging|E:\MSSQL\Data" -p "\\sahlm02\Backup\Daily"

PAUSE "Press any key to exit..."
