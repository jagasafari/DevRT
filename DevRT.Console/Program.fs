﻿module DevRT.Console

let config = {
    MsBuildPath =  @"c:\Program Files (x86)\MSBuild\14.0\Bin\MsBuild.exe"
    DeploymentDir =  @"c:\run\nunit"
    NUnitConsole = 
        @"C:\Program Files (x86)\NUnit.org\nunit-console\nunit3-console.exe"
    MsBuildWorkingDir =  @"c:\DevRT"
    FileChangeWithinLastSeconds = 3
    }

[<EntryPoint>]
let main argv =
    let post = ExportApi.getPostToFileWatchAgent config
    while true do
        () |> post
        System.Threading.Thread.Sleep 1000
    System.Console.ReadKey() |> ignore
    0
