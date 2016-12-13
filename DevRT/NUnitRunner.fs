module NUnitRunner

open System
open Common
open DevRT
open ProcessRunner
open ProcessStartInfoProvider
open System.IO

let killProcess sleep (p:System.Diagnostics.Process) = p.Kill(); sleep()

let getProcesses name () = Diagnostics.Process.GetProcessesByName name

let sleep (miliseconds:int) () = Threading.Thread.Sleep miliseconds

let stopNunitProcess getProcesses killProcess =
    getProcesses() |> Seq.iter killProcess
 
let cleanDeploymentDir deploymentDir =
    deleteAllFiles deploymentDir
    Directory.CreateDirectory(deploymentDir) |> ignore

let copyBuildOutput deploymentDir debugDirectories =
    let targets =
        debugDirectories 
        |> Seq.map(
            fun (d:string) -> 
                    let tar = d.Split(Path.DirectorySeparatorChar)
                    let ta = tar.[2]
                    d, Path.Combine(deploymentDir, ta), ta)
    targets |> Seq.iter(fun (d, t, _) -> copyAllFiles d t)
    targets |> Seq.map(fun (_, t, ta) -> t, ta)

let runTest handleOutput nunitConsole dllDirectory dllFile =
    let startInfo = getProcessStartInfo nunitConsole dllFile dllDirectory
    ProcessRunner.run startInfo handleOutput

let runTests handleOutput nunitConsole testDllsDirectories =
    testDllsDirectories 
    |> Seq.iter (fun (directoryPath, directoryName) ->
        let dllName = sprintf "%s.dll" directoryName
        let dllFile = Path.Combine(directoryPath, dllName)
        runTest handleOutput nunitConsole directoryPath dllFile) 

let run (testProjects: string seq) runnerDir deploymentDir buildDir = function
    | false -> ()
    | true ->
        let outputHandler = NUnitOutputHandler()
        let handle = 
            outputHandler.Handle 
                NUnitOutput.getUpdatedStatus NUnitOutput.filterOutput

        stopNunitProcess 
                (getProcesses "nunit-agent") (killProcess (sleep 500))
        cleanDeploymentDir deploymentDir
        let ru = copyBuildOutput deploymentDir testProjects
        runTests handle runnerDir ru
