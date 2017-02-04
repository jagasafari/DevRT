module DevRT.ExportApi

open Common
open ConsoleOutput
open DevRT.DataTypes
open FileUtil
open IOWrapper
open ProcessRunner
open MsBuild
open StringWrapper

let getPostToFileWatchAgent envConfig fileWatchConfig slnConfig =
    let runNUnit' =
        let handleOutput' = NUnit.handleOutput writelineDarkCyan writelineYellow
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
                slnConfig.TestProjects
        NUnit.run prepareAndRunTests

    let nUnitAgent = Agent.createAgent runNUnit' ()
    Agent.handleError Logging.error nUnitAgent

    let runMsBuild' () =
        let getMsBuildStartInfo() =
            getProcessStartInfo
                envConfig.MsBuildPath
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
        runMSBuild processOutput' run' getStatus nUnitAgent.Post

    let stepsRunAgent = Agent.createAgent runMsBuild' ()
    Agent.handleError Logging.error stepsRunAgent

    let getFiles() =
         slnConfig.MsBuildWorkingDir
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
        FileWatchAgent.handle getFiles isModified' stepsRunAgent.Post

    let fileWatchAgent = Agent.createAgent fileWatchHandle ()
    Agent.handleError Logging.error fileWatchAgent

    fileWatchAgent.Post
