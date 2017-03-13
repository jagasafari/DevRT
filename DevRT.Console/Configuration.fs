module Configuration

open System.Configuration
open DevRT

type Config = FSharp.Configuration.YamlConfig<"config.yaml">

let getProject (config: Config) =
    config.Projects
    |> Seq.find(
        fun (projectConfig: Config.Projects_Item_Type) ->
            let msBuildDir: string =
                    projectConfig.Solution.MsBuildWorkingDir
            printfn "%s:%s" msBuildDir config.Project
            msBuildDir.Contains config.Project)

let initConfig() =
    let config = Config()
    ConfigurationManager.AppSettings.["config"]
    |> config.Load
    config, config |> getProject

let getTestProjectConfig = function
    | (project: Config.Projects_Item_Type)
        when project.NUnit.TestProjects.On ->
        let testProjects = project.NUnit.TestProjects
        (testProjects.Directory,
            testProjects.Dlls |> Seq.toList)
        |> RunTestsOn
    | _ -> RunTestsOff

let getFileWatchConfig
    (config: Config)
    (project: Config.Projects_Item_Type) =
    {
    SleepMilliseconds = config.FileWatch.SleepMiliseconds
    ExcludedDirectories = config.FileWatch.ExcludedDirectories
    FileChangeWatchDir = project.WatchDir }

let getRefactorConfig (config: Config) =
    {
    DevRTDeploymentDir =
        config.Environment.DevRTDeploymentDir }

let getNUnitConfig
    (config: Config)
    (project: Config.Projects_Item_Type) =
    {
    TestProjects = project |> getTestProjectConfig
    NUnitConsole = config.Environment.NUnitConsole
    NUnitDeploymentDir =
        config.Environment.NUnitDeploymentDir }

let getMsBuildConfig
    (config: Config)
    (project: Config.Projects_Item_Type) =
    {
    MsBuildPath = config.Environment.MsBuildPath
    MsBuildWorkingDir = project.Solution.MsBuildWorkingDir
    SolutionOrProjectFile =
            project.Solution.SolutionOrProjectFile
    OptionArgs = project.Solution.OptionArgs }
