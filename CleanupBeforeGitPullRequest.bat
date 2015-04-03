REM Use this batch to clean up before submitting a GitHub Pull Request

RD /s /q ".\_ReSharper.ProUpgradeEditor"

FOR /d /r . %%d IN (bin,obj) DO @if exist "%%d" RD /s /q "%%d"

pause

