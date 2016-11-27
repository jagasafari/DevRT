$runningProcess = Get-Process -name DevRT.Console -ErrorAction SilentlyContinue
if($runningProcess) { Stop-Process -name DevRT.Console }
Remove-Item c:\run\RtFeedback.Console\* 
Copy-Item c:\DevRT\DevRT.Console\bin\Debug\* c:\run\RtFeedback.Console
c:\run\RtFeedback.Console\DevRT.Console.exe
