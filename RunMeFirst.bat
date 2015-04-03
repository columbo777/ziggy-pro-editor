cls
@echo off

SET CurrentDate=%date:~-4,4%-%date:~-10,2%-%date:~-7,2%
SET CurrentTIme=%time:~-11,2%%time:~-8,2%%time:~-5,2%
COLOR 0A

echo.
echo. This batch installs standardized PUE-2015-03-26.vssettings to VS2010
echo. ALL pull request and commits to PUE must adhear to these settings
echo.
echo. This batch backups your current VS2010 settings to *.vssettings.bak file
echo.
echo. Read the instructions included in the CONTRIBUTING.md file.
echo.
PAUSE>NUL|SET /P "= Press any key to contiune or Ctrl-C to escape ..."
echo.
echo.
echo f | XCOPY /y "%UserPROFILE%\My Documents\Visual Studio 2010\Settings\CurrentSettings.vssettings" "%UserPROFILE%\My Documents\Visual Studio 2010\Settings\Currentsettings_%CurrentDate%_T%CurrentTime%.vssettings.bak" 

echo.
echo f | XCOPY /y .\PUE-2015-03-26.vssettings "%UserPROFILE%\Documents\Visual Studio 2010\Settings\Currentsettings.vssettings" 


echo.
PAUSE

.\contributing.md >> CON

@echo on

