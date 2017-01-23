module DevRT.NUnitRunner

open System
open System.IO
open DataTypes
open FileUtil
open ProcessRunner
open ProcessStartInfoProvider

let killProcess sleep (p:Diagnostics.Process) = p.Kill(); sleep()

let getProcesses name () = Diagnostics.Process.GetProcessesByName name

let sleep (miliseconds:int) () = Threading.Thread.Sleep miliseconds

let stopNunitProcess getProcesses killProcess =
    getProcesses() |> Seq.iter killProcess
 
let cleanDeploymentDir deploymentDir =
    deleteAllFiles deploymentDir
    Directory.CreateDirectory(deploymentDir) |> ignore

let copyBuildOutput deploymentDir (outputDir: string) =
    let tar = outputDir.Split('/')
    let ta = tar.[2]
    let target = Path.Combine(deploymentDir, ta)
    copyAllFiles outputDir target |> ignore
    target

let runTest handleOutput nunitConsole dllDirectory dllFile =
    let startInfo = getProcessStartInfo nunitConsole dllFile dllDirectory
    ProcessRunner.run startInfo handleOutput

let runTests handleOutput nunitConsole outputDirectory testProjects =
    testProjects
    |> Seq.iter ( fun tp ->
        let dllName = sprintf "%s.dll" tp 
        let dllFile = Path.Combine(outputDirectory, dllName)
        runTest handleOutput nunitConsole outputDirectory dllFile )
let run 
    (outputDir, testProjects: string seq) 
    runnerDir 
    deploymentDir 
    buildDir = function
    | false -> ()
    | true ->
        let outputHandler = NUnitOutputHandler()
        let handle = 
            outputHandler.Handle 
                NUnitOutput.getUpdatedStatus NUnitOutput.filterOutput

        stopNunitProcess 
                (getProcesses "nunit-agent") (killProcess (sleep 500))
        cleanDeploymentDir deploymentDir
        let outputDirectory = 
            copyBuildOutput deploymentDir outputDir
        runTests handle runnerDir outputDirectory testProjects
