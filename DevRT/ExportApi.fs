module DevRT.ExportApi

open System
open Common
open ConsoleOutput
open DevRT.DataTypes
open FileUtil
open IOWrapper
open ProcessRunner
open MsBuild
open Refactor
open StringWrapper

let createAgent' handle =
    let logError e = e |> printfn "%A"; e |> Logging.error
    Agent.createAgent logError handle

let composeNUnitHandle (nUnitConfig: NUnitConfig) =
    let handleOutput' =
        NUnit.handleOutput
            writelineDarkCyan
            writelineYellow
            (NUnit.writeFailureLineNumberInfo writelineYellow)
    let handle' = NUnit.handle NUnit.getUpdatedStatus handleOutput'
    let deleteAllFiles' = deleteAllFiles exists deleteRecursive
    let cleanDirectory'() =
        cleanDirectory deleteAllFiles' createDirectory nUnitConfig.DeploymentDir
    let getProcessStartInfo' = getProcessStartInfo nUnitConfig.NUnitConsole
    let getOutputDirectory dllsSource =
            let outputDirectory =
                dllsSource
                |> NUnit.getTestDirectoryName
                |> combine nUnitConfig.DeploymentDir
            outputDirectory
    let run' startInfo = ProcessRunner.run startInfo handle'
    let prepareAndRunTests() =
        NUnit.prepareAndRunTests
            cleanDirectory'
            getProcessStartInfo'
            getOutputDirectory
            run'
            nUnitConfig.TestProjects
    NUnit.run prepareAndRunTests

let runMsBuild' (msBuildConfig: MsBuildConfig) post () =
    let getMsBuildStartInfo() =
        getProcessStartInfo
            msBuildConfig.MsBuildPath
            (getRunArgsString msBuildConfig.SolutionFile)
            msBuildConfig.MsBuildWorkingDir
    let update, getStatus = createStatus Starting
    let getContinuationStatus' = getStatus >> getContinuationStatus
    let updateStatus = (getUpdatedStatus contains getContinuationStatus') >> update
    let handleStarting' = handleStarting writelineDarkGreen getNow updateStatus
    let processOutput' =
        processOutput
            handleStarting' writelinePurple writelineRed updateStatus
    let run' = run getMsBuildStartInfo
    runMSBuild processOutput' run' getStatus post

let runFileWatch (config: FileWatchConfig) =
    let getFiles() =
         config.MsBuildWorkingDir
         |> FileWatchAgent.getFiles
             (config.ExcludedDirectories |> FileWatchAgent.isBaseCase)
    let getTimeLine' () =
        FileWatchAgent.getTimeLine getNow config.SleepMilliseconds
    let isNewModification' =
        FileWatchAgent.isNewModification getTimeLine' FileWatchAgent.getLastWriteTime
    FileWatchAgent.handle getFiles isNewModification'

let getPostToFileWatchAgent fileWatchConfig nUnitConfig msBuildConfig =
    let nUnitHandle = composeNUnitHandle nUnitConfig
    let nUnitAgent = createAgent' nUnitHandle ()
    let msBuildHandle = runMsBuild' msBuildConfig nUnitAgent.Post
    let msBuildAgent = createAgent' msBuildHandle ()
    let refactorAgent = createAgent' getRefactorHandle ()
    let fileWatchHandle =
        runFileWatch fileWatchConfig msBuildAgent.Post refactorAgent.Post
    let fileWatchAgent = createAgent' fileWatchHandle ()
    let run () =
        Run.run fileWatchConfig.SleepMilliseconds fileWatchAgent.Post refactorAgent.Post
        |> Seq.toList
    run
