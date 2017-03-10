module Configuration

open DevRT

type Config = FSharp.Configuration.YamlConfig<"config.yaml">

let initConfig() = Config()

let getTestProjectConfig = function
    | (project: Config.Projects_Item_Type)
        when project.NUnit.TestProjects.On ->
        let testProjects = project.NUnit.TestProjects
        (testProjects.Directory,
            testProjects.Dlls |> Seq.toList)
        |> RunTestsOn
    | _ -> RunTestsOff

let getProject (config: Config) =
    config.Projects
    |> Seq.find(
        fun (projectConfig: Config.Projects_Item_Type) ->
            let msBuildDir: string =
                    projectConfig.Solution.MsBuildWorkingDir
            printfn "%s:%s" msBuildDir config.Project
            msBuildDir.Contains config.Project)

let getFileWatchConfig (config: Config) =
    {
    SleepMilliseconds = config.FileWatch.SleepMiliseconds
    ExcludedDirectories = config.FileWatch.ExcludedDirectories
    FileChangeWatchDir =
        config |> getProject |> fun x -> x.WatchDir }

let getRefactorConfig (config: Config) =
    {
    DevRTDeploymentDir =
        config.Environment.DevRTDeploymentDir }

let getNUnitConfig (config: Config) =
    {
    TestProjects = config |> getProject |> getTestProjectConfig
    NUnitConsole = config.Environment.NUnitConsole
    NUnitDeploymentDir =
        config.Environment.NUnitDeploymentDir }

let getMsBuildConfig (config: Config) =
    {
    MsBuildPath = config.Environment.MsBuildPath
    MsBuildWorkingDir =
        config
        |> getProject
        |> fun project -> project.Solution.MsBuildWorkingDir
    SolutionOrProjectFile =
        config
        |> getProject
        |> fun project ->
            project.Solution.SolutionOrProjectFile
    OptionArgs =
        config
        |> getProject
        |> fun project -> project.Solution.OptionArgs }
