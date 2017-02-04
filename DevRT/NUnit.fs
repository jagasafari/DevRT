module DevRT.NUnit

open System
open DataTypes
open FileUtil
open IOWrapper
open ProcessWrapper
open StringWrapper

type private NUnitStatus() =
    let mutable status = Noise
    member x.Update newStatus = status <- newStatus
    member x.Status with get() = status

let createStatus() =
    let status = NUnitStatus()
    status.Update, fun () -> status.Status

let isSummary data = contains "Duration:" data || contains "Test Count:" data
let isRunSettings = contains "Run Settings"
let isTestFailerInfo = contains ") Failed :"

let getUpdatedStatus currentStatus = function
    | d when isSummary d -> Summary
    | d when isRunSettings d -> Noise
    | d when isTestFailerInfo d -> Failure
    | _ when currentStatus = Failure -> currentStatus
    | _ -> Noise

let handleOutput handleSummary handleFailure = function
    | Summary -> handleSummary | Failure -> handleFailure | _ -> fun _ -> ()

let getTestDirectoryName outputDir =
    let parts = split '/' outputDir
    parts.[2]

let copyBuildOutput log source target =
    let copyFiles source dest =
        doCopy (dest |> createPath) (source |> getFiles) copyFile
    let copySubdirectories copyAllFiles source dest =
        doCopy (dest |> createPath) (source |> getDirectories) copyAllFiles
    let copyAllFiles' = copyAllFiles createDirectory copyFiles copySubdirectories
    sprintf "coping from %s to %s" source target |> log
    copyAllFiles' source target |> ignore

let handle getUpdatedStatus handleOutput data =
    let update, getStatus = createStatus()
    data |> getUpdatedStatus (getStatus()) |> update
    data |> handleOutput (getStatus())

let prepareAndRunTests
    cleanDirectory getProcessStartInfo getOutputDirectory run = function
    | RunTestsOn (dllsSource, dlls) ->
        let outputDirectory = dllsSource |> getOutputDirectory
        let startInfo dllFile () = getProcessStartInfo dllFile outputDirectory
        stopNunitProcess
               (getProcesses "nunit-agent") (500 |> sleep |> killProcess)
        () |> cleanDirectory
        copyBuildOutput Logging.info dllsSource outputDirectory
        dlls |> sprintf "running tests: %A" |> Logging.info
        dlls
        |> Seq.iter(fun tp ->
                let arguments = combine outputDirectory ("%s.dll" %% tp)
                Logging.info arguments
                arguments
                |> startInfo
                |> run)
    | _ -> ()

let run prepareAndRunTests = function
    | BuildSucceeded -> prepareAndRunTests() | BuildFailed | _   -> ()
