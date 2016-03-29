powershell.exe -Command "& %~dp0\Build\solutionHelpersScript.ps1 -sourceRoot '%~dp0' -type %1" -name %2"  -Verb RunAs
