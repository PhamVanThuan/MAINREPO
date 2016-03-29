set BASE_DIR=%~dp0

cd ..\..\..\Build\NodeJS
echo %BASE_DIR%
grunt.cmd --gruntfile "%BASE_DIR%GruntFile.js" --base .\ --verbose

PAUSE 'Press any key to exit...'