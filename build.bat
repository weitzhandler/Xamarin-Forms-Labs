@echo off
SETLOCAL

SET TASK=%1
SET CONFIG=%2
SET VERSION=%3
SET PRERELEASE=%4

IF [%1] == [] SET TASK=BUILD
IF [%2] == [] SET CONFIG=Debug
IF [%3] == [] SET VERSION=2.2.0
IF [%4] == [] SET PRERELEASE=pre01

if [%3] == [] GOTO BuildWithVersion
GOTO BuildWithoutVersion

:BuildWithVersion
powershell.exe -command ".\psake.build.ps1 %TASK% -configuration %CONFIG% -version %VERSION% -preRelease %PRERELEASE%"
GOTO Done

:BuildWithOutVersion
powershell.exe -command ".\psake.build.ps1 %TASK% -configuration %CONFIG%"
GOTO Done

:Done
echo Build Complete
ENDLOCAL