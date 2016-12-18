module RunNUnit

open System
open System.Diagnostics
open System.IO
open System.Threading
open Common.FileUtil
open ci
open ProcessRunner
open ProcessStartInfoProvider

let stopNunitProcess runNUnit =
    if runNUnit then
       Process.GetProcessesByName("nunit-agent") 
       |> Seq.iter (fun p -> p.Kill(); Thread.Sleep 500)
    runNUnit
 
let cleanDeploymentDir deploymentDir runNUnit =
    if runNUnit then
       deleteAllFiles deploymentDir
       Directory.CreateDirectory(deploymentDir) |> ignore
    runNUnit

let getDebugDirectories solutionDirectory runNUnit = 
    match runNUnit with
    | true ->
        let dirs = Directory.GetDirectories(solutionDirectory, @"Debug", SearchOption.AllDirectories)
        dirs |> Seq.filter( fun d -> d.Contains("bin")) |> Some
    | false -> None 

let copyBuildOutput deploymentDir debugDirectories =
    debugDirectories |> Option.map (fun ds ->
        let targets =
            ds |> Seq.map(fun (d:string) -> 
                                let tar = d.Split(Path.DirectorySeparatorChar)
                                let ta = tar.[2]
                                d, Path.Combine(deploymentDir, ta), ta)
        targets |> Seq.iter(fun (d, t, _) -> copyAllFiles d t)
        targets |> Seq.map(fun (_, t, ta) -> t, ta))

let wl (d:string) = Console.WriteLine( d)

let runTest nunitConsole dllDirectory dllFile =
    let startInfo = getProcessStartInfo nunitConsole dllFile dllDirectory
    run startInfo wl

let runTests nunitConsole testDllsDirectories =
    match testDllsDirectories with
    | Some td -> 
        td 
        |> Seq.iter (fun (directoryPath, directoryName) ->
            let dllName = sprintf "%s.dll" directoryName
            let dllFile = Path.Combine(directoryPath, dllName)
            runTest nunitConsole directoryPath dllFile) 
    | None -> ()

let runNUnit nunitConsole deploymentDir buildOutputDirectory buildSuccess =
    buildSuccess 
    |> stopNunitProcess 
    |> cleanDeploymentDir deploymentDir 
    |> getDebugDirectories buildOutputDirectory
    |> copyBuildOutput deploymentDir
    |> runTests nunitConsole
        
