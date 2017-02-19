@ECHO OFF
pushd "%~dp0"
ECHO.
ECHO.
ECHO.
ECHO This script deletes all temporary build files in the .vs folder and the
ECHO BIN and OBJ folders contained in the following projects
ECHO.
ECHO PDF Binder
ECHO Components/MLib
ECHO Components/MWindowLib
ECHO Components/MWindowInterfacesLib
ECHO Components/MWindowDialogLib
ECHO.
ECHO Components/Settings/Settings
ECHO Components/Settings/SettingsModel
ECHO.
ECHO Components/ServiceLocator
ECHO.
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
ECHO Deleting BIN and OBJ Folders in MDemo
ECHO.
RMDIR /S /Q "MDemo\bin"
RMDIR /S /Q "MDemo\obj"

ECHO.
ECHO Deleting BIN and OBJ Folders in Components/ServiceLocator
ECHO.
RMDIR /S /Q Components\ServiceLocator\bin
RMDIR /S /Q Components\ServiceLocator\obj

ECHO.
ECHO Deleting BIN and OBJ Folders in Components/MLib
ECHO.
RMDIR /S /Q Components\MLib\bin
RMDIR /S /Q Components\MLib\obj

ECHO.
ECHO Deleting BIN and OBJ Folders in MWindowLib
ECHO.
RMDIR /S /Q Components\MWindowLib\bin
RMDIR /S /Q Components\MWindowLib\obj

ECHO.
ECHO Deleting BIN and OBJ Folders in MWindowInterfacesLib
ECHO.
RMDIR /S /Q Components\MWindowInterfacesLib\bin
RMDIR /S /Q Components\MWindowInterfacesLib\obj

ECHO.
ECHO Deleting BIN and OBJ Folders in MWindowDialogLib
ECHO.
RMDIR /S /Q Components\MWindowDialogLib\bin
RMDIR /S /Q Components\MWindowDialogLib\obj

ECHO.
ECHO Deleting BIN and OBJ Folders in Components/Settings/Settings
ECHO.
RMDIR /S /Q Components\Settings\Settings\bin
RMDIR /S /Q Components\Settings\Settings\obj

ECHO.
ECHO Deleting BIN and OBJ Folders in Components/Settings/SettingsModel
ECHO.
RMDIR /S /Q Components\Settings\SettingsModel\bin
RMDIR /S /Q Components\Settings\SettingsModel\obj

PAUSE

:EndOfBatch
