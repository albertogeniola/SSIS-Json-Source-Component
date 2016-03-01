# Manual Installation
This process isn't that complex, but requires some attention. The entire installation process is composed by the following steps:
 * Check Requirements
 * Download the DLL Pack
 * Copy DLL into MS SQL DTS path
 * Register DLLs into the GAC

 We will go through each one in order to make the installation process as smooth as possible.
 
 
### Preliminary step: requirements and download.
At the time of writing, JSONSource only supports MS SQL Server 2012 and MS SQL Server 2014, with .NET 4.0 framework. If you are using a different MS SQL version, or a different .NET framework, this component will not show up into the Visual Studio SSIS ToolBox. 
Beside, for the further steps of the installation, it is crucial to understand which version of MSSQL Server are you going to use. If you are not sure about which version of MS SQL Server is installed on your system, please follow [this guide](https://support.microsoft.com/en-us/kb/321185)

Once you know your MS SQL Server version, you can download the DLL Pack. As you will see, it contains different folders. Each subforlder contains the required DLL to be installed in order to enable the JSONSource component on different MS SQL Server versions. This means that you need to pick one sub folder and install the contained DLL, as explained later. 
The mapping between the MSSQL Version and the MS SQL Name is shown in the following table.

| MS Sql Name           | MS Sql Version | DLL Pack folder | Default DTS Installation Path                                          |
|-----------------------|----------------|-----------------|------------------------------------------------------------------------|
| SQL Server 2014 (x32) | 12.X           | 120             | C:\Program Files (x86)\Microsoft SQL Server\120\DTS\PipelineComponents |
| SQL Server 2014 (x64) | 12.X           | 120             | C:\Program Files\Microsoft SQL Server\120\DTS\PipelineComponents       |
| SQL Server 2012 (x32) | 11.X           | 110             | C:\Program Files (x86)\Microsoft SQL Server\110\DTS\PipelineComponents |
| SQL Server 2012 (x64) | 11.X           | 110             | C:\Program Files\Microsoft SQL Server\110\DTS\PipelineComponents       |


### Copying the DLL.
In order to make JSONSOurce to work you need to copy all the DLL of the selected MS SQL Version into its appropriate installation folder. The table above shows the default directory where you should dopy the DLLs for each supported MS SQL Version. For the sake of clarity, we'll make an example. Let the DLL pack reside on the desktop. Let's assume we have MS SQL Server 2014 both in x32 and x64 versions. By looking at the table we realize we need the DLLs contained into the folder "120". So, we copy both the DLLs contained into the 120 source folder into both SQL Server 2014 x32 and x64 DTS paths, which means:

```batch
copy ".../desktop/120/*.dll" "C:\Program Files (x86)\Microsoft SQL Server\120\DTS\PipelineComponents"
copy ".../desktop/120/*.dll" "C:\Program Files\Microsoft SQL Server\120\DTS\PipelineComponents"
```

If at the installation time you customized the installation path of the MS SQL Server, you need to copy the DLL accordingly to that location. You can rely on the windows registry in order to spot where this path is. For more info [look here]( https://msdn.microsoft.com/en-us/library/ms143547.aspx).

### Registering the DLLs into the GAC.
The last step is to register the DLLs we have just copied into the Global Assembly Cache. To do so, we can either use the GACUTIL.EXE command line tool (shipped with Visual Studio) or use some GAC utilities, such as [GacManager](https://gacmanager.codeplex.com/). If you are familiar with gacutil and with command lines tools, then use gacutils; otherwise if you prefer a GUI tool, use the GacManager procedure.

#### Procedure with GacUtil.exe
Run the VS2012 command prompt as administrator. If Visual Studio is installed on the system, a commodity shortcut should be present into the start menu (VS2012 x86 Native Tools Command Prompt). Once started as administrator, navigate to the same DLL folder source where you copied the DLL before. In our previous example it was "../desktop/120/". From there, execute the following commands:

```batch
gacutils /f /i Newtonsoft.json.dll
gacutils /f /i com.webkingsoft.JSONSource_120.dll
```

Check the output of every command and make sure the DLL installation go fine. Upon successful command execution, your should read a message similar to 

"Assembly successfully added to the cache"

Congratulations! JSONSource is now installed and ready to be used.


#### Procedure with GacManager
Download and install GacManager utility [from here](https://gacmanager.codeplex.com).
Once installed, start it as administrator. Make sure you start that as administrator, otherwise the program will not work.
Once started, type "json" into the search box in the upper-right corner, so that we make sure there is no previous version installed. If you spot any previous DLL version of JSONSource or Newtonsoft.json, uninstall them.
At this point, you can proceed with the installation: pick and install first the Newtonsoft.json and then com.webkingsoft.JSONSource_XXX.dll, from the folder choosen in the previous step (DLL Copy). Please install each DLL at a time. This tools is nice, but seems to have problem with batch operations.
Once the DLLs are installed, reload the DLL list and perform the search by "json" again. If everything went ok, you should be able to see the new installed DLLs in the list! 

Congratulation! JSONSource is now installed and ready to be used.


### Test
In order to see if the component is working, open visual studio and create a new SSIS project. Create an empty DataFlowTask and look into the SSIS ToolBox if there are those two components:
JSON Filter
JSON Source Component

If so, you are now ready to use this component.