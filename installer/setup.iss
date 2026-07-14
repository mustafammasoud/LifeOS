[Setup]
AppName=LifeOS
AppVersion=1.0.0
AppPublisher=Kaidoms
DefaultDirName={autopf}\LifeOS
DefaultGroupName=LifeOS
OutputDir=installer\output
OutputBaseFilename=LifeOS-Setup
Compression=lzma
SolidCompression=yes
SetupIconFile=src\LifeOS.Desktop\Assets\icon.ico

[Files]
Source: "publish\win-x64\*"; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
Name: "{group}\LifeOS"; Filename: "{app}\LifeOS.Desktop.exe"
Name: "{autodesktop}\LifeOS"; Filename: "{app}\LifeOS.Desktop.exe"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional icons:"
