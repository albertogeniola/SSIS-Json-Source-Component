call "%VS140COMNTOOLS%vsvars32.bat"
MSBuild JSONSource.sln /p:Configuration=Release /p:DTSVERSION=110
MSBuild JSONSource.sln /p:Configuration=Release /p:DTSVERSION=120
MSBuild JSONSource.sln /p:Configuration=Release /p:DTSVERSION=130