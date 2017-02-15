$processName = 'DevRT.Console'
$deployPath = 'c:\run\console'
$packagePath = 'c:\DevRT\DevRT.Console\bin\Debug'

$runningProcess = Get-Process -name $processName -ErrorAction SilentlyContinue
if($runningProcess) { Stop-Process -name $processName }
MSBuild.exe /t:clean,build
if(Test-Path $deployPath) { Remove-Item $deployPath -Recurse }
Copy-Item $packagePath $deployPath -Recurse
Write-Host "running dev real time" -foregroundcolor Green
& (Join-Path $deployPath "$processName.exe")

