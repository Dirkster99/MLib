@ECHO OFF
pushd "%~dp0"
ECHO.
ECHO.
ECHO.
ECHO This script deletes all temporary build files in the .vs folder and the
ECHO BIN and OBJ folders contained in the following projects
ECHO.
ECHO TreeViewDemo
ECHO MDemo
ECHO Components/MLib
ECHO Components/MWindowLib
ECHO Components/MWindowInterfacesLib
ECHO Components/MWindowDialogLib
ECHO Components/BindToMLib
ECHO.
ECHO More_Components/Settings/Settings
ECHO More_Components/Settings/SettingsModel
ECHO.
ECHO More_Components/ServiceLocator
ECHO.
ECHO PDF Binder/PDF_Binder_Setup
ECHO PDF Binder/PDF Binder
ECHO PDF Binder/PDFBinderLib
ECHO PDF Binder/Components/Doc
ECHO PDF Binder/Components/ExplorerLib
ECHO PDF Binder/Components/WatermarkControlsLib/Lib
ECHO PDF Binder/Components/WatermarkControlsLib/Demo
ECHO.
ECHO fs3_Components etc...
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
ECHO Removing vs settings folder with *.sou file
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
RMDIR /S /Q More_Components\ServiceLocator\bin
RMDIR /S /Q More_Components\ServiceLocator\obj

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
ECHO Deleting BIN and OBJ Folders in BindToMLib
ECHO.
RMDIR /S /Q Components\BindToMLib\bin
RMDIR /S /Q Components\BindToMLib\obj

ECHO.
ECHO Deleting BIN and OBJ Folders in Components/Settings/Settings
ECHO.
RMDIR /S /Q More_Components\Settings\Settings\bin
RMDIR /S /Q More_Components\Settings\Settings\obj

ECHO.
ECHO Deleting BIN and OBJ Folders in Components/Settings/SettingsModel
ECHO.
RMDIR /S /Q More_Components\Settings\SettingsModel\bin
RMDIR /S /Q More_Components\Settings\SettingsModel\obj

ECHO.
ECHO Deleting BIN and OBJ Folders in TreeViewDemo
ECHO.
RMDIR /S /Q TreeViewDemo\bin
RMDIR /S /Q TreeViewDemo\obj

ECHO.
ECHO Deleting BIN and OBJ Folders in PDF Binder/PDF_Binder_Setup
ECHO.
RMDIR /S /Q "PDF Binder/PDF_Binder_Setup\bin"
RMDIR /S /Q "PDF Binder/PDF_Binder_Setup\obj"

ECHO.
ECHO Deleting BIN and OBJ Folders in PDF Binder
ECHO.
RMDIR /S /Q "PDF Binder/PDF Binder\bin"
RMDIR /S /Q "PDF Binder/PDF Binder\obj"

ECHO.
ECHO Deleting BIN and OBJ Folders in PDFBinderLib
ECHO.
RMDIR /S /Q "PDF Binder/PDFBinderLib\bin"
RMDIR /S /Q "PDF Binder/PDFBinderLib\obj"

ECHO.
ECHO Deleting BIN and OBJ Folders in PDF Binder
ECHO.
RMDIR /S /Q "PDF Binder\Components\Doc\bin"
RMDIR /S /Q "PDF Binder\Components\Doc\obj"

ECHO.
ECHO Deleting BIN and OBJ Folders in PDF Binder
ECHO.
RMDIR /S /Q "PDF Binder\Components\ExplorerLib\bin"
RMDIR /S /Q "PDF Binder\Components\ExplorerLib\obj"

ECHO.
ECHO Deleting BIN and OBJ Folders in PDF Binder
ECHO.
RMDIR /S /Q "PDF Binder\Components\WatermarkControlsLib\Lib\bin"
RMDIR /S /Q "PDF Binder\Components\WatermarkControlsLib\Lib\obj"

ECHO.
ECHO Deleting BIN and OBJ Folders in PDF Binder
ECHO.
RMDIR /S /Q "PDF Binder\Components\WatermarkControlsLib\Demo\bin"
RMDIR /S /Q "PDF Binder\Components\WatermarkControlsLib\Demo\obj"

PAUSE

:EndOfBatch
