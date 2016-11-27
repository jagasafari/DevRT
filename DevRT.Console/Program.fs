open System.Threading
open ci
open ExportApi
open System

let msBuild() = @"c:\Program Files (x86)\MSBuild\14.0\Bin\MsBuild.exe"

let deploymentDir = @"c:\run\nunit"

let buildOutputDirectory = @"c:\DevRT\"

let nunitConsole() = @"C:\Program Files (x86)\NUnit.org\nunit-console\nunit3-console.exe"

let msBuildWorkingDirectory = @"c:\DevRT"

[<EntryPoint>]
let main argv =
    let pathsForTestRunner =
        {
            Runner = nunitConsole 
            DeploymentDir = deploymentDir
            BuildDirectory = buildOutputDirectory
        }

    while true do
        runMsBuild msBuild msBuildWorkingDirectory |> runNUnit pathsForTestRunner |> ignore
        Thread.Sleep 1000
    Console.ReadKey() |> ignore
    0
