set BASE_DIR=%~dp0
..\..\..\Build\NodeJS\node_modules\.bin\karma.cmd start "%BASE_DIR%\specs\karma.conf.js" %* -reporters growler --browsers Chrome

PAUSE 'Press any key to exit...'
