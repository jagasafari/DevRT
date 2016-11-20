module NUnitRunner

open System
open Common.FileUtil
open DevRT
open ProcessRunner
open ProcessStartInfoProvider
open System.IO

let killProcess sleep (p:System.Diagnostics.Process) = p.Kill(); sleep()

let getProcesses name () = System.Diagnostics.Process.GetProcessesByName name

let sleep (miliseconds:int) () = System.Threading.Thread.Sleep miliseconds

let stopNunitProcess getProcesses killProcess previousStepCompleted =
    if previousStepCompleted then getProcesses() |> Seq.iter killProcess
    previousStepCompleted
 
let cleanDeploymentDir deploymentDir runNUnit =
    if runNUnit then
       deleteAllFiles deploymentDir
       Directory.CreateDirectory(deploymentDir) |> ignore
    runNUnit

let getDebugDirectories solutionDirectory runNUnit = 
    match runNUnit with
    | true ->
        let dirs = Directory.GetDirectories(solutionDirectory, @"Debug", SearchOption.AllDirectories)
        dirs |> Seq.filter( fun d -> d.Contains("Tests") && d.Contains("bin")) |> Some
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

let wl (d:string) = 
    //if d.Contains("Overall") || d.Contains("Test Count") || d.Contains("Failed :") then
        Console.WriteLine(d)

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

        
