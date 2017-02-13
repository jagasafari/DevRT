module DevRT.ExportApi

open System
open Common
open ConsoleOutput
open DevRT.DataTypes
open FileUtil
open IOWrapper
open ProcessRunner
open MsBuild
open StringWrapper

let composeNUnitHandle envConfig testProjects =
    let handleOutput' =
        NUnit.handleOutput writelineDarkCyan writelineYellow (NUnit.writeFailureLineNumberInfo writelineYellow)
    let handle' = NUnit.handle NUnit.getUpdatedStatus handleOutput'
    let deleteAllFiles' = deleteAllFiles exists deleteRecursive
    let cleanDirectory'() =
        cleanDirectory deleteAllFiles' createDirectory envConfig.DeploymentDir
    let getProcessStartInfo' = getProcessStartInfo envConfig.NUnitConsole
    let getOutputDirectory dllsSource =
            let outputDirectory =
                dllsSource |> NUnit.getTestDirectoryName |> combine envConfig.DeploymentDir
            outputDirectory
    let run' startInfo = ProcessRunner.run startInfo handle'
    let prepareAndRunTests() =
        NUnit.prepareAndRunTests
            cleanDirectory'
            getProcessStartInfo'
            getOutputDirectory
            run'
            testProjects
    NUnit.run prepareAndRunTests

let runMsBuild' slnConfig msBuildPath post () =
    let getMsBuildStartInfo() =
        getProcessStartInfo
            msBuildPath
            (getRunArgsString slnConfig.SolutionFile)
            slnConfig.MsBuildWorkingDir
    let update, getStatus = createMsBuildStatus()
    let getContinuationStatus' = getStatus >> getContinuationStatus
    let updateStatus = (getUpdatedStatus contains getContinuationStatus') >> update
    let handleStarting' = handleStarting writelineDarkGreen getNow updateStatus
    let processOutput' =
        processOutput
            handleStarting' writelinePurple writelineRed updateStatus
    let run' = run getMsBuildStartInfo
    runMSBuild processOutput' run' getStatus post

let runFileWatch stepsRunAgentPost fileWatchConfig msBuildWorkingDir =
    let getFiles() =
         msBuildWorkingDir
         |> FileWatchAgent.getFiles
             (fileWatchConfig.ExcludedDirectories |> FileWatchAgent.isBaseCase)
    let getTimeLine' () =
        FileWatchAgent.getTimeLine
            getNow
            fileWatchConfig.ChangeWithinPastSeconds
    let isModified' =
         FileWatchAgent.getLastWriteTime
         |> FileWatchAgent.isModified getTimeLine'
    let fileWatchHandle =
        FileWatchAgent.handle getFiles isModified' stepsRunAgentPost
    fileWatchHandle

let getPostToFileWatchAgent envConfig fileWatchConfig slnConfig =
    let nUnitHandle = composeNUnitHandle envConfig slnConfig.TestProjects
    let nUnitAgent = Agent.createAgent nUnitHandle ()
    let msBuildHandle = runMsBuild' slnConfig envConfig.MsBuildPath nUnitAgent.Post
    let stepsRunAgent = Agent.createAgent msBuildHandle ()
    let fileWatchHandle =
        runFileWatch stepsRunAgent.Post fileWatchConfig slnConfig.MsBuildWorkingDir
    let fileWatchAgent = Agent.createAgent fileWatchHandle ()
    let refactorHandle () = ()
    let refactorAgent = Agent.createAgent refactorHandle ()
    let run () = Run.run fileWatchAgent.Post refactorAgent.Post |> Seq.toList
    run

