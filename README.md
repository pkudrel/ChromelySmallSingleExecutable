## Chromely Small Single Executable
This example shows how to build a single executable, small (less than 200KB),  Chromely app - https://github.com/mattkol/Chromely 

It is based on: 
https://github.com/mattkol/Chromely/wiki/Getting-Started-CefSharp-Winapi

### How it works

At the beginning, the application, checks if there is a "CefSharp" environment (package) in the `"%LOCALAPPDATA%\CefSharp\packages"` directory. If the package does not exist, the application will create it (by downloading files from the Nuget server ). The "CefSharp" package contains files from two nuget packages: `CefSharp.Common` and `cef.redist.x64`. 
If the "CefSharp" package is in place (at `"%LOCALAPPDATA%\CefSharp\packages\cefsharp_67.0.0_x64"`), the application will use it.

### Steps to run the application
- `.\build.ps1`
- `.\_artifacts\simple-chromely.exe`

### About build process
It uses 
- http://nuke.build/ (build automation system)
- https://github.com/MiloszKrajewski/LibZ (creates  a single file with assemblies embedded into it)
