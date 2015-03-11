@echo off
CALL "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\vcvarsall.bat"
echo Coping DLL into BISD (x86) directory
copy "C:\Users\Alberto Geniola\Documents\visual studio 2013\Projects\JSONSource\JSONSource\bin\Debug\JSONSource.dll" "C:\Program Files (x86)\Microsoft SQL Server\120\DTS\PipelineComponents"
echo Coping DLL into BISD (x64) directory
copy "C:\Users\Alberto Geniola\Documents\visual studio 2013\Projects\JSONSource\JSONSource\bin\Debug\JSONSource.dll" "C:\Program Files\Microsoft SQL Server\120\DTS\PipelineComponents"
echo Unregistering previous dll 
gacutil /uf JSONSource
echo Registering dll into GAC
gacutil /f /i "C:\Users\Alberto Geniola\Documents\visual studio 2013\Projects\JSONSource\JSONSource\bin\debug\JSONSource.dll"