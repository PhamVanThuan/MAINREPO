#$nugetRunner = "\Build\Tools\NUnit.Runners.2.6.3\tools\nunit-console.exe"

$inputDll = '".\Legacy\Automation.Test\HaloTestAutomation\Release"\LifeTests.dll'
.\Build\Tools\NUnit.Runners.2.6.3\tools\nunit-console-x86.exe $inputDll /framework:net-4.0