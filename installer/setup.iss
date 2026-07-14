#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif

[Setup]
AppName=LifeOS
AppVersion={#MyAppVersion}
AppPublisher=Kaidoms
DefaultDirName={autopf}\LifeOS
DefaultGroupName=LifeOS
OutputDir=output
OutputBaseFilename=LifeOS-Setup
Compression=lzma
SolidCompression=yes
SetupIconFile=..\src\LifeOS.Desktop\Assets\app.ico

[Files]
Source: "..\publish\win-x64\*"; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
Name: "{group}\LifeOS"; Filename: "{app}\LifeOS.Desktop.exe"; IconFilename: "{app}\LifeOS.Desktop.exe"
Name: "{autodesktop}\LifeOS"; Filename: "{app}\LifeOS.Desktop.exe"; IconFilename: "{app}\LifeOS.Desktop.exe"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional icons:"

[Run]
Filename: "powershell.exe"; Parameters: "-NoProfile -ExecutionPolicy Bypass -Command ""Get-ChildItem -Path '{app}' -Recurse | Unblock-File"""; Flags: runhidden; StatusMsg: "Finishing setup..."
