SET SHFB_EXE="C:\Program Files\EWSoftware\Sandcastle Help File Builder\SandcastleBuilderConsole.exe"

%SHFB_EXE% "SAHL.Docs.Framework.shfb"
if not %errorlevel%==0 goto :error

REM copy "D:\Builds\Source\Documentation\All\SAHL.Docs.Framework.chm" "D:\Builds\Source\Documentation\"
REM del "D:\Builds\Source\Documentation\All\*.*" /S /Y
REM del "D:\Builds\Source\Documentation\All\LastBuild.log"
REM del "D:\Inetpub\wwwroot\Documentation\*.*" /S /Y
REM xcopy "D:\Builds\Source\Documentation\All\*.*" "D:\Inetpub\wwwroot\Documentation\" /S /Y
REM rmdir /S /Q "D:\Builds\Source\Documentation\All"
REM if not %errorlevel%==0 goto :error

%SHFB_EXE% "SAHL.Docs.BusinessModels.shfb"
if not %errorlevel%==0 goto :error
exit

%SHFB_EXE% "SAHL.Docs.Application.shfb"
if not %errorlevel%==0 goto :error
exit

:error
exit /B -1