$devRTDir = 'c:\DevRT'
$processName = 'DevRT.Console'
$deployPath = 'c:\run\console'
$packagePath = Join-Path $devRTDir 'DevRT.Console\bin\Debug'
$paketPackagePath = Join-Path $devRTDir 'packages'
$sln = 'MyDev.sln'
$runningProcess = Get-Process -name $processName -ErrorAction SilentlyContinue
if($runningProcess) { Stop-Process -name $processName }
if(-Not (Test-Path $paketPackagePath)) { 
    Write-Host "running paket" -foregroundcolor Green
    .\.paket\paket.exe update 
}
MSBuild.exe $sln /t:clean
MSBuild.exe $sln /t:build
if(Test-Path $deployPath) { Remove-Item $deployPath -Recurse }
Copy-Item $packagePath $deployPath -Recurse
Write-Host "running dev real time" -foregroundcolor Green
& (Join-Path $deployPath "$processName.exe")

