@REM
@REM For quick compilation without VS (%WINDIR%\Microsoft.Net\Framework\v*\csc.exe won't work [for average user], as it supports C#5 only)
@REM
@REM https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
@REM nuget.exe install Microsoft.Net.Compilers -ExcludeVersion
@REM
csc.exe Properties\AssemblyInfo.cs Properties\Resources.Designer.cs Properties\Settings.Designer.cs scripting\interpreter.cs frmMain.cs frmMain.Designer.cs ini.cs Program.cs -out:fo2-config.exe
