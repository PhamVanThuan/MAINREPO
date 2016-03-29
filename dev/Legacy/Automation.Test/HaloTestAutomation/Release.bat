rmdir "ApplicationCaptureTests\bin" /q /s
rmdir "OriginationTests\bin"  /q /s
rmdir "FurtherLendingTests\bin" /q /s
rmdir "LoanAdjustmentsTests\bin" /q /s
rmdir "LifeTests\bin" /q /s
rmdir "DebtCounselling\bin" /q /s
rmdir "CAP2Tests\bin" /q /s
rmdir "InternetComponentTests\bin" /q /s
rmdir "LoanServicingTests\bin" /q /s
rmdir "PersonalLoansTests\bin" /q /s
rmdir "DisabilityClaimTests\bin" /q /s
rmdir "Common\bin" /q /s
rmdir "BuildingBlocks\bin" /q /s
rmdir "Automation.Framework\bin" /q /s
rmdir "Automation.Workflow\bin" /q /s
rmdir "Automation.DataModels\bin" /q /s

"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" SAHLSTestAutomation.sln /t:Build /p:Configuration=Release /nologo /verbosity:quiet

REM *** Copy all the bins of HaloTestAutomation TestSuites to a "Release" Directory inside the current folder***

rmdir %currentDir%\Release /q /s
md %currentDir%\Release

xcopy /Y "ApplicationCaptureTests\bin\Release\*.*" Release\
xcopy /Y "OriginationTests\bin\Release\*.*" Release\
xcopy /Y "FurtherLendingTests\bin\Release\*.*" Release\
xcopy /Y "LoanAdjustmentsTests\bin\Release\*.*" Release\
xcopy /Y "LifeTests\bin\Release\*.*" Release\
xcopy /Y "DebtCounselling\bin\Release\*.*" Release\
xcopy /Y "CAP2Tests\bin\Release\*.*" Release\
xcopy /Y "External DLLs\Watin\*.*" Release\
xcopy /Y "RunAutomationTests.bat" Release\
xcopy /Y "InternetComponentTests\bin\Release\*.*" Release\
xcopy /Y "LoanServicingTests\bin\Release\*.*" Release\
xcopy /Y "PersonalLoansTests\bin\Release\*.*" Release\
xcopy /Y "DisabilityClaimTests\bin\Release\*.*" Release\
xcopy /Y "Common\bin\Release\*.*" Release\
xcopy /Y "BuildingBlocks\bin\Release\Templates\*.*" Release\Templates\
xcopy /Y "Automation.Framework\bin\Release\*.*" Release\
xcopy /Y "Automation.Workflow\bin\Release\*.*" Release\
xcopy /Y "Automation.DataModels\bin\Release\*.*" Release\

cd..
cd..
REM *** Copying mshtml binary for VMs ***
xcopy /Y "External Binaries\NuGet\WatiN.2.1.0\lib\net40\Microsoft.mshtml.dll" Automation.Test\HaloTestAutomation\Release\

REM *** Successfully copied to current directory release folder***
pause
