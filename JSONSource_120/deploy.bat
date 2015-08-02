@echo off
CALL "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\vcvarsall.bat"
echo Coping DLL into BISD (x86) directory
copy "C:\Users\Alberto Geniola\Dropbox\Area Personale\Progetti\JSONSource\JSONSource_Devel\JSONSource_120\bin\Debug\com.webkingsoft.JSONSource_120.dll" "C:\Program Files (x86)\Microsoft SQL Server\120\DTS\PipelineComponents"
echo Coping DLL into BISD (x64) directory
copy "C:\Users\Alberto Geniola\Dropbox\Area Personale\Progetti\JSONSource\JSONSource_Devel\JSONSource_120\bin\Debug\com.webkingsoft.JSONSource_120.dll" "C:\Program Files\Microsoft SQL Server\120\DTS\PipelineComponents"
echo Unregistering previous dll 
gacutil /uf Newtonsoft.json
gacutil /uf JSONSource
echo Registering dll into GAC
gacutil /f /i "C:\Users\Alberto Geniola\Dropbox\Area Personale\Progetti\JSONSource\JSONSource_Devel\JSONSource_120\bin\Debug\Newtonsoft.Json.dll"
gacutil /f /i "C:\Users\Alberto Geniola\Dropbox\Area Personale\Progetti\JSONSource\JSONSource_Devel\JSONSource_120\bin\Debug\com.webkingsoft.JSONSource_120.dll"
pause