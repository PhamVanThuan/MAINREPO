start /B webdriver-manager start

Timeout /t 5 /nobreak >nul

cd Testing\SAHL.Websites.Halo\

protractor myConf.js

webdriver-manager stop

pause