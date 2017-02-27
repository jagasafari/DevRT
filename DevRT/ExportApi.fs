module DevRT.ExportApi

open System
open Common
open ConsoleOutput
open DevRT.DataTypes
open FileUtil
open IOWrapper
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
    let handle' = NUnit.handleRunning NUnit.getUpdatedStatus handleOutput'
    let deleteAllFiles' = deleteAllFiles exists deleteRecursive
    let cleanDirectory'() =
        cleanDirectory deleteAllFiles' createDirectory nUnitConfig.DeploymentDir
    let getProcessStartInfo' = ProcessRunner.getProcessStartInfo nUnitConfig.NUnitConsole
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

let composeMsBuildHandle (msBuildConfig: MsBuildConfig) post () =
    let args =
        MsBuild.getRunArgsString
            msBuildConfig.OptionArgs msBuildConfig.SolutionOrProjectFile
    let getMsBuildStartInfo() =
        ProcessRunner.getProcessStartInfo
            msBuildConfig.MsBuildPath args msBuildConfig.MsBuildWorkingDir
    let update, getStatus = createStatus Starting
    let getContinuationStatus' = getStatus >> MsBuild.getContinuationStatus
    let updateStatus = (MsBuild.getUpdatedStatus contains getContinuationStatus') >> update
    let handleStarting' = MsBuild.handleStarting writelineDarkGreen getNow updateStatus
    let processOutput' =
        MsBuild.processOutput
            handleStarting' writelinePurple writelineRed updateStatus
    let run' = ProcessRunner.run getMsBuildStartInfo
    MsBuild.handle processOutput' run' getStatus post

let composeFileWatchHandle (config: FileWatchConfig) =
    let isDirectoryExcludedFromWatch =
        (config.ExcludedDirectories |> FileWatchAgent.isBaseCase)
    let getFiles() =
         FileWatchAgent.getFiles isDirectoryExcludedFromWatch config.FileChangeWatchDir
    let getTimeLine' () =
        FileWatchAgent.getTimeLine getNow config.SleepMilliseconds
    let isNewModification' =
        FileWatchAgent.isNewModification getTimeLine' FileWatchAgent.getLastWriteTime
    FileWatchAgent.handle getFiles isNewModification'

let composeRefactorHandle () =
    let refactor' =
        Refactor.refactor
            (Refactor.processFile IOWrapper.readAllLines)
            IOWrapper.writeAllLines
            IOWrapper.fileExists
    Refactor.handle refactor' (Collections.Generic.HashSet<string>())

let getPostToFileWatchAgent fileWatchConfig nUnitConfig msBuildConfig =
    let nUnitHandle = composeNUnitHandle nUnitConfig
    let nUnitAgent = createAgent' nUnitHandle
    let msBuildHandle = composeMsBuildHandle msBuildConfig nUnitAgent.Post
    let msBuildAgent = createAgent' msBuildHandle
    let refactorAgent = createAgent' (composeRefactorHandle())
    let fileWatchHandle =
        composeFileWatchHandle fileWatchConfig msBuildAgent.Post refactorAgent.Post
    let fileWatchAgent = createAgent' fileWatchHandle
    let run () =
        Run.run fileWatchConfig.SleepMilliseconds fileWatchAgent.Post refactorAgent.Post
        |> Seq.toList
    run
