module DevRT.NUnit

open System
open Common
open DataTypes
open FileUtil
open IOWrapper
open ProcessWrapper
open StringWrapper

let isSummary data = contains "Duration:" data || contains "Test Count:" data
let isTestFailerInfo str = contains ") Failed :" str || contains ") Invalid :" str
let isFailureLineNumber = contains ":wiersz"
let isNoise (str:string) =
    str |> startsWith " "
    || contains "Run Settings" str
    || str |> startsWith "NUnit Console"
    || str |> startsWith "Copyright"
    || str = Environment.NewLine
    || str |> isNullOrWhiteSpace
    || str |> startsWith "Test"
    || str |> startsWith "Results"
    || str |> startsWith "Runtime"
    || str |> startsWith "Errors and Failures"

let getUpdatedStatus currentStatus = function
    | d when isSummary d -> Summary
    | d when isTestFailerInfo d -> Failure
    | d when isFailureLineNumber d -> FailureLineInfo
    | _ when currentStatus = Failure -> currentStatus
    | d when isNoise d -> Noise
    | _ -> Failure

let parseFailureLineNumberInfo text =
    text |> splitByString " w" |> Array.last

let writeFailureLineNumberInfo write text =
    text |> parseFailureLineNumberInfo |> write

let handleOutput handleSummary handleFailure handleFailureLineInfo = function
    | Summary -> handleSummary
    | Invalid | Failure -> handleFailure
    | FailureLineInfo -> handleFailureLineInfo
    | _ -> fun _ -> ()

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
    let update, getStatus = createStatus Noise
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
