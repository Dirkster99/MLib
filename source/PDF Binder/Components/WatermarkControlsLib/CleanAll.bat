@ECHO OFF
pushd "%~dp0"
ECHO.
ECHO.
ECHO.
ECHO This script deletes all temporary build files in the .vs folder and the
ECHO BIN and OBJ folders contained in the following projects
ECHO.
ECHO Demo
ECHO Lib
ECHO.
REM Ask the user if hes really sure to continue beyond this point XXXXXXXX
set /p choice=Are you sure to continue (Y/N)?
if not '%choice%'=='Y' Goto EndOfBatch
REM Script does not continue unless user types 'Y' in upper case letter
ECHO.
ECHO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
ECHO.
ECHO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
ECHO.
ECHO Removing .vs folder
ECHO.
RMDIR /S /Q .vs

ECHO.
ECHO Deleting BIN and OBJ Folders in Demo
ECHO.
RMDIR /S /Q "Demo\bin"
RMDIR /S /Q "Demo\obj"

ECHO.
ECHO Deleting BIN and OBJ Folders in Lib
ECHO.
RMDIR /S /Q "Lib\bin"
RMDIR /S /Q "Lib\obj"

PAUSE

:EndOfBatch
